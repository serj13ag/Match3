using System;
using System.Collections.Generic;
using Constants;
using DTO;
using Entities;
using Helpers;

namespace Services.Board.States
{
    public class CollapseColumnsTimeoutBoardState : BaseTimeoutBoardState, IBoardState
    {
        private readonly IBoardService _boardService;

        private readonly HashSet<GamePiece> _gamePiecesToCollapse;

        private readonly GamePiece[] _movedPieces;
        private int _numberOfMovedGamePieces;
        private int _movedPieceNumber;

        public CollapseColumnsTimeoutBoardState(IBoardService boardService, HashSet<GamePiece> gamePiecesToCollapse)
            : base(Settings.Timeouts.CollapseColumnsTimeout)
        {
            _boardService = boardService;

            _gamePiecesToCollapse = gamePiecesToCollapse;

            _movedPieces = new GamePiece[gamePiecesToCollapse.Count];

            foreach (GamePiece gamePiece in gamePiecesToCollapse)
            {
                gamePiece.OnPositionChanged += OnGamePiecePositionChanged;
            }
        }

        protected override void OnTimeoutEnded()
        {
            CollapseColumns(BoardHelper.GetColumnIndexes(_gamePiecesToCollapse));
        }

        private void OnGamePiecePositionChanged(GamePiece gamePiece)
        {
            _movedPieces[_movedPieceNumber] = gamePiece;

            _movedPieceNumber++;
            gamePiece.OnPositionChanged -= OnGamePiecePositionChanged;

            if (_movedPieceNumber > _numberOfMovedGamePieces)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (_movedPieceNumber == _numberOfMovedGamePieces)
            {
                HandleMovedPieces();
            }
        }

        private void CollapseColumns(HashSet<int> columnIndexes)
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

            if (gamePiecesToMoveData.Count == 0)
            {
                _boardService.ChangeStateToFill();
            }

            _numberOfMovedGamePieces = gamePiecesToMoveData.Count;
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