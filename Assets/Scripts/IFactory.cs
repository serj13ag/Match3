using Entities;
using Enums;
using Interfaces;
using UnityEngine;

public interface IFactory
{
    ITile CreateTile(TileType tileType, int x, int y, Transform parentTransform);
    GamePiece CreateNormalGamePieceWithRandomColor(int x, int y, Transform parentTransform);

    GamePiece CreateBombGamePiece(int x, int y, Transform parentTransform, BombType bombType,
        GamePieceColor color);

    GamePiece CreateRandomCollectibleGamePiece(int x, int y, Transform parentTransform);

    GamePiece CreateGamePiece(GamePieceType gamePieceType, GamePieceColor color, int x, int y,
        Transform parentTransform);
}