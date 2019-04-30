using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingBlock : AIAction
{
    public KingBlock(AIController aAIController) : base(aAIController)
    {
        ((AIKingController)m_AIController).m_KingBoss.m_ShieldUp = true;
    }

    // Use this for initialization
    public override void Start()
    {
        ((AIKingController)m_AIController).m_KingBoss.m_ShieldUp = true;

    }

    // Update is called once per frame
    public override void Update()
    {
       // float Force = 200f;
      //  float Radius = 6000;

        ((AIKingController)m_AIController).m_Animator.SetBool("b_KingBlock", ((AIKingController)m_AIController).m_KingBoss.m_ShieldUp);

        // Push the player away
       // Services.GameManager.Player.MovementController.PushPlayer(((AIToadController)m_AIController).transform.position, Force, Radius);
    }
}