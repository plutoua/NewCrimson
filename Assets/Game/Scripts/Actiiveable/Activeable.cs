using TimmyFramework;
using UnityEngine;

public class Activeable : MonoBehaviour
{
    private InventoryController _inventoryController;

    private bool _active;

    private void Start()
    {
        _active = false;

        if (Game.IsReady)
        {
            _inventoryController = Game.GetController<InventoryController>();
        }
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        _inventoryController = Game.GetController<InventoryController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_active)
        {
            return;
        }

        if(collision.TryGetComponent(out ActivationZone activationZone)) 
        {
            _active = true;
            _inventoryController.SetupInnerInventory();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_active)
        {
            return;
        }

        if (collision.TryGetComponent(out ActivationZone activationZone))
        {
            _active = false;
            _inventoryController.OffInnerInventory();
        }
    }
}
