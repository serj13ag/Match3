using Constants;

namespace Services.Board.States
{
    public class FillTimeoutBoardState : BaseTimeoutBoardState
    {
        private readonly IBoardService _boardService;
        private readonly IGamePieceService _gamePieceService;

        public FillTimeoutBoardState(IBoardService boardService, IGamePieceService gamePieceService)
            : base(Settings.Timeouts.FillBoardTimeout)
        {
            _boardService = boardService;
            _gamePieceService = gamePieceService;
        }

        protected override void OnTimeoutEnded()
        {
            _gamePieceService.FillBoardWithRandomGamePieces();
            _boardService.ChangeStateToWaiting();
        }
    }
}