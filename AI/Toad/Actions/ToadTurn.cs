using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadTurn : AIAction
{
    Timer m_ToadTurnTimer;

    private Vector3 m_PreJumpPosition;
    private Vector3 m_TurnHopPosition;

    bool m_ReachedTop;

    public ToadTurn(AIController aAIController) : base(aAIController)
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
            }
        }

        if(m_ReachedTop == true)
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



        // If the next action isn't jump look at the player
        if (!m_AIController.IsNextAction((int)AIToadController.Action.Jump))
        {
            // Get the player watcher to look at the Player
            ((AIToadController)m_AIController).m_PlayerWatcher.transform.LookAt(Services.GameManager.Player.gameObject.transform);
        }
        else // If jump is the next action
        { 
            // Get the player watcher to look at the jump point
            ((AIToadController)m_AIController).m_PlayerWatcher.transform.LookAt(((AIToadController)m_AIController).m_JumpPosition[((AIToadController)m_AIController).m_JumpID].transform.transform);
        }


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
}

// LEGACY TURN

//public void TurnToPlayer()
//{
//    // Get the player watcher to look at the player
//    m_PlayerWatcher.transform.LookAt(Services.GameManager.Player.gameObject.transform);

//    // Calculate the angle between the toad and the player watcher
//    var angle = Quaternion.Angle(transform.rotation, m_PlayerWatcher.transform.rotation);

//    // If the angle is greater than 0
//    if (angle > 0)
//    {
//        // Get the Quaternion and exclude the y axis
//        Quaternion XYRotation = Quaternion.Euler(new Vector3(0f, transform.localEulerAngles.y, 0f));

//        // Look at the player (toad rotation is still screwed so it looks bad)
//        //transform.rotation = Quaternion.Lerp(XYRotation, m_PlayerWatcher.transform.rotation, Time.deltaTime * m_TurnSpeed / angle);

//        transform.rotation = Quaternion.Lerp(XYRotation, m_PlayerWatcher.transform.rotation, Time.deltaTime * Constants.TurnSpeed);
//    }
//}
//public void TurnToJumpPosition()
//{
//    // Get the player watcher to look at the jump position
//    m_PlayerWatcher.transform.LookAt(m_JumpPosition[m_JumpID].transform);

//    // Calculate the angle between the toad and the player watcher
//    var angle = Quaternion.Angle(transform.rotation, m_JumpPosition[m_JumpID].transform.rotation);

//    // If the angle is greater than 0
//    if (angle > 0)
//    {
//        // Get the Quaternion and exclude the y axis
//        Quaternion XYRotation = Quaternion.Euler(new Vector3(0f, transform.localEulerAngles.y, 0f));

//        // Look at the player (toad rotation is still screwed so it looks bad)
//        //transform.rotation = Quaternion.Lerp(XYRotation, m_PlayerWatcher.transform.rotation, Time.deltaTime * m_TurnSpeed / angle);

//        transform.rotation = Quaternion.Lerp(XYRotation, m_PlayerWatcher.transform.rotation, Time.deltaTime * Constants.TurnSpeed);
//    }
//}
