using System;
using System.Collections.Generic;
using DTO;
using Entities;
using Enums;
using UnityEngine;

namespace Services.Board
{
    public interface IBoardService
    {
        Vector2Int BoardSize { get; }

        event Action OnGamePiecesSwitched;

        void ClearGamePieceAt(Vector2Int position, bool breakOnMatch = false);
        IEnumerable<GamePieceMoveData> GetGamePiecesToCollapseMoveData(int columnIndex);
        bool HasMatches(IEnumerable<GamePiece> gamePieces, out HashSet<GamePiece> allMatches);
        bool HasCollectiblesToBreak(out HashSet<GamePiece> gamePieces);
        void FillBoardWithRandomGamePieces();
        void InvokeGamePiecesSwitched();
        bool PlayerMovedColorBomb(GamePiece clickedGamePiece, GamePiece targetGamePiece, out HashSet<GamePiece> gamePiecesToClear);
        void SpawnBombGamePiece(int x, int y, BombType bombType, GamePieceColor color);
        HashSet<GamePiece> GetBombedRowGamePieces(int row);
        HashSet<GamePiece> GetBombedColumnGamePieces(int column);
        HashSet<GamePiece> GetBombedAdjacentGamePieces(Vector2Int position, int range);
        void ChangeStateToBreak(HashSet<GamePiece> gamePiecesToBreak);
        void ChangeStateToCollapse(HashSet<int> columnIndexesToCollapse);
        void ChangeStateToFill();
        void ChangeStateToWaiting();
    }
}