using System.Collections;
using System.Collections.Generic;
using TimmyFramework;
using UnityEngine;

public class UIWindowsController : IController
{
    public bool IsUIMode {  get; private set; }
    public UIMoveable Moveable {  get; private set; }
    public UISlider Slider { get; private set; }

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
        if(Slider != null)
        {
            return;
        }

        Slider = slider;
    }

    public void SetMoveable(UIMoveable moveable)
    {
        if (Moveable != null)
        {
            return;
        }

        Moveable = moveable;
    }
}
