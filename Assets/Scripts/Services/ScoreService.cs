using System;
using Constants;
using Enums;
using EventArguments;
using Services.Mono.Sound;
using UnityEngine;

namespace Services
{
    public class ScoreService : IScoreService
    {
        private readonly IGameRoundService _gameRoundService;

        private int _completedBreakStreakIterations;

        private readonly ISoundMonoService _soundMonoService;
        private readonly int _scoreGoal;
        private int _score;

        public int Score => _score;
        public int ScoreGoal => _scoreGoal;

        public bool ScoreGoalReached => _score >= _scoreGoal;

        public event EventHandler<ScoreChangedEventArgs> OnScoreChanged;

        public ScoreService(ISoundMonoService soundMonoService, IGameRoundService gameRoundService, int scoreGoal)
        {
            _soundMonoService = soundMonoService;
            _gameRoundService = gameRoundService;

            _scoreGoal = scoreGoal;
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

            _score += scoreToAdd;

            OnScoreChanged?.Invoke(this, new ScoreChangedEventArgs(_score));

            if (ScoreGoalReached)
            {
                _gameRoundService.EndRound(true);
            }
        }
    }
}