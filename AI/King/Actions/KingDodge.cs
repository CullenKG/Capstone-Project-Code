using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingDodge : AIAction
{
    bool m_CanDodgeRight = false;
    bool m_CanDodgeLeft = false;
    bool m_CanDodgeBack = false;

    Vector3 m_TargetPosition;
    Vector3 m_PreMovePosition;

    Timer m_KingDodgeTimer;

    private float DodgeDistance = 5.0f;
    private float DodgeDuration = 1.0f;

    public KingDodge(AIController aAIController) : base(aAIController)
    {
        m_KingDodgeTimer = Services.TimerManager.CreateTimer("m_KingDodgeTimer", DodgeDuration, false);
    }

    // Use this for initialization
    public override void Start()
    {
        // Get the king's starting position
        m_PreMovePosition = ((AIKingController)m_AIController).transform.position;

        // Set the dodges to false
        m_CanDodgeRight = false;
        m_CanDodgeLeft = false;
        m_CanDodgeBack = false;

        // TEMPORARY the king will check to see if he can dodge in the direction 

        // Raycast and see if there is a wall blocking the dodge

        m_TargetPosition = ((AIKingController)m_AIController).transform.position + -(((AIKingController)m_AIController).transform.forward * DodgeDistance);

        // Start the king attack
        ((AIKingController)m_AIController).m_Animator.SetTrigger("Dodge");

        //int DodgeDirection = Random.Range(0, 2);

        //// If the dodge direction is 0 then dodge right
        //if (DodgeDirection == 0)
        //{
        //    m_CanDodgeRight = true;
        //}
        //// Temporary

        //// If the king is dodging right
        //if (m_CanDodgeRight == true)
        //{
        //    // Sets the target's position to the right of the king
        //    m_TargetPosition = ((AIKingController)m_AIController).transform.position + (((AIKingController)m_AIController).transform.right * DodgeDistance);
        //}
        //else
        //{
        //    // Sets the target's position to the left of the king
        //    m_TargetPosition = ((AIKingController)m_AIController).transform.position + -(((AIKingController)m_AIController).transform.right * DodgeDistance);
        //}

        float SmallestDis = 100.0f;
        int index = 0;

        for(int i = 0; i < 3; i++)
        {
           float dis = Vector3.Distance(((AIKingController)m_AIController).m_DodgeChecks[i].transform.position, Services.GameManager.Player.transform.position);

            if(dis < SmallestDis)
            {
                SmallestDis = dis;
                index = i;
            }
        }

        if (index == 0)
        {
            // Sets the target's position to the right of the king
            m_TargetPosition = ((AIKingController)m_AIController).transform.position + (((AIKingController)m_AIController).transform.right * DodgeDistance);
            ((AIKingController)m_AIController).m_Animator.SetInteger("DodgeDirection", 1);
        }

        if (index == 1)
        {
            // Sets the target's position to the left of the king
            m_TargetPosition = ((AIKingController)m_AIController).transform.position + -(((AIKingController)m_AIController).transform.right * DodgeDistance);
            ((AIKingController)m_AIController).m_Animator.SetInteger("DodgeDirection", 2);
        }

        if (index == 2)
        {
            // Sets the target's position to the back of the king
            m_TargetPosition = ((AIKingController)m_AIController).transform.position + -(((AIKingController)m_AIController).transform.forward * DodgeDistance);
            ((AIKingController)m_AIController).m_Animator.SetInteger("DodgeDirection", 3);
        }

        // Start the dodge timer
        m_KingDodgeTimer.Restart();

    }

    // Update is called once per frame
    public override void Update()
    {
        // If the toad is charging
        if (m_KingDodgeTimer.IsRunning())
        {
            // CHARGE!!!!!!
            ((AIKingController)m_AIController).transform.position = Vector3.Lerp(m_PreMovePosition, m_TargetPosition, m_KingDodgeTimer.GetPercentage());
        }

        // If the timer is finished
        if(m_KingDodgeTimer.IsFinished())
        {
            // If the animation is done
            if (((AIKingController)m_AIController).m_Animator.GetCurrentAnimatorStateInfo(0).IsTag("DodgeEnd") && ((AIKingController)m_AIController).m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.98f || ((AIKingController)m_AIController).m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                // Attack is finished
                ((AIKingController)m_AIController).CurrentActionFinished();
            }
        }
    }
}