using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingTurn : AIAction
{
    Timer KingTurnTimer;

    public KingTurn(AIController aAIController) : base(aAIController)
    {
        KingTurnTimer = Services.TimerManager.CreateTimer("KingTurnTimer", Constants.KingTurnTimer, false);
    }

    // Use this for initialization
    public override void Start()
    {
        KingTurnTimer.Restart();

    }

    // Update is called once per frame
    public override void Update()
    {
        // WILL ONLY LOOK AT THE PLAYER


        // Get the player watcher to look at the player
        ((AIKingController)m_AIController).m_PlayerWatcher.transform.LookAt(Services.GameManager.Player.gameObject.transform);

            // Calculate the angle between the toad and the player watcher
            var angle = Quaternion.Angle(((AIKingController)m_AIController).transform.rotation, ((AIKingController)m_AIController).m_PlayerWatcher.transform.rotation);

            // If the angle is greater than 0
            if (angle > 0)
            {
                // Get the Quaternion and exclude the y axis
                Quaternion XYRotation = Quaternion.Euler(new Vector3(0f, ((AIKingController)m_AIController).transform.localEulerAngles.y, 0f));


            ((AIKingController)m_AIController).transform.rotation = Quaternion.Lerp(XYRotation, ((AIKingController)m_AIController).m_PlayerWatcher.transform.rotation, Time.deltaTime * Constants.TurnSpeed);
            }
        
         // If the timer is done 
        if(KingTurnTimer.IsFinished())
        {
            // Attack is finished
            ((AIKingController)m_AIController).CurrentActionFinished();
        }
    }
}