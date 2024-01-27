using TimmyFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevelInfoСharacteristics : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _experienceText;
    [SerializeField] private Slider _experienceBar;

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
        _playerStatController.PlayerExperienceChangeEvent += OnExperienceChange;

        OnLevelChange();
        OnExperienceChange();
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        InitialSetup();
    }

    private void OnLevelChange()
    {
        _levelText.text = "Level " + _playerStatController.PlayerLevel;
    }

    private void OnExperienceChange()
    {
        _experienceText.text = _playerStatController.PlayerExperience + " / " + _playerStatController.PlayerExperienceBorder;
        var relativeExperience = _playerStatController.PlayerExperience - LevelSystem.Experience[_playerStatController.PlayerLevel - 1];
        var relativeBorder = _playerStatController.PlayerExperienceBorder - LevelSystem.Experience[_playerStatController.PlayerLevel - 1];
        _experienceBar.value = relativeExperience / (float)relativeBorder;
    }
}
