using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingCutsceneBehaviour : AIBehaviour
{
    //Timer m_FirstPartCamera;
    public KingCutsceneBehaviour(AIController aAIController) : base(aAIController)
    {
       // m_AIController = aAIController;
        //m_FirstPartCamera = Services.TimerManager.CreateTimer("KingCutSceneTimer", 2.0f, false);
    }

    public override void Start()
    {
        ////Debug.Log("AiSystemWorksOnTheBehaviour");
        //((AIToadController)m_AIController).m_PlayerCamera.SetActive(false);
        //((AIToadController)m_AIController).m_CutsceneCamera.SetActive(true);
        //m_FirstPartCamera.StartTimer();
    }

    public override void Update()
    {
     //   ((AIToadController)m_AIController).m_CutsceneCamera.transform.position = Vector3.Lerp(((AIToadController)m_AIController).m_CutsceneCamera.transform.position, ((AIToadController)m_AIController).transform.position, Time.deltaTime * 2);

        //if (m_AIController.m_AiActive == true)
        //{
          
        //}
    }

    public override void OnActionFinished()
    {

    }
}
