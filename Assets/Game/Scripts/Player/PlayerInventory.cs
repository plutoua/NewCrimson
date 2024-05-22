using TimmyFramework;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    private PlayerStatController _playerStatController;
    private ItemStorage _itemStorage;

    private Inventory _playerInventory;
    private ConsoleController _consoleController;

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
        _playerStatController = Game.GetController<PlayerStatController>();
        _itemStorage = Game.GetStorage<ItemStorage>();
        _consoleController = Game.GetController<ConsoleController>();
        _consoleController.AddCommand("addItem", ConsoleAddItem);
    }

    private void ConsoleAddItem(string itemID, string itemRank = "0", string itemNumber = "1")
    {
        InventoryItem tempItem;
        ItemScheme itemScheme;

        if (int.TryParse(itemID, out int _itemID))
        {
            itemScheme = _itemStorage.GetItemByID(_itemID);
            if(itemScheme == null) 
            {
                return;
            }
        }
        else
        {
            return;
        }

        if (int.TryParse(itemNumber, out int _itemNumber))
        {
            tempItem = new InventoryItem(itemScheme, _itemNumber);
        }
        else
        {
            tempItem = new InventoryItem(itemScheme, 1);
        }

        if (int.TryParse(itemRank, out int _itemRank))
        {
            tempItem.SetStatsOfProgress(_itemRank);
        }

        var message = $"Item {tempItem.Name} (progress: {tempItem.Progress}, amount: {tempItem.Amount}) add to inventory.";

        _playerInventory.Add(tempItem);
        
        _consoleController.WriteToConsole(message);
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        ActionOnGameReady();
    }

    private void Update()
    {
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
