using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 _direction;
    private float _damage;
    private float _velocity;
    private int _hitNumber;
    private float _lifeTime;

    private float _timePased;
    private int _hitCounter;

    public void Init(Vector3 direction, float damage,float velocity, int hitNumber, float lifeTime)
    {
        _direction = direction;
        _damage = damage;
        _velocity = velocity;
        _hitNumber = hitNumber;
        _lifeTime = lifeTime;

        _timePased = 0;
        _hitCounter = 0;
    }

    private void Update()
    {
        Move();
        TimeFlow();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO: hit reaction
    }

    private void Move()
    {
        if(_direction == null)
        {
            return;
        }

        transform.position += _direction * _velocity * Time.deltaTime;
    }

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
}
