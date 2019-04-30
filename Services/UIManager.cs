using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public enum Screen
    {
        MainMenu,
        OptionsMenu,
        PauseMenu,
        GameEnd,
        PickUp,
        ErrorMessage,

        _NumberOfScreens
    }

    [NamedArray(typeof(Screen))]
    public GameObject[] Screens;

    private List<KeyValuePair<Screen, GameObject>> ScreenStack = new List<KeyValuePair<Screen, GameObject>>();

    void OnValidate()
    {
        System.Array.Resize(ref Screens, (int)Screen._NumberOfScreens);
    }

    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public Screen GetTopScreenType()
    {
        return ScreenStack[ScreenStack.Count - 1].Key;
    }

    public UIScreen GetTopScreen()
    {
        return ScreenStack[ScreenStack.Count - 1].Value.GetComponent<UIScreen>();
    }

    public void PushScreen(Screen screen)
    {
        GameObject screenToAdd = Screens[(int)screen];
        screenToAdd.GetComponent<UIScreen>().OnPush();

        ScreenStack.Add(new KeyValuePair<Screen, GameObject>(screen, screenToAdd));
    }

    public void PopScreen()
    {
        ScreenStack[ScreenStack.Count - 1].Value.GetComponent<UIScreen>().OnPop();
        ScreenStack.RemoveAt(ScreenStack.Count - 1);
    }

    public void PopAllScreens()
    {
        foreach (var screenPair in ScreenStack)
        {
            GameObject screen = screenPair.Value;

            screen.GetComponent<UIScreen>().OnPop();
        }

        ScreenStack.Clear();
    }
}
