using System;
using System.Collections;
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
            _selfStat[stat] = 0;
            _buffStat[stat] = 0;
            _buffPercentStat[stat] = 0;
            _equipmentStat[stat] = 0;
            _equipmentPercentStat[stat] = 0;

        }

        _selfStat[Stat.MoveSpeed] = 40;
        _selfStat[Stat.InventorySize] = 4;
        _selfStat[Stat.AttackDelay] = 21;


        

        _stats[Stat.ViewAngle] = () => (int)(_selfStat[Stat.ViewAngle] * (_buffPercentStat[Stat.ViewAngle] + _equipmentPercentStat[Stat.ViewAngle] + 100f) / 100f + _buffStat[Stat.ViewAngle] + _equipmentStat[Stat.ViewAngle]);

        _stats[Stat.ViewLenght] = () => (int)(_selfStat[Stat.ViewLenght] * (_buffPercentStat[Stat.ViewLenght] + _equipmentPercentStat[Stat.ViewLenght] + 100f) / 100f + _buffStat[Stat.ViewLenght] + _equipmentStat[Stat.ViewLenght]);

        _stats[Stat.Health] = () => (int)(_selfStat[Stat.Health] * (_buffPercentStat[Stat.Health] + _equipmentPercentStat[Stat.Health] + 100f) / 100f + _buffStat[Stat.Health] + _equipmentStat[Stat.Health]);

        _stats[Stat.Stamina] = () => (int)(_selfStat[Stat.Stamina] * (_buffPercentStat[Stat.Stamina] + _equipmentPercentStat[Stat.Stamina] + 100f) / 100f + _buffStat[Stat.Stamina] + _equipmentStat[Stat.Stamina]);





        _stats[Stat.Strength] = () => (int)(_selfStat[Stat.Strength] * (_buffPercentStat[Stat.Strength] + _equipmentPercentStat[Stat.Strength] + 100f) / 100f + _buffStat[Stat.Strength] + _equipmentStat[Stat.Strength]);
        _stats[Stat.Agility] = () => (int)(_selfStat[Stat.Agility] * (_buffPercentStat[Stat.Agility] + _equipmentPercentStat[Stat.Agility] + 100f) / 100f + _buffStat[Stat.Agility] + _equipmentStat[Stat.Agility]);
        _stats[Stat.Intelligence] = () => (int)(_selfStat[Stat.Intelligence] * (_buffPercentStat[Stat.Intelligence] + _equipmentPercentStat[Stat.Intelligence] + 100f) / 100f + _buffStat[Stat.Intelligence] + _equipmentStat[Stat.Intelligence]);
        _stats[Stat.Luck] = () => (int)(_selfStat[Stat.Luck] * (_buffPercentStat[Stat.Luck] + _equipmentPercentStat[Stat.Luck] + 100f) / 100f + _buffStat[Stat.Luck] + _equipmentStat[Stat.Luck]);
        _stats[Stat.Endurance] = () => (int)(_selfStat[Stat.Endurance] * (_buffPercentStat[Stat.Endurance] + _equipmentPercentStat[Stat.Endurance] + 100f) / 100f + _buffStat[Stat.Endurance] + _equipmentStat[Stat.Endurance]);
        _stats[Stat.Will] = () => (int)(_selfStat[Stat.Will] * (_buffPercentStat[Stat.Will] + _equipmentPercentStat[Stat.Will] + 100f) / 100f + _buffStat[Stat.Will] + _equipmentStat[Stat.Will]);
        _stats[Stat.Charisma] = () => (int)(_selfStat[Stat.Charisma] * (_buffPercentStat[Stat.Charisma] + _equipmentPercentStat[Stat.Charisma] + 100f) / 100f + _buffStat[Stat.Charisma] + _equipmentStat[Stat.Charisma]);
        _stats[Stat.Perception] = () => (int)(_selfStat[Stat.Perception] * (_buffPercentStat[Stat.Perception] + _equipmentPercentStat[Stat.Perception] + 100f) / 100f + _buffStat[Stat.Perception] + _equipmentStat[Stat.Perception]);
        _stats[Stat.MoveSpeed] = () => (int)(_selfStat[Stat.MoveSpeed] * (_buffPercentStat[Stat.MoveSpeed] + _equipmentPercentStat[Stat.MoveSpeed] + 100f) / 100f + _buffStat[Stat.MoveSpeed] + _equipmentStat[Stat.MoveSpeed]) 
            + 2 * _stats[Stat.Agility]();

        _stats[Stat.InventorySize] = () => (int)(_selfStat[Stat.InventorySize] * (_buffPercentStat[Stat.InventorySize] + _equipmentPercentStat[Stat.InventorySize] + 100f) / 100f + _buffStat[Stat.InventorySize] + _equipmentStat[Stat.InventorySize] 
            + _stats[Stat.Strength]() + _stats[Stat.Intelligence]() + _stats[Stat.Endurance]());

        _stats[Stat.AttackDelay] = () => (int)(_selfStat[Stat.AttackDelay] * (_buffPercentStat[Stat.AttackDelay] + _equipmentPercentStat[Stat.AttackDelay] + 100f) / 100f + _buffStat[Stat.AttackDelay] + _equipmentStat[Stat.AttackDelay] +
            - _stats[Stat.Agility]());





    }
}
