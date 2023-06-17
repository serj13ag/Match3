using System;
using UnityEngine;

namespace EventArguments
{
    public class MoveRequestedEventArgs : EventArgs
    {
        public Vector2Int FromPosition { get; }
        public Vector2Int ToPosition { get; }

        public MoveRequestedEventArgs(Vector2Int fromPosition, Vector2Int toPosition)
        {
            FromPosition = fromPosition;
            ToPosition = toPosition;
        }
    }
}