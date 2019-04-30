using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadJump : AIAction
{

    private Vector3 m_PreJumpPosition;
    private Vector3 m_TargetPosition;

    private Timer m_JumpTimer;
    private Timer JumpDelay;

    private Vector3 m_FirstPoint;
    private Vector3 m_SecondPoint;

    private Vector3 m_CurrentGoal;
    private Vector3 m_LastGoal;

    public ToadJump(AIController aAIController) : base(aAIController)
    {
        m_JumpTimer = Services.TimerManager.CreateTimer("ToadJumpTimer", Constants.JumpDuration, false);
        //JumpDelay = Services.TimerManager.CreateTimer("JumpDelay", 20.0f, false, StartJump);
    }

    // Use this for initialization
    public override void Start()
    {
        ((AIToadController)m_AIController).m_Animator.SetTrigger("Toad_Jump");

        // Set the toad position before he jumps
        m_PreJumpPosition = m_AIController.GetPosition();

        // Calculate the toad's jump ID
        ((AIToadController)m_AIController).CalculateJumpID();

        // Set the toad's Jump Target
        m_TargetPosition = ((AIToadController)m_AIController).m_JumpPosition[((AIToadController)m_AIController).m_JumpID].transform.position;

        // Calculate the first point
        m_FirstPoint = (m_PreJumpPosition + m_TargetPosition) / 2;

        // Calculate another point inbetween the first point and the final point
        m_SecondPoint = (m_FirstPoint + m_TargetPosition) / 2;

        // Raise the first point to the jump height
        m_FirstPoint.y = Constants.ToadJumpHeight;

        // Raise the second point to 75% of the jump height
        m_SecondPoint.y = Constants.ToadJumpHeight * 0.75f;

        // Set the current goal to the first point
        m_CurrentGoal = m_FirstPoint;

        // Initialize the last goal.
        m_LastGoal = m_PreJumpPosition;

        // Start the delay timer
        //JumpDelay.StartTimer();

        // Reset the jump duration because it changes for the last 2 lerps
        m_JumpTimer.SetDuration(Constants.JumpDuration);

        // Play jump sfx
        Services.AudioManager.PlayOdellSFX(OdellSFX.Odell_Jump);

        m_JumpTimer.StartTimer();
    }

    // Update is called once per frame
    public override void Update()
    {
        // JUMP CURRENTLY WORKS. DO NOT ADD A TIMER TO DO DELAY

        // Lerp to the next position
        m_AIController.SetPosition(Vector3.Lerp(m_LastGoal, m_CurrentGoal, m_JumpTimer.GetPercentage()));

        // If the toad has reached the mid point
        if (Vector3.Distance(m_AIController.GetPosition(), m_FirstPoint) < 0.05f)
        {
            m_LastGoal = m_CurrentGoal;
            m_CurrentGoal = m_SecondPoint;

            m_JumpTimer.SetDuration(Constants.JumpDuration / 2);
            m_JumpTimer.Restart();
        }

        // If the toad has reached the second point
        if (Vector3.Distance(m_AIController.GetPosition(), m_SecondPoint) < 0.05f)
        {
            m_LastGoal = m_CurrentGoal;
            m_CurrentGoal = m_TargetPosition;

            m_JumpTimer.Restart();
        }

        // If the toad reached its destination
        if (Vector3.Distance(m_AIController.GetPosition(), m_TargetPosition) < 0.05f)
        {
            // If the enemy is going to land on the ground/water

            // If the behaviour is aggressive he'll land in the water
            if (m_AIController.IsCurrentBehaviour((int)AIToadController.Behaviour.Aggressive))
            {
                // Play Odell jump landing sfx
                Services.AudioManager.PlayOdellSFX(OdellSFX.Odell_Jump_Land);

                // Set IsOnPillar to true so he can phase switch in the controller (percaution)
                ((AIToadController)m_AIController).m_IsOnPillar = false;

                // Reenable the colliders
                ((AIToadController)m_AIController).EnableColliders(true);

                // Attack is finished
                ((AIToadController)m_AIController).CurrentActionFinished();
            }

            // If the behaviour is Deffensive he'll land on the pillar
            if (m_AIController.IsCurrentBehaviour((int)AIToadController.Behaviour.Defensive))
            {
                // Play Odell landing on the pillar sound effect
                //Services.AudioManager.PlaySFX(SFX.Odell_Jump_Land);

                // Set IsOnPillar to true so he can phase switch in the controller
                ((AIToadController)m_AIController).m_IsOnPillar = true;

               // Attack is finished
               ((AIToadController)m_AIController).CurrentActionFinished();
            }

        }


            // For turning while jumping

            // Get the player watcher to look at the jump point
            ((AIToadController)m_AIController).m_PlayerWatcher.transform.LookAt(((AIToadController)m_AIController).m_JumpPosition[((AIToadController)m_AIController).m_JumpID].transform.transform);

        // Calculate the angle between the toad and the player watcher
        var angle = Quaternion.Angle(((AIToadController)m_AIController).transform.rotation, ((AIToadController)m_AIController).m_PlayerWatcher.transform.rotation);

        // If the angle is greater than 0
        if (angle > 0)
        {
            // Get the Quaternion and exclude the y axis
            Quaternion XYRotation = Quaternion.Euler(new Vector3(0f, ((AIToadController)m_AIController).transform.localEulerAngles.y, 0f));

            // Look at the player (toad rotation is still screwed so it looks bad)
            //transform.rotation = Quaternion.Lerp(XYRotation, m_PlayerWatcher.transform.rotation, Time.deltaTime * m_TurnSpeed / angle);

            ((AIToadController)m_AIController).transform.rotation = Quaternion.Lerp(XYRotation, ((AIToadController)m_AIController).m_PlayerWatcher.transform.rotation, Time.deltaTime * Constants.TurnSpeed);
        }
    }

    private void StartJump()
    {
        m_JumpTimer.StartTimer();
    }
}
