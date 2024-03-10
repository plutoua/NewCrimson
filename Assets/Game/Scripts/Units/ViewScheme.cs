using UnityEngine;

[CreateAssetMenu(fileName = "NewViewScheme", menuName = "ScriptableObjects/ViewScheme/New ViewScheme", order = 1)]
public class ViewScheme : ScriptableObject
{
    [SerializeField] private Sprite _front;
    [SerializeField] private Sprite _back;
    [SerializeField] private Sprite _profile;

    public Sprite Front => _front;
    public Sprite Back => _back;
    public Sprite Profile => _profile;
}
