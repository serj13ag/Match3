using Constants;

namespace Services.Board.States
{
    public class FillTimeoutBoardState : BaseTimeoutBoardState
    {
        private readonly IBoardService _boardService;

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