using System;
using UnityEngine;

public class GroundItem: InventoryItem
{
  
    public Vector2 Coords { get; private set; }

   
    public GroundItem(ItemScheme itemScheme, int amount, Vector2 coords): base(itemScheme, amount)
    {
        Coords = coords;
    }

    public void SetCoords(Vector2 coords)
    {
        Coords = coords;
    }

    public Vector2 GetCoords() { return Coords; }

    public InventoryItem CloneItemWithAmount(int amount)
    {
        return new InventoryItem(_itemScheme, amount);
    }

    public ItemScheme GetItemScheme()
    {
        return _itemScheme;
    }
}
