using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingLunge : AIAction
{
    Vector3 m_TargetPosition;
    Vector3 m_PreMovePosition;
    Timer m_KingLungeTimer;
    Timer m_KingLungeDelayTimer;

    bool m_Charging;
    bool m_AlreadyHit;

    // Variables that will be constants later
    private float m_ChargeDistanceBehindPlayer = -1.0f;
    float LungeDuration = 0.75f;
    float LungeDelay = 2.25f;
    float PushTimer = 0.75f;

    public KingLunge(AIController aAIController) : base(aAIController)
    {
        m_KingLungeTimer = Services.TimerManager.CreateTimer("m_KingLungeTimer", LungeDuration, false);
        m_KingLungeDelayTimer = Services.TimerManager.CreateTimer("m_KingLungeDelayTimer", LungeDelay, false);
    }

    // Use this for initialization
    public override void Start()
    {
        // Get the king's starting position
        m_PreMovePosition = ((AIKingController)m_AIController).transform.position;

        // Set hit to false
        m_AlreadyHit = false;

        // Set charging to false
        m_Charging = false;

        // Setup the timers
        m_KingLungeDelayTimer.Restart();

        ((AIKingController)m_AIController).m_Animator.SetTrigger("Lunge");
    }

    // Update is called once per frame
    public override void Update()
    {
        // If the delay timer is running
        if (m_KingLungeDelayTimer.IsRunning())
        {
            // If the king can't see the player
            //if (((AIKingController)m_AIController).CanSeePlayer(1) == false)
            //{
                // Turn to the player
                ((AIKingController)m_AIController).TurnToPlayer();
            //}
        }

        // If the delay is done and the king isin't lunging
        if (m_KingLungeDelayTimer.IsFinished() && m_Charging == false)
        {
            // Set charging to true and start the charge timer
            m_Charging = true;
            m_KingLungeTimer.Restart();

            float Distance = (((AIKingController)m_AIController).GetDistanceToPlayer() + m_ChargeDistanceBehindPlayer);

            // If the lunge distance less than 6
            if (Distance < 6.0f)
            {
                // Make it 6
                Distance = 6.0f;
            }

            // Sets the target's position to the Player's position and 
            m_TargetPosition = ((AIKingController)m_AIController).transform.position + (((AIKingController)m_AIController).transform.forward * Distance);
        }

        // If the king is lunging
        if (m_KingLungeTimer.IsRunning() && m_Charging == true)
        {
            // LUNGE!!!!!!
            ((AIKingController)m_AIController).transform.position = Vector3.Lerp(m_PreMovePosition, m_TargetPosition, m_KingLungeTimer.GetPercentage());
        }

        // If the king is done lungeing
        if (m_KingLungeTimer.IsFinished() && m_Charging == true)
        {
            // If the animation is done
            if (((AIKingController)m_AIController).m_Animator.GetCurrentAnimatorStateInfo(0).IsTag("LungeEnd") && ((AIKingController)m_AIController).m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.98f || ((AIKingController)m_AIController).m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                // Attack is finished
                ((AIKingController)m_AIController).CurrentActionFinished();
            }
        }

        // If the king is charging, the player is inside the trigger and he hasn't already hit the player
        if (m_Charging == true && ((AIKingController)m_AIController).m_KingChargeTrigger.m_IsPlayerinside == true && m_AlreadyHit == false)
        {
            // Set hit to true
            m_AlreadyHit = true;

            // Deal Damage to player
            Services.GameManager.Player.TakeDamage(Constants.KingLungeDamage);

            // Set the force
            float Force = 40;

            // Push the player away
            Services.GameManager.Player.MovementController.PushPlayer(((AIKingController)m_AIController).transform.position, Force);
        }
    }
}

