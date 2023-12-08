using TimmyFramework;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] private AttackScheme _attackScheme;
    [SerializeField] private Transform _attackPosition;

    private float _attackDelay;
    private float _lastAttackTimePass;
    private bool _isCanAttack;
    private MouseLocatorController _mouseLocatorController;

    private void Start()
    {
        _attackDelay = _attackScheme.AttackDelay;
        if (Game.IsReady)
        {
            _mouseLocatorController = Game.GetController<MouseLocatorController>();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
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
        if(_mouseLocatorController == null)
        {
            return Vector3.zero;
        }
        
        var mouseWorldPositionDirection = _mouseLocatorController.MouseWorldPosition - _attackPosition.position;
        mouseWorldPositionDirection.z = 0;
        return mouseWorldPositionDirection.normalized;
    }

    private void OnGameReady()
    {
        _mouseLocatorController = Game.GetController<MouseLocatorController>();
    }
}
