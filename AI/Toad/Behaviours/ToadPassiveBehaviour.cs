using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadPassiveBehaviour : AIBehaviour
{
    public ToadPassiveBehaviour(AIController aAIController) : base(aAIController)
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
           // m_AIController.SetBehaviour((int)AIToadController.Behaviour.Aggressive);
           // m_AIController.m_MakeDecision = true;
        }
    }

    public override void OnActionFinished()
    {

    }
}
