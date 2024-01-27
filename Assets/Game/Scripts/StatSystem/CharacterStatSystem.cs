using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine.Events;

public class CharacterStatSystem
{
    public event UnityAction StatChangeEvent
    {
        add { _statChangeEvent += value; }
        remove { _statChangeEvent -= value; }
    }

    public event UnityAction ExperienceChangeEvent
    {
        add { _experienceChangeEvent += value; }
        remove { _experienceChangeEvent -= value; }
    }

    public event UnityAction LevelUpEvent
    {
        add { _levelUpEvent += value; }
        remove { _levelUpEvent -= value; }
    }

    public int Level {  get; private set; }
    public int Experience { get; private set; }
    public int ExperienceBorder { get; private set; }
    public int StatsPoints { get; private set; }

    private event UnityAction _statChangeEvent;
    private event UnityAction _experienceChangeEvent;
    private event UnityAction _levelUpEvent;

    private Dictionary<Stat, int> _selfStat;
    private Dictionary<Stat, int> _buffStat; 
    private Dictionary<Stat, int> _buffPercentStat;
    private Dictionary<Stat, int> _equipmentStat;
    private Dictionary<Stat, int> _equipmentPercentStat;
    private Dictionary<Stat, Func<int>> _stats;

    public CharacterStatSystem()
    {
        Level = 1;
        Experience = 0;
        ExperienceBorder = LevelSystem.Experience[Level];
        StatsPoints = 0;

        SetupStat();
    }

    public void AddExperience(int value)
    {
        Experience += value;

        if(Experience >= ExperienceBorder)
        {
            LevelUp();
        }

        _experienceChangeEvent?.Invoke();
    }

    public void AddStatByPoint(Stat stat, int value)
    {
        if(StatsPoints > 0)
        {
            StatsPoints--;
            _selfStat[stat] += value;
            _statChangeEvent?.Invoke();
        }
    }

    public int GetStat(Stat stat)
    {
        return _stats[stat]();
    }

    public void SetEquipmentStats(Dictionary<Stat, int> stats, Dictionary<Stat, int> statsPercent)
    {
        MakeAllStatsZero(_equipmentStat);
        CopyStats(stats, _equipmentStat);

        MakeAllStatsZero(_equipmentPercentStat);
        CopyStats(statsPercent, _equipmentPercentStat);

        _statChangeEvent?.Invoke();
    }

    private void MakeAllStatsZero(Dictionary<Stat, int> stats)
    {
        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            if (stats.ContainsKey(stat))
            {
                stats[stat] = 0;
            }
        }
    }

    private void CopyStats(Dictionary<Stat, int> from, Dictionary<Stat, int> to)
    {
        var statKeys = from.Keys;
        foreach (var key in statKeys)
        {
            to[key] = from[key];
        }
    }

    private void LevelUp()
    {
        Level++;
        ExperienceBorder = LevelSystem.Experience[Level];
        StatsPoints += 3;
        _levelUpEvent?.Invoke();
    }

    private void SetupStat()
    {
        _selfStat = new Dictionary<Stat, int>();
        _buffStat = new Dictionary<Stat, int>();
        _buffPercentStat = new Dictionary<Stat, int>();
        _equipmentStat = new Dictionary<Stat, int>();
        _equipmentPercentStat = new Dictionary<Stat, int>();
        _stats = new Dictionary<Stat, Func<int>>();

        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            _selfStat[stat] = 3;
            _buffStat[stat] = 0;
            _buffPercentStat[stat] = 0;
            _equipmentStat[stat] = 0;
            _equipmentPercentStat[stat] = 0;

            _stats[stat] = () => (int)(_selfStat[stat] * (_buffPercentStat[stat] + _equipmentPercentStat[stat] + 100f) / 100f + _buffStat[stat] + _equipmentStat[stat]);
        }
    }
}
