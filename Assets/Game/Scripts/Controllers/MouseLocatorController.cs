using System;
using TimmyFramework;
using UnityEngine;
using UnityEngine.Events;


public class MouseLocatorController : IController
{

    
    public event UnityAction MouseChangePositionEvent
    {
        add { _mouseChangePositionEvent += value; }
        remove { _mouseChangePositionEvent -= value; }
    }
    private event UnityAction _mouseChangePositionEvent;
    public Vector3 MouseWorldPosition => _mouseWorldPosition;

    private Camera _mainCamera;
    private Vector3 _mousePosition;
    private Vector3 _mouseWorldPosition;

    public void Initialize()
    {
        _mainCamera = Camera.main;
        _mousePosition = Input.mousePosition;
    }

    public void CheckMousePosition()
    {
        var mousePosition = Input.mousePosition;

        if (mousePosition != _mousePosition)
        {
            _mousePosition = mousePosition;
            _mouseWorldPosition = _mainCamera.ScreenToWorldPoint(_mousePosition);

            _mouseChangePositionEvent?.Invoke();
        }
    }
}
