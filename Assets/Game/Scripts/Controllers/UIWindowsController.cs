using System;
using System.Collections.Generic;
using TimmyFramework;
using UnityEngine;

public class UIWindowsController : IController, IOnStart
{
    public bool IsUIMode => _activeWindows.Count > 0;
    public UIMoveable Moveable {  get; private set; }
    public UISlider Slider { get; private set; }
    public UIPlayerInventory PlayerInventory { get; private set; }
    public UIGroundInventory GroundInventory { get; private set; }
    public UIInnerInventory InnerInventory { get; private set; }

    private Dictionary<Type, UIIWindow> _activeWindows;
    private InventoryItemMover _itemOnMove;
    private InventoryController _inventoryController;

    //for test
    public void SetTest(ItemScheme itemScheme)
    {
        _inventoryController.SetTest(itemScheme);
    }

    public void Initialize()
    {
        _activeWindows = new Dictionary<Type, UIIWindow>();
    }

    public void OnStart()
    {
        _inventoryController = Game.GetController<InventoryController>();
    }

    public void SetSlider(UISlider slider)
    {
        if(Slider != null)
        {
            return;
        }

        Slider = slider;
    }

    public void SetItemOnMove(InventoryItemMover item)
    {
        if(_itemOnMove != null)
        {
            _itemOnMove.MakeEndDrag();
        }
        _itemOnMove = item;
    }

    public void SetMoveable(UIMoveable moveable)
    {
        if (Moveable != null)
        {
            return;
        }

        Moveable = moveable;
    }

    public void SetPlayerInventory(UIPlayerInventory playerInventory)
    {
        if (PlayerInventory != null)
        {
            return;
        }

        PlayerInventory = playerInventory;
    }

    public void SetGroundInventory(UIGroundInventory groundInventory)
    {
        if (GroundInventory != null)
        {
            return;
        }

        GroundInventory = groundInventory;
    }

    public void SetInnerInventory(UIInnerInventory innerInventory)
    {
        if (InnerInventory != null)
        {
            return;
        }

        InnerInventory = innerInventory;
    }

    public void OpenInventory()
    {
        OpenWindow(PlayerInventory);
        _inventoryController.SetupGroundInventory();
        OpenWindow(GroundInventory);
    }

    public void OpenInnerInventory()
    {
        if(_inventoryController.IsInnerInventoryReady)
        {
            OpenInventory();
            OpenWindow(InnerInventory);
        }   
    }

    public void OpenWindow(UIIWindow window)
    {
        if (!_activeWindows.ContainsKey(window.GetType()))
        {
            _activeWindows[window.GetType()] = window;
            window.Activate();
        }
    }

    public void CloseWindow(UIIWindow window)
    {
        if (_activeWindows.ContainsKey(window.GetType()))
        {
            _activeWindows.Remove(window.GetType());
            window.Deactivate();
        }
    }

    public void CloseAllWindows()
    {
        foreach (var window in _activeWindows.Values)
        {
            window.Deactivate();
        }
        _activeWindows.Clear();
    }

    
}
