using System.Collections;
using System.Collections.Generic;
using TimmyFramework;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIInventoryBase : MonoBehaviour, UIIWindow
{
    [SerializeField] private UIInventorySlot _uiSlotPrefab;

    protected List<UIInventorySlot> _uiSlots;

    protected MoveableWindow _moveableWindow;
    protected CanvasScaler _canvasScaler;
    protected Inventory _inventory;
    protected UIWindowsController _windowsController;
    protected LanguageStorage _languageStorage;

    public void Activate()
    {
        _moveableWindow.Activate();
    }

    public void Deactivate()
    {
        _moveableWindow.Deactivate();
    }

    private void Start()
    {
        _moveableWindow = GetComponentInParent<MoveableWindow>();
        _moveableWindow.CloseButtonEvent += OnCloseButton;
        _canvasScaler = GetComponentInParent<CanvasScaler>();

        _uiSlots = new List<UIInventorySlot>();

        if (Game.IsReady)
        {
            InitialSetup();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void InitialSetup()
    {
        var inventoryController = Game.GetController<InventoryController>();
        _windowsController = Game.GetController<UIWindowsController>();
        _languageStorage = Game.GetStorage<LanguageStorage>();

        SetInventory(inventoryController);
        SetInventorySizesAndPosition();

        SetName();

        CreateSlots();

        ActivateSlots();

        _inventory.InventoryUpdatedEvent += OnInventoryUpdated;
        _inventory.InventoryChangeSlotNumberEvent += OnInventoryChangeSlotNumber;
    }

    private void OnCloseButton()
    {
        _windowsController.CloseWindow(this);
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        InitialSetup();
    }

    private void OnInventoryUpdated()
    {
        foreach (var slot in _uiSlots)
        {
            slot.UpdateSlot();
        }
    }

    private void OnInventoryChangeSlotNumber()
    {
        SetInventorySizesAndPosition();
        DeactivateAllSlots();
        if(_uiSlots.Count < _inventory.SlotNumber)
        {
            CreateSlots();
        }
        ActivateSlots();
    }

    private void SetInventorySizesAndPosition()
    {
        int collumNumber = 10;
        if (_inventory.IsGround)
        {
            collumNumber = 27;
        }

        else if (_inventory.SlotNumber < 10)
        {
            collumNumber = _inventory.SlotNumber;
        }
        

        var rowNumber = _inventory.SlotNumber / collumNumber;
        if(rowNumber * collumNumber < _inventory.SlotNumber)
        {
            rowNumber++;
        }

        int width = collumNumber * 65 + 20;
        int height = rowNumber * 65 + 20;

        _moveableWindow.SetSizes(width, height);
        SetPosition(width, height);
    }

    private void CreateSlots()
    {
        UIInventorySlot tempSlot;
        for (int i = _uiSlots.Count; i < _inventory.SlotNumber; i++)
        {
            tempSlot = Instantiate(_uiSlotPrefab, transform);
            tempSlot.Deactivate();
            _uiSlots.Add(tempSlot);
        }
    }

    private void DeactivateAllSlots()
    {
        foreach (var slot in _uiSlots)
        {
            slot.Deactivate();
        }
    }

    private void ActivateSlots()
    {
        var inventorySlots = _inventory.Slots;
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            _uiSlots[i].SetSlot(inventorySlots[i]);
        }
    }

    protected abstract void SetInventory(InventoryController inventoryController);

    protected abstract void SetPosition(float width, float height);

    protected abstract void SetName();
}
