using System;
using Constants;
using EventArgs;
using UnityEngine;

namespace Services
{
    public class ScoreService
    {
        private readonly GameRoundService _gameRoundService;

        private readonly int _scoreGoal;
        private int _score;

        public int Score => _score;

        public bool ScoreGoalReached => _score >= _scoreGoal;

        public event EventHandler<ScoreChangedEventArgs> OnScoreChanged;

        public ScoreService(GameRoundService gameRoundService, int scoreGoal)
        {
            _scoreGoal = scoreGoal;
            _gameRoundService = gameRoundService;
        }

        public void AddScore(int gamePieceScore, int numberOfBreakGamePieces,
            int completedBreakIterationsAfterSwitchedGamePieces)
        {
            int bonusScore = numberOfBreakGamePieces >= Settings.Score.MinNumberOfBreakGamePiecesToGrantBonus
                ? Settings.Score.BonusScore
                : 0;
            int scoreMultiplier = completedBreakIterationsAfterSwitchedGamePieces + 1;
            int totalScore = gamePieceScore * scoreMultiplier + bonusScore;

            AddScore(totalScore);
        }

        private void AddScore(int scoreToAdd)
        {
            if (scoreToAdd < 0)
            {
                Debug.LogError($"{nameof(ScoreService)} : Attempt to add negative score: {scoreToAdd}!"); // TODO: add logger
                return;
            }

            if (scoreToAdd == 0)
            {
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