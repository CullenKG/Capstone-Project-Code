using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadStackScript : AIBehaviour
{
    public ToadStackScript(AIController aAIController) : base(aAIController)
    {
        m_AIController = aAIController;
    }

    private float Close = 10.0f;
    private float MiddleDistance = 16.0f;
    private float Far = 30.0f;

    private int[] ActionStack;

    // Codex for actions
    // 1 - ProMine
    // 2 - Proj
    // 3 - SpinHead
    // 4 - Stomp
    // 5 - Charge
    // 6 - Tongue Whip
    // 7 - Jump
    // 8 - Tongue Grab
    // 9 - None


    public override void Start()
    {
        ActionStack[0] = 1;
        ActionStack[1] = 2;
        ActionStack[2] = 3;
        ActionStack[3] = 4;
        ActionStack[4] = 5;
        ActionStack[5] = 6;
        ActionStack[6] = 7;
    }

    public override void Update()
    {
        // Some temp values     

        // For Testing one attack in perticular
        //m_MakeDecision = false;
        // m_ExecuteAction = EnemyAction.Charge;


        //  Set the action 
        //  m_AIController.SetAction((int)AIToadController.Action.None);

        //  Is the action this
        //  m_AIController.IsCurrentAction((int)AIToadController.Action.None)

        // Get Action
        //  ((AIToadController)m_AIController).CurrentAction

        ActionStack[0] = 1;
        ActionStack[1] = 2;
        ActionStack[2] = 3;
        ActionStack[3] = 4;
        ActionStack[4] = 5;
        ActionStack[5] = 6;
        ActionStack[6] = 7;

        //Checks if the toad should make a decision
        if (m_AIController.m_MakeDecision == true)
        {
            // Checks to see if the toad should switch behaviours
            if (((AIToadController)m_AIController).m_NumberOfAttacks < Constants.MaxAttacksBeforeBehaviourSwitch)
            {
                // Checks if next action has an action assigned and the current action is none
                if (m_AIController.IsCurrentAction((int)AIToadController.Action.None) && !m_AIController.IsNextAction((int)AIToadController.Action.None))
                {
                    // Make the current action equal to the next action, and set the next action to none
                    m_AIController.SetDecidedAsNextAction();
                    m_AIController.SetNextAction((int)AIToadController.Action.None);
                }

                // If the current action is none
                if (m_AIController.IsCurrentAction((int)AIToadController.Action.None))
                {
                    // If the player is far away
                    if (((AIToadController)m_AIController).GetDistanceToPlayer() > Far)
                    {
                        for (int i = ActionStack.Length; i >= 0; i--)
                        {
                            if (ActionStack[i] == 2 || ActionStack[i] == 6 || ActionStack[i] == 4)
                            {
                                m_AIController.SetAction(ActionStack[i]);
                                SendBackToStack(ActionStack[i]);
                                break;
                            }
                        }
                    }
                    // If the player isin't far or close
                    else if (((AIToadController)m_AIController).GetDistanceToPlayer() < Far && ((AIToadController)m_AIController).GetDistanceToPlayer() > MiddleDistance)
                    {
                        for (int i = ActionStack.Length; i >= 0; i--)
                        {
                            if (ActionStack[i] == 4 || ActionStack[i] == 5 || ActionStack[i] == 7)
                            {
                                m_AIController.SetAction(ActionStack[i]);
                                SendBackToStack(ActionStack[i]);
                                break;
                            }
                        }
                    } // If the player is close
                    else if (((AIToadController)m_AIController).GetDistanceToPlayer() > Close || ((AIToadController)m_AIController).GetDistanceToPlayer() < Close)
                    {
                        for (int i = ActionStack.Length; i >= 0; i--)
                        {
                            if (ActionStack[i] == 7 || ActionStack[i] == 5 || ActionStack[i] == 3)
                            {
                                m_AIController.SetAction(ActionStack[i]);
                                SendBackToStack(ActionStack[i]);
                                break;
                            }
                        }
                    }

                }
            }
            else // If the toad should switch behaviours
            {
                // The toad will change its behaviour and therefore jump on top of the pillars
                m_AIController.SetBehaviour((int)AIToadController.Behaviour.Defensive);
                m_AIController.SetAction((int)AIToadController.Action.SpinHead);
                ((AIToadController)m_AIController).m_NumberOfAttacks = 0;
            }
            m_AIController.m_MakeDecision = false;

        }
    }

    public override void OnActionFinished()
    {
        ((AIToadController)m_AIController).SetAction((int)AIToadController.Action.None);
        ((AIToadController)m_AIController).m_CanAct = false;
    }

    private void SendBackToStack(int Back)
    {
        for (int i = 0; i < ActionStack.Length - 1; i++)
        {
            ActionStack[i + 1] = ActionStack[i];
        }
        ActionStack[0] = Back;
    }
}
