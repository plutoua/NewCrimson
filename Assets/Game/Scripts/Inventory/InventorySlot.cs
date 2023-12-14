using System;

public class InventorySlot
{
    public InventoryItem Item { get; private set; }
    public bool IsEmpty => Item == null;
    public bool IsFull => Item.Amount >= _slotCapacity;
    public int SlotCapacity => _slotCapacity < Item.MaxItemInStack ? _slotCapacity : Item.MaxItemInStack;
    public int SlotAmount => Item.Amount;
    public int ItemID => Item.ID;

    private int _slotCapacity;

    public InventorySlot(int slotCapacity)
    {
        _slotCapacity = slotCapacity;
    }

    public InventoryItem AddItemAndReturnRest(InventoryItem item)
    {
        if (IsEmpty)
        {
            Item = item.CloneItemWithAmount(0);
        }

        if(Item.ID != item.ID)
        {
            throw new InvalidOperationException("Different types of items.");
        }

        item.SetAmount(AddAmountAndReturnRest(item.Amount));

        return item;
    }

    public InventoryItem RemoveItemAndReturnRest(InventoryItem item)
    {
        if (IsEmpty)
        {
            throw new ArgumentException("Slot is empty.");
        }
        if (Item.ID != item.ID)
        {
            throw new InvalidOperationException("Different types of items.");
        }

        item.SetAmount(RemoveAmountAndReturnRest(item.Amount));
        
        if(SlotAmount == 0)
        {
            RemoveItem();
        }

        return item;
    }

    private int AddAmountAndReturnRest(int amount)
    {
        var totalAmount = SlotAmount + amount;

        if(totalAmount <= SlotCapacity) { 
            Item.SetAmount(totalAmount);
            return 0;
        }
        else
        {
            Item.SetAmount(SlotCapacity);
            return totalAmount - SlotCapacity;
        }
    }

    private int RemoveAmountAndReturnRest(int amount) 
    {
        if(SlotAmount < amount)
        {
            amount -= SlotAmount;
            Item.SetAmount(0);
            return amount;
        }
        else
        {
            Item.SetAmount(SlotAmount - amount);
            return 0;
        }
    }

    private void RemoveItem()
    {
        Item = null;
    }
}