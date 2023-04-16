using Enums;
using UnityEngine;

namespace Entities
{
    public class CollectibleGamePiece : GamePiece
    {
        [SerializeField] private CollectibleType _collectibleType;

        public CollectibleType CollectibleType => _collectibleType;
    }
}