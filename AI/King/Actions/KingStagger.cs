using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingStagger : AIAction
{
    Timer KingStaggerTimer;

    public KingStagger(AIController aAIController) : base(aAIController)
    {
        KingStaggerTimer = Services.TimerManager.CreateTimer("KingStaggerTimer", 1.0f, false);
    }

    // Use this for initialization
    public override void Start()
    {
        KingStaggerTimer.Restart();

    }

    // Update is called once per frame
    public override void Update()
    {  
         // If the timer is done 
        if(KingStaggerTimer.IsFinished())
        {
            // Attack is finished
            ((AIKingController)m_AIController).CurrentActionFinished();
        }
    }
}