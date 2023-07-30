using System;
using Constants;
using Data;
using Enums;
using EventArguments;
using Interfaces;
using Services.Mono.Sound;
using UnityEngine;

namespace Services
{
    public class ScoreService : IScoreService, IProgressWriter
    {
        private int _completedBreakStreakIterations;

        private readonly ISoundMonoService _soundMonoService;
        private readonly IProgressUpdateService _progressUpdateService;

        private int _scoreGoal;
        private int _score;

        public int Score
        {
            get => _score;
            private set
            {
                _score = value;
                OnScoreChanged?.Invoke(this, new ScoreChangedEventArgs(value));
            }
        }

        public int ScoreGoal => _scoreGoal;

        public bool ScoreGoalReached => _score >= _scoreGoal;

        public event EventHandler<ScoreChangedEventArgs> OnScoreChanged;

        public ScoreService(string levelName, ISoundMonoService soundMonoService,
            IPersistentProgressService persistentProgressService, IProgressUpdateService progressUpdateService,
            int scoreGoal)
        {
            _soundMonoService = soundMonoService;

            _scoreGoal = scoreGoal;

            LevelBoardData levelBoardData = persistentProgressService.Progress.BoardData.LevelBoardData;
            if (levelName == levelBoardData.LevelName)
            {
                _score = levelBoardData.Score;
            }

            progressUpdateService.Register(this);
        }

        public void AddScore(int gamePieceScore, int numberOfBreakGamePieces)
        {
            int bonusScore = numberOfBreakGamePieces >= Settings.Score.MinNumberOfBreakGamePiecesToGrantBonus
                ? Settings.Score.BonusScore
                : 0;
            int scoreMultiplier = _completedBreakStreakIterations + 1;
            int totalScore = gamePieceScore * scoreMultiplier + bonusScore;

            AddScore(totalScore);
        }

        public void IncrementCompletedBreakStreakIterations()
        {
            if (_completedBreakStreakIterations > 0)
            {
                _soundMonoService.PlaySound(SoundType.Bonus);
            }

            _completedBreakStreakIterations++;
        }

        public void ResetBreakStreakIterations()
        {
            _completedBreakStreakIterations = 0;
        }

        public void SetScoreGoal(int scoreToNextLevel)
        {
            _scoreGoal = scoreToNextLevel;
            Score = 0;
        }

        public void WriteToProgress(PlayerProgress progress)
        {
            progress.BoardData.LevelBoardData.Score = _score;
        }

        private void AddScore(int scoreToAdd)
        {
            switch (scoreToAdd)
            {
                case < 0:
                    Debug.LogError($"{nameof(ScoreService)} : Attempt to add negative score: {scoreToAdd}!");
                    return;
                case 0:
                    return;
            }

            Score += scoreToAdd;
        }
    }
}