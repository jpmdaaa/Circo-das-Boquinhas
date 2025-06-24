using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// This class manages the audio system of the game. It controls different audio channels and their volumes.
/// </summary>
public class AudioSystem : StaticInstance<AudioSystem>
{
    [Header("Audio Sources")]
    // Audio source for silenceable narration
    public AudioSource silenceableNarrationChannel;
    // Audio source for un-silenceable narration
    public AudioSource unSilenceableNarrationChannel;
    // Audio source for silenceable sound effects
    public AudioSource silenceableSFXChannel;
    // Audio source for un-silenceable sound effects
    public AudioSource unSilenceableSFXChannel;
    // Audio source for music
    public AudioSource musicChannel;

    [Header("Audio volumes")]
    // Volume control for silenceable narration
    [Range(0f,1f)] public float silenceableNarrationVolume = 1f;
    // Volume control for un-silenceable narration
    [Range(0f,1f)] public float unSilenceableNarrationVolume = 1f;
    // Volume control for silenceable sound effects
    [Range(0f,1f)] public float silenceableSfxVolume = 1f;
    // Volume control for un-silenceable sound effects
    [Range(0f,1f)] public float unSilenceableSfxVolume = 1f;
    // Volume control for menu music
    [Range(0f,1f)] public float musicVolume = 0.5f;
    public float originalMusicVolume = 0;
    // Continually checks for audio sources that are not the ones set in the inspector
    [Header("Util")] public bool invalidSourcesCheck = false;

    // Coroutine variables
    private Coroutine musicFadeCoroutine;
    private Coroutine narrationFadeCoroutine;
    private float musicFadeDuration = 0.5f; // Adjust as needed
    private float narrationFadeDuration = 0.5f; // Adjust as needed
    private bool isFading = false;

    /// <summary>
    /// Plays main menu music on the music channel. If music is already playing, it fades out the current music and fades in the new one.
    /// </summary>
    /// <param name="clip">The audio clip to play.</param>
    /// <param name="volume">The volume level to play the clip at.</param>
    public void PlayMusic(AudioClip clip, float volume = 0.5f)
    {
        //Debug.Log("Playing menu music with volume: " + volume + " - " + clip.name);
        if (clip == musicChannel.clip)
        {
            return;
        }
        
        musicChannel.loop = true;
        musicVolume = volume;
        musicChannel.volume = musicVolume;
        
        if (musicChannel.isPlaying)
        {
            if (musicFadeCoroutine != null)
            {
                StopCoroutine(musicFadeCoroutine);
            }
            musicFadeCoroutine = StartCoroutine(FadeOutAndIn(clip, musicChannel, musicVolume));
        }
        else
        {
            musicChannel.clip = clip;
            musicChannel.Play();
        }
    }
    
    /// <summary>
    /// Stops the music that is currently playing on the music channel.
    /// </summary>
    public void StopMusic()
    {
        musicChannel.Stop();
    }
    
    /// <summary>
    /// Mutes the music on music channel.
    /// </summary>
    public void MuteMusic()
    {
        musicVolume = 0f;
        musicChannel.volume = musicVolume;
    }
    
    /// <summary>
    /// Unmutes the menu music that is currently playing on the music channel.
    /// </summary>
    public void UnmuteMusic()
    {
        musicVolume = originalMusicVolume;
        musicChannel.volume = musicVolume;
        //Debug.Log("Unmutting music to volume: " + musicVolume);
    }

    /// <summary>
    /// Plays a silenceable narration (a narration that can be muted on the settings menu).
    /// </summary>
    /// <param name="clip">The audio clip to play.</param>
    public void PlaySilenceableNarration(AudioClip clip)
    {
        silenceableNarrationChannel.PlayOneShot(clip);
    }
    
    /// <summary>
    /// Stops the silenceable narration that is currently playing.
    /// </summary>
    public void StopSilenceableNarration()
    {
        silenceableNarrationChannel.Stop();
    }
    
    /// <summary>
    /// Mutes the silenceable narration that is currently playing.
    /// </summary>
    public void MuteNarration()
    {
        silenceableNarrationVolume = 0;
        silenceableNarrationChannel.volume = silenceableNarrationVolume;
    }
    
    /// <summary>
    /// Unmutes the silenceable narration that is currently playing.
    /// </summary>
    public void UnmuteNarration()
    {
        silenceableNarrationVolume = 1;
        silenceableNarrationChannel.volume = silenceableNarrationVolume;
    }

    /// <summary>
    /// Plays an un-silenceable narration (a narration that can only be muted by lowering the PlayTable volume and turns down all the other audiosources while it is playing). The volume is reduced by the specified reduction amount.
    /// </summary>
    /// <param name="clip">The audio clip to play.</param>
    /// <param name="reductionAmount">The amount to reduce the volume by.</param>
    public void PlayUnSilenceableNarration(AudioClip clip)
    {
        float reductionAmount = 0.4f;
        if (unSilenceableNarrationChannel.isPlaying)
        {
            if (narrationFadeCoroutine != null)
            {
                StopCoroutine(narrationFadeCoroutine);
                unSilenceableNarrationChannel.Stop();
            }
            
            narrationFadeCoroutine = StartCoroutine(PlayUnSilenceableNarrationCorountine(clip,reductionAmount, musicVolume, silenceableSfxVolume, unSilenceableSfxVolume, silenceableNarrationVolume));
        }
        else
        {
            StartCoroutine(PlayUnSilenceableNarrationCorountine(clip, reductionAmount, musicVolume, silenceableSfxVolume, unSilenceableSfxVolume, silenceableNarrationVolume));
        }

    }
    
    /// <summary>
    /// Stops the un-silenceable narration that is currently playing.
    /// </summary>
    public void StopUnSilenceableNarration()
    {
        unSilenceableNarrationChannel.Stop();
    }

    /// <summary>
    /// Plays a silenceable sound effect (an sound effect that can be muted on the settings menu).
    /// </summary>
    /// <param name="clip">The audio clip to play.</param>
    public void PlaySilenceableSfx(AudioClip clip)
    {
        silenceableSFXChannel.PlayOneShot(clip);
    }
    
    /// <summary>
    /// Stops the silenceable sound effect that is currently playing.
    /// </summary>
    public void StopSilenceableSfx()
    {
        silenceableSFXChannel.Stop();
    }
    
    /// <summary>
    /// Mutes the silenceable sound effect that is currently playing.
    /// </summary>
    public void MuteSfx()
    {
        silenceableSFXChannel.volume = 0;
    }
    
    /// <summary>
    /// Unmutes the silenceable sound effect that is currently playing.
    /// </summary>
    public void UnmuteSfx()
    {
        silenceableSfxVolume = 1;
        silenceableSFXChannel.volume = silenceableSfxVolume;
    }

    /// <summary>
    /// Plays an un-silenceable sound effect (an sound effect that can only be muted by lowering the PlayTable volume).
    /// </summary>
    /// <param name="clip">The audio clip to play.</param>
    public void PlayUnSilenceableSfx(AudioClip clip)
    {
        unSilenceableSFXChannel.PlayOneShot(clip);
    }
    
    /// <summary>
    /// Stops the un-silenceable sound effect that is currently playing.
    /// </summary>
    public void StopUnSilenceableSfx()
    {
        unSilenceableSFXChannel.Stop();
    }

    #region Utils
    // Continually checks the scene for audio sources that are not the ones set in the inspector
    private void Update()
    {
#if UNITY_EDITOR
        if (invalidSourcesCheck)
        {
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audioSource in audioSources)
            {
                if (audioSource != silenceableNarrationChannel &&
                    audioSource != unSilenceableNarrationChannel &&
                    audioSource != silenceableSFXChannel &&
                    audioSource != unSilenceableSFXChannel &&
                    audioSource != musicChannel)
                {
                    Debug.LogWarning("AudioSource " + audioSource.name + " não está sendo controlado pelo AudioSystem, negue o jogo e peça para que o parceiro utilize o sistema de audio do boquinhas!");
                }
            }
        }
#endif
    }
    #endregion

    #region Coroutines
    // Coroutines

    /// <summary>
    /// Restores the original volumes after a narration clip finishes playing.
    /// </summary>
    private IEnumerator PlayUnSilenceableNarrationCorountine(AudioClip narrationClip, float reductionAmount, float originalMusicVolume, float originalSFXVolume, float originalUnSilenceableSFXVolume, float originalSilenceableNarrationVolume)
    {
        unSilenceableNarrationChannel.clip = narrationClip;
        unSilenceableNarrationChannel.Play();
        
        // Store the current volumes
        float currentMusicVolume = musicChannel.volume;
        float currentSFXVolume = silenceableSFXChannel.volume;
        float currentUnSilenceableSFXVolume = unSilenceableSFXChannel.volume;
        float currentSilenceableNarrationVolume = silenceableNarrationChannel.volume;
    
        // Calculate the target volumes after reduction
        float targetMusicVolume = Mathf.Clamp(originalMusicVolume - reductionAmount, 0f, 1f);
        float targetSFXVolume = Mathf.Clamp(originalSFXVolume - reductionAmount, 0f, 1f);
        float targetUnSilenceableSFXVolume = Mathf.Clamp(originalUnSilenceableSFXVolume - reductionAmount, 0f, 1f);
        float targetSilenceableNarrationVolume = Mathf.Clamp(originalSilenceableNarrationVolume - reductionAmount, 0f, 1f);
    
        // Fade out
        float timer = 0f;
        while (timer < narrationFadeDuration)
        {
            timer += Time.deltaTime;
            musicChannel.volume = Mathf.Lerp(currentMusicVolume, targetMusicVolume, timer / narrationFadeDuration);
            silenceableSFXChannel.volume = Mathf.Lerp(currentSFXVolume, targetSFXVolume, timer / narrationFadeDuration);
            unSilenceableSFXChannel.volume = Mathf.Lerp(currentUnSilenceableSFXVolume, targetUnSilenceableSFXVolume, timer / narrationFadeDuration);
            silenceableNarrationChannel.volume = Mathf.Lerp(currentSilenceableNarrationVolume, targetSilenceableNarrationVolume, timer / narrationFadeDuration);
            yield return null;
        }

        // Wait for the narration clip to finish playing
        yield return new WaitForSeconds(unSilenceableNarrationChannel.clip.length);
    
        // Fade in
        timer = 0f;
        while (timer < narrationFadeDuration)
        {
            timer += Time.deltaTime;
            musicChannel.volume = Mathf.Lerp(targetMusicVolume, originalMusicVolume, timer / narrationFadeDuration);
            silenceableSFXChannel.volume = Mathf.Lerp(targetSFXVolume, originalSFXVolume, timer / narrationFadeDuration);
            unSilenceableSFXChannel.volume = Mathf.Lerp(targetUnSilenceableSFXVolume, originalUnSilenceableSFXVolume, timer / narrationFadeDuration);
            silenceableNarrationChannel.volume = Mathf.Lerp(targetSilenceableNarrationVolume, originalSilenceableNarrationVolume, timer / narrationFadeDuration);
            yield return null;
        }
        narrationFadeCoroutine = null;
    }


    /// <summary>
    /// Fades out the current audio clip and fades in a new one.
    /// </summary>
    /// <param name="newClip">The new audio clip to fade in.</param>
    /// <param name="audioSource">The audio source to play the clip on.</param>
    /// <param name="originalVolume">The original volume of the audio source.</param>
    private IEnumerator FadeOutAndIn(AudioClip newClip, AudioSource audioSource, float originalVolume)
    {
        // Fade out
        float startVolume = audioSource.volume;
        float timer = 0f;
        while (timer < musicFadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / musicFadeDuration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();

        // Change clip
        audioSource.clip = newClip;

        // Fade in
        audioSource.Play();
        timer = 0f;
        while (timer < musicFadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, originalVolume, timer / musicFadeDuration);
            yield return null;
        }

        audioSource.volume = originalVolume;
        musicFadeCoroutine = null;
    }
    #endregion
}