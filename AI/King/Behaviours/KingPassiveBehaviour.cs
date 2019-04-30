using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingPassiveBehaviour : AIBehaviour
{
    public KingPassiveBehaviour(AIController aAIController) : base(aAIController)
    {
        m_AIController = aAIController;
    }

    public override void Start()
    {
        ////Debug.Log("AiSystemWorksOnTheBehaviour");

    }

    public override void Update()
    {
        if (m_AIController.m_AiActive == true)
        {
           // m_AIController.SetBehaviour((int)AIKingController.Behaviour.Offensive);
           m_AIController.m_MakeDecision = true;
        }
    }

    public override void OnActionFinished()
    {
        // Set the current action to none
        m_AIController.SetAction((int)AIKingController.Action.None);

      
            // Update the behaviour to make a new decision
            m_AIController.m_MakeDecision = true;
        
    }
}
