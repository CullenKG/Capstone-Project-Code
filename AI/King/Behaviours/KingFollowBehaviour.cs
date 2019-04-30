using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFollowBehaviour : AIBehaviour
{
    public KingFollowBehaviour(AIController aAIController) : base(aAIController)
    {
        m_AIController = aAIController;
    }

    // these will be constants later
    private float MeleeRange = 5.0f;

    // Member vars
    public bool m_AbleToDuel = false;
    public bool DestinationSet = false;

    public override void Start()
    {

    }

    public override void Update()
    {
        //Checks if the toad needs to decide an action
        if (m_AIController.m_MakeDecision == true)
        {
            // Set action to walk
            //((AIKingController)m_AIController).SetAction((int)AIKingController.Action.Walk);

           // m_AIController.m_MakeDecision = false;
        }

        if (m_AbleToDuel == false)
        {
            MoveToPlayer();
        }
        else
        {
            MoveToDuel();
        }
    }

    public override void OnActionFinished()
    {

    }

    public void MoveToPlayer()
    {

        

       


    }


    public void MoveToDuel()
    {

    }
}
