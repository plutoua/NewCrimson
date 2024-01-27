using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour
{
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TMP_Text _itemAmount;
    [SerializeField] private GameObject _itemNumberView;

    public UIInventorySlot UIInventorySlot { get; private set; }
    public InventoryItem InventoryItem { get; private set; }

    public void Activate(InventoryItem inventoryItem)
    {
        gameObject.SetActive(true);
        InventoryItem = inventoryItem;
        _itemAmount.text = InventoryItem.Amount.ToString();
        _itemIcon.sprite = InventoryItem.Sprite;
        if (InventoryItem.Amount == 1)
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
        if(InventoryItem.Amount == 1)
        {
            _itemNumberView.SetActive(false);
        }
        else
        {
            _itemNumberView.SetActive(true);
        }
        _itemAmount.text = InventoryItem.Amount.ToString();
    }

    public void Deactivate()
    {
        InventoryItem = null;
        _itemAmount.text = string.Empty; 
        _itemIcon.sprite = null;
        gameObject.SetActive(false);
    }

    public void SetSlotLink(UIInventorySlot uIInventorySlot)
    {
        UIInventorySlot = uIInventorySlot;
    }
}
