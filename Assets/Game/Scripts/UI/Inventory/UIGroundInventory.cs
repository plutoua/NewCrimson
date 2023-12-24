public class UIGroundInventory : UIInventoryBase
{
    protected override void SetInventory(InventoryController inventoryController)
    {
        _inventory = inventoryController.GroundInventory;
    }

    protected override void SetName()
    {
        _moveableWindow.SetName("Ground");
    }

    protected override void SetPosition(float width, float height)
    {
        var canvasSizes = _canvasScaler.referenceResolution;

        _moveableWindow.SetPosition((canvasSizes.x - width)/2, height + 50);
    }
}
