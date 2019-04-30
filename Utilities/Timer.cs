using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple timer class.
/// Created by Cullen.
/// </summary>
public class Timer : MonoBehaviour
{
    public delegate void TimerCallbackDelegate();

    [SerializeField] private string m_TimerName;
    [SerializeField] private float m_Duration;
    [SerializeField] private float m_TimeLeft;
    [SerializeField] private bool m_IsFinished;
    [SerializeField] private bool m_IsRunning;
    private bool m_OnFinish;


    private bool m_DeleteOnFinish;
    private bool m_HasBeenInitialized;

    private TimerCallbackDelegate m_Callback;

    public string TimerName
    {
        get
        {
            return m_TimerName;
        }

        set
        {
            m_TimerName = value;
        }
    }

    /// <summary>
    /// Delegate to be invoked if the timer is supposed to delete itself upon completion.
    /// </summary>
    public TimerManager.DeleteTimerDelegate SelfDestruct { get; set; }

    /// <param name="name">Name of the timer.</param>
    /// <param name="duration">Duration you want the timer to run for. (Seconds)</param>
    /// <param name="deleteOnFinish">Should the timer delete itself when finished? (Callback function will still be called)</param>
    /// <param name="callback">Delegate (function) to be called when the timer has finished; use null if no function callback is required. (Function must have no parameters)</param>
    public void Initialize(string name, float duration, bool deleteOnFinish, TimerCallbackDelegate callback)
    {
        m_HasBeenInitialized = true;

        TimerName = name;
        m_Duration = duration;
        m_Callback = callback;
        m_DeleteOnFinish = deleteOnFinish;
        m_OnFinish = false;
    }

    private void Update()
    {
        // If the timer is running
        if (m_IsRunning)
        {
            // Subtract deltatime from the time left variable
            m_TimeLeft -= Time.unscaledDeltaTime;

            // Check if there is no more time left on the timer and call the finished function if there isn't
            if (m_TimeLeft <= 0.0f)
                Finished();
        }
    }

    private void Finished()
    {
        m_IsRunning = false;
        m_IsFinished = true;
        m_TimeLeft = 0.0f;

        // Check if the callback delegate exists
        if (m_Callback != null)
        {
            // Invoke the delegate (call the function)
            m_Callback.Invoke();
        }

        // Check to see if the timer is supposed to delete upon completion
        if (m_DeleteOnFinish)
        {
            // Make sure the self destruct delegate has been assigned
            if (SelfDestruct != null)
            {
                // Invoke the delegate (call the function)
                SelfDestruct.Invoke(this);
            }
            else // If the DeleteSelf delegate has not been assigned print an error
            {
                Debug.LogError("Error in Timer::Finished : Delete on finish failed for '" + m_TimerName + "' due to Timer::DeleteSelf delegate never being assigned.");
            }
        }
    }

    /// <summary>
    /// Returns the amount of time left until the timer ends.
    /// </summary>
    /// <returns></returns>
    public float GetTimeLeft()
    {
        if (m_HasBeenInitialized == false)
        {
            Debug.LogError("Error in Timer::GetTimeLeft : Timer '" + TimerName + "' has not been initialized properly before use.");
            return -1.0f;
        }

        return m_TimeLeft;
    }

    public bool IsRunning()
    {
        if (m_HasBeenInitialized == false)
        {
            Debug.LogError("Error in Timer::IsRunning : Timer '" + TimerName + "' has not been initialized properly before use.");
            return false;
        }

        return m_IsRunning;
    }


    /// <summary>
    /// Returns whether the timer has finished running or not.
    /// </summary>
    /// <returns></returns>
    public bool IsFinished()
    {
        if (m_HasBeenInitialized == false)
        {
            Debug.LogError("Error in Timer::HasFinished : Timer '" + TimerName + "' has not been initialized properly before use.");
            return false;
        }

        return m_IsFinished;
    }

    /// <summary>
    /// Pauses the timer.
    /// </summary>
    public void Pause()
    {
        if (m_HasBeenInitialized == false)
        {
            Debug.LogError("Error in Timer::Pause : Timer '" + TimerName + "' has not been initialized properly before use.");
            return;
        }

        m_IsRunning = false;
    }

    /// <summary>
    /// Resumes the timer from its current position.
    /// </summary>
    public void Resume()
    {
        if (m_HasBeenInitialized == false)
        {
            Debug.LogError("Error in Timer::Resume : Timer '" + TimerName + "' has not been initialized properly before use.");
            return;
        }

        if (m_Duration == 0.0f || m_TimeLeft <= 0.0f || m_IsFinished == true)
            return;

        m_IsRunning = true;
    }

    /// <summary>
    /// Start the timer. (Will start timer from beginning, use the Resume function if you wish to continue a timer from its current position)
    /// </summary>
    public void StartTimer()
    {
        Restart();
    }

    /// <summary>
    /// Restarts the timer.
    /// </summary>
    public void Restart()
    {
        if (m_HasBeenInitialized == false)
        {
            Debug.LogError("Error in Timer::Restart : Timer '" + TimerName + "' has not been initialized properly before use.");
            return;
        }

        m_TimeLeft = m_Duration;
        m_IsFinished = false;
        m_IsRunning = true;
        m_OnFinish = false;
    }



    /// <summary>
    /// Stops and resets the timer.
    /// </summary>
    public void Stop()
    {
        if (m_HasBeenInitialized == false)
        {
            Debug.LogError("Error in Timer::Stop : Timer '" + TimerName + "' has not been initialized properly before use.");
            return;
        }

        m_IsRunning = false;
        m_TimeLeft = m_Duration;
        m_IsFinished = false;
    }

    /// <summary>
    /// Get the amount of time since the timer was started.
    /// </summary>
    public float GetTimePassed()
    {
        return m_Duration - m_TimeLeft;
    }

    /// <summary>
    /// Get the percentage the timer has completed so far.
    /// </summary>
    public float GetPercentage()
    {
        return GetTimePassed() / m_Duration;
    }

    /// <summary>
    /// Sets the duration of the timer. (Timer cannot be running)
    /// </summary>
    public void SetDuration(float aDuration)
    {
        if (!m_IsRunning)
            m_Duration = aDuration;
    }

    public bool OnFinish()
    {
        if (m_OnFinish == false)
        {
            if (m_IsFinished == true)
            {
                m_OnFinish = true;
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }
}
