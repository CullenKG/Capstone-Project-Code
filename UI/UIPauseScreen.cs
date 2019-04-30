using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIPauseScreen : UIScreen
{
    // Use this for initialization

    void Start()
    {
        Debug.Log("inStart");

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Running");


        //if (EventSystem.current.firstSelectedGameObject == null)
        //{
        //    EventSystem.current.firstSelectedGameObject = FirstSelected;
        //}
        //EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);

        //Debug.Log(EventSystem.current.firstSelectedGameObject.name);








        if (Services.InputManager.GetLookInput().magnitude > 0.0f)
        {
            Services.GameManager.SetCursorVisibility(true);
        }

        //Debug.Log(Input.GetAxis("Vertical"));
    }

    public void OnResume()
    {
        Services.GameManager.ResumeGame();
        Services.AudioManager.PlayMenuSFX(MenuSFX.Menu_Button_Select);
    }

    public void OnOptionsButtonPressed()
    {
        Services.UIManager.PopScreen();
        Services.UIManager.PushScreen(UIManager.Screen.OptionsMenu);
        Services.AudioManager.PlayMenuSFX(MenuSFX.Menu_Button_Select);
    }

    public void OnQuitToMenu()
    {
        Services.AudioManager.PlayMenuSFX(MenuSFX.Menu_Button_Select);

        Services.GameManager.CurrentGameState = GameState.Menu;

        Time.timeScale = 1;

        Services.UIManager.PopAllScreens();

        SceneManager.LoadScene("MainMenu");
    }

    public override void OnPush()
    {
        base.OnPush();
    }

}
