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

    public int Strength => _selfStats.Strength + _tempStats.Strength;
    public int Agility => _selfStats.Agility + _tempStats.Agility;
    public int Intelligence => _selfStats.Intelligence + _tempStats.Intelligence;
    public int Luck => _selfStats.Luck + _tempStats.Luck;
    public int Endurance => _selfStats.Endurance + _tempStats.Endurance;
    public int Will => _selfStats.Will + _tempStats.Will;
    public int Charisma => _selfStats.Charisma + _tempStats.Charisma;
    public int Perception => _selfStats.Perception + _tempStats.Perception;
    public float MoveSpeed => _selfStats.MoveSpeed + _selfStats.MoveSpeed * _tempStats.MoveSpeed;
    public float AttackSpeed => _selfStats.AttackSpeed + _selfStats.AttackSpeed * _tempStats.AttackSpeed;
    public int InventorySize => _selfStats.InventorySize + _tempStats.InventorySize;
    public float ViewAngle => _selfStats.ViewAngle + _tempStats.ViewAngle;
    public float ViewLenght => _selfStats.ViewLenght + _tempStats.ViewLenght;

    private event UnityAction _statChangeEvent;
    private event UnityAction _experienceChangeEvent;
    private event UnityAction _levelUpEvent;
    private Stats _selfStats;
    private Stats _tempStats;

    public CharacterStatSystem()
    {
        Level = 1;
        Experience = 0;
        ExperienceBorder = LevelSystem.Experience[Level];
        StatsPoints = 0;

        _selfStats = new Stats();
        _tempStats = new Stats();
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

    public void AddStatByPoint(Stats stats)
    {
        if(StatsPoints > 0)
        {
            StatsPoints--;
            _selfStats += stats;
            _statChangeEvent?.Invoke();
        }
    }

    private void LevelUp()
    {
        Level++;
        ExperienceBorder = LevelSystem.Experience[Level];
        StatsPoints += 3;
        _levelUpEvent?.Invoke();
    }
}
