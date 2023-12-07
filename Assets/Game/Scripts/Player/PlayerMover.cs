using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    public event UnityAction PlayerMovedEvent
    {
        add { _playerMovedEvent += value; } 
        remove { _playerMovedEvent -= value; }
    }

    private event UnityAction _playerMovedEvent;

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Move(Vector3.left);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Move(Vector3.right);
        }
        if (Input.GetKey(KeyCode.W))
        {
            Move(Vector3.up);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Move(Vector3.down);
        }
    }

    private void Move(Vector3 direction)
    {
        transform.position += direction * _speed * Time.deltaTime;
        _playerMovedEvent?.Invoke();
    }
}
