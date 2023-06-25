using Constants;

namespace Services.Board.States
{
    public class FillTimeoutBoardState : BaseTimeoutBoardState, IBoardState
    {
        private readonly IBoardService _boardService;

        // TODO : fix timeout bug because must start timeout when collapsed pieces moved to end positions
        public FillTimeoutBoardState(IBoardService boardService)
            : base(Settings.Timeouts.FillBoardTimeout)
        {
            _boardService = boardService;
        }

        protected override void OnTimeoutEnded()
        {
            _boardService.FillBoardWithRandomGamePieces();
            _boardService.ChangeStateToWaiting();
        }
    }
}