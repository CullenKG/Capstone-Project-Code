using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIGameSettingsScreen : UIScreen
{
    [SerializeField] Dropdown ResolutionDropdown;
    [SerializeField] Toggle FullscreenToggle;
    [SerializeField] FloatVariable BrightnessValue;
    EventSystem eventSystem;
    public GameObject selectedObject;

    void Start()
    {
        eventSystem = EventSystem.current;

        if(Services.InputManager.CurrentInputType == InputType.Controller)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
        }
       // eventSystem.SetSelectedGameObject(selectedObject);    
        foreach (Resolution resolution in Screen.resolutions)
        {
            ResolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }

        int index = 0;
        foreach (Resolution resolution in Screen.resolutions)
        {
            if (resolution.Equals(Services.GameManager.CurrentResolution))
            {
                ResolutionDropdown.value = index;
            }

            index++;
        }
    }

    public override void OnPush()
    {
        base.OnPush();

        int index = 0;

        foreach (Resolution resolution in Screen.resolutions)
        {
            if (resolution.Equals(Services.GameManager.CurrentResolution))
            {
                ResolutionDropdown.value = index;
            }

            index++;
        }

        FullscreenToggle.isOn = Services.GameManager.IsGameFullscreen;
    }

    public void SetMusicVolume(float value)
    {
        Services.AudioManager.SetMusicVolume(value);
    }

    public void SetSFXVolume(float value)
    {
        Services.AudioManager.SetSFXVolume(value);
    }


    public void SetHorizontalSensitivity(float value)
    {
        Services.GameManager.Player.CameraController.m_SensitivityY = value;
    }

    public void SetVerticalSensitivity(float value)
    {
        Services.GameManager.Player.CameraController.m_SensitivityX = value;
    }
    public void SetResolution(int index)
    {
        Services.GameManager.CurrentResolution = Screen.resolutions[index];

        UpdateScreenResolution();
    }

    public void SetBrightness(float value)
    {
        // Color temp = RenderSettings.ambientLight;
        Light[] lights = FindObjectsOfType<Light>();
        foreach(Light light in lights)
        {
            light.intensity = value;
        }
        //RenderSettings.ambientLight = new Color(temp.r *value, temp.g*value, temp.b * value, 1);      
        //RenderSettings.ambientIntensity = value*50;
        //BrightnessValue.Value = value;
    }

    public void OnFullscreenToggled(bool toggle)
    {
        Services.GameManager.IsGameFullscreen = toggle;
        UpdateScreenResolution();
    }

    public void OnInvertYAxisToggled(bool toggle)
    {
        Services.GameManager.Player.CameraController.InvertYAxis = !Services.GameManager.Player.CameraController.InvertYAxis;
    }

    void UpdateScreenResolution()
    {
        Resolution currentResolution = Services.GameManager.CurrentResolution;
        bool fullscreen = Services.GameManager.IsGameFullscreen;

        Screen.SetResolution(currentResolution.width, currentResolution.height, fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed, currentResolution.refreshRate);
    }
}
