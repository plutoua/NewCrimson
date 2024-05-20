using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipmentScheme", menuName = "ScriptableObjects/Items/New EquipmentScheme", order = 1)]
public class EquipmentScheme : ItemScheme
{
    [SerializeField] private EquipmentType _equipmentType;
    [SerializeField] private StatProp[] _stats;
    [SerializeField] private AttackScheme[] _attackSchemes;
    [SerializeField] private ViewScheme _viewScheme;

    public EquipmentType EquipmentType => _equipmentType;
    public StatProp[] Stats => _stats;
    public AttackScheme[] AttackSchemes => _attackSchemes;
    public ViewScheme ViewScheme => _viewScheme;
}
