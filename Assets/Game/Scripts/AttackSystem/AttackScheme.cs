using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackScheme", menuName = "ScriptableObjects/New AttackScheme", order = 1)]
public class AttackScheme : ScriptableObject
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private int _bulletPerAttack;
    [SerializeField] private float _bulletDamage;
    [SerializeField] private float _bulletVelocity;
    [SerializeField] private float _bulletTimeLife;
    [SerializeField] private int _bulletHitNumber;
    [SerializeField] private Vector2 _bulletAccuracy;
    [SerializeField] private float _attackDelay;

    public float AttackDelay => _attackDelay;

    public void Attack(Transform spawnTransform, Vector3 target)
    {
        Bullet bulletTemp;
        for(int i = 0;  i < _bulletPerAttack; i++)
        {
            bulletTemp = GetBullet(spawnTransform.position);
            InitBullet(bulletTemp, target, spawnTransform);
        }
    }

    private Bullet GetBullet(Vector3 spawnPosition)
    {
        // TODO: objects pull
        return Instantiate(_bulletPrefab, spawnPosition, Quaternion.identity);
    }

    private void InitBullet(Bullet bullet, Vector3 target, Transform spawnTransform)
    {
        bullet.Init(
            TargetPositionAccuracyImpact(target),
            _bulletDamage,
            _bulletVelocity, 
            _bulletHitNumber,
            _bulletTimeLife,
            spawnTransform
            );
    }

    private Vector3 TargetPositionAccuracyImpact(Vector3 target)
    {
        target.x += Random.Range(_bulletAccuracy.x, _bulletAccuracy.y);
        target.y += Random.Range(_bulletAccuracy.x, _bulletAccuracy.y);

        return target;
    }
}
