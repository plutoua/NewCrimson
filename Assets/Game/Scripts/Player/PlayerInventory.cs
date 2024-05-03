using TimmyFramework;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // Test only
    [SerializeField] private ItemScheme _testItemScheme;
    [SerializeField] private int _testItemAmount;
    [SerializeField] private ItemScheme _testItem1Scheme;
    [SerializeField] private int _testItem1Amount;
    [SerializeField] private ItemScheme _testItem2Scheme;
    [SerializeField] private int _testItem2Amount;

    private PlayerStatController _playerStatController;

    private Inventory _playerInventory;

    private void Start()
    {
        if (Game.IsReady)
        {
            ActionOnGameReady();
        }
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void ActionOnGameReady()
    {
        var inventoryController = Game.GetController<InventoryController>();
        _playerInventory = inventoryController.PlayerInventory;
        _playerInventory.Add(new InventoryItem(_testItem1Scheme, _testItem1Amount));
        _playerInventory.Add(new InventoryItem(_testItem2Scheme, _testItem2Amount));

        _playerStatController = Game.GetController<PlayerStatController>();
        var console = Game.GetController<ConsoleController>();
        console.AddCommand("addItem", ConsoleAddItem);
    }

    private void ConsoleAddItem(string itemName, string itemRank = "0", string itemNumber = "1")
    {
        InventoryItem tempItem;

        if (int.TryParse(itemNumber, out int _itemNumber))
        {
            tempItem = new InventoryItem(_testItemScheme, _itemNumber);
        }
        else
        {
            tempItem = new InventoryItem(_testItemScheme, 1);
        }

        if (int.TryParse(itemRank, out int _itemRank))
        {
            tempItem.SetStatsOfProgress(_itemRank);
        }

        _playerInventory.Add(tempItem);
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        ActionOnGameReady();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.O)) 
        {
            _playerInventory.Add(new InventoryItem(_testItemScheme, Random.Range(1, 15)));
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            _playerInventory.Remove(new InventoryItem(_testItemScheme, 4));
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            _playerInventory.ChangeInventorySize(_playerInventory.SlotNumber + 1);
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            _playerInventory.ChangeInventorySize(_playerInventory.SlotNumber - 1);
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            _playerStatController.AddExperience(500);
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            _playerStatController.ChangePlayerHealth(-10);
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            _playerStatController.ChangePlayerHealth(10);
        }
    }
}
