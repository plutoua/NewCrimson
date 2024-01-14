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
            AddItemToInventory();
        }
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void AddItemToInventory()
    {
        var inventoryController = Game.GetController<InventoryController>();
        _playerInventory = inventoryController.PlayerInventory;
        _playerInventory.Add(new InventoryItem(_testItemScheme, _testItemAmount));
        _playerInventory.Add(new InventoryItem(_testItem1Scheme, _testItem1Amount));
        _playerInventory.Add(new InventoryItem(_testItem2Scheme, _testItem2Amount));

        _playerStatController = Game.GetController<PlayerStatController>();
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        AddItemToInventory();
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
            _playerStatController.Stats.AddExperience(10);
        }
    }
}
