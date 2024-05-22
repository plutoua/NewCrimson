using TimmyFramework;
using UnityEngine;

public class PlayerEquipmentView : MonoBehaviour
{
    private UnitView _unitView;

    private UIWindowsController _uiWindowsController;

    private void Start()
    {
        _unitView = GetComponentInChildren<UnitView>();

        if (Game.IsReady)
        {
            WhenGameReady();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void WhenGameReady()
    {
        _uiWindowsController = Game.GetController<UIWindowsController>();

        if(_uiWindowsController.Equipment != null)
        {
            _uiWindowsController.Equipment.OnItemSetChangeEvent += SetEquipment;
        }
        else
        {
            _uiWindowsController.OnEqupmentReadyEvent += OnEquipmentReady;
        }
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        WhenGameReady();
    }

    private void OnEquipmentReady(UIEquipment uIEquipment)
    {
        _uiWindowsController.OnEqupmentReadyEvent -= OnEquipmentReady;
        uIEquipment.OnItemSetChangeEvent += SetEquipment;
    }

    private void SetEquipment(InventoryItem[] items)
    {
        ViewScheme head = null;
        ViewScheme body = null;

        EquipmentScheme tempEquipment;
        foreach (InventoryItem item in items)
        {
            tempEquipment = (EquipmentScheme) item.ItemScheme;

            if (tempEquipment.EquipmentType == EquipmentType.Helmet)
            {
                head = tempEquipment.ViewScheme;
            }
            if(tempEquipment.EquipmentType == EquipmentType.Armor)
            {
                
                body = tempEquipment.ViewScheme;
            }
        }
        _unitView.SetEquipment(head, body);
    }
}
