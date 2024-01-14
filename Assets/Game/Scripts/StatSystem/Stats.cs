using System;

[Serializable]
public class Stats
{
    public int Strength;
    public int Agility;
    public int Intelligence;
    public int Luck;
    public int Endurance;   
    public int Will;
    public int Charisma;
    public int Perception;
    public float MoveSpeed;
    public float AttackSpeed;
    public int InventorySize;
    public float ViewAngle;
    public float ViewLenght;

    public Stats()
    {
        Strength = 0;
        Agility = 0;
        Intelligence = 0;
        Luck = 0;
        Endurance = 0;
        Will = 0;
        Charisma = 0;
        Perception = 0;
        MoveSpeed = 0;
        AttackSpeed = 0;
        InventorySize = 0;
        ViewAngle = 0;
        ViewLenght = 0;
    }

    public static Stats operator +(Stats first, Stats second)
    {
        Stats result = new Stats();

        result.Strength = first.Strength + second.Strength;
        result.Agility = first.Agility + second.Agility;
        result.Intelligence = first.Intelligence + second.Intelligence;
        result.Luck = first.Luck + second.Luck;
        result.Endurance = first.Endurance + second.Endurance;
        result.Will = first.Will + second.Will;
        result.Charisma = first.Charisma + second.Charisma;
        result.Perception = first.Perception + second.Perception;
        result.MoveSpeed = first.MoveSpeed + second.MoveSpeed;
        result.AttackSpeed = first.AttackSpeed + second.AttackSpeed;
        result.InventorySize = first.InventorySize + second.InventorySize;
        result.ViewAngle = first.ViewAngle + second.ViewAngle;
        result.ViewLenght = first.ViewLenght + second.ViewLenght;

        return result;
    }

    public static Stats operator -(Stats first, Stats second)
    {
        Stats result = new Stats();

        result.Strength = first.Strength - second.Strength;
        result.Agility = first.Agility - second.Agility;
        result.Intelligence = first.Intelligence - second.Intelligence;
        result.Luck = first.Luck - second.Luck;
        result.Endurance = first.Endurance - second.Endurance;
        result.Will = first.Will - second.Will;
        result.Charisma = first.Charisma - second.Charisma;
        result.Perception = first.Perception - second.Perception;
        result.MoveSpeed = first.MoveSpeed - second.MoveSpeed;
        result.AttackSpeed = first.AttackSpeed - second.AttackSpeed;
        result.InventorySize = first.InventorySize - second.InventorySize;
        result.ViewAngle = first.ViewAngle - second.ViewAngle;
        result.ViewLenght = first.ViewLenght - second.ViewLenght;

        return result;
    }
}
