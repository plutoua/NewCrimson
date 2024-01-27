using System.Collections.Generic;
using System.Linq;
using TimmyFramework;
using UnityEngine;

public class UIEquipment : MonoBehaviour
{
    private UIEquipmentSlot[] _slots;
    private PlayerStatController _playerStatController;

    private void Start()
    {
        InitialSetup();

        if (Game.IsReady)
        {
            SetLinks();
        }
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void InitialSetup()
    {
        _slots = GetComponentsInChildren<UIEquipmentSlot>();

        for(int i = 0; i < _slots.Length; i++)
        {
            _slots[i].OnEquipmentChangeEvent += OnEquipmentChange;
        }
    }

    private void SetLinks()
    {
        _playerStatController = Game.GetController<PlayerStatController>();
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        SetLinks();
    }

    private void OnEquipmentChange()
    {
        var items = SelectItemsFromSlots();
        var stats = SelectStatsFromItems(items);
        var statsPercent = SelectStatsPercentFromItems(items);
        var attackSchemes = GetAttackSchemes(items);
        _playerStatController.SetEquipmentStats(stats, statsPercent);
        _playerStatController.SetAttackSchemes(attackSchemes);
    }

    private EquipmentScheme[] SelectItemsFromSlots()
    {
        return _slots.Where(slot => slot.Item != null).Select(slot => (EquipmentScheme)slot.Item.ItemScheme).ToArray();
    }

    private Dictionary<Stat, int> SelectStatsFromItems(EquipmentScheme[] items)
    {
        var result = new Dictionary<Stat, int>();

        Stat tempStat;
        int tempAmount;
        for (int i = 0; i < items.Length; i++)
        {
            for (int j = 0; j < items[i].Stats.Length; j++)
            {
                tempStat = items[i].Stats[j].StatType;
                tempAmount = items[i].Stats[j].Amount;
                if (result.ContainsKey(tempStat))
                {
                    result[tempStat] += tempAmount;
                }
                else
                {
                    result[tempStat] = tempAmount;
                }
            }
        }

        return result;
    }

    private Dictionary<Stat, int> SelectStatsPercentFromItems(EquipmentScheme[] items)
    {
        var result = new Dictionary<Stat, int>();

        Stat tempStat;
        int tempAmount;
        for (int i = 0; i < items.Length; i++)
        {
            for (int j = 0; j < items[i].StatsPercent.Length; j++)
            {
                tempStat = items[i].StatsPercent[j].StatType;
                tempAmount = items[i].StatsPercent[j].Amount;
                if (result.ContainsKey(tempStat))
                {
                    result[tempStat] += tempAmount;
                }
                else
                {
                    result[tempStat] = tempAmount;
                }
            }
        }

        return result;
    }

    private List<AttackScheme> GetAttackSchemes(EquipmentScheme[] items)
    {
        var result = new List<AttackScheme>();

        for (int i = 0; i < items.Length; i++)
        {
            result.AddRange(items[i].AttackSchemes);
        }

        return result;
    }
}
