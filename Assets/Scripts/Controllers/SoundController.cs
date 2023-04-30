using System;
using Enums;
using UnityEngine;

namespace Controllers
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource; // TODO spawn multiple sources

        [SerializeField] private AudioClip[] _musicClips;
        [SerializeField] private AudioClip _winClip;
        [SerializeField] private AudioClip _loseClip;
        [SerializeField] private AudioClip _bonusClip;
        [SerializeField] private AudioClip _breakGamePieceClip;
        [SerializeField] private AudioClip _bombGamePieceClip;
        [SerializeField] private AudioClip _breakCollectibleClip;

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
                SoundType.BreakGamePieces => Constants.Sound.FxVolume,
                SoundType.BombGamePieces => Constants.Sound.FxVolume,
                SoundType.BreakCollectible => Constants.Sound.FxVolume,
                _ => throw new ArgumentOutOfRangeException(nameof(soundType), soundType, null)
            };
        }

        private AudioClip GetClip(SoundType soundType)
        {
            return soundType switch
            {
                SoundType.Music => GetRandomClip(_musicClips),
                SoundType.Win => _winClip,
                SoundType.Lose => _loseClip,
                SoundType.BreakGamePieces => _breakGamePieceClip,
                SoundType.Bonus => _bonusClip,
                SoundType.BombGamePieces => _bombGamePieceClip,
                SoundType.BreakCollectible => _breakCollectibleClip,
                _ => throw new ArgumentOutOfRangeException(nameof(soundType), soundType, null)
            };
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