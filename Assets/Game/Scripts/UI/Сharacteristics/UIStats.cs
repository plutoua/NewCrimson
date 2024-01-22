using TimmyFramework;
using TMPro;
using UnityEngine;

public class UIStats : MonoBehaviour
{
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
    }

    private void ChangeFreePointsText()
    {
        _freePointsText.text = "Free points: " + _playerStatController.Stats.StatsPoints;
    }
}
