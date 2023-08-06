namespace Services.Board.States
{
    public class WaitingBoardState : IBoardState
    {
        public WaitingBoardState(IBoardService boardService, IGamePieceService gamePieceService)
        {
            if (!gamePieceService.HasAvailableMoves(out _))
            {
                gamePieceService.ClearBoard();
                boardService.ChangeStateToFill();
            }
        }

        public void Update(float deltaTime)
        {
        }
    }
}