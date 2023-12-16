using UnityEngine;

public class UIInventorySlot : MonoBehaviour
{
    [SerializeField] private UIInventoryItem _inventoryItem;

    public InventorySlot InventorySlot => _inventorySlot;

    private InventorySlot _inventorySlot;
    private bool _isEmpty;

    public void Deactivate()
    {
        _inventorySlot = null;
        _isEmpty = true;
        if (_inventoryItem != null)
        {
            _inventoryItem.Deactivate();
        }
        gameObject.SetActive(false);
    }

    private void Start()
    {
        _inventoryItem.SetSlotLink(this);
    }

    public void SetSlot(InventorySlot slot)
    {
        gameObject.SetActive(true);
        _inventorySlot = slot;

        if(!slot.IsEmpty) 
        {
            _isEmpty = false;
            ActivateItem(_inventorySlot.Item);
        }
        else
        {
            _isEmpty = true;
        }
    }

    public void UpdateSlot()
    {
        if(_inventorySlot == null)
        {
            return;
        }

        if(!_isEmpty && !_inventorySlot.IsEmpty)
        {
            _inventoryItem.UpdateItem();
        }
        else if(_isEmpty && !_inventorySlot.IsEmpty) 
        {
            _isEmpty = false;
            ActivateItem(_inventorySlot.Item);
        }
        else if (!_isEmpty && _inventorySlot.IsEmpty)
        {
            _isEmpty = true;
            DeactivateItem();
        }
    }

    private void ActivateItem(InventoryItem item)
    {
        _inventoryItem.Activate(item);
    }

    private void DeactivateItem()
    {
        _inventoryItem.Deactivate();
    }
}
