using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAction
{
    protected AIController m_AIController;

    public AIAction(AIController aAIController)
    {
        m_AIController = aAIController;
    }

    public abstract void Start();

    public abstract void Update();
}
