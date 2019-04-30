using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIMainMenuScreen : UIScreen
{
    void Start()
    {
        Services.AudioManager.SetMusicLooping(true, "Global");
        Services.AudioManager.PlayMusic(Music.MainMenu);
    }

    void Update()
    {

    }

    public void OnPlayButtonPressed()
    {
        Services.GameManager.SetCursorVisibility(false);

        // Play play select SFX
        Services.AudioManager.PlayMenuSFX(MenuSFX.Menu_Play_Select);

        Services.GameManager.CurrentGameState = GameState.Playing;
        Services.GameManager.SceneToLoad = "Game";
        SceneManager.LoadScene("Loading");

        Services.UIManager.PopAllScreens();
    }

    public void OnOptionsButtonPressed()
    {
        Services.UIManager.PopScreen();
        Services.UIManager.PushScreen(UIManager.Screen.OptionsMenu);
        Services.AudioManager.PlayMenuSFX(MenuSFX.Menu_Button_Select);
    }

    public void OnCreditsButtonPressed()
    {
        SceneManager.LoadScene("Credits");
        Services.AudioManager.PlayMenuSFX(MenuSFX.Menu_Button_Select);
    }

    public void OnQuitButtonPressed()
    {
        Services.AudioManager.PlayMenuSFX(MenuSFX.Menu_Button_Select);
        Application.Quit();
    }

    public void LoadScene(string scene)
    {
        Services.GameManager.SetCursorVisibility(false);


        Services.GameManager.CurrentGameState = GameState.Playing;


        Services.GameManager.SceneToLoad = scene;
        SceneManager.LoadScene("Loading");
    }
    //public override void OnPush()
    //{
    //  //  Debug.Log("main menu scene being called");
    //   // Services.UIManager.PopScreen();
    //}


}
