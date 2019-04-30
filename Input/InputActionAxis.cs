using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputActionAxis
{
    InputActionBind m_PositiveBind;
    InputActionBind m_NegativeBind;

    public InputActionAxis(InputActionBind aPositiveBind, InputActionBind aNegativeBind)
    {
        m_PositiveBind = aPositiveBind;
        m_NegativeBind = aNegativeBind;
    }

    public float GetAxisValue()
    {
        return m_PositiveBind.GetActionValue() + -m_NegativeBind.GetActionValue();
    }

    public float GetAxisRawValue()
    {
        return m_PositiveBind.GetActionRawValue() + -m_NegativeBind.GetActionRawValue();
    }
}
