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
    private PlayerStatController _playerStatController;

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
        // _attackScheme.SetIsPlayerBullets(true);
        if (Game.IsReady)
        {
            _mouseLocatorController = Game.GetController<MouseLocatorController>();
            _windowsController = Game.GetController<UIWindowsController>();
            _playerStatController = Game.GetController<PlayerStatController>();
            _attackDelay = SetAttackDelay();
            _playerStatController.PlayerStatChangeEvent += OnStatChange;

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

        if(_lastAttackTimePass >= _attackDelay / 10f)
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
        _playerStatController = Game.GetController<PlayerStatController>();
        _attackDelay = SetAttackDelay();
        _playerStatController.PlayerStatChangeEvent += OnStatChange;

    }

    private void OnStatChange()
    {
        _attackDelay = SetAttackDelay();

    }

    private float CalculateDelay(float start, float finish, float lowBorder, float hightBorder, int attackSpeed)
    {
       
        var delay = (finish - start) / (hightBorder - lowBorder);
        var attackDeley = start - (attackSpeed - lowBorder) * delay;
        return attackDeley;
    }


    private float SetAttackDelay()
    {

        var attackSpeed = _playerStatController.GetStat(Stat.AttackSpeed);
        
        float attackDeley;

        // Make Formula
        if (attackSpeed > 60) {

            attackDeley = 0.01f;

        }
        else if (30 < attackSpeed && attackSpeed <= 60)
        {
            attackDeley = CalculateDelay(0.2f, 0.01f, 30f, 60f, attackSpeed);
        }
        else if (10 < attackSpeed && attackSpeed <= 30)
        {
            attackDeley = CalculateDelay(1f, 0.2f, 10f, 30f, attackSpeed);
        }
        else if (0 < attackSpeed && attackSpeed <= 10)
        {
            attackDeley = CalculateDelay(8f, 1f, 0f, 10f, attackSpeed);
        }
        else
        {
            attackDeley = 8f;
        }



        
        return attackDeley;
    }
}
