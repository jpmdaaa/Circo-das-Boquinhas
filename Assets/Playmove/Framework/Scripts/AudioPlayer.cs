using System;
using System.Linq;
using Playmove.Core.Audios;
using Playmove.Core.Bundles;
using UnityEngine;

namespace Playmove.Framework
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private bool _playOnEnable;
        [SerializeField] private bool _playOnStart;

        [Header("Audio Settings")] [SerializeField]
        private float _volume = 1;

        [SerializeField] private float _pitch = 1;
        [SerializeField] private float _delay;
        [SerializeField] private float _delayBetween;
        [SerializeField] private bool _loop;
        [SerializeField] private float _fadeTo;
        [SerializeField] private float _fadeDuration;

        [Header("Audios")] [SerializeField] private Audio[] _audios;

        private void Start()
        {
            if (_playOnStart)
                Play();
        }

        private void OnEnable()
        {
            if (_playOnEnable)
                Play();
        }

        public void PlayInSequence()
        {
            if (_audios.Length == 0) return;
            if (_audios[0].AudioClip)
                ApplySettings(AudioManager.StartAudio(_audios[0].Channel, _audios.Where(audio => audio.AudioAsset)
                    .Select(audio => audio.AudioClip).ToArray())).Play();
            else if (_audios[0].AudioAsset)
                ApplySettings(AudioManager.StartAudio(_audios[0].Channel, _audios.Where(audio => audio.AudioAsset)
                    .Select(audio => audio.AudioAsset).ToArray())).Play();
            else if (!string.IsNullOrEmpty(_audios[0].AudioName))
                ApplySettings(AudioManager.StartAudio(_audios[0].Channel, _audios.Where(audio => audio.AudioAsset)
                    .Select(audio => audio.AudioName).ToArray())).Play();
        }

        public void Play()
        {
            if (_audios.Length == 0) return;
            Play(_audios[0].Tag);
        }

        public void Play(string tag)
        {
            var audio = _audios.FirstOrDefault(a => a.Tag == tag);
            if (audio == null) return;
            Play(audio);
        }

        private void Play(Audio audio)
        {
            if (audio.AudioClip)
                ApplySettings(AudioManager.StartAudio(audio.Channel, audio.AudioClip)).Play();
            else if (audio.AudioAsset)
                ApplySettings(AudioManager.StartAudio(audio.Channel, audio.AudioAsset)).Play();
            else if (!string.IsNullOrEmpty(audio.AudioName))
                ApplySettings(AudioManager.StartAudio(audio.Channel, audio.AudioName)).Play();
        }

        private IAudioData ApplySettings(IAudioData audioData)
        {
            if (_loop) audioData.SetLoop();
            return audioData.SetVolume(_volume).SetPitch(_pitch)
                .SetDelay(_delay).SetDelayBetween(_delayBetween)
                .SetFade(_fadeTo, _fadeDuration);
        }

        [Serializable]
        public class Audio
        {
            public string Tag = string.Empty;
            public AudioChannel Channel = AudioChannel.Master;

            [Header("Only one will be used. AudioClip has more priority")]
            public AudioClip AudioClip;

            public PlayAsset AudioAsset;
            public string AudioName = string.Empty;
        }
    }
}