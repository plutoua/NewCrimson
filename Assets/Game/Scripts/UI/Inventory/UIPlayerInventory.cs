public class UIPlayerInventory : UIInventoryBase
{
    protected override void SetInventory(InventoryController inventoryController)
    {
        _inventory = inventoryController.PlayerInventory;
    }

    protected override void SetName()
    {
        _moveableWindow.SetName("Inventory");
    }

    protected override void SetPosition(float width, float height)
    {
        var canvasSizes = _canvasScaler.referenceResolution;

        _moveableWindow.SetPosition(canvasSizes.x - width, canvasSizes.y);
    }
}
