using System;
using UnityEngine;

namespace Services
{
    public interface IBoardService
    {
        Vector2Int BoardSize { get; }
        event Action OnGamePiecesSwitched;
    }
}