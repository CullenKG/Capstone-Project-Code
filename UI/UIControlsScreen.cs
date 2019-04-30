using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIControlsScreen : UIScreen
{
    /// <summary>
    /// Prefab for the keybind element shown on the options menu.
    /// </summary>
    [SerializeField] GameObject UIKeybindElementPrefab;

    /// <summary>
    /// The panel that contains all the keybind elements.
    /// </summary>
    [SerializeField] GameObject UIKeybindsPanel;

    /// <summary>
    /// Box that shows up when an error message needs to be displayed.
    /// </summary>
    [SerializeField] GameObject UIErrorMessageBox;

    /// <summary>
    /// Text components for the keybind elements keyboard buttons.
    /// </summary>
    Text[] m_KeyboardTextElements = new Text[System.Enum.GetNames(typeof(InputAction)).Length];

    /// <summary>
    /// Text components for the keybind elements controller buttons.
    /// </summary>
    Text[] m_ControllerTextElements = new Text[System.Enum.GetNames(typeof(InputAction)).Length];

    void OnEnable()
    {
        // Subscribe to binding events.
        InputManager.KeyboardBindEvent += OnKeyboardBind;
        InputManager.ControllerBindEvent += OnControllerBind;
        InputManager.BindFailedEvent += OnBindFailed;
        InputManager.BindStoppedEvent += OnBindStopped;
    }

    void OnDisable()
    {
        // Unsubscribe to binding events.
        InputManager.KeyboardBindEvent -= OnKeyboardBind;
        InputManager.ControllerBindEvent -= OnControllerBind;
        InputManager.BindFailedEvent -= OnBindFailed;
        InputManager.BindStoppedEvent -= OnBindStopped;
    }

    // Use this for initialization
    void Start()
    {
        // Loop through all the input actions
        for (int i = 0; i < (int)InputAction._NumberOfInputActions; i++)
        {
            // Instantiate a keybind UI element
            GameObject keybindElement = Instantiate(UIKeybindElementPrefab, UIKeybindsPanel.transform);

            // Set the text to show what input action it is
            keybindElement.GetComponent<Text>().text = ((InputAction)i).ToString();

            // Get all the text components from the keybind UI elements
            m_KeyboardTextElements[i] = keybindElement.transform.GetChild(0).GetComponentInChildren<Text>();
            m_ControllerTextElements[i] = keybindElement.transform.GetChild(1).GetComponentInChildren<Text>();

            InputAction action = (InputAction)i;

            // Get the currently bound key/button for the action and set the text to display it
            m_KeyboardTextElements[i].text = Services.InputManager.GetBoundKey(action).ToString();
            m_ControllerTextElements[i].text = string.Empty;

            m_ControllerTextElements[i].transform.parent.GetComponent<Image>().sprite = Services.InputManager.GetBoundControllerIcon(action);

            // Add a listener to the button so that when pressed will call the function with the correct ID
            m_KeyboardTextElements[i].transform.parent.GetComponent<Button>().onClick.AddListener(() => { StartKeyAssignment(action, InputType.Keyboard); });
            m_ControllerTextElements[i].transform.parent.GetComponent<Button>().onClick.AddListener(() => { StartKeyAssignment(action, InputType.Controller); });           
        }
    }

    /// <summary>
    /// Resets the keybind elements text back to the currently bound key.
    /// </summary>
    private void ResetBindText(InputAction aInputAction, InputType aInputType)
    {
        if (aInputType == InputType.Controller)
        {
            m_ControllerTextElements[(int)aInputAction].text = string.Empty;
            m_ControllerTextElements[(int)aInputAction].transform.parent.GetComponent<Image>().sprite = Services.InputManager.GetBoundControllerIcon(aInputAction);
        }
        else
        {
            m_KeyboardTextElements[(int)aInputAction].text = Services.InputManager.GetBoundKey(aInputAction).ToString();
        }
    }

    public void OnKeyboardBind(InputAction aInputAction, KeyCode aKeyCode)
    {
        m_KeyboardTextElements[(int)aInputAction].text = aKeyCode.ToString();
    }

    public void OnControllerBind(InputAction aInputAction, ControllerInput aControllerInput)
    {
        m_ControllerTextElements[(int)aInputAction].text = aControllerInput.ToString();
    }

    public void OnBindFailed(InputAction aInputAction, InputType aInputType)
    {
        ResetBindText(aInputAction, aInputType);

        // Show error message
        Services.UIManager.PushScreen(UIManager.Screen.ErrorMessage);

        ((UIErrorMessageScreen)Services.UIManager.GetTopScreen()).SetErrorMessage("Already bound to " + aInputAction.ToString());      
    }

    public void OnBindStopped(InputAction aInputAction, InputType aInputType)
    {
        ResetBindText(aInputAction, aInputType);
    }

    /// <summary>
    /// Called whenever a keybind button is pressed.
    /// </summary>
    public void StartKeyAssignment(InputAction aInputAction, InputType aInputType)
    {
        if (aInputType == InputType.Controller)
        {
            m_ControllerTextElements[(int)aInputAction].text = "...";
            m_ControllerTextElements[(int)aInputAction].transform.parent.GetComponent<Image>().sprite = null;
        }
        else if (aInputType == InputType.Keyboard)
        {
            m_KeyboardTextElements[(int)aInputAction].text = "...";
        }

        Services.InputManager.StartKeyAssignment(aInputAction, aInputType);
    }

    public override void OnPop()
    {
        Services.InputManager.StopBindIfInProgress();

        base.OnPop();
    }
}