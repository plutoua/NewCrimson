using System.Collections;
using System.Collections.Generic;
using TimmyFramework;
using UnityEngine;

public class UIWindowSystem : MonoBehaviour
{
    private bool _isUIMode => _windowsController.IsUIMode;

    private UIGroundInventory _groundInventory;
    private UIPlayerInventory _playerInventory;

    private UIWindowsController _windowsController;

    private void Start()
    {
        _groundInventory = GetComponentInChildren<UIGroundInventory>();
        _playerInventory = GetComponentInChildren<UIPlayerInventory>();

        if (Game.IsReady)
        {
            _windowsController = Game.GetController<UIWindowsController>();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void Update()
    {
        if(_windowsController == null) 
        {
            return;
        }

        if(Input.GetKeyUp(KeyCode.I)) 
        {
            if (_isUIMode)
            {
                DeactivateInventory();
            }
            else
            {
                ActivateInventory();
            }
        }
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        _windowsController = Game.GetController<UIWindowsController>();
    }

    private void DeactivateInventory()
    {
        _groundInventory.Deactivate();
        _playerInventory.Deactivate();
        _windowsController.TurnOffUIMode();
    }

    private void ActivateInventory()
    {
        _groundInventory.Activate();
        _playerInventory.Activate();
        _windowsController.TurnOnUIMode();
    }
}
