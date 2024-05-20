using UnityEngine;

[CreateAssetMenu(fileName = "NewItemScheme", menuName = "ScriptableObjects/Items/New ItemScheme", order = 2)]
public class ItemScheme : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private Sprite _spriteIcon;
    [SerializeField] private string _name;
    [SerializeField] private int _maxItemsInStack;
    [SerializeField] private int _stackMultiplier;
    [SerializeField] private int _price;

    public int ID => _id;
    public Sprite Sprite => _spriteIcon;
    public string Name => _name;
    public int MaxItemInStack => _maxItemsInStack;
    public int StackMultiplier => _stackMultiplier;
    public int Price => _price;
}
