using UnityEngine;

[CreateAssetMenu(fileName = "NewViewFieldScheme", menuName = "ScriptableObjects/New ViewFieldScheme", order = 1)]
public class ViewFieldScheme : ScriptableObject
{

    [SerializeField] private bool player;
    [SerializeField] private int rays;
    [SerializeField] private float distance;
    [SerializeField] private float fov;

}
