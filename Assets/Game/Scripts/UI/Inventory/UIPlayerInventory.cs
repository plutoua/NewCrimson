public class UIPlayerInventory : UIInventoryBase
{
    protected override void SetInventory(InventoryController inventoryController)
    {
        _inventory = inventoryController.PlayerInventory;
        _windowsController.SetPlayerInventory(this);
    }

    protected override void SetName()
    {
        var text = _languageStorage.Translation("player_inventory_name");
        _moveableWindow.SetName(text);
    }

    protected override void SetPosition(float width, float height)
    {
        var canvasSizes = _canvasScaler.referenceResolution;

        _moveableWindow.SetPosition(canvasSizes.x - width, canvasSizes.y);
    }
}
