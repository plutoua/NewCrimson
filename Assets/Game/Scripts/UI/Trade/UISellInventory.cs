using System.Collections.Generic;
using System.Linq;
using TimmyFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISellInventory : MonoBehaviour
{
    [SerializeField] private UIInventorySlot _uiSlotPrefab;
    [SerializeField] private ItemScheme _coinScheme;
    [SerializeField] private TMP_Text _totalPrice;
    [SerializeField] private Button _sellButton;

    private Inventory _sellInventory;
    private Inventory _playerInventory;
    private List<UIInventorySlot> _uiSlots;
    private int _price;
    
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
        _sellInventory = inventoryController.SellInventory;
        _playerInventory = inventoryController.PlayerInventory;
        _sellInventory.InventoryUpdatedEvent += OnInventoryUpdated;

        _playerStatController = Game.GetController<PlayerStatController>();
        _playerStatController.PlayerStatChangeEvent += OnInventoryUpdated;

        CreateSlots();
        ActivateSlots();
        CalculatePrice();
        ShowPrice();

        _sellButton.onClick.AddListener(OnSellButtonClick);
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        InitialSetup();
    }

    private void CreateSlots()
    {
        UIInventorySlot tempSlot;
        for (int i = _uiSlots.Count; i < _sellInventory.SlotNumber; i++)
        {
            tempSlot = Instantiate(_uiSlotPrefab, transform);
            tempSlot.Deactivate();
            _uiSlots.Add(tempSlot);
        }
    }

    private void ActivateSlots()
    {
        var inventorySlots = _sellInventory.Slots;
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            _uiSlots[i].SetSlot(inventorySlots[i]);
        }
    }

    private void OnInventoryUpdated()
    {
        UpdateAllSlots();
        CalculatePrice();
        ShowPrice();
    }

    private void UpdateAllSlots()
    {
        foreach (var slot in _uiSlots)
        {
            slot.UpdateSlot();
        }
    }

    private void CalculatePrice()
    {
        var allItems = _sellInventory.GetInventoryItems();
        _price = allItems.Sum(item => item.Price * item.Amount);
        _price = (int)(_price * (0.75f + 0.01f * _playerStatController.GetStat(Stat.Charisma)));
    }

    private void ShowPrice()
    {
        _totalPrice.text = "Total: " + _price;
    }

    private void OnSellButtonClick()
    {
        var coins = new InventoryItem(_coinScheme, _price);
        _playerInventory.Add(coins);
        _sellInventory.ClearInventory();   
    }

    public void ReturnItems()
    {
        var allItems = _sellInventory.GetInventoryItems();

        for(int i = 0; i < allItems.Length; i++)
        {
            _playerInventory.Add(allItems[i]);
        }

        _sellInventory.ClearInventory();
    }
}
