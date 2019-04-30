using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBehaviour
{
    protected AIController m_AIController; 
    protected AIBehaviour(AIController aAIController)
    {
        m_AIController = aAIController;
    }

    public abstract void Start();

    public abstract void OnActionFinished();

    public abstract void Update();
}
