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
    private Quadtree[] _ItemMaps;
    private InventoryController _inventoryController;

    public void OnCreate()
    {
        _playerLocatorController = Game.GetController<PlayerLocatorController>();
        _inventoryController = Game.GetController<InventoryController>();

    }

    public void ChangeLevel(int direction) {
     
        if (direction == 1)
        {
            // Up
            // blank to 2, 3, 4
            // set 1 as 0
            // blank 1
            _ItemMaps[2] = new Quadtree();
            _ItemMaps[3] = new Quadtree();
            _ItemMaps[4] = new Quadtree();
            _ItemMaps[0] = _ItemMaps[1];
            _ItemMaps[1] = new Quadtree();
        }
        else if (direction == 2)
        {
            // Right
            // blank to 1, 3, 4
            // set 2 as 0
            // blank 2
            _ItemMaps[1] = new Quadtree();
            _ItemMaps[3] = new Quadtree();
            _ItemMaps[4] = new Quadtree();
            _ItemMaps[0] = _ItemMaps[2];
            _ItemMaps[2] = new Quadtree();
        }
        else if(direction == 3)
        {
            // Down
            // blank to 1, 2, 4
            // set 3 as 0
            // blank 3
            _ItemMaps[1] = new Quadtree();
            _ItemMaps[2] = new Quadtree();
            _ItemMaps[4] = new Quadtree();
            _ItemMaps[0] = _ItemMaps[3];
            _ItemMaps[3] = new Quadtree();
        }
        else if(direction == 4)
        {
            // Left
            // blank to 2, 3, 1
            // set 4 as 0
            // blank 4
            _ItemMaps[1] = new Quadtree();
            _ItemMaps[2] = new Quadtree();
            _ItemMaps[3] = new Quadtree();
            _ItemMaps[0] = _ItemMaps[4];
            _ItemMaps[4] = new Quadtree();
        }
    }

    public void Initialize()
    {
        _ItemMaps = new Quadtree[5];

        _inventory = new Inventory(_inventoryController.GroundSlotCapacity, _inventoryController.GroundInventoryStackSize);
        // Current
        _ItemMaps[0] = new Quadtree();
        // Up
        _ItemMaps[1] = new Quadtree();
        // Right
        _ItemMaps[2] = new Quadtree();
        // Down
        _ItemMaps[3] = new Quadtree();
        // Left
        _ItemMaps[4] = new Quadtree();
    }

    public Inventory GetInventory()
    {
        _inventory.ClearInventory();
     
        Vector3 playerPosition = _playerLocatorController.PlayerPosition;
        Vector2 searchCoords = new Vector2(playerPosition[0], playerPosition[1]);
        
        List<GroundItem> groundItems = _ItemMaps[0].RetrieveInRadius(searchCoords);
        foreach (GroundItem groundItem in groundItems)
        {
            InventoryItem inventoryItem = new InventoryItem(groundItem.GetItemScheme(), groundItem.Amount);
            _inventory.Add(inventoryItem);
            Debug.Log(inventoryItem);
        }
        return _inventory;
    }

    public void DownloadMap(int[,] itemsMap, int roomId=0)
    {
        // parse items by scheme and set coords
    }

    public void SetItem(Vector2 coords, ItemScheme itemScheme, int amount = 1, int roomId=0)
    {
        GroundItem groundItem = new GroundItem(itemScheme, amount, coords);
        _ItemMaps[roomId].Insert(coords, groundItem);
    }

    public void UpdateInventory(Inventory updatedInentory)
    {
        Vector3 playerPosition = _playerLocatorController.PlayerPosition;
        Vector2 searchCoords = new Vector2(playerPosition[0], playerPosition[1]);
        List<GroundItem> itemToRemove = _ItemMaps[0].RetrieveInRadius(searchCoords);
        foreach(GroundItem it in itemToRemove)
        {
            _ItemMaps[0].Remove(it.Coords, it );
        }

        foreach (InventorySlot slot in updatedInentory.Slots)
        {
            if (!slot.IsEmpty) { 
                InventoryItem slotI = slot.Item;
                ItemScheme itemScheme = slot.Item.GetItemScheme();
                SetItem(searchCoords, itemScheme, slot.Item.Amount);
             
                // Debug.Log("SAVED SHIT ON COORDS: ");
                // Debug.Log(searchCoords);
            }
        }
        _inventory.CopyFromOtherInventory(updatedInentory);
    }

    public void OnInventoryExit()
    {
        _inventory.ClearInventory();
    }

}
