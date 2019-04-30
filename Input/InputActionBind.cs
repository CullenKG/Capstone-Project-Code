using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputActionBind
{
    public KeyboardBind KeyboardBind { get; private set; }
    public ControllerBind ControllerBind { get; private set; }

    public InputActionBind(KeyboardBind aKeyboardBind, ControllerBind aControllerBind)
    {
        KeyboardBind = aKeyboardBind;
        ControllerBind = aControllerBind;
    }
    
	public bool GetAction()
    {
        if (KeyboardBind.GetKey())
        {
            return true;
        }

        return ControllerBind.GetButton();
    }

    public bool GetActionDown()
    {
        if (KeyboardBind.GetKeyDown())
        {
            return true;
        }

        return ControllerBind.GetButtonDown();
    }

    public bool GetActionUp()
    {
        if (KeyboardBind.GetKeyUp())
        {
            return true;
        }

        return ControllerBind.GetButtonUp();
    }

    public float GetActionValue()
    {
        if (KeyboardBind.GetValue() > 0.0f)
        {
            return KeyboardBind.GetValue();
        }

        return ControllerBind.GetAxis();
    }

    public float GetActionRawValue()
    {
        if (KeyboardBind.GetValue() > 0.0f)
        {
            return KeyboardBind.GetValue();
        }

        return ControllerBind.GetAxisRaw();
    }
}
