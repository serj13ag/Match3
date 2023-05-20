﻿using Entities;
using Enums;
using Interfaces;

public interface IGameFactory
{
    ITile CreateTile(TileType tileType, int x, int y);
    GamePiece CreateNormalGamePieceWithRandomColor(int x, int y);

    GamePiece CreateBombGamePiece(int x, int y, BombType bombType, GamePieceColor color);

    GamePiece CreateRandomCollectibleGamePiece(int x, int y);

    GamePiece CreateGamePiece(GamePieceType gamePieceType, GamePieceColor color, int x, int y);
}