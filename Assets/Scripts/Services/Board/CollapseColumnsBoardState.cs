using System.Collections.Generic;
using Constants;
using DTO;
using Entities;
using Helpers;

namespace Services.Board
{
    public class CollapseColumnsBoardState : IBoardState
    {
        private readonly IBoardService _boardService;

        private readonly HashSet<GamePiece> _gamePiecesToCollapse;
        private float _timeTillExecute;
        private int _collapsedGamePieces;
        private readonly Stack<GamePiece> _movedPieces;

        public CollapseColumnsBoardState(IBoardService boardService, HashSet<GamePiece> gamePiecesToCollapse)
        {
            _boardService = boardService;

            _gamePiecesToCollapse = gamePiecesToCollapse;
            _timeTillExecute = Settings.Commands.CollapseColumnsTimeout;

            _movedPieces = new Stack<GamePiece>();
        }

        public void Update(float deltaTime)
        {
            if (_timeTillExecute < 0f)
            {
                CollapseColumns(BoardHelper.GetColumnIndexes(_gamePiecesToCollapse));
            }
            else
            {
                _timeTillExecute -= deltaTime;
            }
        }

        public void OnGamePiecePositionChanged(GamePiece gamePiece)
        {
            _movedPieces.Push(gamePiece);

            if (_movedPieces.Count == _collapsedGamePieces)
            {
                _collapsedGamePieces = 0;

                List<GamePiece> movedGamePieces = new List<GamePiece>();
                while (_movedPieces.Count > 0)
                {
                    movedGamePieces.Add(_movedPieces.Pop());
                }

                if (_boardService.HasMatches(movedGamePieces, out HashSet<GamePiece> allMatches))
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

        private void CollapseColumns(HashSet<int> columnIndexes)
        {
            var gamePiecesToMoveData = new List<GamePieceMoveData>();

            foreach (int columnIndex in columnIndexes)
            {
                gamePiecesToMoveData.AddRange(_boardService.GetGamePiecesToCollapseMoveData(columnIndex));
            }

            foreach (GamePieceMoveData gamePieceMoveData in gamePiecesToMoveData)
            {
                gamePieceMoveData.GamePiece.Move(gamePieceMoveData.Direction, gamePieceMoveData.Distance);
            }

            _collapsedGamePieces = gamePiecesToMoveData.Count;

            if (gamePiecesToMoveData.Count == 0)
            {
                _boardService.ChangeStateToFill();
            }
        }
    }
}