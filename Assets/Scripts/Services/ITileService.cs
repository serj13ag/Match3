using System;
using EventArguments;
using Interfaces;
using UnityEngine;

namespace Services
{
    public interface ITileService
    {
        ITile[,] Tiles { get; }

        event EventHandler<MoveRequestedEventArgs> OnMoveRequested;

        void ProcessTileMatchAt(Vector2Int position);
    }
}