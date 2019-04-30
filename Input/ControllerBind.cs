using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerBind
{
    public ControllerInput ControllerInput { get; set; }

    public ControllerBind(ControllerInput aControllerInput)
    {
        ControllerInput = aControllerInput;
    }

    public bool GetButton()
    {
        return Services.InputManager.ControllerInputManager.GetButton(ControllerInput);
    }

    public bool GetButtonDown()
    {
        return Services.InputManager.ControllerInputManager.GetButtonDown(ControllerInput);
    }

    public bool GetButtonUp()
    {
        return Services.InputManager.ControllerInputManager.GetButtonUp(ControllerInput);
    }

    public float GetAxis()
    {
        return Services.InputManager.ControllerInputManager.GetAxis(ControllerInput);
    }

    public float GetAxisRaw()
    {
        return Services.InputManager.ControllerInputManager.GetAxisRaw(ControllerInput);
    }
}
