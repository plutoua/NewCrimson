using System.Collections;
using System.Collections.Generic;
using TimmyFramework;
using UnityEngine;

public class ItemStorage : IStorage
{
    private Dictionary<int, ItemScheme> _items;

    public void Initialize()
    {
        _items = new Dictionary<int, ItemScheme>();
    }

    public void SetItems(ItemScheme[] items)
    {
        foreach (var item in items)
        {
            _items[item.ID] = item;
        }
    }

    public ItemScheme GetItemByID(int id)
    {
        if(!_items.ContainsKey(id))
        {
            return null;
        }

        return _items[id];
    }
}
