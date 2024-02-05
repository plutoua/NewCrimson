using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipItemHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UIInventorySlot _inventorySlot;

    private void Start()
    {
        _inventorySlot = GetComponent<UIInventorySlot>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_inventorySlot.InventorySlot.IsEmpty)
        {
            UITooltipIItem.Show(_inventorySlot.InventorySlot.Item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UITooltipIItem.Hide();
    }
}
