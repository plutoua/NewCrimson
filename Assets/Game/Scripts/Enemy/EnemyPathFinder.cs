using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TimmyFramework;

public class EnemyPathFinder : TileObject
{

    public int partyId = -1;
    private PlayerLocatorController _playerLocatorController;
    private EnemysLocatorController _enemysLocatorController;
    private Vector3 savedPosition;
    private float timer = 0.0f;
    private float interval = 0.5f;
    private float decision_time_max = 1.5f;
    private float decision_time_min = 0.3f;
    // temporaty public
    public int _id = -1;
    private bool new_move = false;
    public bool group_leader = false;
    public bool chain_part = false;

    public FieldOfView prefabFieldOfView;

    public FieldOfView enemy_view;

    public List<GameObject> party;
    public bool playerInWiev;

    public int healthPersentage = 100;

    public int playerExistanceConfirmation; // якщо ворог зна, що ти існуєш, він не буде знижувати рівень тривоги. це ідіотизм.
                                            // крики союзників в малому радіусі - є трігером цієї херні також.
    public int alertLewel; // рівень тривоги. від нього залежить врубання всякої херні. + рівень гравця * коефіцієнт
    public int partyBlasted; // якщо пропадають союзники і список це "нони" - чувак тіка, для передачі "знання" іншим.
                             // режим пошуку союзника, не є станом, бо запускається 1 раз і чисто прока напрямок.
                             // механіка зачистки, якщо ГГ дуже страшний і так можна шукати інших ворогів. + рівень гравця * коефіцієнт
    public int leaderLevel; // перевірка довжини можливого побудованого ланцюгу.
                            // типу якщо ланцюг потрапля на "лідера" - він дуууже продовжується і пачка величезна.
                            // так пачки 2-3 рила, + рівень гравця * коефіцієнт.

    // всі шість статусів покриваються 2ма булями. частина статусів виведена в механіки.

    // стоїть-чилить (false,false) (ні разу не дьоргали)
    // патруль (підозрілість) (false, true)/(true, true) по суті режим охорони точки, від корової характеристики, що не міняється - stayStill. того не окремий статус. 
    // переслідування(зна де гравець) (true, false) (лічильник "активацій", "шорохів" та спільна пригніченість группи поряд час від часу скидує стан группи до середнього) (потрапля в зону видимості, рівень "тривоги" виріс за короткий період часу до критичного (різкі звуки, крик союзників))
    // механіка - різні дії дають різний рівень шуму * коефіцієнт відстані. таким чином ми можем зарядить "крик" союзника як моментальну активацію. Дії накидуються в спільний контроллер для оповіщення.
    // режим пошуку ворога (не зна де гравець) (false, true) (рівень "тривоги" ріша)
    // атака(коли гравець видимий) (false, true). якщо видимий і в зоні - атака сама собою прока і перевіря необхідну відстань до цілі. додати в атак схему.
    // режим пошуку від точки (не зна де гравець) (активація якщо ми атакували або втікли з зони огляду) (по суті втеча з зони огляду і активує більш активний пошук, бо залишиться високий рівень тривоги)

    //основні тактичні фішки:
    // малий рівень тривоги - іде шукать сам як дундук, великий - функція пошуку найближчого союзника, оповіщення його що на нього повісились, той шука ще союзника, говорить що на нього повісився, передються всі ід по ланцюгу. також передається лідерство, якщо воно більше ніж зараз.
    // від розміру списку і заповнених позицій вибирається місце юніта алгоритмом "вихор", щоб группа після збору виглядала природно. включити AStarPathFinder по карті тайлів, (ігнорувати тільки великі дистанції, в залежності від рівня тривоги)
    // той шука ще союзника, не знаходить - дає в спільний контроллер сигнал на збір в центрі між ними і спробу вирушити разом. контроллер оповіщує про точку збору группи по фінально зібраному списку інтів колбеками.

    // ворог не передає свої координати в ланцюгу, хоч і шукає найбижчого далі,  тільки якщо ворог в режимі "охорона точки". треба для захисту проходу, і шоб не виманювать, якщо треба стоять в одній точці для тригеру. ворог стріля або не стріля в залежності від тих же двох булів. stayStill просто вбива весь рух, крім патрулю.

    // створити баферів з атак схемою. можна зробити окрему схему, назвати "схема бафу". + рівень гравця * коефіцієнт

    // чим більший рівень тривоги і підтвердженнь існування ворога і чим більш в группі рівень лідерства - тим крутіша групова тактика.

    // це колбек, який розкидується з спільного контроллеру. отримується точка збору.


    public void MAKECALLBACKFROMCONTROLLER(Vector3 changePosition) {
        MovementTo(changePosition);
    }

    public void MAKECALLBACKFROM2CONTROLLER(Vector3 changePosition)
    {
        AlertLevel();
    }

    public void MAKECALLBACKFROM3CONTROLLER(Vector3 changePosition)
    {
        PlayerExistanceConfirmation();
    }

    public void MovementTo(Vector3 changePosition)
    {
        savedPosition = changePosition;
    }

    public void AlertLevel(int level=1)
    {
        alertLewel += level;
    }

    public void PlayerExistanceConfirmation(int level = 1)
    {
        playerExistanceConfirmation += level;
    }

    public bool is_chasing;
    public bool is_looking;
    public bool stayStill;

    public NavMeshAgent agent;

    public void MoveCloser()
    {  
        if (agent.stoppingDistance > 1.5f) { 
            agent.stoppingDistance = agent.stoppingDistance - 1.0f;
        }
    }

    void Start()
    {
        healthPersentage = 100;
        partyId = -1;
        chain_part = false;
        playerInWiev = false;
        _id = -1;
        timer = 0.0f;
        interval = 0.5f;
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        // start new_move position, may be game logic part
        new_move = true;
        if (prefabFieldOfView != null) { 
            enemy_view = Instantiate(prefabFieldOfView, null);
        }
    }

    private void OnChangePlayerPosition(Vector3 position){
        new_move = true;
    }

    private void Awake(){
        if (Game.IsReady){
           _playerLocatorController = Game.GetController<PlayerLocatorController>();
           _enemysLocatorController = Game.GetController<EnemysLocatorController>();
        }
        else {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        _playerLocatorController = Game.GetController<PlayerLocatorController>();
        _playerLocatorController.PlayerChangePositionEvent+= OnChangePlayerPosition;
        if (_enemysLocatorController == null) { 
            _enemysLocatorController = Game.GetController<EnemysLocatorController>();
            _id = _enemysLocatorController.InsertEnemy(this.gameObject);
        }
        

    }

    private static Vector3 GenerateRandomDirection(Vector3 inVector)
    {
        float randomX = inVector.x + Random.Range(-10.0f, 10.0f);
        float randomY = inVector.y + Random.Range(-10.0f, 10.0f);

        Vector3 outVector = new Vector3(randomX, randomY, inVector.z);
        return outVector;
    }

    private bool CheckDistanse(Vector3 pointA, Vector3 pointB)
    {
        // Розрахунок відстані використовуючи лише x та y координати
        float distance = Vector2.Distance(new Vector2(pointA.x, pointA.y), new Vector2(pointB.x, pointB.y));

        // Перевірка чи відстань більша за 10.0f
        return distance < 10.0f;
    }

    public void LinkEnemy(int _leaderLevel, List<GameObject> _party)
    {
        chain_part = true;
        group_leader = true;
        leaderLevel = _leaderLevel;
        party = _party;
        if (group_leader && leaderLevel > party.Count)
        {
            List<int> ids = new List<int>();
            ids.Add(_id);
            foreach (GameObject partyMember in party)
            {
                ids.Add(partyMember.GetComponent<EnemyPathFinder>()._id);

            }
            group_leader = !_enemysLocatorController.LinkEnemy(ids, leaderLevel, this.gameObject.transform.position);
        }

        playerInWiev = true;



    }
    

    private void Update()
    {
        if (Game.IsReady) {

            //make sound wave with pathfinding (for optimisation) and using angles of objects near
            if (_playerLocatorController != null) { 
                if (CheckDistanse(_playerLocatorController.PlayerPosition, gameObject.transform.position))
                {
                    playerExistanceConfirmation++;
                }
            }
            if (playerExistanceConfirmation > 120)
            {
                playerInWiev = true;
            }
            timer += Time.deltaTime;
            if (new_move && timer >= interval)
            {
                timer = 0f; // Скидання таймера
                interval = UnityEngine.Random.Range(decision_time_min, decision_time_max);
                if (agent && enemy_view != null)
                {
                
                    if (playerInWiev)
                    {
                        // player in view


                        EnemyAttacker _enemyAttacker = gameObject.GetComponent<EnemyAttacker>();
                        _enemyAttacker.SetAttackStatus(true);

                        if (!chain_part) {
                            chain_part = true;
                            List<int> ids = new List<int>();

                            ids.Add(_id);
                            if (_enemysLocatorController != null) {
                                Debug.Log(gameObject.transform.position);
                                partyId = _enemysLocatorController.GetNewPartyId();
                                chain_part = _enemysLocatorController.LinkEnemy(ids, leaderLevel, gameObject.transform.position);
                            }
                        }
                        agent.SetDestination(_playerLocatorController.PlayerPosition);
                        enemy_view.setOriginObject(this.gameObject);
                        // ON PLAYER CHANGE POSITION
                        enemy_view.setTargetVector(_playerLocatorController.PlayerPosition);
                        // enemy_view.SetTarget(_playerLocatorController.PlayerPosition);}
                    }

                    else
                    {
                        // simple patroling
                        Vector3 destination = GenerateRandomDirection(this.gameObject.transform.position);
                        agent.SetDestination(destination);
                        enemy_view.setOriginObject(this.gameObject);
                        enemy_view.setTargetVector(GenerateRandomDirection(destination));
                    }
                    new_move = false;
                }
            }
        }
    }


}
