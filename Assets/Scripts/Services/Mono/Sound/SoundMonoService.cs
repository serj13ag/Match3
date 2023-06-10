using System;
using Constants;
using Enums;
using UnityEngine;

namespace Services.Mono.Sound
{
    public class SoundMonoService : MonoBehaviour
    {
        [SerializeField] private OneShotAudioSource _oneShotAudioSourcePrefab;
        [SerializeField] private LoopAudioSource _loopAudioSource;

        [SerializeField] private AudioClip[] _musicClips;
        [SerializeField] private AudioClip _winClip;
        [SerializeField] private AudioClip _loseClip;
        [SerializeField] private AudioClip _bonusClip;
        [SerializeField] private AudioClip _breakGamePieceClip;
        [SerializeField] private AudioClip _bombGamePieceClip;
        [SerializeField] private AudioClip _breakCollectibleClip;

        private IRandomService _randomService;

        private LoopAudioSource _backgroundMusicAudioSource;

        public void Init(IRandomService randomService)
        {
            _randomService = randomService;

            DontDestroyOnLoad(this);
        }

        public void PlayBackgroundMusic()
        {
            if (_backgroundMusicAudioSource == null)
            {
                _backgroundMusicAudioSource = CreateBackgroundMusicAudioSource();
            }
        }

        public void PlaySound(SoundType soundType)
        {
            PlayOneShotClip(GetClip(soundType), GetVolume(soundType));
        }

        private static float GetVolume(SoundType soundType)
        {
            return soundType switch
            {
                SoundType.Music => Settings.Sound.MusicVolume,
                SoundType.Win => Settings.Sound.FxVolume,
                SoundType.Lose => Settings.Sound.FxVolume,
                SoundType.Bonus => Settings.Sound.FxVolume,
                SoundType.BreakGamePieces => Settings.Sound.FxVolume,
                SoundType.BombGamePieces => Settings.Sound.FxVolume,
                SoundType.BreakCollectible => Settings.Sound.FxVolume,
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
                _ => throw new ArgumentOutOfRangeException(nameof(soundType), soundType, null),
            };
        }

        private AudioClip GetRandomClip(AudioClip[] musicClips)
        {
            return musicClips[_randomService.Next(musicClips.Length)];
        }

        private void PlayOneShotClip(AudioClip audioClip, float volume)
        {
            OneShotAudioSource oneShotAudioSource = Instantiate(_oneShotAudioSourcePrefab, transform);

            float randomPitch = _randomService.Next(Settings.Sound.LowPitch, Settings.Sound.HighPitch);
            oneShotAudioSource.Init(audioClip, volume, randomPitch);
        }

        private LoopAudioSource CreateBackgroundMusicAudioSource()
        {
            LoopAudioSource backgroundMusicAudioSource = Instantiate(_loopAudioSource, transform);
            backgroundMusicAudioSource.Init(GetClip(SoundType.Music), GetVolume(SoundType.Music));
            return backgroundMusicAudioSource;
        }
    }
}