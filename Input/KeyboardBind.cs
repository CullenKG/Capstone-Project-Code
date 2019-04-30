using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardBind
{
    public KeyCode Key { get; set; }

    public KeyboardBind(KeyCode aKey)
    {
        Key = aKey;
    }
    
    public bool GetKey()
    {
        return Input.GetKey(Key);
    }

    public bool GetKeyDown()
    {
        return Input.GetKeyDown(Key);
    }

    public bool GetKeyUp()
    {
        return Input.GetKeyUp(Key);
    }

    public float GetValue()
    {
        return Input.GetKey(Key) ? 1.0f : 0.0f;
    }
}
