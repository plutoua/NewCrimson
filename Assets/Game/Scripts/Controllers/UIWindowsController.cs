using System.Collections;
using System.Collections.Generic;
using TimmyFramework;
using UnityEngine;

public class UIWindowsController : IController
{
    public bool IsUIMode {  get; private set; }

    private UISlider _slider;

    public void Initialize()
    {
        IsUIMode = false;
    }

    public void TurnOnUIMode()
    {
        IsUIMode = true;
    }
    public void TurnOffUIMode()
    {
        IsUIMode = false;
    }

    public void SetSlider(UISlider slider)
    {
        if(_slider != null)
        {
            return;
        }

        _slider = slider;
    }
}
