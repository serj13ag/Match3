using System;
using Data;
using EventArguments;
using Interfaces;

namespace Services.MovesLeft
{
    public class MovesLeftService : IMovesLeftService, IProgressWriter
    {
        private readonly string _levelName;
        private int _movesLeft;

        public int MovesLeft => _movesLeft;

        public event EventHandler<MovesLeftChangedEventArgs> OnMovesLeftChanged;

        public MovesLeftService(string levelName, IPersistentDataService persistentDataService,
            IProgressUpdateService progressUpdateService, int movesLeft)
        {
            _levelName = levelName;

            progressUpdateService.Register(this);

            if (persistentDataService.Progress.BoardData.TryGetValue(levelName, out LevelBoardData levelBoardData))
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
            progress.BoardData[_levelName].MovesLeft = _movesLeft;
        }

        public void DecrementMovesLeft()
        {
            _movesLeft--;

            OnMovesLeftChanged?.Invoke(this, new MovesLeftChangedEventArgs(_movesLeft));
        }
    }
}