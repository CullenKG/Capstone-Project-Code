using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingWalkToPlayer : AIAction
{
    // these will be constants later
    private float AbandonFollowRange = 25.0f;

    // Member vars
    public bool m_Moving = false;

    public KingWalkToPlayer(AIController aAIController) : base(aAIController)
    {

    }

    // Use this for initialization
    public override void Start()
    {
        // Set the destination
        ((AIKingController)m_AIController).m_NavMeshAgent.SetDestination(Services.GameManager.Player.gameObject.transform.position);

        // Make sure the nav agent can move
        ((AIKingController)m_AIController).m_NavMeshAgent.isStopped = true;
        m_Moving = false;

        ((AIKingController)m_AIController).m_Animator.SetBool("Walking", m_Moving);
    }

    // Update is called once per frame
    public override void Update()
    {
        // Make sure the nav agent can move
        ((AIKingController)m_AIController).m_NavMeshAgent.isStopped = false;
        m_Moving = true;

        // Set the destination constantly
        ((AIKingController)m_AIController).m_NavMeshAgent.SetDestination(Services.GameManager.Player.gameObject.transform.position);

        // For Debuging mostly
        float Distance = ((AIKingController)m_AIController).GetDistanceToPlayer();

        // If the boss reaches melee range
        if (((AIKingController)m_AIController).GetDistanceToPlayer() < Constants.MeleeRange)
        {
            // End the action and the behaviour will handle the action change
            FinishAction();
        }

        // If the player gets to far from the boss
        if (((AIKingController)m_AIController).GetDistanceToPlayer() > AbandonFollowRange)
        {
            // Set the next action to a projectile attack
            //m_AIController.SetNextAction((int)AIKingController.Action.ProjectileAttack);

            FinishAction();
        }

         ((AIKingController)m_AIController).m_Animator.SetBool("Walking", m_Moving);
    }

    public void FinishAction()
    {
        m_Moving = false;

        // Stop the navmeshagent
        ((AIKingController)m_AIController).m_NavMeshAgent.isStopped = true;

        // Finish the action
        ((AIKingController)m_AIController).CurrentActionFinished();
    }
   
  }