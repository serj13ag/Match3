using System;
using System.Collections.Generic;
using Data;
using EventArguments;
using UnityEngine;

namespace Services
{
    public interface ITileService
    {
        event EventHandler<MoveRequestedEventArgs> OnMoveRequested;

        void Initialize();
        void Initialize(List<TileSaveData> tiles);

        void ProcessTileMatchAt(Vector2Int position);
        bool IsObstacleAt(int column, int row);
    }
}