using System;
using System.Collections.Generic;
using System.Linq;
using TimmyFramework;
using UnityEngine;

public class UIEquipment : MonoBehaviour, UIIWindow
{
    public event Action<InventoryItem[]> OnItemSetChangeEvent
    {
        add { _onItemSetChangeEvent += value; }
        remove { _onItemSetChangeEvent -= value; }
    }

    private event Action<InventoryItem[]> _onItemSetChangeEvent;

    private MoveableWindow _moveableWindow;
    private UIWindowsController _windowsController;

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
        _moveableWindow = GetComponentInParent<MoveableWindow>();
        _moveableWindow.CloseButtonEvent += OnCloseButton;

        _slots = GetComponentsInChildren<UIEquipmentSlot>();

        for(int i = 0; i < _slots.Length; i++)
        {
            _slots[i].OnEquipmentChangeEvent += OnEquipmentChange;
        }
    }

    private void SetLinks()
    {
        _playerStatController = Game.GetController<PlayerStatController>();

        _windowsController = Game.GetController<UIWindowsController>();
        _windowsController.SetEquipment(this);
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        SetLinks();
    }

    private void OnEquipmentChange()
    {
        var items = SelectItemsFromSlots();
        _onItemSetChangeEvent?.Invoke(items);
        var stats = SelectStatsFromItems(items);
        var statsPercent = SelectStatsPercentFromItems(items);
        var attackSchemes = GetAttackSchemes(items);
        _playerStatController.SetEquipmentStats(stats, statsPercent);
        _playerStatController.SetAttackSchemes(attackSchemes);
    }

    private InventoryItem[] SelectItemsFromSlots()
    {
        return _slots.Where(slot => slot.Item != null).Select(slot => slot.Item).ToArray();
    }

    private Dictionary<Stat, int> SelectStatsFromItems(InventoryItem[] items)
    {
        var result = new Dictionary<Stat, int>();
        
        Stat tempStat;
        int tempAmount;
        for (int i = 0; i < items.Length; i++)
        {
            for (int j = 0; j < items[i].Stats.Count; j++)
            {
                if (!items[i].Stats[j].IsPercent)
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
        }

        return result;
    }

    private Dictionary<Stat, int> SelectStatsPercentFromItems(InventoryItem[] items)
    {
        var result = new Dictionary<Stat, int>();

        Stat tempStat;
        int tempAmount;
        for (int i = 0; i < items.Length; i++)
        {
            for (int j = 0; j < items[i].Stats.Count; j++)
            {
                if (items[i].Stats[j].IsPercent)
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
        }

        return result;
    }

    private List<AttackScheme> GetAttackSchemes(InventoryItem[] items)
    {
        var result = new List<AttackScheme>();

        for (int i = 0; i < items.Length; i++)
        {
            result.AddRange(((EquipmentScheme)items[i].ItemScheme).AttackSchemes);
        }

        return result;
    }

    private void OnCloseButton()
    {
        _windowsController.CloseWindow(this);
    }

    public void Activate()
    {
        _moveableWindow.Activate();
    }

    public void Deactivate()
    {
        _moveableWindow.Deactivate();
    }
}
