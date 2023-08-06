using System;
using EventArguments;

namespace Services
{
    public interface IPlayerLevelService
    {
        int CurrentLevel { get; }
        int ScoreToNextLevel { get; }

        public event EventHandler<PlayerLevelChangedEventArgs> OnCurrentLevelChanged;

        void GoToNextLevel();
    }
}