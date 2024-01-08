public class Stats
{
    public int Strength {  get; set; }
    public int Agility { get; set; }
    public int Intelligence { get; set; }
    public int Luck { get; set; }
    public int Endurance { get; set; }
    public int Will { get; set; }
    public int Charisma { get; set; }
    public int Perception { get; set; }
    public float MoveSpeed { get; set; }
    public float AttackSpeed { get; set; }
    public int InventorySize { get; set; }
    public float ViewAngle {  get; set; }
    public float ViewLenght {  get; set; }

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
}
