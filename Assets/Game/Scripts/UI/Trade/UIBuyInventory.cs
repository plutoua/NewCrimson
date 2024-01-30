using System.Collections;
using System.Collections.Generic;
using TimmyFramework;
using UnityEngine;
using UnityEngine.UI;

public class UIBuyInventory : MonoBehaviour
{
    [SerializeField] private UIInventorySlot _uiSlotPrefab;
    [SerializeField] private BuyItemInfo _buyItemInfo;

    private Inventory _buyInventory;
    private List<UIInventorySlot> _uiSlots;

    private PlayerStatController _playerStatController;

    private void Start()
    {
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
        _uiSlots = new List<UIInventorySlot>();

        var inventoryController = Game.GetController<InventoryController>();
        _buyInventory = inventoryController.BuyInventory;
        _buyInventory.InventoryUpdatedEvent += OnInventoryUpdated;

        _playerStatController = Game.GetController<PlayerStatController>();
        _playerStatController.PlayerStatChangeEvent += OnInventoryUpdated;

        _buyItemInfo.SetInventory(inventoryController.PlayerInventory, _buyInventory);

        CreateSlots();
        ActivateSlots();
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        InitialSetup();
    }

    private void CreateSlots()
    {
        UIInventorySlot tempSlot;
        for (int i = _uiSlots.Count; i < _buyInventory.SlotNumber; i++)
        {
            tempSlot = Instantiate(_uiSlotPrefab, transform);
            tempSlot.Deactivate();
            tempSlot.GetComponent<SelectionSlot>().OnMouseClickEvent += OnSlotSelected;
            _uiSlots.Add(tempSlot);
        }
    }

    private void ActivateSlots()
    {
        var inventorySlots = _buyInventory.Slots;
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            _uiSlots[i].SetSlot(inventorySlots[i]);
        }
    }

    private void OnInventoryUpdated()
    {
        UpdateAllSlots();
    }

    private void UpdateAllSlots()
    {
        foreach (var slot in _uiSlots)
        {
            slot.UpdateSlot();
        }
    }

    private void OnSlotSelected(UIInventorySlot inventorySlot)
    {
        if(!inventorySlot.InventorySlot.IsEmpty) 
        {
            _buyItemInfo.SetItem(inventorySlot.InventorySlot.Item);
        }
    }
}
