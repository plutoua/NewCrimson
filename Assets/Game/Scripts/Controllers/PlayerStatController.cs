using TimmyFramework;
using UnityEngine.Events;

public class PlayerStatController : IController, IOnAwake
{
    public event UnityAction PlayerStatChangeEvent 
    {
        add { Stats.StatChangeEvent += value; }
        remove { Stats.StatChangeEvent -= value; }
    }

    public event UnityAction PlayerExperienceChangeEvent
    {
        add { Stats.ExperienceChangeEvent += value; }
        remove { Stats.ExperienceChangeEvent -= value; }
    }

    public event UnityAction PlayerLevelUpEvent
    {
        add { Stats.LevelUpEvent += value; }
        remove { Stats.LevelUpEvent -= value; }
    }

    public int InventorySlotNumber => 10;

    public CharacterStatSystem Stats { get; private set; }

    public void OnAwake()
    {
        Stats = new CharacterStatSystem();
    }

    public void Initialize()
    {
        
    }

    public void AddStatsByPoint(Stats stats)
    {
        Stats.AddStatByPoint(stats);
    }
}
