using System.Collections.Generic;
using TimmyFramework;
using UnityEngine.Events;

public class PlayerStatController : IController, IOnAwake
{
    public event UnityAction PlayerStatChangeEvent 
    {
        add { _stats.StatChangeEvent += value; }
        remove { _stats.StatChangeEvent -= value; }
    }

    public event UnityAction PlayerExperienceChangeEvent
    {
        add { _stats.ExperienceChangeEvent += value; }
        remove { _stats.ExperienceChangeEvent -= value; }
    }

    public event UnityAction PlayerLevelUpEvent
    {
        add { _stats.LevelUpEvent += value; }
        remove { _stats.LevelUpEvent -= value; }
    }

    public int PlayerLevel => _stats.Level;
    public int PlayerExperience => _stats.Experience;
    public int PlayerExperienceBorder => _stats.ExperienceBorder;
    public int PlayerStatsPoints => _stats.StatsPoints;

    private CharacterStatSystem _stats;

    public void OnAwake()
    {
        _stats = new CharacterStatSystem();
    }

    public void Initialize()
    {
        
    }

    public void AddStatsByPoint(Stat stat, int value)
    {
        _stats.AddStatByPoint(stat, value);
    }

    public int GetStat(Stat stat)
    {
        return _stats.GetStat(stat);
    }

    public void AddExperience(int value)
    {
        _stats.AddExperience(value);
    }

    public void SetEquipmentStats(Dictionary<Stat, int> stats, Dictionary<Stat, int> statsPercent)
    {
        _stats.SetEquipmentStats(stats, statsPercent);
    }

    public void SetAttackSchemes(List<AttackScheme> attackSchemes)
    {

    }
}
