using TimmyFramework;
using UnityEngine;

[RequireComponent(typeof(Activeable))]
public class ActiveableInventory : MonoBehaviour, IActivable
{
    // TODO remove this
    [SerializeField] private ItemScheme[] _items;
    [SerializeField] private int[] _itemsNumber;

    private Inventory _inventory;

    private UIWindowsController _uiWindowsController;
    private InventoryController _inventoryController;

    private void Start()
    {
        if (Game.IsReady)
        {
            InitialSetup();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }

        GetComponent<Activeable>().SetActiveable(this);
        
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        InitialSetup();
        SetupInventory();
    }

    private void InitialSetup()
    {
        _uiWindowsController = Game.GetController<UIWindowsController>();
        _inventoryController = Game.GetController<InventoryController>();
    }

    private void SetupInventory()
    {
        var inventorySize = _items.Length <= _itemsNumber.Length ? _items.Length : _itemsNumber.Length;

        _inventory = new Inventory(inventorySize, _inventoryController.DefaultStackSize);

        for (int i = 0; i < inventorySize; i++)
        {
            _inventory.Add(new InventoryItem(_items[i], _itemsNumber[i]));
        }
    }

    public void Activate()  
    {
        _inventoryController.SetupInnerInventory(_inventory);
        _uiWindowsController.OpenInnerInventory();
    }
}
