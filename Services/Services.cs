using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;

public static class Services
{
    public static GameManager GameManager { get; private set; }
    public static InputManager InputManager { get; private set; }
    public static AudioManager AudioManager { get; private set; }
    public static TimerManager TimerManager { get; private set; }
    public static UIManager UIManager { get; private set; }
    public static Constants Constants { get; private set; }
    public static EventSystem EventSystem { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitializeApp()
    {
       

        GameObject _appPrefab = Resources.Load("_app") as GameObject;
        GameObject _app = GameObject.Instantiate(_appPrefab);

        GameObject.DontDestroyOnLoad(_app);

        GameManager = _app.GetComponent<GameManager>();
        InputManager = _app.GetComponent<InputManager>();
        AudioManager = _app.GetComponent<AudioManager>();
        TimerManager = _app.GetComponent<TimerManager>();
        UIManager = _app.GetComponent<UIManager>();
        Constants = _app.GetComponent<Constants>();
        EventSystem = _app.GetComponentInChildren<EventSystem>();
    }
}
