using UnityEngine;

[CreateAssetMenu(fileName = "NewItemScheme", menuName = "ScriptableObjects/New ItemScheme", order = 2)]
public class ItemScheme : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _name;
    [SerializeField, Range(1, 100)] private int _maxItemsInStack;

    public int ID => _id;
    public Sprite Sprite => _sprite;
    public string Name => _name;
    public int MaxItemInStack => _maxItemsInStack;
}
