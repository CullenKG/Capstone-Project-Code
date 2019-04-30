using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToadCutsceneBehaviour : AIBehaviour
{
    Timer m_FirstPartCamera;
    public ToadCutsceneBehaviour(AIController aAIController) : base(aAIController)
    {
        m_AIController = aAIController;
        
    }

    public override void Start()
    {
    
    }

    public override void Update()
    {
        
    }

    public override void OnActionFinished()
    {
        // Set the action to none
        ((CutSceneToad)m_AIController).SetAction((int)CutSceneToad.Action.None);

        if (((CutSceneToad)m_AIController).m_JumpID < ((CutSceneToad)m_AIController).m_NumberOfJumps - 1)
        {
            ((CutSceneToad)m_AIController).SetNextAction((int)CutSceneToad.Action.Jump);
        }
    }
}
