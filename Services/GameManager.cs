using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public enum GameState
{
    Menu,
    Playing,
    Paused
}


/// <summary>
/// Manages the overview of the game.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Pause events
    public static System.Action OnResume;
    public static System.Action OnPause;

    /// <summary>
    /// The player that is currently active.
    /// </summary>
    public Player Player { get; private set; }

    public string SceneToLoad;

    public AIToadController ToadBoss { get; private set; }
    
    public AIKingController KingBoss { get; private set; }

    public bool OdelBossTriggered = false;
    public bool Odeldefeated = false;
    public bool KingTriggered = false;
    public bool Kingdefeated = false;

    // Screen information
    public Resolution CurrentResolution { get; set; }
    public bool IsGameFullscreen { get; set; }

    // Has the player won? 
    public bool PlayerWon { get; set; }

    public GameState CurrentGameState { get; set; }

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            Services.GameManager.CurrentGameState = GameState.Playing;
            SetCursorVisibility(false);
        }
        else
        {
            CurrentGameState = GameState.Menu;
        }

        CurrentResolution = Screen.currentResolution;
        IsGameFullscreen = Screen.fullScreen;
        SceneToLoad = "";
    }

    void LateUpdate()
    {
        
       // Debug.Log(CurrentGameState);
        if (CurrentGameState == GameState.Playing)
        {
            if (Services.InputManager.GetActionDown(InputAction.Pause) && SceneManager.GetActiveScene().name =="Game")
            {
                if(Services.InputManager.CurrentInputType  == InputType.Keyboard)
                {
                    SetCursorVisibility(true);
                }

                //Debug.Log("Pause Game");
                PauseGame();
            }
        }
        else if (CurrentGameState == GameState.Paused)
        {
            if (Services.InputManager.GetActionDown(InputAction.Pause))
            {
                ResumeGame();
            }
        }
    }

    public void RegisterPlayer(Player aPlayer)
    {
        if (Player != null)
        {
            Debug.LogError("Player already registered!");
            return;
        }

        Player = aPlayer;
    }

    public void RegisterToad(AIToadController aToad)
    {
        if (ToadBoss != null)
        {
            Debug.LogError("ToadBoss already registered!");
            return;
        }

        ToadBoss = aToad;
    }

    public void RegisterKing(AIKingController aKing)
    {
        if (KingBoss != null)
        {
            Debug.LogError("KingBoss already registered!");
            return;
        }

        KingBoss = aKing;
    }


    /// <summary>
    /// Sets the visibility of the cursor. (Also locks the cursor to the center of the screen)
    /// </summary>
    public void SetCursorVisibility(bool visibility)
    {
        Cursor.visible = visibility;
        Cursor.lockState = visibility ? CursorLockMode.None : CursorLockMode.Locked;
    }

    /// <summary>
    /// Go back to the main menu.
    /// </summary>
    public void ReturnToMenu()
    {
        Services.UIManager.PopAllScreens();

        SetCursorVisibility(true);

        UnFreezeGame();
        CurrentGameState = GameState.Menu;

        SceneManager.LoadScene("MainMenu");
    }



    public void Respawn()
    {
        Services.UIManager.PopAllScreens();

        //SetCursorVisibility(true);
        UnFreezeGame();
        Player.Respawn();

        Services.GameManager.SetCursorVisibility(false);
        CurrentGameState = GameState.Playing;

       
        ToadBoss.Reset();

        //SceneManager.LoadScene("MainMenu");
    }

    // TODO: Do pausing & freezing better, preferably one function
    public void FreezeGame()
    {
        CurrentGameState = GameState.Paused;

        Time.timeScale = 0;

        if (OnPause != null)
            OnPause();
    }

    public void UnFreezeGame()
    {
        Services.GameManager.CurrentGameState = GameState.Playing;

        Time.timeScale = 1;

        if (OnResume != null)
            OnResume();
    }

    public void PauseGame()
    {
        

        CurrentGameState = GameState.Paused;

        Time.timeScale = 0;

        Services.UIManager.PushScreen(UIManager.Screen.PauseMenu);



        if (OnPause != null)
            OnPause();
    }

    public void ResumeGame()
    {
        SetCursorVisibility(false);

        Services.GameManager.CurrentGameState = GameState.Playing;

        Time.timeScale = 1;

        Services.UIManager.PopScreen();

        if (OnResume != null)
            OnResume();
    }

    void LoadInBackground(string SceneName)
    {

    }

}