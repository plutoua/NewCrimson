using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class Inventory
{
    public event UnityAction InventoryUpdatedEvent
    {
        add { _inventoryUpdatedEvent += value; }
        remove { _inventoryUpdatedEvent -= value; }
    }
    private event UnityAction _inventoryUpdatedEvent;

    public event UnityAction InventoryChangeSlotNumberEvent
    {
        add { _inventoryChangeSlotNumberEvent += value; }
        remove { _inventoryChangeSlotNumberEvent -= value; }
    }
    private event UnityAction _inventoryChangeSlotNumberEvent;

    public List<InventorySlot> Slots => _slots;
    public int SlotNumber => _slotNumber;
    public bool IsGround => _dropInventory == null;

    private int _slotNumber;
    private int _slotCapacity;
    private List<InventorySlot> _slots;
    private Inventory _dropInventory;

    public Inventory(int slotNumber, int slotCapacity)
    {
        _slotNumber = slotNumber;
        _slotCapacity = slotCapacity;
        _dropInventory = null;

        CreateInventorySlots();
    }

    public Inventory(int slotNumber, int slotCapacity, Inventory dropInventory)
    {
        _slotNumber = slotNumber;
        _slotCapacity = slotCapacity;
        _dropInventory = dropInventory;

        CreateInventorySlots();
    }

    public void Add(InventoryItem item)
    {
        if (item.Amount == 0)
        {
            _inventoryUpdatedEvent?.Invoke();
            return;
        }

        var slotsOfID = _slots.Where(slot => !slot.IsEmpty && slot.ItemID == item.ID && !slot.IsFull).ToArray();

        foreach (var slot in slotsOfID)
        {
            item = slot.AddItemAndReturnRest(item);
            if (item.Amount == 0)
            {
                _inventoryUpdatedEvent?.Invoke();
                return;
            }
        }

        var emptySlots = _slots.Where(slot => slot.IsEmpty).ToArray();

        foreach (var slot in emptySlots)
        {
            item = slot.AddItemAndReturnRest(item);
            if (item.Amount == 0)
            {
                _inventoryUpdatedEvent?.Invoke();
                return;
            }
        }

        _inventoryUpdatedEvent?.Invoke();
        DropItem(item);
    }

    public void Remove(InventoryItem item)
    {
        if (item.Amount == 0)
        {
            _inventoryUpdatedEvent?.Invoke();
            return;
        }

        var slotsOfID = _slots.Where(slot => !slot.IsEmpty && slot.ItemID == item.ID).Reverse().ToArray();
        foreach (var slot in slotsOfID)
        {
            item = slot.RemoveItemAndReturnRest(item);
            
            if (item.Amount == 0)
            {
                _inventoryUpdatedEvent?.Invoke();
                return;
            }
        }
        _inventoryUpdatedEvent?.Invoke();
    }

    public InventoryItem GetAllOfID(InventoryItem item)
    {
        var amountInAllSlots = _slots.Where(slot => !slot.IsEmpty && slot.ItemID == item.ID && !slot.IsFull).Sum(slot => slot.SlotAmount);
        item.SetAmount(amountInAllSlots);
        return item;
    }

    public InventoryItem[] GetInventoryItems()
    {
        return _slots.Where(slot => !slot.IsEmpty).Select(slot => slot.Item).ToArray();
    }

    public void ChangeInventorySize(int slotNumber)
    {
        var allItens = GetInventoryItems();
        _slotNumber = slotNumber;
        CreateInventorySlots();

        foreach (var item in allItens)
        {
            Add(item);
        }
        _inventoryChangeSlotNumberEvent?.Invoke();
    }

    private void CreateInventorySlots()
    {
        _slots = new List<InventorySlot>();
        for (int i = 0; i < _slotNumber; i++)
        {
            _slots.Add(new InventorySlot(_slotCapacity, this));
        }
    }

    private void DropItem(InventoryItem item)
    {
        if(_dropInventory == null)
        {
            return;
        }

        _dropInventory.Add(item);
    }
}
