using System;
using TimmyFramework;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GroundDetectionController : IController, IOnCreate
{
    private Inventory _inventory;
    private PlayerLocatorController _playerLocatorController;
    private Quadtree _itemsMap;
    public ItemScheme _testItemScheme;
    // public bool _started = false;

    public void OnCreate()
    {
        _playerLocatorController = Game.GetController<PlayerLocatorController>();
    }

    public void Initialize()
    {

        _inventory = new Inventory(51, 99);
        _itemsMap = new Quadtree();
        // _started = false;
    }

    public Inventory GetInventory()
    {
        _inventory.ClearInventory();
        // ADD SHIT with vector 3
        //if (!_started) { 
        //    Vector2 testItemCoords = new Vector2(82.50f, 87.50f);
        //    GroundItem testGroundItem = new GroundItem(_testItemScheme, 2, testItemCoords);
        //
        //    _itemsMap.Insert(testItemCoords, testGroundItem);
        //}
        //_started = true;

        Vector3 playerPosition = _playerLocatorController.PlayerPosition;
        Vector2 searchCoords = new Vector2(playerPosition[0], playerPosition[1]);
        Debug.Log(searchCoords);
        List<GroundItem> groundItems = _itemsMap.RetrieveInRadius(searchCoords);
        foreach (GroundItem groundItem in groundItems)
        {
            InventoryItem inventoryItem = new InventoryItem(groundItem.GetItemScheme(), groundItem.Amount);
            _inventory.Add(inventoryItem);
        }
        return _inventory;
    }

    public void UpdateInventory(Inventory updatedInentory)
    {
        Vector3 playerPosition = _playerLocatorController.PlayerPosition;
        Vector2 searchCoords = new Vector2(playerPosition[0], playerPosition[1]);
        List<GroundItem> itemToRemove = _itemsMap.RetrieveInRadius(searchCoords);
        foreach(GroundItem it in itemToRemove)
        {
            _itemsMap.Remove(it.Coords, it );
        }

        foreach (InventorySlot slot in updatedInentory.Slots)
        {
            if (!slot.IsEmpty) { 
                InventoryItem slotI = slot.Item;
                Debug.Log(slotI);
                ItemScheme itemScheme = slot.Item.GetItemScheme();
                Debug.Log(itemScheme);
                GroundItem groundItem = new GroundItem(itemScheme, slot.Item.Amount, searchCoords);
                Debug.Log("SAVE SHIT");
                _itemsMap.Insert(searchCoords, groundItem);
            }
        }
        _inventory.CopyFromOtherInventory(updatedInentory);
    }

    public void OnInventoryExit()
    {
        _inventory.ClearInventory();
    }

}
