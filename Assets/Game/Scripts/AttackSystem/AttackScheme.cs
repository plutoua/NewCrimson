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
    [SerializeField] private bool _isPlayerBullets;

    public void SetIsPlayerBullets(bool isPlayerBullets = false)
    {
        // якщо треба поміняти пулю на дружню. хз нашо.
        _isPlayerBullets = isPlayerBullets;
    }

    public float AttackDelay => _attackDelay;

    public void Attack(Transform spawnTransform, Vector3 targetDirection)
    {
        //Debug.Log("Spawn Transform Position: " + spawnTransform.position);
        //Debug.Log("Target Direction: " + targetDirection);

        Bullet bulletTemp;
        for (int i = 0; i < _bulletPerAttack; i++)
        {
            // Створення Vector2 для розрахунку напрямку лише використовуючи x та y
            Vector2 direction2D = new Vector2(targetDirection.x, targetDirection.y).normalized;

            // Конвертація назад в Vector3, зберігаючи оригінальне значення z
            Vector3 direction = new Vector3(direction2D.x, direction2D.y, 0);
            Vector3 spawnPosition = spawnTransform.position + direction * 0.5f;

            bulletTemp = GetBullet(spawnPosition);
            InitBullet(bulletTemp, targetDirection, spawnPosition);
        }
    }

    private Bullet GetBullet(Vector3 spawnPosition)
    {
        // TODO: objects pull
        return Instantiate(_bulletPrefab, spawnPosition, Quaternion.identity);
    }

    private void InitBullet(Bullet bullet, Vector3 target, Vector3 spawnTransform)
    {
        bullet.Init(
            TargetPositionAccuracyImpact(target),
            _bulletDamage,
            _bulletVelocity, 
            _bulletHitNumber,
            _bulletTimeLife,
            spawnTransform,
            _isPlayerBullets
            );
    }

    private Vector3 TargetPositionAccuracyImpact(Vector3 target)
    {
        target.x += Random.Range(_bulletAccuracy.x / 100f, _bulletAccuracy.y / 100f);
        target.y += Random.Range(_bulletAccuracy.x / 100f, _bulletAccuracy.y / 100f);

        return target;
    }
}
