using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryItem
{
    public int ID => _itemScheme.ID;
    public string Name => _itemScheme.Name;
    public Sprite Sprite => _itemScheme.Sprite;
    public int MaxItemInStack => _itemScheme.MaxItemInStack;
    public int Price => _itemScheme.Price + _statsPrice;
    public int Multiplier => _itemScheme.StackMultiplier;
    public int Progress => _progress;
    public int Amount { get; private set; }
    public ItemScheme ItemScheme => _itemScheme;
    public List<StatAmount> Stats => _stats;

    protected ItemScheme _itemScheme;
    protected int _progress;
    protected int _statsPrice;
    protected List<StatAmount> _stats;

    public InventoryItem(ItemScheme itemScheme, int amount)
    {
        _itemScheme = itemScheme;
        Amount = amount;
        _statsPrice = 0;
        _progress = 0;
        _stats = new List<StatAmount>();
    }

    public InventoryItem(ItemScheme itemScheme, int amount, List<StatAmount> stats)
    {
        _itemScheme = itemScheme;
        Amount = amount;
        _statsPrice = 0;
        _progress = 0;
        _stats = stats;
    }

    public void SetAmount(int amount)
    {
        if(amount > MaxItemInStack * Multiplier)
        {
            throw new ArgumentOutOfRangeException("Item amount is more then MAX value.");
        }

        Amount = amount;
    }

    public InventoryItem CloneItemWithAmount(int amount)
    {
        return new InventoryItem(_itemScheme, amount, _stats);
    }

    public ItemScheme GetItemScheme()
    {
        return _itemScheme;
    }

    public void SetStatsOfProgress(int progressValue)
    {
        if(_itemScheme is EquipmentScheme)
        {
            _stats.Clear();
            _statsPrice = 0;
            _progress = progressValue;
            var statFromScheme = ((EquipmentScheme)_itemScheme).Stats;
            var statCount = statFromScheme.Length > 5 ? 5 : statFromScheme.Length;

            List<int> statNumbers = new List<int>();
            int tempInt;
            System.Random random = new System.Random();

            while(statCount > statNumbers.Count) 
            {
                tempInt = random.Next(0, statFromScheme.Length);
                if (!statNumbers.Contains(tempInt))
                {
                    statNumbers.Add(tempInt);
                }
            }

            foreach( var number in statNumbers)
            {
                StatAmount statAmount = new StatAmount();
                statAmount.StatType = statFromScheme[number].Stat;
                statAmount.Amount = 0;
                statAmount.IsPercent = statFromScheme[number].IsPercent;
                _stats.Add(statAmount);
            }

            int tempSum;
            StatProp tempProp;
            while(progressValue > 0)
            {
                tempInt = random.Next(0, statCount);
                tempProp = statFromScheme[statNumbers[tempInt]];
                tempSum = _stats[tempInt].Amount + tempProp.Value;

                if(tempSum <= tempProp.MaxValue)
                {
                    _stats[tempInt].Amount = tempSum;
                    progressValue -= tempProp.PointCost;
                    _statsPrice += tempProp.CoinCost;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
