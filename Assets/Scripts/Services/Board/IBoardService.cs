using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Services.Board
{
    public interface IBoardService
    {
        Vector2Int BoardSize { get; }

        bool PlayerMovedColorBomb(GamePiece clickedGamePiece, GamePiece targetGamePiece, out HashSet<GamePiece> gamePiecesToBreak);

        void ChangeStateToBreak(HashSet<GamePiece> gamePiecesToBreak);
        void ChangeStateToCollapse(HashSet<int> columnIndexesToCollapse);
        void ChangeStateToFill();
        void ChangeStateToWaiting(bool needSave = false);
        void Cleanup();
    }
}