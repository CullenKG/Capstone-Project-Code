using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingPillarBehaviour : AIBehaviour
{
    //Timer m_FirstPartCamera;
    public KingPillarBehaviour(AIController aAIController) : base(aAIController)
    {
        m_AIController = aAIController;
    }

    public override void Start()
    {
        
    }

    public override void Update()
    {


        // Stop updating the behaviour
        m_AIController.m_MakeDecision = false;
    }

    public override void OnActionFinished()
    {

    }
}
