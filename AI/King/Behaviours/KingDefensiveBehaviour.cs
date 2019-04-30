using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingDefensiveBehaviour : AIBehaviour
{
    public KingDefensiveBehaviour(AIController aAIController) : base(aAIController)
    {
        m_AIController = aAIController;
    }

    private float TooFar = 30.0f;
    public override void Start()
    {
        ////Debug.Log("AiSystemWorksOnTheBehaviour");
        //m_AIController.AddAction((int)AIToadController.Action.Jump, new ToadJump(m_AIController));
        //m_AIController.SetAction((int)AIToadController.Action.Jump);
    }

    public override void Update()
    {
        //Checks if the toad can attack
        if (m_AIController.m_MakeDecision == true)
        {

            // Checks to see if the toad should switch behaviours
            if (((AIKingController)m_AIController).m_NumberOfAttacks < (Constants.MaxAttacksBeforeBehaviourSwitch - 5))
            {

               
            }
            else // If the toad should switch behaviours
            {
                // The behaviour will change to aggressive
                m_AIController.SetBehaviour((int)AIKingController.Behaviour.Offensive);
                ((AIKingController)m_AIController).m_NumberOfAttacks = 0;
            }

            m_AIController.m_MakeDecision = false;
        }
    }

    public override void OnActionFinished()
    {
        // If the toad is in the second phase start counting the attacks for phase switchings

        ((AIKingController)m_AIController).SetDecidedAction((int)AIKingController.Action.None);
        ((AIKingController)m_AIController).SetAction((int)AIKingController.Action.None);
        ((AIKingController)m_AIController).m_CanAct = false;
    }
}
