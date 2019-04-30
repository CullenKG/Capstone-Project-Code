using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to create and manage timers.
/// Created by Cullen.
/// </summary>
public class TimerManager : MonoBehaviour
{
    public delegate void DeleteTimerDelegate(Timer timer);

    /// <summary>
    /// List of all active timers.
    /// </summary>
    Dictionary<string, Timer> m_Timers = new Dictionary<string, Timer>();

    /// <summary>
    /// The total number of timers the TimerManager has created during its lifetime.
    /// </summary>
    public int NumberOfTimersEverCreated { get; private set; }

    /// <summary>
    /// Creates a timer.
    /// </summary>
    /// <param name="aName">Name of the timer.</param>
    /// <param name="aDuration">Duration you want the timer to run for. (Seconds)</param>
    /// <param name="aCallback">Delegate (function) to be called when the timer has finished. Null if no function callback is required. (Function must have 0 parameters)</param>
    /// <returns>Returns the timer that was created.</returns>
    public Timer CreateTimer(string name, float duration, bool deleteOnFinish, Timer.TimerCallbackDelegate callback = null)
    {
        // Check to see if the name is already being used
        //if (m_Timers.ContainsKey(name))
        //{           
        //    Debug.LogError("Error in TimerManager::CreateTimer : Failed to create timer due to name being in use.");
        //    return m_Timers[name];
        //}

        // Create the timer and initialize it with the provided values
        Timer timer = gameObject.AddComponent<Timer>();
        timer.Initialize(name, duration, deleteOnFinish, callback);

        // Check if the timer needs to delete itself upon completion
        if (deleteOnFinish)
        {
            // Set the DeleteSelf delegate so the timer can call the DeleteTimer function when it has finished running
            timer.SelfDestruct = DeleteTimer;
        }

        // Add the timer to the dictionary
        m_Timers.Add(name + NumberOfTimersEverCreated, timer);

        // Increment the number of timers created so far
        NumberOfTimersEverCreated++;

        // Return the timer we just created
        return timer;
    }

    /// <summary>
    /// Remove a timer from existence.
    /// </summary>
    /// <param name="timer">The timer you would like to delete.</param>
    public void DeleteTimer(Timer timer)
    {
        // Make sure the timer exists in our dictionary
        if (m_Timers.ContainsValue(timer))
        {
            // Remove the timer from the dictionary and destroy it
            m_Timers.Remove(timer.TimerName);
            Destroy(timer);
        }
        else // If the timer does not exist print an error
        {
            Debug.LogError("Error in TimerManager::DeleteTimer : Provided timer not found in list.");
        }
        
    }

    /// <summary>
    /// Get a timer using its name.
    /// </summary>
    /// <param name="name">The name of the timer you would like to find. Returns null if not found.</param>
    /// <returns></returns>
    public Timer GetTimerByName(string name)
    {
        // Make sure the provided name exists in our dictionary
        if (m_Timers.ContainsKey(name))
        {
            // Return the timer that was found
            return m_Timers[name];
        }
        else // If the timer cannot be found print an error
        {
            Debug.LogError("Error in TimerManager::GetTimerByName : Provided name not found in list.");
            return null;
        }   
    }

    /// <summary>
    /// Remove a timer from existence using its name.
    /// </summary>
    /// <param name="name"></param>
    public void DeleteTimerByName(string name)
    {
        // Make sure the provided name exists in our dictionary
        if (m_Timers.ContainsKey(name))
        {
            // Timer is found
            Timer timer = m_Timers[name];

            // Remove timer from dictionary
            m_Timers.Remove(name);

            // Destroy the timer
            Destroy(timer);
        }
        else // If timer cannot be found print an error
        {
            Debug.LogError("Error in TimerManager::DeleteTimerByName : Provided name not found in list.");
        }
    }

    /// <summary>
    /// Get the number of currently active timers in the TimerManager.
    /// </summary>
    /// <returns></returns>
    public int GetNumberOfActiveTimers()
    {
        return m_Timers.Count;
    }
}