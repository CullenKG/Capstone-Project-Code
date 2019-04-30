using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// List of input axes.
/// </summary>
public enum InputAxis
{
    MoveFrontward,
    MoveHorizontal,

    _NumberOfInputAxes
}

/// <summary>
/// List of input actions.
/// </summary>
public enum InputAction
{
    MoveForward,
    MoveBackward,
    MoveRight,
    MoveLeft,
    Dash,
    Jump,
    Attack,
    Fire,
    LockCamera,
    Heal,
    Interact,
    Pause,

    _NumberOfInputActions,
    None
}

/// <summary>
/// List of input types.
/// </summary>
public enum InputType
{
    Keyboard,
    Controller
}

/// <summary>
/// Used to make adding/getting controller images easier.
/// </summary>
public enum ControllerIcons
{
    None,
    LT,
    RT,
    LeftStickLeft,
    LeftStickRight,
    LeftStickDown,
    LeftStickUp,
    RightStickLeft,
    RightStickRight,
    RightStickDown,
    RightStickUp,
    DPadLeft,
    DPadRight,
    DPadDown,
    DPadUp,
    A,
    B,
    X,
    Y,
    LB,
    RB,
    Back,
    Start,
    LeftStickClick,
    RightStickClick,

    // Always last
    _NumberOfControllerIcons
}

/// <summary>
/// Handles all input. Created by Cullen.
/// </summary>
public class InputManager : MonoBehaviour
{
    // Events for input binding.
    public static UnityAction<InputAction, KeyCode> KeyboardBindEvent;
    public static UnityAction<InputAction, ControllerInput> ControllerBindEvent;
    public static UnityAction<InputAction, InputType> BindFailedEvent;
    public static UnityAction<InputAction, InputType> BindStoppedEvent;

    // Used for inputting icons in inspector.
    [NamedArray(typeof(ControllerIcons))]
    [SerializeField] Sprite[] ControllerIconTextures = new Sprite[(int)ControllerIcons._NumberOfControllerIcons];

    /// <summary>
    /// Maps the textures from ControllerIconTextures array to their ControllerInputs.
    /// </summary>
    Dictionary<ControllerInput, Sprite> m_ControllerIcons = new Dictionary<ControllerInput, Sprite>();

    /// <summary>
    /// Reference to the controller input manager.
    /// </summary>
    public ControllerInputManager ControllerInputManager { get; private set; }

    /// <summary>
    /// The currenly used input type. (keyboard or controller)
    /// </summary>
    public InputType CurrentInputType { get; private set; }

    /// <summary>
    /// List of input binds.
    /// </summary>
    Dictionary<InputAction, InputActionBind> m_InputBinds = new Dictionary<InputAction, InputActionBind>();

    /// <summary>
    /// List of axis binds.
    /// </summary>
    Dictionary<InputAxis, InputActionAxis> m_AxisBinds = new Dictionary<InputAxis, InputActionAxis>();

    /// <summary>
    /// The action we're trying to rebind.
    /// </summary>
    InputAction m_NextActionToAssign;

    /// <summary>
    /// The input type we're trying to rebind.
    /// </summary>
    InputType m_NextTypeToAssign;

    /// <summary>
    /// Has a bind assignment been started? True when waiting for an input to be pressed for rebinding.
    /// </summary>
    bool m_BindAssignStarted;

    #region Unity Functions

    // Use this for initialization
    void Start()
    {
        ControllerInputManager = new ControllerInputManager();

        CurrentInputType = InputType.Keyboard;

        for (int i = 0; i < (int)InputAction._NumberOfInputActions; i++)
        {
            string inputActionName = ((InputAction)i).ToString();

            KeyCode keyboardKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(inputActionName + InputType.Keyboard.ToString(), Services.Constants.DefaultKeyMouseBinds[i].ToString()));
            ControllerInput controllerInput = (ControllerInput)System.Enum.Parse(typeof(ControllerInput), PlayerPrefs.GetString(inputActionName + InputType.Controller.ToString(), Services.Constants.DefaultControllerBinds[i].ToString()));

            m_InputBinds[(InputAction)i] = new InputActionBind(new KeyboardBind(keyboardKeyCode), new ControllerBind(controllerInput));
        }

        int c = 0;

        foreach(ControllerInput input in System.Enum.GetValues(typeof(ControllerInput)))
        {
            m_ControllerIcons[input] = ControllerIconTextures[c];

            c++;
        }

        CreateAxisBind(InputAxis.MoveFrontward, m_InputBinds[InputAction.MoveForward], m_InputBinds[InputAction.MoveBackward]);
        CreateAxisBind(InputAxis.MoveHorizontal, m_InputBinds[InputAction.MoveRight], m_InputBinds[InputAction.MoveLeft]);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_BindAssignStarted)
        {
            if (m_NextTypeToAssign == InputType.Controller)
            {
                ControllerInput input = ControllerInputManager.GetAnyButtonPress();

                if (input != ControllerInput.None)
                {
                    if (IsControllerInputInUse(input))
                    {
                        // Bind failed
                        BindFailedEvent.Invoke(m_NextActionToAssign, InputType.Controller);
                        m_BindAssignStarted = false;
                    }
                    else
                    {
                        // Bind succeeded
                        m_InputBinds[m_NextActionToAssign].ControllerBind.ControllerInput = input;
                        ControllerBindEvent.Invoke(m_NextActionToAssign, input);
                        m_BindAssignStarted = false;
                    }
                }              
            }
        }
    }

    void OnApplicationQuit()
    {
        for (int i = 0; i < (int)InputAction._NumberOfInputActions; i++)
        {
            InputAction inputAction = (InputAction)i;

            PlayerPrefs.SetString(inputAction.ToString() + InputType.Keyboard.ToString(), m_InputBinds[inputAction].KeyboardBind.Key.ToString());
            PlayerPrefs.SetString(inputAction.ToString() + InputType.Controller.ToString(), m_InputBinds[inputAction].ControllerBind.ControllerInput.ToString());
        }
    }

    void OnGUI()
    {
        Event e = Event.current;

        // If we're currently trying to bind an action
        if (m_BindAssignStarted)
        {
            // If we're trying to bind a keyboard action
            if (m_NextTypeToAssign == InputType.Keyboard)
            {
                KeyCode newKey = KeyCode.None;

                if (e.isKey)
                {
                    newKey = e.keyCode;
                }
                else if (e.shift)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        newKey = KeyCode.LeftShift;
                    }
                    else if (Input.GetKey(KeyCode.RightShift))
                    {
                        newKey = KeyCode.RightShift;
                    }                   
                }
                else if (e.isMouse)
                {
                    newKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), "Mouse" + e.button);
                }

                if (newKey != KeyCode.None)
                {
                    if (IsKeyInUse(newKey))
                    {
                        // Bind failed (overwrite other bind or put error message.)
                        BindFailedEvent.Invoke(m_NextActionToAssign, InputType.Keyboard);
                        m_BindAssignStarted = false;
                    }
                    else
                    {
                        // Bind succeeded
                        m_InputBinds[m_NextActionToAssign].KeyboardBind.Key = newKey;
                        KeyboardBindEvent.Invoke(m_NextActionToAssign, newKey);
                        m_BindAssignStarted = false;
                    }
                }               
            }
        }
    }

    #endregion

    #region Private Functions

    /// <summary>
    /// Returns true if the key is already bound to another action.
    /// </summary>
    private bool IsKeyInUse(KeyCode aKey)
    { 
        for (int i = 0; i < (int)InputAction._NumberOfInputActions; i++)
        {
            if (i == (int)m_NextActionToAssign)
                continue;

            if (m_InputBinds[(InputAction)i].KeyboardBind.Key == aKey)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Returns true if the controller input is already bound to another action.
    /// </summary>
    private bool IsControllerInputInUse(ControllerInput aControllerInput)
    {
        for (int i = 0; i < (int)InputAction._NumberOfInputActions; i++)
        {
            if (i == (int)m_NextActionToAssign)
                continue;

            if (m_InputBinds[(InputAction)i].ControllerBind.ControllerInput == aControllerInput)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Creates an axis bind from the specified positive and negative binds.
    /// </summary>
    private void CreateAxisBind(InputAxis aInputAxis, InputActionBind aPositiveBind, InputActionBind aNegativeBind)
    {
        m_AxisBinds[aInputAxis] = new InputActionAxis(aPositiveBind, aNegativeBind);
    }

    #endregion

    #region Public Functions

    /// <summary>
    /// Returns true if anything (keyboard or controller) is pressed.
    /// </summary>
    public bool IsAnythingPressed()
    {
        ControllerInput input = ControllerInputManager.GetAnyButtonPress();

        if (input != ControllerInput.None)
            return true;

        return Input.anyKeyDown;
    }

    /// <summary>
    /// Starts a key assignment. The next input of the specified input type will be bound to the specified action.
    /// </summary>
    public void StartKeyAssignment(InputAction aInputAction, InputType aInputType)
    {
        // Other bind already started. TODO: Show error, or do something else.
        if (m_BindAssignStarted)
            return;

        m_BindAssignStarted = true;

        m_NextActionToAssign = aInputAction;
        m_NextTypeToAssign = aInputType;
    }

    /// <summary>
    /// Stops any binds that are currently in progress.
    /// </summary>
    public void StopBindIfInProgress()
    {
        if (m_BindAssignStarted)
        {
            BindStoppedEvent.Invoke(m_NextActionToAssign, m_NextTypeToAssign);
            m_BindAssignStarted = false;
        }
    }

    /// <summary>
    /// Returns the KeyCode bound to the specified action.
    /// </summary>
    public KeyCode GetBoundKey(InputAction aInputAction)
    {
        return m_InputBinds[aInputAction].KeyboardBind.Key;
    }

    

    /// <summary>
    /// Returns the ControllerInput bound to the specified action.
    /// </summary>
    public ControllerInput GetBoundControllerInput(InputAction aInputAction)
    {
        return m_InputBinds[aInputAction].ControllerBind.ControllerInput;
    }

    /// <summary>
    /// Returns the texture that represents the ControllerInput bound to the specified action.
    /// </summary>
    public Sprite GetBoundControllerIcon(InputAction aInputAction)
    {
        return m_ControllerIcons[GetBoundControllerInput(aInputAction)];
    }

    /// <summary>
    /// Returns true while the key or controller input bound to the action is pressed.
    /// </summary>
    public bool GetAction(InputAction aInputAction)
    {
        return m_InputBinds[aInputAction].GetAction();
    }

    /// <summary>
    /// Returns true during the frame the key or controller input bound to the action is pressed. (Controller axes - dpad, sticks, triggers - don't work properly)
    /// </summary>
    public bool GetActionDown(InputAction aInputAction)
    {
        return m_InputBinds[aInputAction].GetActionDown();
    }

    /// <summary>
    /// Returns true during the frame the key or controller input bound to the action is released. (Controller axes - dpad, sticks, triggers - don't work properly)
    /// </summary>
    public bool GetActionUp(InputAction aInputAction)
    {
        return m_InputBinds[aInputAction].GetActionUp();
    }

    /// <summary>
    /// Returns the value of key or controller input bound to the action.
    /// </summary>
    public float GetActionValue(InputAction aInputAction)
    {
        return m_InputBinds[aInputAction].GetActionValue();
    }

    /// <summary>
    /// Returns the value of the key or controller input bound to the action without smoothing.
    /// </summary>
    public float GetActionRawValue(InputAction aInputAction)
    {
        return m_InputBinds[aInputAction].GetActionRawValue();
    }

    /// <summary>
    /// Returns the value of the axis.
    /// </summary>
    public float GetAxisValue(InputAxis aInputAxis)
    {
        return m_AxisBinds[aInputAxis].GetAxisValue();
    }

    /// <summary>
    /// Returns the value of the axis without smoothing.
    /// </summary>
    public float GetAxisRawValue(InputAxis aInputAxis)
    {
        return m_AxisBinds[aInputAxis].GetAxisRawValue();
    }

    /// <summary>
    /// Returns the value of the mouse movement or right stick.
    /// </summary>
    public Vector2 GetLookInput()
    {
        Vector2 lookInput = new Vector2(Input.GetAxis("MouseHorizontal"), Input.GetAxis("MouseVertical"));

        if (lookInput.magnitude == 0)
        {
            lookInput = new Vector2(Input.GetAxisRaw("RightStickHorizontal"), -Input.GetAxisRaw("RightStickVertical"));

            CurrentInputType = InputType.Controller;
        }
        else
        {
            CurrentInputType = InputType.Keyboard;
        }

        return lookInput;
    }

    #endregion
}