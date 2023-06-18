using Constants;
using Entities;

namespace Services.Board
{
    public class FillBoardState : BaseBoardStateWithTimeout, IBoardState
    {
        private readonly IBoardService _boardService;

        // TODO : fix timeout bug because must start timeout when collapsed pieces moved to end positions
        public FillBoardState(IBoardService boardService)
            : base(Settings.Timeouts.FillBoardTimeout)
        {
            _boardService = boardService;
        }

        public void OnGamePiecePositionChanged(GamePiece gamePiece)
        {
        }

        protected override void OnTimeoutEnded()
        {
            _boardService.FillBoardWithRandomGamePieces();
            _boardService.ChangeStateToWaiting();
        }
    }
}