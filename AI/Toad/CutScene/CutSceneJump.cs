using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneJump : AIAction
{

    private Vector3 m_PreJumpPosition;
    private Vector3 m_TargetPosition;

    private Timer m_JumpTimer;
    private Timer JumpDelay;

    private Vector3 m_FirstPoint;
    private Vector3 m_SecondPoint;

    private Vector3 m_CurrentGoal;
    private Vector3 m_LastGoal;

    public CutSceneJump(AIController aAIController) : base(aAIController)
    {
        m_JumpTimer = Services.TimerManager.CreateTimer("CutsceneToadJumpTimer", Constants.JumpDuration, false);
        //JumpDelay = Services.TimerManager.CreateTimer("JumpDelay", 20.0f, false, StartJump);
    }

    // Use this for initialization
    public override void Start()
    {
        ((CutSceneToad)m_AIController).m_Animator.SetTrigger("Toad_Jump");

        // Set the toad position before he jumps
        m_PreJumpPosition = m_AIController.GetPosition();

        // Calculate the toad's jump ID
        ((CutSceneToad)m_AIController).CalculateJumpID();

        // Set the toad's Jump Target
        m_TargetPosition = ((CutSceneToad)m_AIController).m_JumpPosition[((CutSceneToad)m_AIController).m_JumpID].transform.position;

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
        m_JumpTimer.SetDuration(4);

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
         
                // Play Odell jump landing sfx
                Services.AudioManager.PlayOdellSFX(OdellSFX.Odell_Jump_Land);

                // Attack is finished
                ((CutSceneToad)m_AIController).CurrentActionFinished();                   
        }
           
            // Get the player watcher to look at the jump point
            ((CutSceneToad)m_AIController).m_PlayerWatcher.transform.LookAt(((CutSceneToad)m_AIController).m_JumpPosition[((CutSceneToad)m_AIController).m_JumpID].transform.transform);

        // Calculate the angle between the toad and the player watcher
        var angle = Quaternion.Angle(((CutSceneToad)m_AIController).transform.rotation, ((CutSceneToad)m_AIController).m_PlayerWatcher.transform.rotation);

        // If the angle is greater than 0
        if (angle > 0)
        {
            // Get the Quaternion and exclude the y axis
            Quaternion XYRotation = Quaternion.Euler(new Vector3(0f, ((CutSceneToad)m_AIController).transform.localEulerAngles.y, 0f));

            // Look at the player (toad rotation is still screwed so it looks bad)
            //transform.rotation = Quaternion.Lerp(XYRotation, m_PlayerWatcher.transform.rotation, Time.deltaTime * m_TurnSpeed / angle);

            ((CutSceneToad)m_AIController).transform.rotation = Quaternion.Lerp(XYRotation, ((CutSceneToad)m_AIController).m_PlayerWatcher.transform.rotation, Time.deltaTime * Constants.TurnSpeed);
        }
    }

    private void StartJump()
    {
        m_JumpTimer.StartTimer();
    }
}
