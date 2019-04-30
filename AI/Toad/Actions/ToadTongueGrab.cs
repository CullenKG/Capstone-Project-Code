//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ToadTongueGrab : AIAction
//{

//    Timer TongueGrabDelayTimer;
//    Timer TongueGrabWaitTimer;
//    Timer TonguePullTimer;
//    Timer TongueDamageTimer;
//    Timer PushTimer;
//    Timer AfterDelayTimer;

//    private Vector3 m_PlayerStartPosition;
//    private Vector3 m_PlayerEndPosition;

//    ToadTongueGrabScript m_TongueGrabScript;

//    bool m_TongueExtended;
//    bool m_Hit;
//    bool m_Pushed;
//    bool m_EndAnimation;

//    public ToadTongueGrab(AIController aAIController) : base(aAIController)
//    {
//        TongueGrabDelayTimer = Services.TimerManager.CreateTimer("TongueGrabDelayTimer", Constants.TongueGrabDelay + 1.0f, false);
//        TongueGrabWaitTimer = Services.TimerManager.CreateTimer("TongueGrabWaitTimer", Constants.TongueGrabDelay, false);
//        TonguePullTimer = Services.TimerManager.CreateTimer("TongueGrabPullTimer", Constants.TonguePullDurration, false);
//        PushTimer = Services.TimerManager.CreateTimer("TonguePushTimer", Constants.PushTimer, false);
//        TongueDamageTimer = Services.TimerManager.CreateTimer("TongueGrabDamageTimer", 1.0f, false);
//        AfterDelayTimer = Services.TimerManager.CreateTimer("AfterDelayTimer", 1.0f, false);
//    }
//    // Use this for initialization
//    public override void Start()
//    {
//        // Find the tongue grab script
//        m_TongueGrabScript = ((AIToadController)m_AIController).m_TongueGrabGameOject.GetComponent<ToadTongueGrabScript>();

//        // Set triggered to false
//        m_TongueGrabScript.SetTriggered(false);

//        // Start the delay timer
//        TongueGrabDelayTimer.StartTimer();

//        // Set the tongue bool
//        m_TongueExtended = false;

//        // Set the hit to false
//        m_Hit = false;

//        m_EndAnimation = false;

//        // Set pushed to false
//        m_Pushed = false;

//        // Set the player's start position
//        m_PlayerStartPosition = Services.GameManager.Player.gameObject.transform.position;

//        // Set the Players end position in front of the toad
//        m_PlayerEndPosition = ((AIToadController)m_AIController).transform.position;

//        // Add an offset so the player will be right in front of the toad
//        m_PlayerEndPosition.z -= 3.0f;
//        m_PlayerEndPosition.y += 0.5f;

//        ((AIToadController)m_AIController).m_Animator.SetTrigger("Toad_Forward_Lick");

//        // Play anounceing sound
//        Services.AudioManager.PlaySFX(SFX.Odell_Croak);

//        // Play tongue grab fire sfx
//        Services.AudioManager.PlaySFX(SFX.Odell_Tongue_Grab_Fire);
//    }

//    // Update is called once per frame
//    public override void Update()
//    {       
//        // If the delay timer is over and the tongue isint extended
//        if (TongueGrabDelayTimer.OnFinish() && m_TongueExtended == false)
//        {
//            // The tongue is extended
//            m_TongueExtended = true;

//            // Start the wait Timer
//            TongueGrabWaitTimer.Restart();

//            // Enable the tongue collider
//            m_TongueGrabScript.EnableTrigger(true);
//        }

//        // If the delay timer is done and the tongue is extended
//        if (AfterDelayTimer.OnFinish() && m_TongueExtended == true)
//        {
//            // Attack is finished
//            ((AIToadController)m_AIController).CurrentActionFinished();
//        }

//        // If the wait timer is done
//        if (TongueGrabWaitTimer.OnFinish())
//        {
//            // If the player wasn't hit
//            if (m_Hit == false)
//            {
//                // Restart the wait timer
//                AfterDelayTimer.Restart();

//                // Disable the tongue collider
//                m_TongueGrabScript.EnableTrigger(false);

//                m_EndAnimation = true;
//            }
//        }

//        // If the player is hit by the tongue
//        if (m_TongueExtended == true && m_TongueGrabScript.Triggered == true && m_Hit == false)
//        {
//            // Loop tongue grab reel
//            Services.AudioManager.SetSFXLooping(true, "Global");
//            // Play grab sfx
//            Services.AudioManager.PlaySFX(SFX.Odell_Tongue_Grab_Fire);

//            // Disable player's movement
//            Services.GameManager.Player.MovementController.m_playerMovementDissabled = true;

//            // Disable camera input
//            Services.GameManager.Player.CameraController.IsInputEnabled = false;
//            // Disable gravity
//            Physics.gravity = new Vector3(0, 0, 0);

//            // Set hit to true
//            m_Hit = true;

//            // Set the toad's hit bool to false
//            ((AIToadController)m_AIController).m_ToadBoss.SetHit(false);

//            // Get the player to lock on to the toad TOOODDOOOO
//            Services.GameManager.Player.CameraController.LockToEnemy();

//            // Start the tongue pull timer
//            TonguePullTimer.StartTimer();

//            // Retract tongue animation
//            m_EndAnimation = true;
//        }

//        // If the tongue got the player 
//        if (TonguePullTimer.IsRunning())
//        {
//            // Pull him to the boss
//            Services.GameManager.Player.gameObject.transform.position = Vector3.Lerp(m_PlayerStartPosition, m_PlayerEndPosition, TonguePullTimer.GetPercentage());
//        }

//        // When the pull is done
//        if (TonguePullTimer.OnFinish())
//        {
//            // Restart the delay timer
//            TongueDamageTimer.Restart();
//        }

//        // If the push Timer is done finish the attack
//        if (PushTimer.OnFinish())
//        {
//            FinishAttack();
//        }

//        // If the player has been grabed and the pull is done
//        if (TonguePullTimer.IsFinished() && m_Hit == true && PushTimer.IsRunning() == false && m_Pushed == false)
//        {
//            // Set the player's position again
//           Services.GameManager.Player.gameObject.transform.position = m_PlayerEndPosition;

//            // If the toad has been hit
//            if (((AIToadController)m_AIController).m_ToadBoss.m_HasBeenHit == true)
//            {
//                // End the attack
//                FinishAttack();
//            }

//            // If the delay finishes before the player hits the toad
//            if (TongueDamageTimer.OnFinish())
//            {
//                // Set pushed
//                m_Pushed = true;

//                // Deal Damage to the player
//                Services.GameManager.Player.TakeDamage(Constants.TongueGrabDamage);

//                // Enable gravity
//                Physics.gravity = new Vector3(0, -9.81f, 0);

//                // Push the player back
//                Services.GameManager.Player.MovementController.PushPlayer(((AIToadController)m_AIController).transform.position, 550, 600);

//                // Start the push Timer
//                PushTimer.StartTimer();             
//            }
//        }
//         ((AIToadController)m_AIController).m_Animator.SetBool("Bool_GrabIsFinished", m_EndAnimation);
//    }

//    private void FinishAttack()
//    {
//        // enable camera input
//        Services.GameManager.Player.CameraController.IsInputEnabled = true;

//        // Disable player lock on
//        Services.GameManager.Player.CameraController.TargetLock = false;

//        // Enable player movement
//        Services.GameManager.Player.MovementController.m_playerMovementDissabled = false;

//        // Enable gravity
//        Physics.gravity = new Vector3(0, -9.81f, 0);

//        // Attack is finished
//        ((AIToadController)m_AIController).CurrentActionFinished();
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadTongueGrab : AIAction
{
    Timer m_ToadTurnTimer;

    private Vector3 m_PreJumpPosition;
    private Vector3 m_TurnHopPosition;

    bool m_ReachedTop;

    public ToadTongueGrab(AIController aAIController) : base(aAIController)
    {
        m_ToadTurnTimer = Services.TimerManager.CreateTimer("m_ToadTurnTimer", 1.0f, false);
    }

    // Use this for initialization
    public override void Start()
    {
        ////Debug.Log("AiSystemWorksFor the jump attack?");
        // Services.AudioManager.PlaySFX(SFX.Odell_Charge);

        m_ToadTurnTimer.Restart();

        m_PreJumpPosition = m_AIController.GetPosition();

        m_TurnHopPosition = m_PreJumpPosition;
        m_TurnHopPosition.y += 10.0f;

        m_ReachedTop = false;

        ((AIToadController)m_AIController).m_Animator.SetTrigger("Toad_Jump");

    }

    // Update is called once per frame
    public override void Update()
    {
        if (m_ReachedTop == false)
        {
            // If the toad is close to the hop point
            if (Vector3.Distance(m_AIController.GetPosition(), m_TurnHopPosition) < 0.05f)
            {
                m_ReachedTop = true;
                m_ToadTurnTimer.Restart();
            }

            // Lerp to the first position
            m_AIController.SetPosition(Vector3.Lerp(m_PreJumpPosition, m_TurnHopPosition, m_ToadTurnTimer.GetPercentage()));
        }

        if (m_ReachedTop == true)
        {
            // If the toad is close to the hop point
            if (Vector3.Distance(m_AIController.GetPosition(), m_PreJumpPosition) < 0.05f)
            {
                // Attack is finished
                ((AIToadController)m_AIController).CurrentActionFinished();
            }

            // Lerp to the first position
            m_AIController.SetPosition(Vector3.Lerp(m_TurnHopPosition, m_PreJumpPosition, m_ToadTurnTimer.GetPercentage()));
        }

            //else
            //{
            // Get the player watcher to look at the Player
            ((AIToadController)m_AIController).m_PlayerWatcher.transform.LookAt(Services.GameManager.Player.gameObject.transform);
        //}

        // If the next action is jump look at the jump point
        //if (m_AIController.IsNextAction((int)AIToadController.Action.Jump))
        //{
        //    // Get the player watcher to look at the jump point
        //    ((AIToadController)m_AIController).m_PlayerWatcher.transform.LookAt(((AIToadController)m_AIController).m_JumpPosition[((AIToadController)m_AIController).m_JumpID].transform.transform);
        //}


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
