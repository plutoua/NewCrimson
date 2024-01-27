using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEquipmentSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private EquipmentType _equipmentType;
    [SerializeField] private Image _image;

    public event UnityAction OnEquipmentChangeEvent
    {
        add { _onEquipmentChangeEvent += value; }
        remove { _onEquipmentChangeEvent -= value;}
    }
    public InventoryItem Item { get; private set; }

    private event UnityAction _onEquipmentChangeEvent;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.gameObject.TryGetComponent(out UIInventoryItem uiInventoryItem))
        {
            if (uiInventoryItem.InventoryItem.ItemScheme is EquipmentScheme)
            {
                var equipment = (EquipmentScheme)uiInventoryItem.InventoryItem.ItemScheme;
                if(_equipmentType == equipment.EquipmentType)
                {
                    SetItem(uiInventoryItem.InventoryItem);
                    uiInventoryItem.UIInventorySlot.InventorySlot.RemoveFromItemAndInventory(uiInventoryItem.InventoryItem);
                }
            }
        }
    }

    public void RemoveItem()
    {
        Item = null;
        _image.gameObject.SetActive(false);
        _onEquipmentChangeEvent?.Invoke();
    }

    private void Start()
    {
        Item = null;
    }

    private void SetItem(InventoryItem item)
    {
        Item = item;
        _image.gameObject.SetActive(true);
        _image.sprite = Item.Sprite;
        _onEquipmentChangeEvent?.Invoke();
    }
}
