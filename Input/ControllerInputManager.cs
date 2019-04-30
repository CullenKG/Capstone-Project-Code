using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControllerInput
{
    A = KeyCode.JoystickButton0,
    B = KeyCode.JoystickButton1,
    X = KeyCode.JoystickButton2,
    Y = KeyCode.JoystickButton3,
    LeftBumper = KeyCode.JoystickButton4,
    RightBumper = KeyCode.JoystickButton5,
    LeftTrigger = 1,
    RightTrigger = 2,
    LeftStickLeft = 3,
    LeftStickRight = 4,
    LeftStickDown = 5,
    LeftStickUp = 6,
    LeftStickClick = KeyCode.JoystickButton8,
    RightStickLeft = 7,
    RightStickRight = 8,
    RightStickDown = 9,
    RightStickUp = 10,
    RightStickClick = KeyCode.JoystickButton9,
    DPadLeft = 11,
    DPadRight = 12,
    DPadDown = 13,
    DPadUp = 14,
    Back = KeyCode.JoystickButton6,
    Start = KeyCode.JoystickButton7,

    None = KeyCode.None
}

public class ControllerInputManager
{
    /// <summary>
    /// Returns true if the specified input type is an axis. (control sticks, triggers, dpad)
    /// </summary>
    private bool IsInputTypeAxis(ControllerInput aControllerInputType)
    {
        string inputTypeString = aControllerInputType.ToString();

        if ((inputTypeString.Contains("Stick") && !inputTypeString.Contains("Click")) || inputTypeString.Contains("DPad") || inputTypeString.Contains("Trigger"))
            return true;

        return false;
    }

    /// <summary>
    /// Get the axis value for the specified input type.
    /// </summary>
    private float GetAxisValue(ControllerInput aControllerInputType, bool getRawValue = false)
    {
        string inputTypeString = aControllerInputType.ToString();

        string axisName = string.Empty;

        if (inputTypeString.StartsWith("LeftStick"))
        {
            axisName = "LeftStick";
        }
        else if (inputTypeString.StartsWith("RightStick"))
        {
            axisName = "RightStick";
        }
        else if (inputTypeString.StartsWith("DPad"))
        {
            axisName = "DPad";
        }
        else
        {
            axisName = aControllerInputType.ToString();
        }

        if (inputTypeString.EndsWith("Right") || inputTypeString.EndsWith("Left"))
        {
            axisName += "Horizontal";
        }
        else if (inputTypeString.EndsWith("Up") || inputTypeString.EndsWith("Down"))
        {
            axisName += "Vertical";
        }

        return getRawValue ? Input.GetAxisRaw(axisName) : Input.GetAxis(axisName);
    }

    /// <summary>
    /// Returns true if the axis value correlates to the axis direction.
    /// </summary>
    private bool AxisValueToBool(float aValue, ControllerInput aControllerInputType)
    {
        string inputTypeString = aControllerInputType.ToString();

        if (inputTypeString.Contains("Trigger"))
        {
            if (aValue > 0.0f)
                return true;
        }
        else if (inputTypeString.EndsWith("Right") || inputTypeString.EndsWith("Up"))
        {
            if (aValue > 0.0f)
                return true;
        }
        else if (inputTypeString.EndsWith("Left") || inputTypeString.EndsWith("Down"))
        {
            if (aValue < 0.0f)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Returns the axis value if the axis value correlates to the axis direction.
    /// </summary>
    private float AxisValueToFloat(float aValue, ControllerInput aControllerInputType)
    {
        string inputTypeString = aControllerInputType.ToString();

        if (inputTypeString.Contains("Trigger"))
        {
            if (aValue > 0.0f)
                return aValue;
        }
        else if (inputTypeString.EndsWith("Right") || inputTypeString.EndsWith("Up"))
        {
            if (aValue > 0.0f)
                return aValue;
        }
        else if (inputTypeString.EndsWith("Left") || inputTypeString.EndsWith("Down"))
        {
            if (aValue < 0.0f)
                return aValue;
        }

        return 0.0f;
    }

    /// <summary>
    /// Returns any controller input that is currently pressed.
    /// </summary>
    public ControllerInput GetAnyButtonPress()
    {
        foreach (ControllerInput input in System.Enum.GetValues(typeof(ControllerInput)))
        {
            if (GetButton(input))
                return input;
        }

        return ControllerInput.None;
    }

    /// <summary>
    /// Returns true while the specified controller input is pressed.
    /// </summary>
    public bool GetButton(ControllerInput aControllerInputType)
    {
        if (aControllerInputType == ControllerInput.None)
            return false;

        if (IsInputTypeAxis(aControllerInputType))
        {
            return AxisValueToBool(GetAxisValue(aControllerInputType), aControllerInputType);
        }
        else
        {
            return Input.GetKey((KeyCode)aControllerInputType);
        }
    }

    /// <summary>
    /// Returns true during the frame the specified controller input is pressed. (Any axis values (control sticks, triggers, dpad) will act as a GetButton call)
    /// </summary>
    public bool GetButtonDown(ControllerInput aControllerInputType)
    {
        if (aControllerInputType == ControllerInput.None)
            return false;

        if (IsInputTypeAxis(aControllerInputType))
        {
            return AxisValueToBool(GetAxisValue(aControllerInputType), aControllerInputType);
        }
        else
        {
            return Input.GetKeyDown((KeyCode)aControllerInputType);
        }
    }

    /// <summary>
    /// Returns true during the frame the specified controller input is released. (Any axis values (control sticks, triggers, dpad) will act as a GetButton call)
    /// </summary>
    public bool GetButtonUp(ControllerInput aControllerInputType)
    {
        if (aControllerInputType == ControllerInput.None)
            return false;

        if (IsInputTypeAxis(aControllerInputType))
        {
            return AxisValueToBool(GetAxisValue(aControllerInputType), aControllerInputType);
        }
        else
        {
            return Input.GetKeyUp((KeyCode)aControllerInputType);
        }
    }

    /// <summary>
    /// Returns the value of the specified controller input.
    /// </summary>
    public float GetAxis(ControllerInput aControllerInputType)
    {
        if (aControllerInputType == ControllerInput.None)
            return 0.0f;

        if (IsInputTypeAxis(aControllerInputType))
        {
            return Mathf.Abs(AxisValueToFloat(GetAxisValue(aControllerInputType), aControllerInputType));
        }
        else
        {
            return Mathf.Abs(Input.GetAxis(aControllerInputType.ToString()));
        }
    }

    /// <summary>
    /// Returns the value of specified controller input with no smoothing filtering applied.
    /// </summary>
    public float GetAxisRaw(ControllerInput aControllerInputType)
    {
        if (aControllerInputType == ControllerInput.None)
            return 0.0f;

        if (IsInputTypeAxis(aControllerInputType))
        {
            return Mathf.Abs(AxisValueToFloat(GetAxisValue(aControllerInputType, true), aControllerInputType));
        }
        else
        {
            return Mathf.Abs(Input.GetAxisRaw(aControllerInputType.ToString()));
        }
    }
}