using System;
using UnityEngine;

namespace Interfaces
{
    public interface ITile
    {
        Vector2Int Position { get; }
        bool IsObstacle { get; }

        void ProcessMatch();

        event Action OnMouseReleased;
        event Action<ITile> OnMouseEntered;
        event Action<ITile> OnClicked;
    }
}