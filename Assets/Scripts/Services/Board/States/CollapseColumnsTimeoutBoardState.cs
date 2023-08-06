using System.Collections.Generic;
using Constants;
using DTO;
using Entities;
using Services.GameRound;
using Services.MovesLeft;

namespace Services.Board.States
{
    public class CollapseColumnsTimeoutBoardState : BaseTimeoutBoardState
    {
        private readonly IBoardService _boardService;
        private readonly IGamePieceService _gamePieceService;
        private readonly IGameRoundService _gameRoundService;
        private readonly IMovesLeftService _movesLeftService;

        private readonly HashSet<int> _columnIndexesToCollapse;

        private GamePiece[] _movedPieces;
        private int _numberOfGamePiecesToMove;
        private int _movedPieceNumber;

        public CollapseColumnsTimeoutBoardState(IBoardService boardService, IGamePieceService gamePieceService,
            IGameRoundService gameRoundService, IMovesLeftService movesLeftService, HashSet<int> columnIndexesToCollapse)
            : base(Settings.Timeouts.CollapseColumnsTimeout)
        {
            _boardService = boardService;
            _gamePieceService = gamePieceService;
            _gameRoundService = gameRoundService;
            _movesLeftService = movesLeftService;

            _columnIndexesToCollapse = columnIndexesToCollapse;
        }

        protected override void OnTimeoutEnded()
        {
            TryCollapseColumns(_columnIndexesToCollapse);
        }

        private void OnGamePiecePositionChanged(GamePiece gamePiece)
        {
            _movedPieces[_movedPieceNumber] = gamePiece;

            _movedPieceNumber++;
            gamePiece.OnPositionChanged -= OnGamePiecePositionChanged;

            if (_movedPieceNumber == _numberOfGamePiecesToMove)
            {
                HandleMovedPieces();
            }
        }

        private void TryCollapseColumns(HashSet<int> columnIndexes)
        {
            List<GamePieceMoveData> gamePiecesToMoveData = new List<GamePieceMoveData>();

            foreach (int columnIndex in columnIndexes)
            {
                gamePiecesToMoveData.AddRange(_gamePieceService.GetGamePiecesToCollapseMoveData(columnIndex));
            }

            foreach (GamePieceMoveData gamePieceMoveData in gamePiecesToMoveData)
            {
                gamePieceMoveData.GamePiece.Move(gamePieceMoveData.Direction, gamePieceMoveData.Distance);
            }

            if (gamePiecesToMoveData.Count > 0)
            {
                _movedPieces = new GamePiece[gamePiecesToMoveData.Count];
                _numberOfGamePiecesToMove = gamePiecesToMoveData.Count;

                foreach (GamePieceMoveData gamePieceMoveData in gamePiecesToMoveData)
                {
                    gamePieceMoveData.GamePiece.OnPositionChanged += OnGamePiecePositionChanged;
                }
            }
            else // If removed game pieces at top of columns
            {
                ChangeStateOrEndGame();
            }
        }

        private void HandleMovedPieces()
        {
            if (_gamePieceService.HasMatches(_movedPieces, out HashSet<GamePiece> allMatches))
            {
                _boardService.ChangeStateToBreak(allMatches);
            }
            else if (_gamePieceService.HasCollectiblesToBreak(out HashSet<GamePiece> collectiblesToBreak))
            {
                _boardService.ChangeStateToBreak(collectiblesToBreak);
            }
            else
            {
                ChangeStateOrEndGame();
            }
        }

        private void ChangeStateOrEndGame()
        {
            if (_movesLeftService.MovesLeft > 0)
            {
                _boardService.ChangeStateToFill();
            }
            else
            {
                _gameRoundService.EndRound();
            }
        }
    }
}