using UnityEngine;

public class UIEquipmentItem : MonoBehaviour
{
    public UIEquipmentSlot uiEquipmentSlot {  get; private set; }

    private void Start()
    {
        uiEquipmentSlot = GetComponentInParent<UIEquipmentSlot>();
    }
}
