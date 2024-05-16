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
    private bool _isCanMove;

    private BlockerController _blockerController;
    private PlayerStatController _playerStatController;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _moveDirection = Vector2.zero;
        _isCanMove = true;

        if (Game.IsReady)
        {
            WhenGameReady();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void Update()
    {
        if (!_isCanMove)
        {
            return;
        }

        _moveDirection.x = Input.GetAxis("Horizontal");
        _moveDirection.y = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        if (!_isCanMove)
        {
            return;
        }

        Move();
        
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
        Game.OnInitializedEvent -= OnGameReady;
        WhenGameReady();
    }

    private void WhenGameReady()
    {
        _blockerController = Game.GetController<BlockerController>();
        _playerStatController = Game.GetController<PlayerStatController>();
        _speed = _playerStatController.GetStat(Stat.MoveSpeed);
        var playerLocatorController = Game.GetController<PlayerLocatorController>();
        playerLocatorController.SetPlayerMover(this);
        _playerStatController.PlayerStatChangeEvent += OnStatChange;
        _blockerController.UIChangeActivityEvent += OnUIChangeActivity;
    }

    private void OnStatChange()
    {
        _speed = _playerStatController.GetStat(Stat.MoveSpeed);
    }

    private void OnUIChangeActivity(bool isCanMove)
    {
        _isCanMove = isCanMove;
    }
}
