public class MeleeBullet : Bullet
{
    protected override void Action()
    {
        transform.position = _spawnTransform.position;
        SetBulletAngle(_direction);
    }
}
