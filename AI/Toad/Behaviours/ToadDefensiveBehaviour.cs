using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadDefensiveBehaviour : AIBehaviour
{
    public ToadDefensiveBehaviour(AIController aAIController) : base(aAIController)
    {
        m_AIController = aAIController;
    }

    bool m_HasMined;
    bool m_HasJumped;

    public override void Start()
    {
        ////Debug.Log("AiSystemWorksOnTheBehaviour");
        //m_AIController.AddAction((int)AIToadController.Action.Jump, new ToadJump(m_AIController));
        //m_AIController.SetAction((int)AIToadController.Action.Jump);

        m_HasMined = false;
        m_HasJumped = false;
    }

    public override void Update()
    {
        // Checks if next action has an action assigned and the current action is none
        if (m_AIController.IsCurrentAction((int)AIToadController.Action.None) && !m_AIController.IsNextAction((int)AIToadController.Action.None))
        {
            // Make the current action equal to the next action, and set the next action to none
            m_AIController.SetCurrentActionAsNextAction();
            m_AIController.SetNextAction((int)AIToadController.Action.None);
            ((AIToadController)m_AIController).m_MakeDecision = false;

            // If the toad hasn't jumped to the pillar yet
            if(m_HasJumped == false)
            {
                m_AIController.SetNextAction((int)AIToadController.Action.Jump);
                m_HasJumped = true;
            }
        }

        // If the toad can't see the player and he doesn't have an action assaigned, turn
        if (((AIToadController)m_AIController).CanSeePlayer() == false && m_AIController.IsCurrentAction((int)AIToadController.Action.None))
        {
            m_AIController.SetAction((int)AIToadController.Action.Turn);
            m_AIController.m_MakeDecision = false;
        }

        // If there is no action assigned
        if (m_AIController.IsCurrentAction((int)AIToadController.Action.None))
        {

            if (m_HasMined == false)
            {
                m_AIController.SetAction((int)AIToadController.Action.ProjectileMines);
                m_HasMined = true;
            }
            if (m_AIController.IsCurrentAction((int)AIToadController.Action.None))
            {

                if (((AIToadController)m_AIController).GetDistanceToPlayer() < 5.5f)
                {
                    // Melee Attack
                    m_AIController.SetAction((int)AIToadController.Action.TongueWhip);
                }

                if (((AIToadController)m_AIController).GetDistanceToPlayer() > 5.5f)
                {
                    // Projectile Attack
                    m_AIController.SetAction((int)AIToadController.Action.ProjectileAttack);
                }
            }
        }     

        m_AIController.m_MakeDecision = false;
    }

    public override void OnActionFinished()
    {
        // After the toad has reached the pillar set GoToPillar to false
        if (((AIToadController)m_AIController).m_GoToPillar == true)
        {
            ((AIToadController)m_AIController).m_GoToPillar = false;
        }

        // Set the action to none
        ((AIToadController)m_AIController).SetAction((int)AIToadController.Action.None);

        // And let the behaviour make a new decision
        ((AIToadController)m_AIController).m_MakeDecision = true;
    }
}
