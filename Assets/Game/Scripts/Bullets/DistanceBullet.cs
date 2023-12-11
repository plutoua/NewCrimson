using UnityEngine;

public class DistanceBullet : Bullet
{
    protected override void Action()
    {
        if (_direction == null)
        {
            return;
        }

        transform.position += _direction * _velocity * Time.deltaTime;
    }
}

