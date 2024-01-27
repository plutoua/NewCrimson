using TimmyFramework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] private AttackScheme _attackScheme;
    [SerializeField] private Transform _attackPosition;

    private float _attackDelay;
    private float _lastAttackTimePass;
    private bool _isCanAttack;
    private MouseLocatorController _mouseLocatorController;
    private UIWindowsController _windowsController;

    // [SerializeField] private AttackScheme _attackScheme;
    private int _currentAttackIndex = 0;

    /*
    private void NextAttackScheme()
    {
        if (_attackSchemes.Count == 0)
        {
            return;
        }

        ChangeAttackScheme(_attackSchemes[_currentAttackIndex]);

        // «б≥льшуЇмо ≥ндекс, але €кщо в≥н дос€гаЇ к≥нц€ списку, починаЇмо знову з 0
        _currentAttackIndex = (_currentAttackIndex + 1) % _attackSchemes.Count;
    }*/


    private void ChangeAttackScheme(AttackScheme newAttackScheme)
    {
        StartAttackDelay();
        _attackScheme = newAttackScheme;
        // _attackScheme.SetIsPlayerBullets(true);
    }

    private void Start()
    {
        _attackScheme = _attackScheme;
        _attackDelay = _attackScheme.AttackDelay;
        // _attackScheme.SetIsPlayerBullets(true);
        if (Game.IsReady)
        {
            _mouseLocatorController = Game.GetController<MouseLocatorController>();
            _windowsController = Game.GetController<UIWindowsController>();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void Update()
    {
        if(_windowsController != null && _windowsController.IsUIMode)
        {
            return;
        }

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
        _attackScheme.Attack(_attackPosition, GetAttackDirection());
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
        _windowsController = Game.GetController<UIWindowsController>();
    }
}
