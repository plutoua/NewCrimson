using TimmyFramework;

public class PlayerStatController : IController, IOnAwake
{
    public int InventorySlotNumber;

    public void OnAwake()
    {
        InventorySlotNumber = 10;
    }

    public void Initialize()
    {
    
    }

    
}
