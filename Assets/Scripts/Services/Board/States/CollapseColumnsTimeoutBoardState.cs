using System;
using System.Collections.Generic;
using Constants;
using DTO;
using Entities;

namespace Services.Board.States
{
    public class CollapseColumnsTimeoutBoardState : BaseTimeoutBoardState, IBoardState
    {
        private readonly IBoardService _boardService;

        private readonly HashSet<int> _columnIndexesToCollapse;

        private GamePiece[] _movedPieces;
        private int _numberOfGamePiecesToMove;
        private int _movedPieceNumber;

        public CollapseColumnsTimeoutBoardState(IBoardService boardService, HashSet<int> columnIndexesToCollapse)
            : base(Settings.Timeouts.CollapseColumnsTimeout)
        {
            _boardService = boardService;

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

            if (_movedPieceNumber > _numberOfGamePiecesToMove)
            {
                throw new ArgumentOutOfRangeException();
            }

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
                gamePiecesToMoveData.AddRange(_boardService.GetGamePiecesToCollapseMoveData(columnIndex));
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
            else
            {
                // If removed game pieces at top of columns
                _boardService.ChangeStateToFill();
            }
        }

        private void HandleMovedPieces()
        {
            if (_boardService.HasMatches(_movedPieces, out HashSet<GamePiece> allMatches))
            {
                _boardService.ChangeStateToBreak(allMatches);
            }
            else if (_boardService.HasCollectiblesToBreak(out HashSet<GamePiece> collectiblesToBreak))
            {
                _boardService.ChangeStateToBreak(collectiblesToBreak);
            }
            else
            {
                _boardService.ChangeStateToFill();
            }
        }
    }
}