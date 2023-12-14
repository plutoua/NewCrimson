using System;
using System.Collections.Generic;
using System.Linq;

public class Inventory
{
    public List<InventorySlot> Slots => _slots;

    private int _slotNumber;
    private int _slotCapacity;
    private List<InventorySlot> _slots;

    public Inventory(int slotNumber, int slotCapacity)
    {
        _slotNumber = slotNumber;
        _slotCapacity = slotCapacity;

        for (int i = 0; i < _slotNumber; i++) 
        { 
            _slots.Add(new InventorySlot(_slotCapacity));
        }
    }

    public void Add(InventoryItem item) 
    {
        if(item.Amount == 0)
        { 
            return; 
        }

        var slotsOfID = _slots.Where(slot => !slot.IsEmpty && slot.ItemID == item.ID && !slot.IsFull).ToArray();

        foreach (var slot in slotsOfID) 
        {
            item = slot.AddItemAndReturnRest(item);
            if (item.Amount == 0)
            {
                return;
            }
        }

        var emptySlots = _slots.Where(slot => slot.IsEmpty).ToArray();

        foreach (var slot in emptySlots)
        {
            item = slot.AddItemAndReturnRest(item);
            if (item.Amount == 0)
            {
                return;
            }
        }

        DropItem(item);
    }

    public void Remove(InventoryItem item)
    {
        if (item.Amount == 0)
        {
            return;
        }
    }

    private void DropItem(InventoryItem item)
    {

    }
}
