using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIGameEndScreen : UIScreen
{
    [SerializeField] private Text MessageText;
    [SerializeField] private Text SubMessageText;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if(Services.GameManager.PlayerWon)
        //{
        //    SubMessageText.text = "Press any button to play the credits";

        //    if (Input.anyKeyDown)
        //    {
        //        //Services.GameManager.ReturnToMenu();

        //        Services.GameManager.UnFreezeGame();
        //        Services.UIManager.PopScreen();
        //        Services.GameManager.SceneToLoad = "Credits";
        //        SceneManager.LoadScene("Credits"); 
        //    }
        //}

        if (!Services.GameManager.PlayerWon)
        {
            SubMessageText.text = "Press any button to reincarnate";
            Services.GameManager.SetCursorVisibility(true);

        }

    }

    public override void OnPush()
    {
        base.OnPush();

        Services.GameManager.FreezeGame();

        

        MessageText.text = Services.GameManager.PlayerWon ? "You Have Prevailed" : "You Have Perished";
    }

    public override void OnPop()
    {
        base.OnPop();
    }

    public void RespawnButton()
    {
        Services.GameManager.Respawn();
    }

    public void QuitButton()
    {
        Services.GameManager.UnFreezeGame();
      
        Services.GameManager.SceneToLoad = "MainMenu";
        SceneManager.LoadScene("Loading");
    }

    public void CreditsButton()
    {
        Services.GameManager.UnFreezeGame();
       // Services.UIManager.PopScreen();
        Services.GameManager.SceneToLoad = "Credits";
        SceneManager.LoadScene("Credits");
    }
}
