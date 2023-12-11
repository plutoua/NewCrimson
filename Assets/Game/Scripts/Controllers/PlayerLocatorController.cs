using TimmyFramework;
using UnityEngine;
using UnityEngine.Events;

public class PlayerLocatorController : IController
{
    public event UnityAction<Vector3> PlayerChangePositionEvent
    {
        add { _playerChangePositionEvent += value; }
        remove { _playerChangePositionEvent -= value; }
    }
    private event UnityAction<Vector3> _playerChangePositionEvent;

    public Vector3 PlayerPosition => GetPlayerPosition();

    private PlayerMover _playerMover;

    public void Initialize()
    {
    }


    public void SetPlayerMover(PlayerMover playerMover)
    {
        _playerMover = playerMover;
        _playerMover.PlayerMovedEvent += OnPlayerMoved;
    }

    private Vector3 GetPlayerPosition()
    {
        if (_playerMover == null)
        {
            return Vector3.zero;
        }

        return _playerMover.transform.position;
    }

    private void OnPlayerMoved(Vector3 position)
    {
        _playerChangePositionEvent?.Invoke(position);
    }
}

