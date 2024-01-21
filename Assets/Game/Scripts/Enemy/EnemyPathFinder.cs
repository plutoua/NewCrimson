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

    public int playerExistanceConfirmation; // ���� ����� ���, �� �� �����, �� �� ���� ��������� ����� �������. �� �������.
                                            // ����� �������� � ������ ����� - � ������� ���� ���� �����.
    public int alertLewel; // ����� �������. �� ����� �������� �������� ����� ����. + ����� ������ * ����������
    public int partyBlasted; // ���� ���������� �������� � ������ �� "����" - ����� ���, ��� �������� "������" �����.
                             // ����� ������ ��������, �� � ������, �� ����������� 1 ��� � ����� ����� ��������.
                             // ������� ��������, ���� �� ���� �������� � ��� ����� ������ ����� ������. + ����� ������ * ����������
    public int leaderLevel; // �������� ������� ��������� ������������ �������.
                            // ���� ���� ������ �������� �� "�����" - �� ������ ������������ � ����� ���������.
                            // ��� ����� 2-3 ����, + ����� ������ * ����������.

    // �� ����� ������� ������������ 2�� ������. ������� ������� �������� � �������.

    // �����-������ (false,false) (� ���� �� ��������)
    // ������� (���������) (false, true)/(true, true) �� ��� ����� ������� �����, �� ������ ��������������, �� �� �������� - stayStill. ���� �� ������� ������. 
    // �������������(��� �� �������) (true, false) (�������� "���������", "�������" �� ������ ����������� ������ ����� ��� �� ���� ����� ���� ������ �� ����������) (�������� � ���� ��������, ����� "�������" ���� �� �������� ����� ���� �� ���������� (��� �����, ���� ��������))
    // ������� - ��� 䳿 ����� ����� ����� ���� * ���������� ������. ����� ����� �� ����� �������� "����" �������� �� ����������� ���������. ĳ� ����������� � ������� ���������� ��� ���������.
    // ����� ������ ������ (�� ��� �� �������) (false, true) (����� "�������" ���)
    // �����(���� ������� �������) (false, true). ���� ������� � � ��� - ����� ���� ����� ����� � ������� ��������� ������� �� ���. ������ � ���� �����.
    // ����� ������ �� ����� (�� ��� �� �������) (��������� ���� �� ��������� ��� ����� � ���� ������) (�� ��� ����� � ���� ������ � ������ ���� �������� �����, �� ���������� ������� ����� �������)

    //������ ������� �����:
    // ����� ����� ������� - ��� ������ ��� �� ������, ������� - ������� ������ ����������� ��������, ��������� ���� �� �� ����� ���������, ��� ���� �� ��������, �������� �� �� ����� ��������, ���������� �� �� �� �������. ����� ���������� ��������, ���� ���� ����� �� �����.
    // �� ������ ������ � ���������� ������� ���������� ���� ���� ���������� "�����", ��� ������ ���� ����� ��������� ��������. �������� AStarPathFinder �� ���� �����, (���������� ����� ����� ���������, � ��������� �� ���� �������)
    // ��� ���� �� ��������, �� ��������� - �� � ������� ���������� ������ �� ��� � ����� �� ���� � ������ �������� �����. ���������� ������ ��� ����� ����� ������ �� �������� �������� ������ ���� ���������.

    // ����� �� ������ ��� ���������� � �������, ��� � ���� ���������� ���,  ����� ���� ����� � ����� "������� �����". ����� ��� ������� �������, � ��� �� ����������, ���� ����� ������ � ���� ����� ��� �������. ����� ����� ��� �� ����� � ��������� �� ��� �� ���� ����. stayStill ������ ����� ���� ���, ��� �������.

    // �������� ������ � ���� ������. ����� ������� ������ �����, ������� "����� ����". + ����� ������ * ����������

    // ��� ������ ����� ������� � ������������ ��������� ������ � ��� ���� � ����� ����� �������� - ��� ������ ������� �������.

    // �� ������, ���� ����������� � �������� �����������. ���������� ����� �����.


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
        // ���������� ������ �������������� ���� x �� y ����������
        float distance = Vector2.Distance(new Vector2(pointA.x, pointA.y), new Vector2(pointB.x, pointB.y));

        // �������� �� ������� ����� �� 10.0f
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
                timer = 0f; // �������� �������
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
