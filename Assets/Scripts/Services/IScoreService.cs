using System;
using EventArguments;

namespace Services
{
    public interface IScoreService
    {
        int Score { get; }
        bool ScoreGoalReached { get; }
        int ScoreGoal { get; }

        event EventHandler<ScoreChangedEventArgs> OnScoreChanged;

        void AddScore(int gamePieceScore, int numberOfBreakGamePieces);

        void IncrementCompletedBreakStreakIterations();
        void ResetBreakStreakIterations();
        void UpdateProgress();
    }
}