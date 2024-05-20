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
        var itemSchemes = Resources.LoadAll("ItemSchemes", typeof(ItemScheme));

        SetItems(itemSchemes);
    }

    public void SetItems(Object[] items)
    {
        ItemScheme itemScheme;
        foreach (var item in items)
        {
            itemScheme = (ItemScheme)item;
            _items[itemScheme.ID] = itemScheme;
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
