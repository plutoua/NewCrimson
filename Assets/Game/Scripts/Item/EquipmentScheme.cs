using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipmentScheme", menuName = "ScriptableObjects/Items/New EquipmentScheme", order = 1)]
public class EquipmentScheme : ItemScheme
{
    [SerializeField] private EquipmentType _equipmentType;
    [SerializeField] private StatAmount[] _statsOld;
    [SerializeField] private StatAmount[] _statsPercentOld;
    [SerializeField] private StatProp[] _stats;
    [SerializeField] private AttackScheme[] _attackSchemes;

    public EquipmentType EquipmentType => _equipmentType;
    public StatAmount[] StatsOld => _statsOld;
    public StatAmount[] StatsPercentOld => _statsPercentOld;
    public StatProp[] Stats => _stats;
    public AttackScheme[] AttackSchemes => _attackSchemes;
}
