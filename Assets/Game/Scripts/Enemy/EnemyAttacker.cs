using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TimmyFramework;

public class EnemyAttacker : MonoBehaviour
{
    [SerializeField] private AttackScheme _attackScheme;
    [SerializeField] private Transform _attackPosition;

    private float _attackDelay;
    private float _lastAttackTimePass;
    private bool _isCanAttack;

    private bool _inAttackStatus;
    private PlayerLocatorController _playerLocatorController;
    private UIWindowsController _windowsController;

    public float rayLength = 10.0f;
    public Color rayColor = Color.red;

    public void SetAttackStatus(bool attacking) {
        _inAttackStatus = attacking;
    }


    private void Start()
    {
        _inAttackStatus = false;
        _attackDelay = _attackScheme.AttackDelay;
        if (Game.IsReady)
        {
            _playerLocatorController = Game.GetController<PlayerLocatorController>();
            _windowsController = Game.GetController<UIWindowsController>();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void Update()
    {
        //if (_windowsController != null && _windowsController.IsUIMode)
        //{
        //    return;
        //}

        if (_isCanAttack)
        {

            Attack();
        }
        else
        {
            AttackDelay();
        }
    }

    private void Attack()
    {
        int layerMask = LayerMask.GetMask("InteractiveLayer");
        RaycastHit2D hit = Physics2D.Raycast(_attackPosition.position, GetAttackDirection(), Mathf.Infinity, layerMask);
        
        if (hit.collider != null)
        {
            //Debug.Log(hit.collider);
            // Debug.Log("Промінь зіткнувся з об'єктом: " + hit.collider.gameObject.name);

            if (hit.collider.gameObject.name == "Circle")
            {
                // Debug.Log("Гравець видимий, немає перешкод!");
                StartAttackDelay();
                _attackScheme.Attack(_attackPosition, GetAttackDirection());
            }
            else
            {
                // Debug.Log("Є перешкода між гравцем та об'єктом!");
                // EnemyPathFinder _enemyPathFinder = this.GetComponent<EnemyPathFinder>();
                // _enemyPathFinder.MoveCloser();
            }
        }
        
        


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

        if (_lastAttackTimePass >= _attackDelay && _inAttackStatus)
        {
            EndAttackDelay();
        }
    }

    private Vector3 GetAttackDirection()
    {
        if (_playerLocatorController == null)
        {
            return Vector3.zero;
        }

        var WorldPositionDirection = _playerLocatorController.PlayerPosition - _attackPosition.position;
        WorldPositionDirection.z = 0;
        return WorldPositionDirection.normalized;
    }

    private void OnGameReady()
    {
        _playerLocatorController = Game.GetController<PlayerLocatorController>();
        _windowsController = Game.GetController<UIWindowsController>();
    }
}
