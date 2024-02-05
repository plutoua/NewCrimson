using TimmyFramework;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMover : MonoBehaviour
{
    private int _speed;

    public event UnityAction<Vector3> PlayerMovedEvent
    {
        add { _playerMovedEvent += value; } 
        remove { _playerMovedEvent -= value; }
    }

    private event UnityAction<Vector3> _playerMovedEvent;

    private Vector2 _moveDirection;
    private Rigidbody2D _rigidbody;

    private UIWindowsController _windowsController;
    private PlayerStatController _playerStatController;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _moveDirection = Vector2.zero;

        if (Game.IsReady)
        {
            _windowsController = Game.GetController<UIWindowsController>();
            _playerStatController = Game.GetController<PlayerStatController>();
            _speed = _playerStatController.GetStat(Stat.MoveSpeed);
            _playerStatController.PlayerStatChangeEvent += OnStatChange;
            var playerLocatorController = Game.GetController<PlayerLocatorController>();
            playerLocatorController.SetPlayerMover(this);
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void Update()
    {
        if (_windowsController != null && _windowsController.IsUIMode)
        {
            return;
        }

        _moveDirection.x = Input.GetAxis("Horizontal");
        _moveDirection.y = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        if (_windowsController != null && _windowsController.IsUIMode)
        {
            return;
        }

        if (_windowsController != null) { Move(); }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Game.IsReady)
        {
            _playerMovedEvent?.Invoke(transform.position);
        }
    }

    private void Move()
    {
        if(_rigidbody ==  null) return;
        if(_moveDirection != Vector2.zero)
        {
            _rigidbody.MovePosition(_rigidbody.position + _moveDirection * _speed / 10f * Time.fixedDeltaTime);
            _playerMovedEvent?.Invoke(transform.position);
        }   
    }

    private void OnGameReady()
    {
        _windowsController = Game.GetController<UIWindowsController>();
        _playerStatController = Game.GetController<PlayerStatController>();
        _speed = _playerStatController.GetStat(Stat.MoveSpeed);
        var playerLocatorController = Game.GetController<PlayerLocatorController>();
        playerLocatorController.SetPlayerMover(this);
        _playerStatController.PlayerStatChangeEvent += OnStatChange;
    }

    private void OnStatChange()
    {
        _speed = _playerStatController.GetStat(Stat.MoveSpeed);
    }
}
