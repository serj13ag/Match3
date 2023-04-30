using System;
using Enums;
using UnityEngine;

namespace Controllers
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private AudioClip[] _musicClips;
        [SerializeField] private AudioClip[] _winClips;
        [SerializeField] private AudioClip[] _loseClips;
        [SerializeField] private AudioClip[] _bonusClips;

        private RandomService _randomService;

        public void Init(RandomService randomService)
        {
            _randomService = randomService;
        }

        public void PlaySound(SoundType soundType)
        {
            PlayClip(GetClip(soundType), GetVolume(soundType));
        }

        private static float GetVolume(SoundType soundType)
        {
            return soundType switch
            {
                SoundType.Music => Constants.Sound.MusicVolume,
                SoundType.Win => Constants.Sound.FxVolume,
                SoundType.Lose => Constants.Sound.FxVolume,
                SoundType.Bonus => Constants.Sound.FxVolume,
                _ => throw new ArgumentOutOfRangeException(nameof(soundType), soundType, null)
            };
        }

        private AudioClip GetClip(SoundType soundType)
        {
            AudioClip[] clips = soundType switch
            {
                SoundType.Music => _musicClips,
                SoundType.Win => _winClips,
                SoundType.Lose => _loseClips,
                SoundType.Bonus => _bonusClips,
                _ => throw new ArgumentOutOfRangeException(nameof(soundType), soundType, null)
            };

            return GetRandomClip(clips);
        }

        private AudioClip GetRandomClip(AudioClip[] musicClips)
        {
            return musicClips[_randomService.Next(musicClips.Length)];
        }

        private void PlayClip(AudioClip audioClip, float volume)
        {
            _audioSource.clip = audioClip;

            float randomPitch = _randomService.Next(Constants.Sound.LowPitch, Constants.Sound.HighPitch);
            _audioSource.pitch = randomPitch;

            _audioSource.volume = volume;
            _audioSource.Play();
        }
    }
}