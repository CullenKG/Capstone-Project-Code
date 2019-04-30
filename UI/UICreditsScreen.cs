using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UICreditsScreen : UIScreen
{

    //Animator m_animator; 

	// Use this for initialization
	void Awake () {
        //m_animator = GetComponent<Animator>();
      //  this.transform.position.y = -Screen.height;
      //if(Services.GameManager.CurrentGameState == GameState.Paused)
      //  {
      //      Services.GameManager.UnFreezeGame();
      //  }
	}
   
    // Update is called once per frame
    void Update () {
		if( Services.InputManager.GetActionDown(InputAction.Jump))
        {
            Services.GameManager.SetCursorVisibility(true);
            SceneManager.LoadScene("MainMenu");
        }
	}
}
