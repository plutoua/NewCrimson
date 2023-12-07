using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] private AttackScheme _attackScheme;
    [SerializeField] private Transform _attackPosition;

    private float _attackDelay;
    private float _lastAttackTimePass;
    private bool _isCanAttack;

    private void Start()
    {
        _attackDelay = _attackScheme.AttackDelay;
    }

    private void Update()
    {
        if (_isCanAttack)
        {
            if (Input.GetMouseButton(0))
            {
                Attack();
            }
        }
        else
        {
            AttackDelay();
        }
    }

    private void Attack()
    {
        StartAttackDelay();
        _attackScheme.Attack(_attackPosition.position, GetAttackDirection());
    }

    private void StartAttackDelay()
    {
        _lastAttackTimePass = 0;
        _isCanAttack = false;
    }

    private void EndAttackDelay()
    {
        _isCanAttack = true;
    }

    private void AttackDelay()
    {
        _lastAttackTimePass += Time.deltaTime;

        if(_lastAttackTimePass >= _attackDelay)
        {
            EndAttackDelay();
        }
    }

    private Vector3 GetAttackDirection()
    {
        // TODO: world mouse position service
        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        return mouseWorldPosition.normalized;
    }
}
