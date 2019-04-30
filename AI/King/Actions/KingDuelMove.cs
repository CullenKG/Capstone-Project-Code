using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingDuelMove : AIAction
{
    // Member vars
    public bool m_Moving = false;
    public bool m_MoveRight = false;

    public float m_ClosestDistance;
    public int m_MovePointID;

    public KingDuelMove(AIController aAIController) : base(aAIController)
    {

    }

    // Use this for initialization
    public override void Start()
    {
        //((AIKingController)m_AIController).m_NavMeshAgent.SetDestination(Services.GameManager.Player.gameObject.transform.position);

        // Make sure the nav agent can move
        ((AIKingController)m_AIController).m_NavMeshAgent.isStopped = false;

        // Reset variables
        m_Moving = true;
        m_MoveRight = false;
        m_ClosestDistance = 100.0f;

        // Get the closest move point
        for (int i = 0; i < ((AIKingController)m_AIController).m_CircleMovePositions.Length - 1; i++)
        {
          float Distance = Vector3.Distance(((AIKingController)m_AIController).transform.position, ((AIKingController)m_AIController).m_CircleMovePositions[i].transform.position);

            // If the distance is less than than the closest distance
            if(Distance < m_ClosestDistance)
            {
                // Set the closest distance to the new shortest distance
                m_ClosestDistance = Distance;

                // Save the move id
                m_MovePointID = i;
            }
        }

        // Get a random number to chose a direction to walk in
        int dir = Random.Range(0, 2);

        if(dir == 1)
        {
            // The king will be moving left
            m_MoveRight = true;
            // Add to the move point so the king looks smoother walking
            m_MovePointID++;
        }
        else if(dir == 0)
        {
            // Subtract to the move point so the king looks smoother walking
            m_MovePointID--;
        }

        // Check the move ID to see if its going over or under
        CheckMoveId();
    }

    // Update is called once per frame
    public override void Update()
    {
        // Move towards the move point
        ((AIKingController)m_AIController).m_NavMeshAgent.SetDestination(((AIKingController)m_AIController).m_CircleMovePositions[m_MovePointID].transform.position);

        if(((AIKingController)m_AIController).m_NavMeshAgent.remainingDistance <= 0.5f)
        {
            //FinishAction();

            // If the king is moveing right
            if (m_MoveRight == true)
            {
                m_MovePointID++;
            }
            else if(m_MoveRight == false)
            {
                m_MovePointID--;
            }
        }

        CheckMoveId();

        // If the player is in melee range
        if (((AIKingController)m_AIController).GetDistanceToPlayer() < Constants.MeleeRange)
        {
            m_AIController.SetNextAction((int)AIKingController.Action.Slashing);
            //m_AIController.SetBehaviour((int)AIKingController.Behaviour.Offensive);
            //((AIKingController)m_AIController).m_FireCircle.SetActive(false);

            FinishAction();
        }

        ((AIKingController)m_AIController).m_Animator.SetBool("Walking", m_Moving);
    }

    public void FinishAction()
    {
        // Stop the animator playing the walk animation
        m_Moving = false;
        ((AIKingController)m_AIController).m_Animator.SetBool("Walking", m_Moving);

        // Stop the navmesh from moving
        ((AIKingController)m_AIController).m_NavMeshAgent.isStopped = true;

        // Finish the action
        ((AIKingController)m_AIController).CurrentActionFinished();
    }

    public void CheckMoveId()
    {
        // If the Id is lower or higher adjust accordingly
        if (m_MovePointID > ((AIKingController)m_AIController).m_CircleMovePositions.Length - 1)
        {
            m_MovePointID = 0;
        }
        if (m_MovePointID < 0)
        {
            m_MovePointID = ((AIKingController)m_AIController).m_CircleMovePositions.Length - 1;
        }
    }
   
  }