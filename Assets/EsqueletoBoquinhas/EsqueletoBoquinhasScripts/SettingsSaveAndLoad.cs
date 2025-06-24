using UnityEngine;

public class SettingsSaveAndLoad : MonoBehaviour
{
    public bool musicSfxOn = true;
    public bool narrationOn = true;
    public bool timeOn = true;
    private void Start()
    {
        Load();
    }

    private void Load()
    {
        var isMusicSfxOn = PlayerPrefs.GetInt("isMusicSfxOn", 1) == 1;
        var isNarrationOn = PlayerPrefs.GetInt("isNarrationOn", 1) == 1;
        var isTimeOn = PlayerPrefs.GetInt("isTimeOn", 1) == 1;
        
        //Debug.Log("Loaded settings - Music: " + isMusicSfxOn + " - Narration: " + isNarrationOn + " - Time: " + isTimeOn);
        if (isMusicSfxOn)
        {
            AudioSystem.Instance.UnmuteMusic();
            AudioSystem.Instance.UnmuteSfx();
            musicSfxOn = true;
        }
        else
        {
            AudioSystem.Instance.MuteMusic();
            AudioSystem.Instance.MuteSfx();
            musicSfxOn = false;
        }

        if (isNarrationOn)
        {
            AudioSystem.Instance.UnmuteNarration();
            narrationOn = true;
        }
        else
        {
            AudioSystem.Instance.MuteNarration();
            narrationOn = false;
        }

        if (isTimeOn)
        {
            TimerSystem.Instance.isTimerEnabled = true;
            timeOn = true;
        }
        else
        {
            TimerSystem.Instance.isTimerEnabled = false;
            timeOn = false;
        }
    }
}