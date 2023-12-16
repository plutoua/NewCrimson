using System.Collections;
using System.Collections.Generic;
using TimmyFramework;
using UnityEngine;
using UnityEngine.UI;

public class UISlider : MonoBehaviour
{
    private UISliderStayValue _stayValue;
    private UISliderSendValue _sendValue;
    private Slider _slider;
    private MoveableWindow _moveableWindow;

    private UIWindowsController _windowsController;

    private void Start()
    {
        _moveableWindow = GetComponentInParent<MoveableWindow>();
        _stayValue = GetComponentInChildren<UISliderStayValue>();
        _sendValue = GetComponentInChildren<UISliderSendValue>();

        _slider = GetComponent<Slider>();
        _slider.onValueChanged.AddListener(OnSliderChange);

        if (Game.IsReady)
        {
            SetupWindowController();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void SetupWindowController() 
    {
        _windowsController = Game.GetController<UIWindowsController>();
        _windowsController.SetSlider(this);
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        SetupWindowController();
    }

    private void OnSliderChange(float value)
    {

    }
}
