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

    public event UnityAction<int> PlayerHealthChangeEvent
    {
        add { _stats.HealthChangeEvent += value; }
        remove { _stats.HealthChangeEvent -= value; }
    }

    public event UnityAction<int> StaminaHealthChangeEvent
    {
        add { _stats.StaminaChangeEvent += value; }
        remove { _stats.StaminaChangeEvent -= value; }
    }

    public int PlayerLevel => _stats.Level;
    public int PlayerExperience => _stats.Experience;
    public int PlayerExperienceBorder => _stats.ExperienceBorder;
    public int PlayerStatsPoints => _stats.StatsPoints;
    public int Health => _stats.Health;
    public int Stamina => _stats.Stamina;

    private CharacterStatSystem _stats;

    public void OnAwake()
    {
        _stats = new CharacterStatSystem();
    }

    public void Initialize()
    {
        
    }

    public void ChangePlayerHealth(int value)
    {
        _stats.ChangeHealth(value);
    }

    public void ChangePlayerStamina(int value)
    {
        _stats.ChangeStamina(value);
    }

    public void AddStatsByPoint(Stat stat, int value)
    {
        _stats.AddStatByPoint(stat, value);
    }

    public int GetStat(Stat stat)
    {
        return _stats.GetStat(stat);
    }

    public int GetWhiteStat(Stat stat)
    {
        return _stats.GetWhiteStat(stat);
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
