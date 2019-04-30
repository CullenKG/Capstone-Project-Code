using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingShieldCharge : AIAction
{
    Vector3 m_TargetPosition;
    Vector3 m_PreMovePosition;
    Timer m_KingChargeTimer;
    Timer m_KingChargeDelayTimer;

    bool m_Charging;
    bool m_AlreadyHit;

    // Variables that will be constants later
    private float m_ChargeDistanceBehindPlayer = 2.5f;
    float ChargeDuration = 1.0f;
    float ChargeDelay = 0.75f;
    float PushTimer = 0.75f;

    public KingShieldCharge(AIController aAIController) : base(aAIController)
    {
        m_KingChargeTimer = Services.TimerManager.CreateTimer("m_KingChargeTimer", ChargeDuration, false);
        m_KingChargeDelayTimer = Services.TimerManager.CreateTimer("m_KingChargeDelayTimer", ChargeDelay, false);
    }

    // Use this for initialization
    public override void Start()
    {
        //Services.AudioManager.PlaySFX(SFX.Odell_Charge);

        // Get the king's starting position
        m_PreMovePosition = ((AIKingController)m_AIController).transform.position;

        // Set hit and charging to false
        m_AlreadyHit = false;
        m_Charging = false;

        // Setup the timers
        m_KingChargeDelayTimer.Restart();

        // Start the animation
        ((AIKingController)m_AIController).m_Animator.SetTrigger("Charge");

    }

    // Update is called once per frame
    public override void Update()
    {
        // If the delay timer is running
        if(m_KingChargeDelayTimer.IsRunning() == true)
        {
            // Turn to look at the player
            ((AIKingController)m_AIController).TurnToPlayer();
        }

        // If the delay is done and the king isin't charging
        if (m_KingChargeDelayTimer.IsFinished() && m_Charging == false)
        {
            // Set charging to true and start the charge timer
            m_Charging = true;
            m_KingChargeTimer.Restart();

            // Sets the target's position to the Player's position and 
            m_TargetPosition = ((AIKingController)m_AIController).transform.position + (((AIKingController)m_AIController).transform.forward * (((AIKingController)m_AIController).GetDistanceToPlayer() + m_ChargeDistanceBehindPlayer));
        }

        // If the king is charging
        if (m_KingChargeTimer.IsRunning() && m_Charging == true)
        {
            // CHARGE!!!!!!
            ((AIKingController)m_AIController).transform.position = Vector3.Lerp(m_PreMovePosition, m_TargetPosition, m_KingChargeTimer.GetPercentage());
        }

        // If the king is done charging
        if (m_KingChargeTimer.IsFinished() && m_Charging == true)
        {
            // If the animation is done
            if (((AIKingController)m_AIController).m_Animator.GetCurrentAnimatorStateInfo(0).IsTag("ShieldEnd") && ((AIKingController)m_AIController).m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.98f || ((AIKingController)m_AIController).m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                // Attack is finished
                ((AIKingController)m_AIController).CurrentActionFinished();
            }
        }

        // If the king is charging, hit the player and hasn't already hit the player
        if (m_Charging == true && ((AIKingController)m_AIController).m_KingChargeTrigger.m_IsPlayerinside == true && m_AlreadyHit == false)
        {
            // Set hit to true
            m_AlreadyHit = true;

            // Deal Damage to player
            Services.GameManager.Player.TakeDamage(Constants.KingSheildChargeDamage);

            // Set the force and radius
            float Force = 100;

            // Push the player away
            Services.GameManager.Player.MovementController.PushPlayer(((AIKingController)m_AIController).transform.position, Force);
        }
    }
}