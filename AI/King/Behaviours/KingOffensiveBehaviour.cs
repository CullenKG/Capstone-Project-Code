using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingOffensiveBehaviour : AIBehaviour
{
    Timer KingAggroTimer;

    bool m_CanCharge;

    public KingOffensiveBehaviour(AIController aAIController) : base(aAIController)
    {
        m_AIController = aAIController;
        KingAggroTimer = Services.TimerManager.CreateTimer("KingAggroTimer", Constants.KingAggroDelay, false);
    }

    // Basic Behaviour where the king will only walk up to the player and try to melee attack him

    public override void Start()
    {
        KingAggroTimer.Restart();
        m_CanCharge = false;
    }

    public override void Update()
    {
        // QUICK COPY PASTE ACTION THINGS
        //  Set the action 
        //  m_AIController.SetAction((int)AIKingController.Action.None);

        //  Is the action this
        //  m_AIController.IsCurrentAction((int)AIKingController.Action.None)

        // Get Action
        //  ((AIKingController)m_AIController).CurrentAction

        // If the Aggro Timer is done
        if(KingAggroTimer.OnFinish())
        {
            m_CanCharge = true;
        }

        // Checks if next action has an action assigned and the current action is none
        if (m_AIController.IsCurrentAction((int)AIKingController.Action.None) && !m_AIController.IsNextAction((int)AIKingController.Action.None))
        {
            // Make the current action equal to the next action, and set the next action to none
            m_AIController.SetCurrentActionAsNextAction();
            m_AIController.SetNextAction((int)AIKingController.Action.None);
            m_AIController.m_MakeDecision = false;
        }


        if (m_AIController.IsCurrentAction((int)AIKingController.Action.None))
        {
            // If the previous action was melee and the king can't see the player
            if (m_AIController.IsPreviousAction((int)AIKingController.Action.Slashing) && ((AIKingController)m_AIController).CanSeePlayer(2) == false)
            {
                // then dodge backwards
                m_AIController.SetAction((int)AIKingController.Action.Dodge);
            }
        }

        if (m_AIController.IsCurrentAction((int)AIKingController.Action.None))
        {

            // If the player is in charge range and CanCharge equals true
            if (((AIKingController)m_AIController).GetDistanceToPlayer() < Constants.AggroChargeRange && m_CanCharge == true)
            {
                // Randomly choose either charge or lunge
                int attack = Random.Range(0, 2);

                if (attack == 0)
                {
                    m_AIController.SetAction((int)AIKingController.Action.Lunge);
                }

                if (attack == 1)
                {
                    m_AIController.SetAction((int)AIKingController.Action.ShieldCharge);
                }

                // Set charge to false reset the timer
                m_CanCharge = false;
                KingAggroTimer.Restart();
            }
            // If the player is in melee range
            else if (((AIKingController)m_AIController).GetDistanceToPlayer() < Constants.MeleeRange)
            {
                m_AIController.SetAction((int)AIKingController.Action.Slashing);
            }
            // If the player is not in melee range or charge range
            else if (((AIKingController)m_AIController).GetDistanceToPlayer() > Constants.MeleeRange || ((AIKingController)m_AIController).GetDistanceToPlayer() > Constants.AggroChargeRange)
            {
                m_AIController.SetAction((int)AIKingController.Action.Walk);
            }
        }

        // Stop updating the behaviour
        m_AIController.m_MakeDecision = false;      
    }

    public override void OnActionFinished()
    {
        // Set the current action to none
        m_AIController.SetAction((int)AIKingController.Action.None);

        // Update the behaviour to make a new decision
        m_AIController.m_MakeDecision = true;
    }
}
