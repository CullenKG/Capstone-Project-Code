using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadStomp : AIAction
{
    bool m_Check;
    Vector3 m_TargetPosition;
    Vector3 m_PreJumpPosition;
    Vector3 m_Midpoint;
    Timer StompLandTimer;
    private int m_Count;

    public ToadStomp(AIController aAIController) : base(aAIController)
    {
        StompLandTimer = Services.TimerManager.CreateTimer("StompLandTimer", Constants.StompDurationTimer, false);
    }

    // Use this for initialization
    public override void Start()
    {
        // Set the target position to the players position
        m_TargetPosition = Services.GameManager.Player.gameObject.transform.position;
         m_TargetPosition.y = ((AIToadController)m_AIController).m_JumpPosition[3].transform.position.y;

        // Sets the boss's position before he jumps
        m_PreJumpPosition = ((AIToadController)m_AIController).transform.position;
        m_Count = 1;
        m_Check = false;
        ((AIToadController)m_AIController).m_Animator.SetTrigger("Toad_Jump");

        // Reset the timer duration
        StompLandTimer.SetDuration(Constants.StompDurationTimer);

        // Start the timer
        StompLandTimer.Restart();

        // Calculate the mid point and elevate it
        m_Midpoint = (m_PreJumpPosition + m_TargetPosition) / 2;

        // Raise the mid point to 75% of the jump height
        m_Midpoint.y = Constants.ToadJumpHeight * 0.75f;

        // Play jump sfx
        Services.AudioManager.PlayOdellSFX(OdellSFX.Odell_Jump);
    }

    // Update is called once per frame
    public override void Update()
    {
        // Depending on the count move to the point
        if (m_Count == 1)
        {
            // Move to the mid point
            ((AIToadController)m_AIController).transform.position = Vector3.Lerp(m_PreJumpPosition, m_Midpoint, StompLandTimer.GetPercentage());
        }

        // If the toad has reached the mid point
        if (Vector3.Distance(((AIToadController)m_AIController).transform.position, m_Midpoint) < 0.05f && m_Count == 1)
        {
            // Get the players new position
            m_TargetPosition = Services.GameManager.Player.gameObject.transform.position;
            m_TargetPosition.y = ((AIToadController)m_AIController).m_JumpPosition[3].transform.position.y;

            // Reset the time stamp and add to the count
            m_Count = 2;
            StompLandTimer.Restart();
        }

        if (m_Count == 2)
        {
            // Move to the player's position
            ((AIToadController)m_AIController).transform.position = Vector3.Lerp(m_Midpoint, m_TargetPosition, StompLandTimer.GetPercentage());
        }

        // If the toad is close to landing on the player
        if (Vector3.Distance(((AIToadController)m_AIController).transform.position, m_TargetPosition) < 8.0f)
        {
            ((AIToadController)m_AIController).EnableColliders(false);
        }

        // If the toad reached its final destination
        if (Vector3.Distance(((AIToadController)m_AIController).gameObject.transform.position, m_TargetPosition) < 0.05f && m_Count == 2)
        {
            // If the toad landed pretty close to the player
            if (((AIToadController)m_AIController).GetDistanceToPlayer() < 7.0f)
            {
                // Push the player away
                StompLand();

                // Play Splash sound
                Services.AudioManager.PlayOdellSFX(OdellSFX.Odell_Jump_Land);

                // Change the count so this doesn't repeat
                m_Count = 3;
            }
            else // If not
            {
                // Play Splash sound
                Services.AudioManager.PlayOdellSFX(OdellSFX.Odell_Jump_Land);

                // Attack is finished
                ((AIToadController)m_AIController).CurrentActionFinished();
            }
        }

        // If the toad landed and is close to the player
        if (StompLandTimer.IsFinished() && m_Count == 3)
        {
            // Reenable Player movement
            Services.GameManager.Player.MovementController.m_playerMovementDissabled = false;

            // Reenable the colliders
            ((AIToadController)m_AIController).EnableColliders(true);

            // finish the attack
            ((AIToadController)m_AIController).CurrentActionFinished();
        }
    }

    public void StompLand()
    {
        // Initialize variables
        if (m_Check == false)
        {
            // Disable player movement 
           // Services.GameManager.Player.MovementController.m_playerMovementDissabled = true;

            // Change the duration and Reset the stomp timer
            StompLandTimer.SetDuration(0.75f);
            StompLandTimer.StartTimer();

            // Up the count so this doesn't repeat
            m_Check = true;
        }

        //Debug.Log("Distance to player: " + ((AIToadController)m_AIController).GetDistanceToPlayer());

        // If the toad landed on the player
        if (((AIToadController)m_AIController).GetDistanceToPlayer() > 0.001f && ((AIToadController)m_AIController).GetDistanceToPlayer() <= 2.0f)
        {
            // Set the force and radius
            float Force = 350;
           // float Radius = 3000;

            // Apply damage to the player
            Services.GameManager.Player.TakeDamage(Constants.StompDamage);

            // Push the player away
            Services.GameManager.Player.MovementController.PushPlayer(Services.GameManager.Player.transform.position - ((AIToadController)m_AIController).transform.position, Force);
        }
        // If the toad landed very close to the player
        if (((AIToadController)m_AIController).GetDistanceToPlayer() > 2.0f && ((AIToadController)m_AIController).GetDistanceToPlayer() < 3.0f)
        {
            // Set the force and radius
            float Force = 200;
            //float Radius = 6000;

            Services.GameManager.Player.TakeDamage(Constants.StompDamage);

            // Push the player away
            Services.GameManager.Player.MovementController.PushPlayer(Services.GameManager.Player.transform.position - ((AIToadController)m_AIController).transform.position, Force);
        }
        // If the toad landed pretty close to the player
        if (((AIToadController)m_AIController).GetDistanceToPlayer() > 4.0f && ((AIToadController)m_AIController).GetDistanceToPlayer() < 7.0f)
        {
            // Set the force and radius
            float Force = 100f;
           // float Radius = 6000;
            Services.GameManager.Player.TakeDamage(Constants.StompDamage);

            // Push the player away
            Services.GameManager.Player.MovementController.PushPlayer(Services.GameManager.Player.transform.position - ((AIToadController)m_AIController).transform.position, Force);
        }   
    }
}
