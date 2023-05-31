using System;
using EventArgs;

namespace Services
{
    public class MovesLeftService
    {
        private readonly ScoreService _scoreService;
        private readonly GameRoundService _gameRoundService;

        private int _movesLeft;

        public int MovesLeft => _movesLeft;

        public event EventHandler<MovesLeftChangedEventArgs> OnMovesLeftChanged;

        public MovesLeftService(BoardService boardService, ScoreService scoreService, GameRoundService gameRoundService,
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
                _gameRoundService.GameOver(_scoreService.ScoreGoalReached);
            }
        }
    }
}