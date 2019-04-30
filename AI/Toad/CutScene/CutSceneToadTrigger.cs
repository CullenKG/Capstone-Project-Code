using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneToadTrigger : MonoBehaviour
{
    public Animator m_ToadAnimator;
    public Animator m_CameraAnimator;
    Timer m_CutsceneTime;
    public GameObject m_Odell;
    public GameObject m_CutsceneOdell;
    public GameObject m_PlayerCamera;
    public GameObject m_CutsceneCamera;
    public GameObject m_Player;
    public GameObject m_Canvas;

    bool m_EnteredTrigger;
    // Use this for initialization
	void Start ()
    {
        m_CutsceneTime = Services.TimerManager.CreateTimer("m_CutsceneTimer", 7, false);
        m_EnteredTrigger = false;
        m_CutsceneCamera.SetActive(false);
        m_Canvas.SetActive(true);
        m_CutsceneOdell.SetActive(false);
        m_Odell.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
        if (m_CutsceneTime.IsFinished())
        {
            //Deactivating things for the cutscene
            m_CutsceneOdell.SetActive(false);
            m_CutsceneCamera.SetActive(false);

            //Turning on things for after cutscene
            m_Odell.SetActive(true);
            m_PlayerCamera.SetActive(true);
            m_Player.SetActive(true);
            m_Canvas.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_CutsceneTime.IsRunning() == true)
            {
                m_CutsceneOdell.SetActive(false);
                m_CutsceneCamera.SetActive(false);
                m_PlayerCamera.SetActive(true);
                m_Player.SetActive(true);
                m_Canvas.SetActive(true);
                m_Odell.SetActive(true);
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (m_EnteredTrigger == false)
            {
                m_EnteredTrigger = true;
                m_CutsceneCamera.SetActive(true);
                m_PlayerCamera.SetActive(false);
                m_Player.SetActive(false);
                m_Canvas.SetActive(false);
                m_CutsceneOdell.SetActive(true);
               // m_CutsceneOdell.transform.rotation = Quaternion.Euler(m_CutsceneOdell.transform.rotation.x, 180, m_CutsceneOdell.transform.rotation.z);
                m_CameraAnimator.SetTrigger("t_Odell");

                m_CutsceneTime.StartTimer();
            }
        }
    }
}
