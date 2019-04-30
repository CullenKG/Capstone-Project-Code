using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadTongueWhip : AIAction
{
    bool m_BeenParried;
    bool m_TongueWhipIsFinished;

    Timer ToadTongueWhipTimer;
    Timer ToadTongueWhipParryEnd;
    Timer ToadTongueWhipEnd;

    int m_AnimDirection;

    AttackDirection m_AttackDirection;

    public ToadTongueWhip(AIController aAIController) : base(aAIController)
    {
        ToadTongueWhipTimer = Services.TimerManager.CreateTimer("TongueParryTimer", 2.0f, false);
        ToadTongueWhipParryEnd = Services.TimerManager.CreateTimer("TongueParryEndTimer", 1.7f, false);
        ToadTongueWhipEnd = Services.TimerManager.CreateTimer("TongueWhipEnd", 4, false);
    }

    // Use this for initialization
    public override void Start()
    {
        // Get an attack direction
        m_AnimDirection = Random.Range(1, 5);

        if (m_AnimDirection == 1)
        {
            m_AttackDirection = AttackDirection.SouthWest;
        }
        if (m_AnimDirection == 2)
        {
            m_AttackDirection = AttackDirection.West;
        }
        if (m_AnimDirection == 3)
        {
            m_AttackDirection = AttackDirection.East;
        }
        if (m_AnimDirection == 4)
        {
            m_AttackDirection = AttackDirection.NorthWest;
        }

        m_TongueWhipIsFinished = false;

        // Set parried to false
        ((AIToadController)m_AIController).m_ToadTongueWhipCube.m_HasBeenParried = false;
        m_BeenParried = false;

        // Turn the tongue whip cube off
        ((AIToadController)m_AIController).m_ToadTongueWhipCube.gameObject.SetActive(false);

        // Start the timers
        ToadTongueWhipTimer.StartTimer();
        ToadTongueWhipEnd.StartTimer();

        // Turn the whip colliders on
        for (int i = 0; i < ((AIToadController)m_AIController).m_WhipColliders.Count; i++)
        {
            ((AIToadController)m_AIController).m_WhipColliders[i].gameObject.SetActive(true);
        }
        // Play tongue slash sfx
        Services.AudioManager.PlayOdellSFX(OdellSFX.Odell_Tongue_Slash);
    }

    // Update is called once per frame
    public override void Update()
    {
        if (ToadTongueWhipTimer.OnFinish())
        {
            // Turn the parry cube on
            ((AIToadController)m_AIController).m_ToadTongueWhipCube.gameObject.SetActive(true);
            // Start the end timer
            ToadTongueWhipParryEnd.StartTimer();

            // Enable the whipcoliders
            for (int i = 0; i < ((AIToadController)m_AIController).m_WhipColliders.Count; i++)
            {
                ((AIToadController)m_AIController).m_WhipColliders[i].gameObject.SetActive(false);
            }
        }
        // If the end timer is done
        if (ToadTongueWhipParryEnd.OnFinish())
        {
            // Disable the parry cube and make sure parried is false
            ((AIToadController)m_AIController).m_ToadTongueWhipCube.m_HasBeenParried = false;
            ((AIToadController)m_AIController).m_ToadTongueWhipCube.gameObject.SetActive(false);
            m_TongueWhipIsFinished = true;
        }

        // If the attack is completely finished
        if (ToadTongueWhipEnd.OnFinish())
        {
            m_AnimDirection = 0;
            m_BeenParried = false;

            // Disable the whipcoliders
            for (int i = 0; i < ((AIToadController)m_AIController).m_WhipColliders.Count; i++)
            {
                ((AIToadController)m_AIController).m_WhipColliders[i].gameObject.SetActive(false);
            }
          
            ((AIToadController)m_AIController).m_ToadTongueWhipCube.m_HasBeenParried = false;
            ((AIToadController)m_AIController).m_ToadTongueWhipCube.gameObject.SetActive(false);
            ((AIToadController)m_AIController).CurrentActionFinished();
        }

        // If the attack was parried
        if (((AIToadController)m_AIController).m_ToadTongueWhipCube.m_HasBeenParried == true)
        {
            if (Services.GameManager.Player.CombatManager.CurrentAttackDirection == m_AttackDirection)
            {
                m_BeenParried = true;

                // Turn off the colliders
                for (int i = 0; i < ((AIToadController)m_AIController).m_WhipColliders.Count; i++)
                {
                    ((AIToadController)m_AIController).m_WhipColliders[i].gameObject.SetActive(false);
                }

                // Set the parry cube's BeenParried to false
                ((AIToadController)m_AIController).m_ToadTongueWhipCube.m_HasBeenParried = false;
            }
            else
            {
                ((AIToadController)m_AIController).m_ToadTongueWhipCube.m_HasBeenParried = false;
            }
        }
           
        //Debug.Log(m_TongueWhipIsFinished);

        // Update the animator
        ((AIToadController)m_AIController).m_Animator.SetInteger("Int_ToadAttackDirection", m_AnimDirection);
        ((AIToadController)m_AIController).m_Animator.SetBool("Bool_WhipIsFInished", m_TongueWhipIsFinished);
        ((AIToadController)m_AIController).m_Animator.SetBool("Bool_Parry", m_BeenParried);
    }
}