public class MeleeBullet : Bullet
{
    protected override void Action()
    {
        transform.position = _spawnPosition;
        SetBulletAngle(_direction);
    }
}
