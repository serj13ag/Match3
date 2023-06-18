using Constants;
using Entities;

namespace Services.Board
{
    public class FillBoardState : IBoardState
    {
        private readonly IBoardService _boardService;

        private float _timeTillExecute;

        public FillBoardState(IBoardService boardService)
        {
            _boardService = boardService;
            _timeTillExecute = Settings.Commands.FillBoardTimeout;
        }

        public void Update(float deltaTime)
        {
            if (_timeTillExecute < 0f)
            {
                _boardService.FillBoardWithRandomGamePieces();
                _boardService.ChangeStateToWaiting();
            }
            else
            {
                _timeTillExecute -= deltaTime;
            }
        }

        public void OnGamePiecePositionChanged(GamePiece gamePiece)
        {
        }
    }
}