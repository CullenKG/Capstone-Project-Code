using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadBuck : AIAction
{
    Timer m_ToadTurnTimer;

    private Vector3 m_PreJumpPosition;
    private Vector3 m_TurnHopPosition;

    bool m_ReachedTop;

    public ToadBuck(AIController aAIController) : base(aAIController)
    {
        m_ToadTurnTimer = Services.TimerManager.CreateTimer("m_ToadTurnTimer", 0.75f, false);
    }

    // Use this for initialization
    public override void Start()
    {
        ////Debug.Log("AiSystemWorksFor the jump attack?");
        // Services.AudioManager.PlaySFX(SFX.Odell_Charge);

        m_ToadTurnTimer.Restart();

        m_PreJumpPosition = m_AIController.GetPosition();

        m_TurnHopPosition = m_PreJumpPosition;
        m_TurnHopPosition.y += 3.0f;

        m_ReachedTop = false;

        // ((AIToadController)m_AIController).m_Animator.SetTrigger("Toad_Jump");

       // Debug.Log("Turn Start");
    }

    // Update is called once per frame
    public override void Update()
    {
        //Debug.Log("Turn Updates");

        if (m_ReachedTop == false)
        {        
            // Lerp to the first position
            m_AIController.SetPosition(Vector3.Lerp(m_PreJumpPosition, m_TurnHopPosition, m_ToadTurnTimer.GetPercentage()));

            //Debug.Log("Going Up");

            // If the toad is close to the hop point
            if (Vector3.Distance(m_AIController.GetPosition(), m_TurnHopPosition) < 0.05f)
            {
                m_ReachedTop = true;
                m_ToadTurnTimer.Restart();

                // If the player is still riding the toad
                if(((AIToadController)m_AIController).m_PlayerRider.m_IsPlayerinside == true)
                {
                    // Push the player away
                    Services.GameManager.Player.MovementController.PushPlayer(Services.GameManager.Player.transform.position - ((AIToadController)m_AIController).transform.position, 250.0f);
                }
            }
        }

        if (m_ReachedTop == true)
        {
            // Lerp to the first position
            m_AIController.SetPosition(Vector3.Lerp(m_TurnHopPosition, m_PreJumpPosition, m_ToadTurnTimer.GetPercentage()));

            // Debug.Log("Going Down");

            // If the toad is close to the ground
            if (Vector3.Distance(m_AIController.GetPosition(), m_PreJumpPosition) < 0.05f)
            {
                // Attack is finished
                ((AIToadController)m_AIController).CurrentActionFinished();
            }
        }
    }
}