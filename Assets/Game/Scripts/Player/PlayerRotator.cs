using System.Collections;
using System.Collections.Generic;
using TimmyFramework;
using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    private PlayerMover _playerMover;
    private MouseLocatorController _mouseLocatorController;

    private void Awake()
    {
        _playerMover = GetComponent<PlayerMover>();
        if (Game.IsReady)
        {
            _mouseLocatorController = Game.GetController<MouseLocatorController>();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void OnEnable()
    {
        _playerMover.PlayerMovedEvent += OnPlayerMoved;
        if(_mouseLocatorController != null) 
        {
            _mouseLocatorController.MouseChangePositionEvent += OnMouseMoved;
        }
    }

    private void Update()
    {
        if (_mouseLocatorController != null)
        {
            _mouseLocatorController.CheckMousePosition();
        }      
    }

    private void OnDisable()
    {
        _playerMover.PlayerMovedEvent -= OnPlayerMoved;
        if (Game.IsReady)
        {
            _mouseLocatorController.MouseChangePositionEvent -= OnMouseMoved;
        }
    }

    private void SetRotation()
    {
        var mouseWorldPosition = _mouseLocatorController.MouseWorldPosition;
        var angle = Mathf.Atan2(mouseWorldPosition.y - transform.position.y, mouseWorldPosition.x - transform.position.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle - 90);
    }

    private void OnPlayerMoved(Vector3 vector3)
    {
        SetRotation();
    }

    private void OnMouseMoved()
    {
        SetRotation();
    }

    private void OnGameReady()
    {
        _mouseLocatorController = Game.GetController<MouseLocatorController>();
        _mouseLocatorController.MouseChangePositionEvent += OnMouseMoved;
    }
}
