using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadAgressiveBehaviour : AIBehaviour
{
    public ToadAgressiveBehaviour(AIController aAIController) : base(aAIController)
    {
        m_AIController = aAIController;
    }
    private float HealthStamp = 0;
    private float Close = 7.0f;
    private float MiddleDistance = 16.0f;
    private float Far = 30.0f;

    public override void Start()
    {
        HealthStamp = ((AIToadController)m_AIController).m_CurrentHealth;
    }

    public override void Update()
    {
        // Quick Copy pastes

        //  Set the action 
        //  m_AIController.SetAction((int)AIToadController.Action.None);

        //  Is the action this
        //  m_AIController.IsCurrentAction((int)AIToadController.Action.None)

        // Get Action
        //  ((AIToadController)m_AIController).CurrentAction

        //Checks if the toad should make a decision
        if (m_AIController.m_MakeDecision == true)
        {
            // Checks to see if the toad should switch behaviours
            if (((AIToadController)m_AIController).m_NumberOfAttacks < Constants.MaxAttacksBeforeBehaviourSwitch)
            {

            }
            // Checks if next action has an action assigned and the current action is none
            if (m_AIController.IsCurrentAction((int)AIToadController.Action.None) && !m_AIController.IsNextAction((int)AIToadController.Action.None))
            {               
                // Make the current action equal to the next action, and set the next action to none
                m_AIController.SetCurrentActionAsNextAction();
                m_AIController.SetNextAction((int)AIToadController.Action.None);
                m_AIController.m_MakeDecision = false;
            }

            // If the player is on top of the toad then buck him off
            if (((AIToadController)m_AIController).m_PlayerRider.m_IsPlayerinside == true && m_AIController.IsCurrentAction((int)AIToadController.Action.None))
            {
                m_AIController.SetAction((int)AIToadController.Action.Buck);
                m_AIController.m_MakeDecision = false;
            }

            // If the toad can't see the player and he doesn't have an action assaigned, turn
            if (((AIToadController)m_AIController).CanSeePlayer() == false && m_AIController.IsCurrentAction((int)AIToadController.Action.None))
                {
                    m_AIController.SetAction((int)AIToadController.Action.Turn);
                    m_AIController.m_MakeDecision = false;
                }

            if (m_AIController.IsCurrentAction((int)AIToadController.Action.None))
            {
                // If the player is far away
                if (((AIToadController)m_AIController).GetDistanceToPlayer() > Far)
                {
                    // If the last two attack weren't projectile attack or tongue whip                       
                    if (m_AIController.IsCurrentAction((int)AIToadController.Action.None))
                    {
                        // Generate a random number
                        int Randnum = Random.Range(0, 2);
                        // 1/2 chance
                        if (Randnum == 0)
                        {
                            // Projectile attack
                            m_AIController.SetAction((int)AIToadController.Action.ProjectileAttack);
                        } // 1/2 chance
                        if (Randnum == 1)
                        {
                            // Stomp
                            m_AIController.SetAction((int)AIToadController.Action.Stomp);
                            m_AIController.SetNextAction((int)AIToadController.Action.Jump);
                        }
                    }
                }
                // If the player isin't far or close
                else if (((AIToadController)m_AIController).GetDistanceToPlayer() < Far && ((AIToadController)m_AIController).GetDistanceToPlayer() > MiddleDistance)
                {
                    // If the last two attack weren't Charge or tongue whip                       
                    if (m_AIController.IsCurrentAction((int)AIToadController.Action.None))
                    {
                        // Generate a random number
                        int Randnum = Random.Range(0, 2);
                        // 1/2 chance
                        if (Randnum == 0)
                        {
                            // Charge
                            m_AIController.SetAction((int)AIToadController.Action.Charge);
                            m_AIController.SetNextAction((int)AIToadController.Action.Jump);
                        } // 1/2 chance
                        if (Randnum == 1)
                        {
                            // Charge
                            m_AIController.SetAction((int)AIToadController.Action.ProjectileAttack);
                        }

                    }
                } // If the player is close
                else if (((AIToadController)m_AIController).GetDistanceToPlayer() > Close || ((AIToadController)m_AIController).GetDistanceToPlayer() < Close)
                {
                    if (m_AIController.IsCurrentAction((int)AIToadController.Action.None))
                    {        
                        // Melee Attack
                       m_AIController.SetAction((int)AIToadController.Action.TongueWhip);                        
                    }
                }
            }

            ////If the decided action was Jump, Calculate the Jump ID (position)
            //if (m_AIController.IsCurrentAction((int)AIToadController.Action.Jump))
            //{
            //    ((AIToadController)m_AIController).CalculateJumpID();
            //}

            m_AIController.m_MakeDecision = false;
        }
    }

    public override void OnActionFinished()
    {     
        // Set the current action to none
        m_AIController.SetAction((int)AIKingController.Action.None);

        // Set make decision to true so the Behvaiour can update
        m_AIController.m_MakeDecision = true;
    }
}
