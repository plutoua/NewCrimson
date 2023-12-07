using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    private Camera _mainCamera;
    private Vector3 _mousePosition;
    private PlayerMover _playerMover;

    private void Awake()
    {
        _playerMover = GetComponent<PlayerMover>();
    }

    private void OnEnable()
    {
        _playerMover.PlayerMovedEvent += OnPlayerMoved;
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        _mousePosition = Vector3.zero;
    }

    private void Update()
    {
        var newMousePosition = Input.mousePosition;
        
        if(newMousePosition != _mousePosition)
        {
            _mousePosition = newMousePosition;
            SetRotation();
        }
    }

    private void OnDisable()
    {
        _playerMover.PlayerMovedEvent -= OnPlayerMoved;
    }

    private void SetRotation()
    {
        var mouseWorldPosition = _mainCamera.ScreenToWorldPoint(_mousePosition);
        var angle = Mathf.Atan2(mouseWorldPosition.y - transform.position.y, mouseWorldPosition.x - transform.position.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle - 90);
    }

    private void OnPlayerMoved()
    {
        SetRotation();
    }
}
