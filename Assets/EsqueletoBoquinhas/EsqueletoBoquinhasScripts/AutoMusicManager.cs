using System;
using Boquinhas.ConfigurationSettings;
using Boquinhas.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoMusicManager : MonoBehaviour
{
    private BoquinhasConfigurationSettings _boquinhasConfigurationSettings;

    private void Start()
    {
        Debug.LogWarning("AutoMusicManager started!");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        if (_boquinhasConfigurationSettings == null && scene.name == "MainMenu")
            _boquinhasConfigurationSettings = GameManager.Instance.boquinhasConfigurationSettings;

        bool isMusicOn = FindObjectOfType<SettingsSaveAndLoad>().musicSfxOn;
        //Debug.Log(isMusicOn);
        AudioSystem audios = FindObjectOfType<AudioSystem>();
        if (audios.originalMusicVolume == 0f)
        {
            audios.originalMusicVolume = _boquinhasConfigurationSettings.musicVolume;
            //Debug.Log("Original music volume: " + audios.originalMusicVolume);
        }
        
        //if the music is muted, returns, otherwise plays the music
        if (isMusicOn == false)
        {
            return;
        }
        else if (isMusicOn == true)
        {
            if (_boquinhasConfigurationSettings != null)
            {
                if (scene.name == _boquinhasConfigurationSettings.gameSceneName)
                {
                    AudioSystem.Instance.PlayMusic(_boquinhasConfigurationSettings.gamePlayMusic,
                        _boquinhasConfigurationSettings.musicVolume);
                }
                else
                {
                    AudioSystem.Instance.PlayMusic(_boquinhasConfigurationSettings.mainMenuMusic,
                        _boquinhasConfigurationSettings.musicVolume);
                }
            }
        }
    }
    
    public void RecheckMusicAfterLoad()
    {
        Debug.Log("RecheckMusicAfterLoad called!");
        
        if (_boquinhasConfigurationSettings == null)
            _boquinhasConfigurationSettings = GameManager.Instance.boquinhasConfigurationSettings;

        bool isMusicOn = FindObjectOfType<SettingsSaveAndLoad>().musicSfxOn;
        Debug.Log("Reched: " + isMusicOn);
        AudioSystem audios = FindObjectOfType<AudioSystem>();
        if (audios.originalMusicVolume == 0f)
        {
            audios.originalMusicVolume = _boquinhasConfigurationSettings.musicVolume;
            Debug.Log("Original music volume: " + audios.originalMusicVolume);
        }
        
        //if the music is muted, returns, otherwise plays the music
        if (isMusicOn == false)
        {
            return;
        }
        else if (isMusicOn == true)
        {
            if (_boquinhasConfigurationSettings != null)
            {
                AudioSystem.Instance.PlayMusic(_boquinhasConfigurationSettings.mainMenuMusic,
                        _boquinhasConfigurationSettings.musicVolume);
            }
        }
    }
}