using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour
{
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TMP_Text _itemAmount;
    [SerializeField] private GameObject _itemNumberView;

    public UIInventorySlot UIInventorySlot { get; private set; }

    private InventoryItem _inventoryItem;

    public void Activate(InventoryItem inventoryItem)
    {
        gameObject.SetActive(true);
        _inventoryItem = inventoryItem;
        _itemAmount.text = _inventoryItem.Amount.ToString();
        _itemIcon.sprite = _inventoryItem.Sprite;
        if (_inventoryItem.Amount == 1)
        {
            _itemNumberView.SetActive(false);
        }
        else
        {
            _itemNumberView.SetActive(true);
        }
    }

    public void UpdateItem()
    {
        if(_inventoryItem.Amount == 1)
        {
            _itemNumberView.SetActive(false);
        }
        else
        {
            _itemNumberView.SetActive(true);
        }
        _itemAmount.text = _inventoryItem.Amount.ToString();
    }

    public void Deactivate()
    {
        _inventoryItem = null;
        _itemAmount.text = string.Empty; 
        _itemIcon.sprite = null;
        gameObject.SetActive(false);
    }

    public void SetSlotLink(UIInventorySlot uIInventorySlot)
    {
        UIInventorySlot = uIInventorySlot;
    }
}
