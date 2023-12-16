using TimmyFramework;

public class InventoryController : IController, IOnCreate
{
    public Inventory PlayerInventory => _playerInventory;
    public Inventory GroundInventory => _groundInventory;
    public Inventory InnerInventory => _innerInventory;

    private PlayerStatController _playerStatController;

    private Inventory _playerInventory;
    private Inventory _groundInventory;
    private Inventory _innerInventory;

    private int _groundInventorySlotCapacity;
    private int _groundInventoryStackSize;
    private int _defaultStackSize;

    public void OnCreate()
    {
        _playerStatController = Game.GetController<PlayerStatController>();

        _groundInventorySlotCapacity = 51;
        _groundInventoryStackSize = 99;
        _defaultStackSize = 10;
    }

    public void Initialize()
    {
        _groundInventory = new Inventory(_groundInventorySlotCapacity, _groundInventoryStackSize);
        _playerInventory = new Inventory(_playerStatController.InventorySlotNumber, _defaultStackSize, _groundInventory);
        _innerInventory = new Inventory(1, _defaultStackSize, _groundInventory); 
    }


}
