using System;
using System.Collections.Generic;
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
    private Dictionary<Stat, int> _tempStat;
    private Dictionary<Stat, int> _percentStat;
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
        _tempStat = new Dictionary<Stat, int>();
        _percentStat = new Dictionary<Stat, int>();
        _stats = new Dictionary<Stat, Func<int>>();

        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            _selfStat[stat] = 1;
            _tempStat[stat] = 0;
            _percentStat[stat] = 0;

            _stats[stat] = () => (int)(_selfStat[stat] * (_percentStat[stat] + 100f) / 100f + _tempStat[stat]);
        }
    }
}
