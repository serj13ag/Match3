using Enums;
using UnityEngine;

namespace Entities
{
    public class BombGamePiece : GamePiece
    {
        [SerializeField] private BombType _bombType;

        public BombType BombType => _bombType;
    }
}