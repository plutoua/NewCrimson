using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    protected Vector3 _direction;
    protected float _damage;
    protected float _velocity;
    protected int _hitNumber;
    protected float _lifeTime;

    protected float _timePased;
    protected int _hitCounter;
    protected Vector3 _spawnPosition;
    // protected Vector3 _spawnSpeed;

    public GameObject _testEnemyHit;
    public GameObject _testPlayerHit;

    public bool _playerBullet;

    public void Init(Vector3 direction, float damage,float velocity, int hitNumber, 
        float lifeTime, Vector3 spawnPosition, bool isPlayerBullet= false)
    {
        _direction = direction;
        _damage = damage;
        _velocity = velocity;
        _hitNumber = hitNumber;
        _lifeTime = lifeTime;
        _spawnPosition = spawnPosition;
        // _spawnSpeed = spawnSpeed;
        _playerBullet = isPlayerBullet;

        _timePased = 0;
        _hitCounter = 0;
        
        SetBulletAngle(direction);
    }

    private void Update()
    {
        Action();
        TimeFlow();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log(collision.tag);
        if (_playerBullet)
        {
            // Логіка для снарядів ворога
            HandleEnemyCollision(collision);
        }
        else
        {
            // Логіка для снарядів гравця
            
            HandlePlayerCollision(collision);
        }
    }

    private void HandleEnemyCollision(Collider2D collision)
    {
        // TODO: Реакція на зіткнення з ворогом
        Instantiate(_testPlayerHit, transform.position, Quaternion.identity);
    }

    private void HandlePlayerCollision(Collider2D collision)
    {
        // TODO: Реакція на зіткнення з гравцем
        Instantiate(_testEnemyHit, transform.position, Quaternion.identity);
    }

    protected abstract void Action();

    private void TimeFlow()
    {
        if(_lifeTime > 0)
        {
            _timePased += Time.deltaTime;
            if(_timePased >= _lifeTime) 
            {
                Deactivation();
            }
        }
    }

    private void HitReaction()
    {
        if(_hitNumber > 0)
        {
            _hitCounter++;
            if(_hitCounter >= _hitNumber) 
            {
                Deactivation();
            }
        }
    }

    private void Deactivation()
    {
        // TODO: deactivation in objects pull 
        Destroy(gameObject);
    }

    protected void SetBulletAngle(Vector3 direction)
    {
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle - 90);
    }
}
