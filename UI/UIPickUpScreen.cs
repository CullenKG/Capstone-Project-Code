using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPickUpScreen : UIScreen
{

    Player m_Player;
    // Use this for initialization
    void Start()
    {
        Services.GameManager.SetCursorVisibility(true);
        Services.GameManager.CurrentGameState = GameState.Menu;
        m_Player = GameObject.Find("Player").GetComponent<Player>();
    }

    public void IncreaseMaxStamina()
    {
        m_Player.AddStaminaChunk();
        Services.UIManager.PopScreen();
        Services.GameManager.SetCursorVisibility(false);
    }

    public void IncreaseMaxFocus()
    {
        m_Player.AddFocusChunk();
        Services.UIManager.PopScreen();
        Services.GameManager.SetCursorVisibility(false);
    }

    public void IncreaseMaxHealth()
    {
        m_Player.m_MaxHealth += 40;
        Services.UIManager.PopScreen();
        Services.GameManager.SetCursorVisibility(false);
        //////Debug.Log
    }

   
    // Update is called once per frame
    void Update()
    {
    }

    public override void OnPush()
    {
        Services.GameManager.SetCursorVisibility(true);
        Services.GameManager.CurrentGameState = GameState.Menu;
        base.OnPush();
    }

    public override void OnPop()
    {
        base.OnPop();
    }
}
