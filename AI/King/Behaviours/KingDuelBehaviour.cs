using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingDuelBehaviour : AIBehaviour
{
    Timer KingDuelAttackTimer;

    public KingDuelBehaviour(AIController aAIController) : base(aAIController)
    {
        m_AIController = aAIController;
        KingDuelAttackTimer = Services.TimerManager.CreateTimer("KingDuelAttackTimer", 1.0f, false);
    }

    public override void Start()
    {
        ////Debug.Log("AiSystemWorksOnTheBehaviour");
        //m_AIController.AddAction((int)AIToadController.Action.Jump, new ToadJump(m_AIController));
        //m_AIController.SetAction((int)AIToadController.Action.Jump);

        // Get a random time for the attack timer
        float randtime = Random.Range(1, 4);

        // Set timer duration
        KingDuelAttackTimer.SetDuration(randtime * 4.0f);

        // Start Duel attack Timer
        KingDuelAttackTimer.Restart();
    }

    public override void Update()
    {
        //Checks if the toad can attack
        if (m_AIController.m_MakeDecision == true)
        {
            // If the fire circle length timer is done stop the duel
            if(((AIKingController)m_AIController).FireCircleLengthTimer.IsFinished())
            {
                ((AIKingController)m_AIController).m_FireCircle.SetActive(false);
                m_AIController.SetBehaviour((int)AIKingController.Behaviour.Offensive);
            }

            // Checks if next action has an action assigned and the current action is none
            if (m_AIController.IsCurrentAction((int)AIKingController.Action.None) && !m_AIController.IsNextAction((int)AIKingController.Action.None))
            {
                // Make the current action equal to the next action, and set the next action to none
                m_AIController.SetCurrentActionAsNextAction();
                m_AIController.SetNextAction((int)AIKingController.Action.None);
                m_AIController.m_MakeDecision = false;
            }

            // If the kings duel attack timer is done
            if (KingDuelAttackTimer.IsFinished())
            {
                // Lunge?
                m_AIController.SetAction((int)AIKingController.Action.ShieldCharge);
                KingDuelAttackTimer.Restart();
            }

            // If there is no action assigned
            if (m_AIController.IsCurrentAction((int)AIKingController.Action.None))
            {
                // Set the current action to none
                m_AIController.SetAction((int)AIKingController.Action.DuelMove);

                // If the player isin't dueling properly
                //if(((AIKingController)m_AIController).GetDistanceToPlayer() )
            
            } 
            
            m_AIController.m_MakeDecision = false;
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
