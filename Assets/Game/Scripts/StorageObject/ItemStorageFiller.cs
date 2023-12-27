using System.Collections;
using System.Collections.Generic;
using TimmyFramework;
using UnityEngine;

public class ItemStorageFiller : MonoBehaviour
{
    [SerializeField] private ItemScheme[] _items;

    private void Start()
    {
        if(Game.IsReady)
        {
            var itemStorage = Game.GetStorage<ItemStorage>();
            itemStorage.SetItems(_items);
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        var itemStorage = Game.GetStorage<ItemStorage>();
        itemStorage.SetItems(_items);
    }
}
