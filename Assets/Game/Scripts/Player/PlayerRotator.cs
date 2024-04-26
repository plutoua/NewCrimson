using TimmyFramework;
using UnityEngine;
using UnityEngine.Events;

public class PlayerRotator : MonoBehaviour
{
    public event UnityAction<float> PlayerRotateEvent
    {
        add
        {
            _playerRotateEvent += value;
        }
        remove
        {
            _playerRotateEvent -= value;
        }
    }

    private event UnityAction<float> _playerRotateEvent;

    private PlayerMover _playerMover;
    private MouseLocatorController _mouseLocatorController;
    private UIWindowsController _windowsController;
    private Transform _circle;

    private void Awake()
    {
        _playerMover = GetComponent<PlayerMover>();
        _circle = transform.Find("Circle").transform;
        if (Game.IsReady)
        {
            _windowsController = Game.GetController<UIWindowsController>();
            _mouseLocatorController = Game.GetController<MouseLocatorController>();
            _mouseLocatorController.MouseChangePositionEvent += OnMouseMoved;
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
        if (_windowsController != null && _windowsController.IsUIMode)
        {
            return;
        }

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
        _circle.eulerAngles = new Vector3(0, 0, angle - 90);
        _playerRotateEvent?.Invoke(angle + 90);
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
        _windowsController = Game.GetController<UIWindowsController>();
        _mouseLocatorController = Game.GetController<MouseLocatorController>();
        _mouseLocatorController.MouseChangePositionEvent += OnMouseMoved;
    }
}
