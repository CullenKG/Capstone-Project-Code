using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEditor;

/// <summary>
/// List of music clips.
/// </summary>
public enum Music
{
    ToadBossIntro,
    ToadBossRepeating,
    MainMenu,
    CourtyardTheme,
    OldMenuTheme,

    // Always last
    _NumberOfMusicClips
}

/// <summary>
/// List of sound effects.
/// </summary>
public enum EnvironmentSFX
{
    Environment_Crickets,
    Environment_Elevator,
    Water_Splash,

    _NumberOfEnvironmenSFX
}
public enum MenuSFX
{
    Menu_Button_Switch,
    Menu_Button_Select,
    Menu_Play_Select,

    _NumberOfMenuSFX
}
public enum OdellSFX
{
    Odell_Charge,
    Odell_Croak,
    Odell_Death,
    Odell_Head_Turn,
    Odell_Hurt,
    Odell_Hurt_Old,
    Odell_Jump,
    Odell_Jump_Land,
    Odell_Projectile_Fire,
    Odell_Projectile_Wheeze,
    Odell_Tongue_Grab_Fire,
    Odell_Tongue_Grab_Reel,
    Odell_Tongue_Slash,
    Frog_Chirp_x3,
    Frog_Chirp_x5,

    _NumberOfOdellSFX
}
public enum PlayerSFX
{
    Player_Death_Chord,
    Player_Hurt,
    Player_Hurt_Old,
    Player_Dash,
    Player_Heal,
    Player_Projectile_Fire,
    Player_Footsteps_Groud_1,
    Player_Footsteps_Groud_2,
    Player_Footsteps_Groud_3,
    Player_Footsteps_Groud_4,
    Player_Footsteps_Groud_5,
    Player_Footsteps_Groud_6,
    Player_Footsteps_Groud_7,
    Player_Footsteps_Groud_8,
    Player_Footsteps_Groud_9,
    Player_Footsteps_Groud_10,
    Player_Footsteps_Water_1,
    Player_Footsteps_Water_2,
    Player_Footsteps_Water_3,
    Player_Footsteps_Water_4,
    Player_Footsteps_Water_5,
    Player_Footsteps_Water_6,
    Player_Footsteps_Water_7,
    Player_Footsteps_Water_8,
    Player_Footsteps_Water_9,
    Player_Footsteps_Water_10,
    Sword_Slash_1,
    Sword_Slash_2,
    Sword_Slash_3,

    _NumberOfPlayerSFX
}

/// <summary>
/// Used to play sound effects or music.
/// Created by Cullen.
/// </summary>
public class AudioManager : MonoBehaviour
{
    // Keep a dictionary of all audio sources so we can stop them easily
    private Dictionary<string, AudioSource> m_MusicSources = new Dictionary<string, AudioSource>();
    private Dictionary<string, AudioSource> m_SFXSources = new Dictionary<string, AudioSource>();

    /// <summary>
    /// The main audio mixer.
    /// </summary>
    [SerializeField] private AudioMixer MainMixer;

    // Audio mixer groups
    [SerializeField] private AudioMixerGroup MusicGroup;
    [SerializeField] private AudioMixerGroup SFXGroup;

    [Header("List of audio clips. Add to respective enum to expand.")]

    // List of music audio clips. Adding to the enum will automatically expand the array in the editor.
    [NamedArray(typeof(Music))]
    [SerializeField] AudioClip[] m_MusicClips;

    // List of sound effect audio clips. Adding to the enum will automatically expand the array in the editor.
    [NamedArray(typeof(EnvironmentSFX))]
    [SerializeField] AudioClip[] m_EnvironmentSoundEffectClips;
    [NamedArray(typeof(MenuSFX))]
    [SerializeField] AudioClip[] m_MenuSoundEffectClips;
    [NamedArray(typeof(OdellSFX))]
    [SerializeField] AudioClip[] m_OdellSoundEffectClips;
    [NamedArray(typeof(PlayerSFX))]
    [SerializeField] AudioClip[] m_PlayerSoundEffectClips;


    #region Unity Functions

    void OnValidate()
    {
        System.Array.Resize(ref m_MusicClips, (int)Music._NumberOfMusicClips);
        System.Array.Resize(ref m_EnvironmentSoundEffectClips, (int)EnvironmentSFX._NumberOfEnvironmenSFX);
        System.Array.Resize(ref m_MenuSoundEffectClips, (int)MenuSFX._NumberOfMenuSFX);
        System.Array.Resize(ref m_OdellSoundEffectClips, (int)OdellSFX._NumberOfOdellSFX);
        System.Array.Resize(ref m_PlayerSoundEffectClips, (int)PlayerSFX._NumberOfPlayerSFX);
    }

    void Start()
    {
        // Register the global audio sources
        RegisterMusicSource("Global", GameObject.Find("MusicSource").GetComponent<AudioSource>());
        RegisterSFXSource("Global", GameObject.Find("SfxSource").GetComponent<AudioSource>());
        //RegisterSFXSource("Global", GameObject.Find("LoopingSfxSource").GetComponent<AudioSource>());
    }

    #endregion

    #region Private Functions

    /// <summary>
    /// Coroutine that will fade an AudioSource's volume to 0 over a set amount of time.
    /// </summary>
    /// <param name="source">The source to fade out.</param>
    /// <param name="fadeOutTime">The amount of time it takes to fade to 0. (Seconds)</param>
    private IEnumerator FadeOut(AudioSource source, float fadeOutTime)
    {
        float startVolume = source.volume;

        while (source.volume > 0)
        {
            source.volume -= startVolume * Time.deltaTime / fadeOutTime;
            yield return null;
        }

        source.Stop();
        source.volume = startVolume;
    }

    /// <summary>
    /// Does the music source already exist in the dictionary?
    /// </summary>
    private bool DoesMusicSourceExist(string sourceName)
    {
        if (m_MusicSources.ContainsKey(sourceName))
            return true;

        //Debug.LogError("Music source: " + sourceName + " doesn't exist!");
        return false;
    }

    /// <summary>
    /// Does the SFX source already exist in the dictionary?
    /// </summary>
    private bool DoesSFXSourceExist(string sourceName)
    {
        if (m_SFXSources.ContainsKey(sourceName))
            return true;

        //Debug.LogError("SFX source: " + sourceName + " doesn't exist!");
        return false;
    }

    #endregion

    #region Public Functions

    /// <summary>
    /// Adds the audio source to the AudioManager's dictionary of music sources.
    /// </summary>
    public void RegisterMusicSource(string sourceName, AudioSource source)
    {
        if (m_MusicSources.ContainsKey(sourceName))
        {
            //Debug.LogError("An audio source called: " + sourceName + " has already been registered. Please use a different name.");
            return;
        }

        source.outputAudioMixerGroup = MusicGroup;
        m_MusicSources[sourceName] = source;
    }

    /// <summary>
    /// Adds the audio source to the AudioManager's dictionary of SFX sources.
    /// </summary>
    public void RegisterSFXSource(string sourceName, AudioSource source)
    {
        if (m_SFXSources.ContainsKey(sourceName))
        {
            //Debug.LogError("An audio source called: " + sourceName + " has already been registered. Please use a different name.");
            return;
        }

        source.outputAudioMixerGroup = SFXGroup;
        m_SFXSources[sourceName] = source;
    }

    /// <summary>
    /// Start playing a music clip.
    /// </summary>
    /// <param name="music">The music clip to be played.</param>
    public void PlayMusic(Music music, string sourceName = "Global")
    {
        if (DoesMusicSourceExist(sourceName))
        {
            m_MusicSources[sourceName].clip = m_MusicClips[(int)music];
            m_MusicSources[sourceName].Play();
        } 
    }

    /// <summary>
    /// Play a sound effect.
    /// </summary>
    /// <param name="soundEffect">The sound effect to be played.</param>
    public void PlayEnvironmentSFX(EnvironmentSFX soundEffect, string sourceName = "Global")
    {
        if (DoesSFXSourceExist(sourceName))
            m_SFXSources[sourceName].PlayOneShot(m_EnvironmentSoundEffectClips[(int)soundEffect]);
    }
    public void PlayMenuSFX(MenuSFX soundEffect, string sourceName = "Global")
    {
        if (DoesSFXSourceExist(sourceName))
            m_SFXSources[sourceName].PlayOneShot(m_MenuSoundEffectClips[(int)soundEffect]);
    }
    public void PlayOdellSFX(OdellSFX soundEffect, string sourceName = "Global")
    {
        if (DoesSFXSourceExist(sourceName))
            m_SFXSources[sourceName].PlayOneShot(m_OdellSoundEffectClips[(int)soundEffect]);
    }
    public void PlayPlayerSFX(PlayerSFX soundEffect, string sourceName = "Global")
    {
        if (DoesSFXSourceExist(sourceName))
            m_SFXSources[sourceName].PlayOneShot(m_PlayerSoundEffectClips[(int)soundEffect]);
    }
    /// <summary>
    /// Play a sound effect.
    /// </summary>
    /*public void PlayLoopingSFX(SFX soundEffect, string sourceName = "Global")
    {
        if (DoesSFXSourceExist(sourceName))
            m_SFXSources[sourceName].PlayOneShot(m_SoundEffectClips[(int)soundEffect]);
    }*/

    /// <summary>
    /// Stop any sound that is currently playing.
    /// </summary>
    public void StopAllSound()
    {
        StopAllMusic();
        StopAllSFX();      
    }

    /// <summary>
    /// Stop any music that is currently playing.
    /// </summary>
    public void StopAllMusic()
    {
        foreach (AudioSource source in m_MusicSources.Values)
        {
            source.Stop();
        }
    }

    /// <summary>
    /// Stop any SFX that is currently playing.
    /// </summary>
    public void StopAllSFX()
    {
        foreach (AudioSource source in m_SFXSources.Values)
        {
            source.Stop();
        }
    }

    /// <summary>
    /// Stops a specific music source.
    /// </summary>
    public void StopMusic(string sourceName)
    {
        if (DoesMusicSourceExist(sourceName))
            m_MusicSources[sourceName].Stop();
    }

    /// <summary>
    /// Stops the sound effect.
    /// </summary>
    public void StopSFX(string sourceName)
    {
        if (DoesSFXSourceExist(sourceName))
            m_SFXSources[sourceName].Stop();
    }

    /// <summary>
    /// Fades the music volume to 0 over a set amount of time.
    /// </summary>
    /// <param name="fadeOutTime">The amount of time it takes to fade to 0. (Seconds)</param>
    public void FadeOutMusic(float fadeOutTime, string sourceName)
    {
        if (DoesMusicSourceExist(sourceName))
            StartCoroutine(FadeOut(m_MusicSources[sourceName], fadeOutTime));
    }

    /// <summary>
    /// Fades the sound effect volume to 0 over a set amount of time.
    /// </summary>
    /// <param name="fadeOutTime">The amount of time it takes to fade to 0. (Seconds)</param>
    public void FadeOutSFX(float fadeOutTime, string sourceName)
    {
        if (DoesSFXSourceExist(sourceName))
            StartCoroutine(FadeOut(m_SFXSources[sourceName], fadeOutTime));
    }

    /// <summary>
    /// Sets the volume for 
    /// </summary>
    public void SetMasterVolume(float volume)
    {
        MainMixer.SetFloat("MasterVolume", volume);
    }

    /// <summary>
    /// Sets the volume for music in the game.
    /// </summary>
    public void SetMusicVolume(float volume)
    {
        MainMixer.SetFloat("MusicVolume", volume);
    }

    /// <summary>
    /// Sets the volume for all SFX soures in the game.
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        MainMixer.SetFloat("SfxVolume", volume);
    }

    /// <summary>
    /// Set if the music should loop.
    /// </summary>
    public void SetMusicLooping(bool loop, string sourceName)
    {
        if (DoesMusicSourceExist(sourceName))
            m_MusicSources[sourceName].loop = loop;
    }

    /// <summary>
    /// Set if sound effect should loop.
    /// </summary>
    public void SetSFXLooping(bool loop, string sourceName)
    {
        if (DoesSFXSourceExist(sourceName))
            m_SFXSources[sourceName].loop = loop;
    }

    #endregion
}