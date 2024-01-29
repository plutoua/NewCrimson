using TimmyFramework;

public class InventoryController : IController, IOnCreate
{
    public Inventory PlayerInventory => _playerInventory;
    public Inventory GroundInventory => _groundInventory;
    public Inventory InnerInventory => _innerInventory;
    public Inventory SellInventory => _sellInventory;
    public int GroundSlotCapacity => _groundInventorySlotCapacity;

    public bool IsInnerInventoryReady {  get; private set; }

    private PlayerStatController _playerStatController;
    private GroundDetectionController _groundDetectionController;
    public int GroundInventoryStackSize => _groundInventoryStackSize;

    private Inventory _playerInventory;
    private Inventory _groundInventory;
    private Inventory _innerInventory;
    private Inventory _sellInventory;

    private int _groundInventorySlotCapacity;
    private int _groundInventoryStackSize;
    private int _defaultStackSize;
    private int _sellInventorySize;

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
        _sellInventorySize = 70;

        IsInnerInventoryReady = false;
    }

    public void Initialize()
    {
        _groundInventory = new Inventory(_groundInventorySlotCapacity, _groundInventoryStackSize);
        _playerInventory = new Inventory(_playerStatController.GetStat(Stat.InventorySize), _defaultStackSize, _groundInventory);
        _innerInventory = new Inventory(1, _defaultStackSize, _groundInventory);
        _sellInventory = new Inventory(_sellInventorySize, _groundInventoryStackSize);

        _playerStatController.PlayerStatChangeEvent += OnPlayerStatChange;
        _groundInventory.InventoryUpdatedEvent += UpdateGroundInventory;
        _innerInventory.InventoryUpdatedEvent -= UpdateInnerInventory;
    }

    public void SetupInnerInventory()
    {
        var innerInventory = _testInnerInventory;

        _innerInventory.CopyFromOtherInventory(innerInventory);

        IsInnerInventoryReady = true;
    }

    private void UpdateInnerInventory() 
    { 
        _testInnerInventory.CopyFromOtherInventory(_innerInventory);
    }

    public void OffInnerInventory()
    {
        IsInnerInventoryReady = false;
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
