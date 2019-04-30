using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIOptionsMenuScreen : UIScreen
{
    [SerializeField] private GameObject GameSettingsCanvas;
    [SerializeField] private GameObject ControlsCanvas;

    public override void OnPush()
    {
        base.OnPush();

        GameSettingsCanvas.GetComponent<UIScreen>().OnPush();
    }

    public void OnGameSettingsButtonPressed()
    {
        GameSettingsCanvas.GetComponent<UIScreen>().OnPush();
        ControlsCanvas.GetComponent<UIScreen>().OnPop();
    }

    public void OnControlsButtonPressed()
    {
        GameSettingsCanvas.GetComponent<UIScreen>().OnPop();
        ControlsCanvas.GetComponent<UIScreen>().OnPush();
    }

    public void OnBackButtonPressed()
    {
        GameSettingsCanvas.GetComponent<UIScreen>().OnPop();
        ControlsCanvas.GetComponent<UIScreen>().OnPop();
        Services.UIManager.PopScreen();

        if (Services.GameManager.CurrentGameState != GameState.Menu)
        {
            Services.UIManager.PushScreen(UIManager.Screen.PauseMenu);
        }
        else
        {
            Services.UIManager.PushScreen(UIManager.Screen.MainMenu);
        }
    }
}
