using TimmyFramework;
using UnityEngine;

public class InventoryController : IController, IOnCreate
{
    public Inventory PlayerInventory => _playerInventory;
    public Inventory GroundInventory => _groundInventory;
    public Inventory InnerInventory => _innerInventory;
    public Inventory SellInventory => _sellInventory;
    public Inventory BuyInventory => _buyInventory;
    public int GroundSlotCapacity => _groundInventorySlotCapacity;
    public int GroundInventoryStackSize => _groundInventoryStackSize;
    public int DefaultStackSize => _defaultStackSize;

    private PlayerStatController _playerStatController;
    private GroundDetectionController _groundDetectionController;

    private Inventory _playerInventory;
    private Inventory _groundInventory;
    private Inventory _innerInventory;
    private Inventory _sellInventory;
    private Inventory _buyInventory;

    private Inventory _tempInnerInventory;

    private int _groundInventorySlotCapacity;
    private int _groundInventoryStackSize;
    private int _defaultStackSize;
    private int _sellInventorySize;
    private int _buyInventorySize;

    public void OnCreate()
    {
        _playerStatController = Game.GetController<PlayerStatController>();
        _groundDetectionController = Game.GetController<GroundDetectionController>();

        _groundInventorySlotCapacity = 81;
        _groundInventoryStackSize = 99;
        _defaultStackSize = 10;
        _sellInventorySize = 70;
        _buyInventorySize = 48;
    }

    public void Initialize()
    {
        _groundInventory = new Inventory(_groundInventorySlotCapacity, _groundInventoryStackSize);
        _playerInventory = new Inventory(_playerStatController.GetStat(Stat.InventorySize), _defaultStackSize, _groundInventory);
        _innerInventory = new Inventory(1, _defaultStackSize, _groundInventory);
        _sellInventory = new Inventory(_sellInventorySize, _groundInventoryStackSize);
        _buyInventory = new Inventory(_buyInventorySize, _groundInventoryStackSize);

        _playerStatController.PlayerStatChangeEvent += OnPlayerStatChange;
        _groundInventory.InventoryUpdatedEvent += UpdateGroundInventory;
        _innerInventory.InventoryUpdatedEvent += UpdateInnerInventory;
    }

    public void SetupInnerInventory(Inventory innerInventory)
    {
        _innerInventory.InventoryUpdatedEvent -= UpdateInnerInventory;
        _tempInnerInventory = innerInventory;

        _innerInventory.CopyFromOtherInventory(innerInventory);
        _innerInventory.InventoryUpdatedEvent += UpdateInnerInventory;
    }

    private void UpdateInnerInventory() 
    {
        _tempInnerInventory.CopyFromOtherInventory(_innerInventory);
    }

    public void SetupGroundInventory()
    {
        var groundInventory = _groundDetectionController.GetInventory();

        _groundInventory.CopyFromOtherInventory(groundInventory);
    }

    private void UpdateGroundInventory()
    {
        _groundDetectionController.UpdateInventory(_groundInventory);
    }

    private void OnPlayerStatChange()
    {
        if(_playerStatController.GetStat(Stat.InventorySize) != _playerInventory.SlotNumber)
        {
            _playerInventory.ChangeInventorySize(_playerStatController.GetStat(Stat.InventorySize));
        }
    }
}
