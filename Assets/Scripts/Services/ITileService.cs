using System;
using EventArguments;
using UnityEngine;

namespace Services
{
    public interface ITileService
    {
        event EventHandler<MoveRequestedEventArgs> OnMoveRequested;

        void ProcessTileMatchAt(Vector2Int position);
        bool IsObstacleAt(int column, int row);
        void UpdateProgress();
    }
}