using System;
using System.Collections;
using System.Collections.Generic;
using TimmyFramework;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.UI;

public class UISlider : MonoBehaviour, UIIWindow
{
    private UISliderStayValue _stayValue;
    private UISliderSendValue _sendValue;
    private Slider _slider;
    private Button _button;
    private MoveableWindow _moveableWindow;

    private UIWindowsController _windowsController;

    private InventorySlot _fromSlot;
    private InventorySlot _toSlot;
    private int _workingValue;

    private void Start()
    {
        _moveableWindow = GetComponentInParent<MoveableWindow>();
        _moveableWindow.CloseButtonEvent += OnCloseButton;
        _stayValue = GetComponentInChildren<UISliderStayValue>();
        _sendValue = GetComponentInChildren<UISliderSendValue>();

        _slider = GetComponentInChildren<Slider>();
        _slider.onValueChanged.AddListener(OnSliderChange);

        _button = GetComponentInChildren<Button>();
        _button.onClick.AddListener(MakeAction);

        if (Game.IsReady)
        {
            SetupWindowController();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void SetupWindowController() 
    {
        _windowsController = Game.GetController<UIWindowsController>();
        _windowsController.SetSlider(this);
    }

    private void OnCloseButton()
    {
        _windowsController.CloseWindow(this);
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        SetupWindowController();
    }

    private void OnSliderChange(float value)
    {
        int picked = (int) Math.Round(_workingValue * value);

        _sendValue.SendText.text = picked.ToString();
        _stayValue.StayText.text = (_workingValue - picked).ToString();
    }

    private void MakeAction()
    {
        int picked = (int)Math.Round(_workingValue * _slider.value);
        var actionItem = _fromSlot.Item.CloneItemWithAmount(picked); 
        _fromSlot.RemoveFromItemAndInventory(actionItem);
        _toSlot.AddToItemAndInventory(actionItem);
        _windowsController.CloseWindow(this);
    }

    public void Activate(InventorySlot fromSlot, InventorySlot toSlot)
    {
        if (fromSlot.Item.MaxItemInStack == 1) 
        {
            SingleItemTransfer(fromSlot, toSlot);
        }
        else
        {
            MultipleItemTransfer(fromSlot, toSlot);
        }
    }

    public void MultipleItemTransfer(InventorySlot fromSlot, InventorySlot toSlot)
    {
        _fromSlot = fromSlot;
        _workingValue = _fromSlot.AllItemAmountInInventory.Amount;
        _toSlot = toSlot;

        _sendValue.SendText.text = _fromSlot.SlotAmount.ToString();
        _stayValue.StayText.text = (_workingValue - _fromSlot.SlotAmount).ToString();

        var sliderValue = _fromSlot.SlotAmount / (float)_workingValue;
        _slider.value = sliderValue;

        _windowsController.OpenWindow(this);
    }

    public void SingleItemTransfer(InventorySlot fromSlot, InventorySlot toSlot)
    {
        var newItem = fromSlot.Item.CloneItemWithAmount(1);
        toSlot.AddToItemAndInventory(newItem);
        fromSlot.RemoveFromItemAndInventory(newItem);
    }

    public void Deactivate()
    {
        _fromSlot = null;
        _workingValue = 0;
        _toSlot = null;

        _stayValue.StayText.text = string.Empty;
        _sendValue.SendText.text = string.Empty;
        _slider.value = 0;

        _moveableWindow.Deactivate();
    }

    public void Activate()
    {
        _moveableWindow.Activate();
    }
}
