using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIErrorMessageScreen : UIScreen
{
    [SerializeField] Text m_ErrorMessageText;

    Timer m_Timer;

    private void Awake()
    {
        m_Timer = Services.TimerManager.CreateTimer("errorTimer", 0.2f, false);
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_Timer.IsFinished())
        {
            if (Services.InputManager.IsAnythingPressed())
            {
                Services.UIManager.PopScreen();
            }
        }              
	}

    public void SetErrorMessage(string aMessage)
    {
        m_ErrorMessageText.text = aMessage;
    }

    public override void OnPush()
    {
        base.OnPush();

        m_Timer.StartTimer();
    }
}
