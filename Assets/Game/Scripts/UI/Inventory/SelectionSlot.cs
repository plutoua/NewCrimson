using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectionSlot : MonoBehaviour, IPointerDownHandler
{
    public event UnityAction<UIInventorySlot> OnMouseClickEvent
    {
        add { _onMouseClickEvent += value; }
        remove { _onMouseClickEvent -= value; }
    }

    private event UnityAction<UIInventorySlot> _onMouseClickEvent;

    private UIInventorySlot _uiInventorySlot;
    private void Start()
    {
        _uiInventorySlot = GetComponent<UIInventorySlot>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _onMouseClickEvent?.Invoke(_uiInventorySlot);
    }
}
