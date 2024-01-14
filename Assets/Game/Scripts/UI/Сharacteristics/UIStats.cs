using System.Collections;
using System.Collections.Generic;
using TimmyFramework;
using TMPro;
using UnityEngine;

public class UIStats : MonoBehaviour
{
    [SerializeField] private UIStatItem _strengthItem;
    [SerializeField] private UIStatItem _agilityItem;
    [SerializeField] private UIStatItem _intelligenceItem;
    [SerializeField] private UIStatItem _luckItem;
    [SerializeField] private UIStatItem _enduranceItem;
    [SerializeField] private UIStatItem _willItem;
    [SerializeField] private UIStatItem _charismaItem;
    [SerializeField] private UIStatItem _perceptionItem;

    [SerializeField] private TMP_Text _freePointsText;

    private PlayerStatController _playerStatController;

    private void Start()
    {
        if (Game.IsReady)
        {
            InitialSetup();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void InitialSetup()
    {
        _playerStatController = Game.GetController<PlayerStatController>();
        _playerStatController.PlayerLevelUpEvent += OnLevelChange;
        _playerStatController.PlayerStatChangeEvent += OnStatChange;

        OnLevelChange();
        OnStatChange();
        SetStatItems();
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        InitialSetup();
    }

    private void OnLevelChange()
    {
        ChangeFreePointsText();
    }

    private void OnStatChange()
    {
        ChangeFreePointsText();
        SetStatItems();
    }

    private void ChangeFreePointsText()
    {
        _freePointsText.text = "Free points: " + _playerStatController.Stats.StatsPoints;
    }

    private void SetStatItems()
    {
        _strengthItem.SetValue(_playerStatController.Stats.Strength);
        _agilityItem.SetValue(_playerStatController.Stats.Agility);
        _intelligenceItem.SetValue(_playerStatController.Stats.Intelligence);
        _luckItem.SetValue(_playerStatController.Stats.Luck);
        _enduranceItem.SetValue(_playerStatController.Stats.Endurance);
        _willItem.SetValue(_playerStatController.Stats.Will);
        _charismaItem.SetValue(_playerStatController.Stats.Charisma);
        _perceptionItem.SetValue(_playerStatController.Stats.Perception);
    }
}
