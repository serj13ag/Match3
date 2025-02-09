﻿using System;
using Data;
using EventArguments;
using Interfaces;

namespace Services
{
    public class PlayerLevelService : IPlayerLevelService, IProgressWriter
    {
        private readonly IStaticDataService _staticDataService;

        private int _scoreToNextLevel;
        private int _currentLevel;

        public int ScoreToNextLevel => _scoreToNextLevel;

        public int CurrentLevel
        {
            get => _currentLevel;
            private set
            {
                _currentLevel = value;
                OnCurrentLevelChanged?.Invoke(this, new PlayerLevelChangedEventArgs(value));
            }
        }

        public event EventHandler<PlayerLevelChangedEventArgs> OnCurrentLevelChanged;

        public PlayerLevelService(IPersistentDataService persistentDataService,
            IStaticDataService staticDataService, IProgressUpdateService progressUpdateService)
        {
            _staticDataService = staticDataService;
            _currentLevel = persistentDataService.Progress.PlayerLevel;

            UpdateScoreToNextLevel(_currentLevel);

            progressUpdateService.Register(this);
        }

        public void GoToNextLevel()
        {
            CurrentLevel++;
            UpdateScoreToNextLevel(_currentLevel);
        }

        public void WriteToProgress(PlayerProgress progress)
        {
            progress.PlayerLevel = _currentLevel;
        }

        private void UpdateScoreToNextLevel(int currentLevel)
        {
            _scoreToNextLevel = currentLevel < _staticDataService.Settings.ScorePerLevel.Length
                ? _staticDataService.Settings.ScorePerLevel[currentLevel - 1]
                : _staticDataService.Settings.ScorePerLevel[^1];
        }
    }
}