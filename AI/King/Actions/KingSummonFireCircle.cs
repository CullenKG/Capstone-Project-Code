using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSummonFireCircle : AIAction
{
    Timer KingSummonFireCircleTimer;

    bool m_Spawned;

    public KingSummonFireCircle(AIController aAIController) : base(aAIController)
    {
        KingSummonFireCircleTimer = Services.TimerManager.CreateTimer("KingSummonFireCircleTimer", 1.0f, false);
    }

    // Use this for initialization
    public override void Start()
    {
        KingSummonFireCircleTimer.Restart();

        m_Spawned = false;
    }

    // Update is called once per frame
    public override void Update()
    {  
        if(KingSummonFireCircleTimer.GetTimeLeft() < 0.5f && m_Spawned == false)
        {
           // Spawn the fire circle in between the player and king
           Vector3 FireSpawnPoint = (((AIKingController)m_AIController).transform.position + ((AIKingController)m_AIController).m_Player.transform.position) / 2;

            ((AIKingController)m_AIController).m_FireCircle.transform.position = FireSpawnPoint;
            ((AIKingController)m_AIController).m_FireCircle.SetActive(true);

            m_Spawned = true;
        }

        // If the timer is done 
        if (KingSummonFireCircleTimer.IsFinished())
        {
            // Attack is finished
            ((AIKingController)m_AIController).CurrentActionFinished();
        }
    }
}