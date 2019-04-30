using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingMeleeAttack : AIAction
{

    int m_MeleeRandom;
    bool m_HasBeenParried;

    Timer KingMeleeTimer;
    Timer KingPostMeleeTimer;
    Timer KingMeleeLookTimer;

    AttackDirection m_AttackDirection;

    public KingMeleeAttack(AIController aAIController) : base(aAIController)
    {
        // Timer for the kings first half of strike
        KingMeleeTimer = Services.TimerManager.CreateTimer("KingMeleeTimer", 1.0f, false);

        // Timer for either the follow through swing or parry
        KingPostMeleeTimer = Services.TimerManager.CreateTimer("KingPostMeleeTimer", 1.0f, false);

        // Timer that lets the king turn to the player while its running
        KingMeleeLookTimer = Services.TimerManager.CreateTimer("KingMeleeLookTimer", 0.3f, false);
    }

    // Use this for initialization
    public override void Start()
    {
        KingMeleeTimer.Restart();
        KingMeleeLookTimer.Restart();

        // Enable the sword collider
        ((AIKingController)m_AIController).m_KingSwordCollider.enabled = true;

        // Set parry to false
        m_HasBeenParried = false;
        ((AIKingController)m_AIController).m_KingParryCube.m_HasBeenParried = false;

        // Enable the parry cube 
        ((AIKingController)m_AIController).m_KingParryCube.enabled = true;

        // Start the king attack
        ((AIKingController)m_AIController).m_Animator.SetTrigger("Attack");

        // Get a random melee direction
        m_MeleeRandom = Random.Range(1, 4);

        if (m_MeleeRandom == 1)
        {
            m_AttackDirection = AttackDirection.North;
            ((AIKingController)m_AIController).m_Animator.SetInteger("AttackDirection", m_MeleeRandom);
        }
        if (m_MeleeRandom == 2)
        {
            m_AttackDirection = AttackDirection.NorthWest;
            ((AIKingController)m_AIController).m_Animator.SetInteger("AttackDirection", m_MeleeRandom);
        }
        if (m_MeleeRandom == 3)
        {
            m_AttackDirection = AttackDirection.West;
            ((AIKingController)m_AIController).m_Animator.SetInteger("AttackDirection", m_MeleeRandom);
        }

        // Reference for later
        // ((AIKingController)m_AIController).m_Animator.GetCurrentAnimatorStateInfo(0).IsName("t_Vertical_Swing");        
        // Disable the sword collider if an attack animation is not running.
        // if (m_Player.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
    }

    // Update is called once per frame
    public override void Update()
    {
        //((AIToadController)m_AIController).m_Animator.SetTrigger("t_Vertical_Swing");

        // If the melee direction is 1
        //if (m_MeleeRandom == 1)
        //{
        //    // If the animation is done
        //    if (((AIKingController)m_AIController).m_Animator.GetCurrentAnimatorStateInfo(0).IsName("NorthAttackRecoil") || ((AIKingController)m_AIController).m_Animator.GetCurrentAnimatorStateInfo(0)
        //        .IsName("NorthAttackFollow") && ((AIKingController)m_AIController).m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        //    {
        //        // Disable the sword collider
        //        ((AIKingController)m_AIController).m_KingSwordCollider.enabled = false;

        //        // Finish the action
        //        ((AIKingController)m_AIController).CurrentActionFinished();

        //        // Update the animator
        //        ((AIKingController)m_AIController).m_Animator.SetBool("Parried", false);
        //    }
        //}

        //if (m_MeleeRandom == 2)
        //{
        //    // If the animation is done
        //    if (((AIKingController)m_AIController).m_Animator.GetCurrentAnimatorStateInfo(0).IsTag("AttackEnd") && ((AIKingController)m_AIController).m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
        //    {
        //        // Disable the sword collider
        //        ((AIKingController)m_AIController).m_KingSwordCollider.enabled = false;

        //        // Finish the action
        //        ((AIKingController)m_AIController).CurrentActionFinished();

        //        // Update the animator
        //        ((AIKingController)m_AIController).m_Animator.SetBool("Parried", false);
        //    }
        //}

        //if (m_MeleeRandom == 3)
        //{
        //    // If the animation is done
        //    if (((AIKingController)m_AIController).m_Animator.GetCurrentAnimatorStateInfo(0).IsTag("AttackEnd") && ((AIKingController)m_AIController).m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
        //    {
        //        // Disable the sword collider
        //        ((AIKingController)m_AIController).m_KingSwordCollider.enabled = false;

        //        // Finish the action
        //        ((AIKingController)m_AIController).CurrentActionFinished();

        //        // Update the animator
        //        ((AIKingController)m_AIController).m_Animator.SetBool("Parried", false);
        //    }
        //}

        // If the animation is done
        if (((AIKingController)m_AIController).m_Animator.GetCurrentAnimatorStateInfo(0).IsTag("AttackEnd") && ((AIKingController)m_AIController).m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.98f || ((AIKingController)m_AIController).m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            // Disable the sword collider
            ((AIKingController)m_AIController).m_KingSwordCollider.enabled = false;

            // Finish the action
            ((AIKingController)m_AIController).CurrentActionFinished();

            // Update the animator
            ((AIKingController)m_AIController).m_Animator.SetBool("Parried", false);
        }

        // If the kings look timer is running (make sure the king is actually aiming at the player
        if (KingMeleeLookTimer.IsRunning())
        {
            // If the king cannot see the player
            if (((AIKingController)m_AIController).CanSeePlayer(1) == false)
            {
                // Then turn to the player
                ((AIKingController)m_AIController).TurnToPlayer();
            }
        }

        // If the king was parried
        if (((AIKingController)m_AIController).m_KingParryCube.m_HasBeenParried == true)
        {
            if (Services.GameManager.Player.CombatManager.CurrentAttackDirection == m_AttackDirection)
            {
                // Set parried to true
                m_HasBeenParried = true;

                // Update the animator
                ((AIKingController)m_AIController).m_Animator.SetBool("Parried", m_HasBeenParried);

                // Turn off the sword collider
                ((AIKingController)m_AIController).m_KingSwordCollider.enabled = false;
            }

            // Set the parry cube parried to false
            ((AIKingController)m_AIController).m_KingParryCube.m_HasBeenParried = false;
        }
    }
}