using TimmyFramework;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    public event UnityAction<Vector3> PlayerMovedEvent
    {
        add { _playerMovedEvent += value; } 
        remove { _playerMovedEvent -= value; }
    }

    private event UnityAction<Vector3> _playerMovedEvent;

    private Vector2 _moveDirection;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _moveDirection = Vector2.zero;

        if (Game.IsReady)
        {
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
        _moveDirection.x = Input.GetAxis("Horizontal");
        _moveDirection.y = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if(_rigidbody ==  null) return;
        if(_moveDirection != Vector2.zero)
        {
            _rigidbody.MovePosition(_rigidbody.position + _moveDirection * _speed * Time.fixedDeltaTime);
            _playerMovedEvent?.Invoke(transform.position);
        }   
    }

    private void OnGameReady()
    {
        var playerLocatorController = Game.GetController<PlayerLocatorController>();
        playerLocatorController.SetPlayerMover(this);
    }
}
