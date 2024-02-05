using System;
using TimmyFramework;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class CameraFolllower : MonoBehaviour
{
    [SerializeField] private float _speed;

    private PlayerLocatorController _playerLocatorController;
    private Vector3 _targetPosition;
    private bool _isMove;

    private void Start()
    {
        _isMove = false;

        if (Game.IsReady)
        {
            SetupPlayerLocatorController();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        SetupPlayerLocatorController();
    }

    private void OnPlayerChangePosition(Vector3 playerPosition)
    {
        playerPosition.z = transform.position.z;
        _targetPosition = playerPosition;
        _isMove = true;   
    }

    private void FixedUpdate()
    {
        if (_isMove)
        {
            var moveDirection = (_targetPosition - transform.position).normalized;
            var moveDistance = Vector3.Distance(_targetPosition, transform.position);

            transform.position += moveDirection * moveDistance * _speed * Time.deltaTime;
            TryToStop(moveDistance);
        }
    }

    private void SetupPlayerLocatorController()
    {
        _playerLocatorController = Game.GetController<PlayerLocatorController>();
        _playerLocatorController.PlayerChangePositionEvent += OnPlayerChangePosition;
        OnPlayerChangePosition(_playerLocatorController.PlayerPosition);
    }

    private void TryToStop(float oldDistance)
    {
        if(oldDistance < 0.01f)
        {
            transform.position = _targetPosition;
            _isMove = false;
        }
        else
        {
            var newDistance = Vector3.Distance(_targetPosition, transform.position);

            if (newDistance > oldDistance)
            {
                transform.position = _targetPosition;
                _isMove = false;
            }
        }
    }

}
