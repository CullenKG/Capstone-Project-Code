using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingDuelPlayer : AIAction
{
    // Member vars
    public bool m_MovingLeft = false;

    public KingDuelPlayer(AIController aAIController) : base(aAIController)
    {

    }

    // Use this for initialization
    public override void Start()
    {
        // Set the destination
        ((AIKingController)m_AIController).m_NavMeshAgent.SetDestination(Services.GameManager.Player.gameObject.transform.position);

        // Make sure the nav agent can move
        ((AIKingController)m_AIController).m_NavMeshAgent.isStopped = false;

        // Generate a random number to determine which direction the king will walk in
        //int Randomx = Random.Range(0, 1);
    }

    // Update is called once per frame
    public override void Update()
    {
        // Not sure to use circle math do get the duel or mimic the players locked on circle movement

        // Set the destination constantly
       // ((AIKingController)m_AIController).m_NavMeshAgent.SetDestination(Services.GameManager.Player.gameObject.transform.position);

        // For Debuging mostly
        float Distance = ((AIKingController)m_AIController).GetDistanceToPlayer();

        // If the boss reaches melee range

            // Set the next action to melee and end the action

            // Set the next action to a melee attack
            //m_AIController.SetNextAction((int)AIKingController.Action.Slashing);

            // Stop the navmeshagent
            ((AIKingController)m_AIController).m_NavMeshAgent.isStopped = true;

            // Finish the action
            ((AIKingController)m_AIController).CurrentActionFinished();
        
        
    }
   
  }