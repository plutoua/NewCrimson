using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewActionScheme", menuName = "ScriptableObjects/New ActionScheme", order = 1)]
public class ActionScheme : ScriptableObject
{
    [SerializeField] private int _type;
    [SerializeField] private float _actionDelay;
}
