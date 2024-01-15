using System;
using TimmyFramework;
using UnityEngine;
using UnityEngine.Events;


public class InventoryController : IController, IOnCreate
{
    public Inventory PlayerInventory => _playerInventory;
    public Inventory GroundInventory => _groundInventory;
    public Inventory InnerInventory => _innerInventory;

    public bool IsInnerInventoryReady {  get; private set; }

    private PlayerStatController _playerStatController;
    private GroundDetectionController _groundDetectionController;

    private Inventory _playerInventory;
    private Inventory _groundInventory;
    private Inventory _innerInventory;

    private int _groundInventorySlotCapacity;
    private int _groundInventoryStackSize;
    private int _defaultStackSize;

    // for test

    private Inventory _testInnerInventory;
    private Inventory _testGroundInventory;

    public void SetTest(ItemScheme testItem)
    {
        _testInnerInventory = new Inventory(10, _defaultStackSize);
        _testInnerInventory.Add(new InventoryItem(testItem, 5));

        _testGroundInventory = new Inventory(_groundInventorySlotCapacity, _groundInventoryStackSize);
        _testGroundInventory.Add(new InventoryItem(testItem, 15));
    }

    public void OnCreate()
    {
        _playerStatController = Game.GetController<PlayerStatController>();
        _groundDetectionController = Game.GetController<GroundDetectionController>();

        _groundInventorySlotCapacity = 81;
        _groundInventoryStackSize = 99;
        _defaultStackSize = 10;

        IsInnerInventoryReady = false;
    }

    public void Initialize()
    {
        _groundInventory = new Inventory(_groundInventorySlotCapacity, _groundInventoryStackSize);
        _playerInventory = new Inventory(_playerStatController.Stats.InventorySize, _defaultStackSize, _groundInventory);
        _innerInventory = new Inventory(1, _defaultStackSize, _groundInventory);

        _playerStatController.PlayerStatChangeEvent += OnPlayerStatChange;
        _groundInventory.InventoryUpdatedEvent += UpdateGroundInventory;
        _innerInventory.InventoryUpdatedEvent -= UpdateInnerInventory;
    }

    public void SetupInnerInventory()
    {
        // set inventory start
        var innerInventory = _testInnerInventory;
        // set inventory end

        _innerInventory.CopyFromOtherInventory(innerInventory);

        IsInnerInventoryReady = true;
    }

    private void UpdateInnerInventory() 
    { 
        // return inventory start
        _testInnerInventory.CopyFromOtherInventory(_innerInventory);
        // return inventory end
    }

    public void OffInnerInventory()
    {
        IsInnerInventoryReady = false;
    }

    public void SetupGroundInventory()
    {
        // set inventory start

        var groundInventory = _groundDetectionController.GetInventory();
        // set inventory end

        _groundInventory.CopyFromOtherInventory(groundInventory);
    }

    private void UpdateGroundInventory()
    {
        // return inventory start
        // _testGroundInventory.CopyFromOtherInventory(_groundInventory);
        _groundDetectionController.UpdateInventory(_groundInventory);
        // return inventory end
    }

    private void OnPlayerStatChange()
    {
        if(_playerStatController.Stats.InventorySize != _playerInventory.SlotNumber)
        {
            _playerInventory.ChangeInventorySize(_playerStatController.Stats.InventorySize);
        }
    }
}
