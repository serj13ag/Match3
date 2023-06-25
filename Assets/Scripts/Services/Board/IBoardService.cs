using System;
using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Services.Board
{
    public interface IBoardService
    {
        Vector2Int BoardSize { get; }

        event Action OnGamePiecesSwitched;

        void InvokeGamePiecesSwitched();

        bool PlayerMovedColorBomb(GamePiece clickedGamePiece, GamePiece targetGamePiece, out HashSet<GamePiece> gamePiecesToClear);

        void ChangeStateToBreak(HashSet<GamePiece> gamePiecesToBreak);
        void ChangeStateToCollapse(HashSet<int> columnIndexesToCollapse);
        void ChangeStateToFill();
        void ChangeStateToWaiting();
    }
}