using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInnerInventory : UIInventoryBase
{
    protected override void SetInventory(InventoryController inventoryController)
    {
        _inventory = inventoryController.InnerInventory;
        _windowsController.SetInnerInventory(this);
    }

    protected override void SetName()
    {
        var text = _languageStorage.Translation("inner_inventory_name");
        _moveableWindow.SetName(text);
    }

    protected override void SetPosition(float width, float height)
    {
        var canvasSizes = _canvasScaler.referenceResolution;

        _moveableWindow.SetPosition(0, canvasSizes.y);
    }
}
