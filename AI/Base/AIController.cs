using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for AI.
/// </summary>
public class AIController : MonoBehaviour
{
    /// <summary>
    /// List of all the behaviours the AI uses.
    /// </summary>
    protected Dictionary<int, AIBehaviour> m_AIBehaviours = new Dictionary<int, AIBehaviour>();

    /// <summary>
    /// List of all the actions the AI uses.
    /// </summary>
    protected Dictionary<int, AIAction> m_AIActions = new Dictionary<int, AIAction>();

    /// <summary>
    /// The behaviour that is currently happening.
    /// </summary>
    public AIBehaviour CurrentBehaviour { get; protected set; }

    /// <summary>
    /// The action that is currently happening.

    public AIAction CurrentAction { get; protected set; }  // The action that is being updated

    public AIAction PreviousAction { get; protected set; }  // The last action that was executed

    public AIAction ActionBeforeLast { get; protected set; }  // The Action before the previous action

    public AIAction DecidedAction { get; protected set; }   // The action that the behaviours decided will become the current action

    public AIAction NextAction { get; protected set; }    // The action that the behaviours decided will be executed next

    // Determines if the AI can attack (is the cooldown)
    public bool m_CanAct = false;
    public bool m_MakeDecision = false;

    // Determines if the AI will function
    public bool m_AiActive = false;

    public Enemy ThisEnemy { get; private set; }


    // Use this for initialization
    void Start()
    {
        ThisEnemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // If the AI is active, then update
        if (m_AiActive == true)
        {
            if (CurrentBehaviour != null)
            {
                // If the boss should make a decision
                if (m_MakeDecision == true)
                {
                    CurrentBehaviour.Update();
                }
            }

            // Always update the current action
            CurrentAction.Update();
        }
    }

    public void SetPosition(Vector3 aPosition)
    {
        transform.position = aPosition;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetBehaviour(int aBehaviour)
    {
        CurrentBehaviour = m_AIBehaviours[aBehaviour];
        CurrentBehaviour.Start();
    }

    public void SetAction(int aAction)
    {
        CurrentAction = m_AIActions[aAction];
        CurrentAction.Start();
    }

    public void StartAction()
    {
        CurrentAction.Start();
    }

    public void CurrentActionFinished()
    {
        // Set the Action Before Last as the previous action
        ActionBeforeLast = PreviousAction;

        // Set Previous action as current action
        PreviousAction = CurrentAction;

        CurrentBehaviour.OnActionFinished();
    }

    public bool IsCurrentBehaviour(int aBehaviour)
    {
        return CurrentBehaviour == m_AIBehaviours[aBehaviour];
    }

    public bool IsCurrentAction(int aAction)
    {
        return CurrentAction == m_AIActions[aAction];
    }

    // Next action functions
    public bool IsNextAction(int aAction)
    {
        return NextAction == m_AIActions[aAction];
    }

    public void SetNextAction(int aAction)
    {
        NextAction = m_AIActions[aAction];
    }

    public void SetDecidedAsNextAction()
    {
        DecidedAction = NextAction;
    }

    // Previous Action functions
    public bool IsPreviousAction(int aAction)
    {
        return PreviousAction == m_AIActions[aAction];
    }

    public void SetPreviousAction(int aAction)
    {
        PreviousAction = m_AIActions[aAction];
    }

    // Decided Action functions
    public bool IsDecidedAction(int aAction)
    {
        return DecidedAction == m_AIActions[aAction];
    }
    public void SetDecidedAction(int aAction)
    {
        DecidedAction = m_AIActions[aAction];
    }
    public void SetCurrentActionAsDecidedAction()
    {
        CurrentAction = DecidedAction;
        CurrentAction.Start();
    }
    public void SetCurrentActionAsNextAction()
    {
        CurrentAction = NextAction;
        CurrentAction.Start();
    }
}
