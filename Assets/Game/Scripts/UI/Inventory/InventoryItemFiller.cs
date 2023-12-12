using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemFiller : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var otherTransform = eventData.pointerDrag.transform;
        otherTransform.SetParent(transform);
        otherTransform.localPosition = Vector3.zero;
    }
}
