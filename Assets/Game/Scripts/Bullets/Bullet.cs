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
    protected Transform _spawnTransform;

    public void Init(Vector3 direction, float damage,float velocity, int hitNumber, 
        float lifeTime, Transform spawnTransform)
    {
        _direction = direction;
        _damage = damage;
        _velocity = velocity;
        _hitNumber = hitNumber;
        _lifeTime = lifeTime;
        _spawnTransform = spawnTransform;

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
        // TODO: hit reaction
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
