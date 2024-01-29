using System;
using UnityEngine;

public class InventoryItem
{
    public int ID => _itemScheme.ID;
    public string Name => _itemScheme.Name;
    public Sprite Sprite => _itemScheme.Sprite;
    public int MaxItemInStack => _itemScheme.MaxItemInStack;
    public int Price => _itemScheme.Price;
    public int Multiplier => _itemScheme.StackMultiplier;
    public int Amount { get; private set; }
    public ItemScheme ItemScheme => _itemScheme;

    protected ItemScheme _itemScheme;

    public InventoryItem(ItemScheme itemScheme, int amount)
    {
        _itemScheme = itemScheme;
        Amount = amount;
    }

    public void SetAmount(int amount)
    {
        if(amount > MaxItemInStack * Multiplier)
        {
            throw new ArgumentOutOfRangeException("Item amount is more then MAX value.");
        }

        Amount = amount;
    }

    public InventoryItem CloneItemWithAmount(int amount)
    {
        return new InventoryItem(_itemScheme, amount);
    }

    public ItemScheme GetItemScheme()
    {
        return _itemScheme;
    }
}
