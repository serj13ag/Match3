using System;
using EventArguments;

namespace Services
{
    public class MovesLeftService : IMovesLeftService
    {
        private readonly IScoreService _scoreService;
        private readonly IGameRoundService _gameRoundService;

        private int _movesLeft;

        public int MovesLeft => _movesLeft;

        public event EventHandler<MovesLeftChangedEventArgs> OnMovesLeftChanged;

        public MovesLeftService(IBoardService boardService, IScoreService scoreService, IGameRoundService gameRoundService,
            int movesLeft)
        {
            _scoreService = scoreService;
            _gameRoundService = gameRoundService;

            boardService.OnGamePiecesSwitched += OnGamePiecesSwitched;

            _movesLeft = movesLeft;
        }

        private void OnGamePiecesSwitched()
        {
            DecrementMovesLeft();
        }

        private void DecrementMovesLeft()
        {
            _movesLeft--;

            OnMovesLeftChanged?.Invoke(this, new MovesLeftChangedEventArgs(_movesLeft));

            if (_movesLeft == 0)
            {
                _gameRoundService.EndRound(_scoreService.ScoreGoalReached);
            }
        }
    }
}