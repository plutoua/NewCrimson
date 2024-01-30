using TimmyFramework;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemFiller : MonoBehaviour, IDropHandler
{
    private UIInventorySlot _uIInventorySlot;
    private UIWindowsController _windowsController;

    private void Start()
    {
        _uIInventorySlot = GetComponent<UIInventorySlot>();

        if (Game.IsReady)
        {
            SetupWindowController();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void SetupWindowController()
    {
        _windowsController = Game.GetController<UIWindowsController>();
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        SetupWindowController();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag.gameObject.TryGetComponent(out UIInventoryItem inventoryItem))
        {
            var toSlot = _uIInventorySlot.InventorySlot;
            var fromSlot = inventoryItem.UIInventorySlot.InventorySlot;        
            _windowsController.Slider.Activate(fromSlot, toSlot);  

            if(_windowsController.ItemOnMove != null)
            {
                _windowsController.ItemOnMove.MakeEndDrag();
            }
        }
        else if(eventData.pointerDrag.gameObject.TryGetComponent(out UIEquipmentItem equipmentItem))
        {
            _uIInventorySlot.InventorySlot.AddToItemAndInventory(equipmentItem.uiEquipmentSlot.Item);
            equipmentItem.uiEquipmentSlot.RemoveItem();

            if (_windowsController.ItemOnMove != null)
            {
                _windowsController.ItemOnMove.MakeEndDrag();
            }
        }
    }
}
