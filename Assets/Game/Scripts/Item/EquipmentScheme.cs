using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipmentScheme", menuName = "ScriptableObjects/Items/New EquipmentScheme", order = 1)]
public class EquipmentScheme : ItemScheme
{
    [SerializeField] private EquipmentType _equipmentType;
    [SerializeField] private StatAmount[] _stats;
    [SerializeField] private StatAmount[] _statsPercent;
    [SerializeField] private AttackScheme[] _attackSchemes;

    public EquipmentType EquipmentType => _equipmentType;
    public StatAmount[] Stats => _stats;
    public StatAmount[] StatsPercent => _statsPercent;
    public AttackScheme[] AttackSchemes => _attackSchemes;
}
