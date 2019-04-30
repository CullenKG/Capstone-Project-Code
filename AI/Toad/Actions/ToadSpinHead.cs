using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadSpinHead : AIAction
{

    private int m_HeadSpinsRemaining = 0;
    private int m_MaxHeadSpins = 2;
    private int m_ToadLipPosition = 0;
    private int m_ToadFacePosition = 0;
    private Timer m_HeadSpinTimer;
    public ToadSpinHead(AIController aAIController) : base(aAIController)
    {
        m_HeadSpinTimer = Services.TimerManager.CreateTimer("m_HeadSpinTimer", Constants.HeadSpinDurationTimer, false);
    }

    // Use this for initialization
    public override void Start()
    {
        ////Debug.Log("AiSystemWorksFor the jump attack?");

        m_HeadSpinTimer.StartTimer();

        m_MaxHeadSpins = 3;
        // Set the head spins remaining
        m_HeadSpinsRemaining = m_MaxHeadSpins;

        m_ToadLipPosition = Random.Range(0, 7);
    }

    // Update is called once per frame
    public override void Update()
    {
        // Turn to the player
        //((AIToadController)m_AIController).TurnToPlayer();

        // If the timer is finished
        if (m_HeadSpinTimer.IsFinished())
        {
            // If there are head spins remaining
            if (m_HeadSpinsRemaining >= 2)
            {
                // Generate a random position for both the head and lips
                m_ToadLipPosition = Random.Range(0, 7);
                m_ToadFacePosition = Random.Range(0, 7);
            }
            else if (m_HeadSpinsRemaining == 1)
            {
                // If this is the last head spin then make sure both the lips and the face rotate to the same position
                m_ToadLipPosition = Random.Range(0, 7);
            }
            else if (m_HeadSpinsRemaining == 0)
            {
                // After the player has attacked the aligned face and lips reset both to thier orginal rotation
                m_ToadLipPosition = 0;
            }

            // Subtract a head spin and reset the timer
            m_HeadSpinsRemaining--;
            m_HeadSpinTimer.Restart();
        }

        // Set the spins duration
        
        // Calculate the spin progress
        float progress =  m_HeadSpinTimer.GetPercentage();

        // Depending on the randomized position rotate to the defined position
        ((AIToadController)m_AIController).m_ToadLips.transform.localRotation = Quaternion.Slerp(((AIToadController)m_AIController).m_ToadLips.transform.localRotation, Quaternion.Euler(new Vector3(((AIToadController)m_AIController).m_ToadLips.transform.localRotation.x,
        ((AIToadController)m_AIController).m_ToadLips.transform.localRotation.y, m_ToadLipPosition * 45.0f)), progress);


        //// Lip position if statements
        //if (m_ToadLipPosition == 0)
        //{
        //    m_AIController.ThisEnemy.m_DamageDirection = AttackDirection.DirectionEast;
        //    m_AIController.ThisEnemy.m_SecondaryDirection = AttackDirection.DirectionWest;
        //}
        //else if (m_ToadLipPosition == 1)
        //{
        //    m_AIController.ThisEnemy.m_DamageDirection = AttackDirection.DirectionNorthWest;
        //    m_AIController.ThisEnemy.m_SecondaryDirection = AttackDirection.DirectionSouthEast;
        //}
        //else if (m_ToadLipPosition == 2)
        //{
        //    m_AIController.ThisEnemy.m_DamageDirection = AttackDirection.DirectionNorth;
        //    m_AIController.ThisEnemy.m_SecondaryDirection = AttackDirection.DirectionSouth;
        //}
        //else if (m_ToadLipPosition == 3)
        //{
        //    m_AIController.ThisEnemy.m_DamageDirection = AttackDirection.DirectionNorthEast;
        //    m_AIController.ThisEnemy.m_SecondaryDirection = AttackDirection.DirectionSouthWest;
        //}
        //else if (m_ToadLipPosition == 4)
        //{
        //    m_AIController.ThisEnemy.m_DamageDirection = AttackDirection.DirectionEast;
        //    m_AIController.ThisEnemy.m_SecondaryDirection = AttackDirection.DirectionWest;
        //}
        //else if (m_ToadLipPosition == 5)
        //{
        //    m_AIController.ThisEnemy.m_DamageDirection = AttackDirection.DirectionNorthWest;
        //    m_AIController.ThisEnemy.m_SecondaryDirection = AttackDirection.DirectionSouthEast;
        //}
        //else if (m_ToadLipPosition == 6)
        //{
        //    m_AIController.ThisEnemy.m_DamageDirection = AttackDirection.DirectionNorth;
        //    m_AIController.ThisEnemy.m_SecondaryDirection = AttackDirection.DirectionSouth;
        //}
        //else if (m_ToadLipPosition == 7)
        //{
        //    m_AIController.ThisEnemy.m_DamageDirection = AttackDirection.DirectionNorthEast;
        //    m_AIController.ThisEnemy.m_SecondaryDirection = AttackDirection.DirectionSouthWest;
        //}


        // If all the head spins are spent and the both are rotated back to thier original position
        if (m_HeadSpinsRemaining == 0 && progress >= 1.0f)
        {
            //m_AIController.ThisEnemy.m_DamageDirection = AttackDirection.DirectionAny;

            // Attack is finished
            ((AIToadController)m_AIController).CurrentActionFinished();

        }
    }
}
