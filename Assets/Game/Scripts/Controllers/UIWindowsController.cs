using System;
using System.Collections.Generic;
using TimmyFramework;
using UnityEngine;

public class UIWindowsController : IController, IOnStart
{
    public bool IsUIMode => _activeWindows.Count > 0;
    public Canvas Canvas { get; private set; }
    public UIMoveable Moveable {  get; private set; }
    public UISlider Slider { get; private set; }
    public UIPlayerInventory PlayerInventory { get; private set; }
    public UIGroundInventory GroundInventory { get; private set; }
    public UIInnerInventory InnerInventory { get; private set; }
    public UICrafter Crafter { get; private set; }
    public UIStaticCharacteristics Ñharacteristics { get; private set; }
    public UITrade Trade { get; private set; }
    public InventoryItemMover ItemOnMove { get; private set; }

    private Dictionary<Type, UIIWindow> _activeWindows;
    
    private InventoryController _inventoryController;

    public void Initialize()
    {
        _activeWindows = new Dictionary<Type, UIIWindow>();
    }

    public void OnStart()
    {
        _inventoryController = Game.GetController<InventoryController>();
    }

    public void SetCanvas(Canvas canvas)
    {
        if (Canvas != null)
        {
            return;
        }
        Canvas = canvas;
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
        if(ItemOnMove != null)
        {
            ItemOnMove.MakeEndDrag();
        }
        ItemOnMove = item;
    }

    public void SetCrafter(UICrafter crafter)
    {
        if (Crafter != null)
        {
            return;
        }
        Crafter = crafter;
    }

    public void SetCharacteristics(UIStaticCharacteristics characteristics)
    {
        if (Ñharacteristics != null)
        {
            return;
        }
        Ñharacteristics = characteristics;
    }

    public void SetTrade(UITrade trade)
    {
        if (Trade != null)
        {
            return;
        }
        Trade = trade;
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
        // _inventoryController.SetupGroundInventory();
        OpenWindow(GroundInventory);
    }

    public void OpenInnerInventory()
    {
        OpenInventory();
        OpenWindow(InnerInventory);  
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
