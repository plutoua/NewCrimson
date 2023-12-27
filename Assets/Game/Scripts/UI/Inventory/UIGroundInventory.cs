public class UIGroundInventory : UIInventoryBase
{
    protected override void SetInventory(InventoryController inventoryController)
    {
        _inventory = inventoryController.GroundInventory;
        _windowsController.SetGroundInventory(this);
    }

    protected override void SetName()
    {
        var text = _languageStorage.Translation("ground_inventory_name");
        _moveableWindow.SetName(text);
    }

    protected override void SetPosition(float width, float height)
    {
        var canvasSizes = _canvasScaler.referenceResolution;

        _moveableWindow.SetPosition((canvasSizes.x - width)/2, height + 50);
    }
}
