using System;
using Data;
using EventArguments;
using Interfaces;

namespace Services.MovesLeft
{
    public class MovesLeftService : IMovesLeftService, IProgressWriter
    {
        private int _movesLeft;

        public int MovesLeft => _movesLeft;

        public event EventHandler<MovesLeftChangedEventArgs> OnMovesLeftChanged;

        public MovesLeftService(string levelName, IPersistentProgressService persistentProgressService,
            IProgressUpdateService progressUpdateService, int movesLeft)
        {
            progressUpdateService.Register(this);

            LevelBoardData levelBoardData = persistentProgressService.Progress.BoardData.LevelBoardData;
            if (levelName == levelBoardData.LevelName && levelBoardData.MovesLeft > 0)
            {
                _movesLeft = levelBoardData.MovesLeft;
            }
            else
            {
                _movesLeft = movesLeft;
            }
        }

        public void WriteToProgress(PlayerProgress progress)
        {
            progress.BoardData.LevelBoardData.MovesLeft = _movesLeft;
        }

        public void DecrementMovesLeft()
        {
            _movesLeft--;

            OnMovesLeftChanged?.Invoke(this, new MovesLeftChangedEventArgs(_movesLeft));
        }
    }
}