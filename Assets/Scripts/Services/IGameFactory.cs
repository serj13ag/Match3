using Entities;
using Enums;
using Interfaces;

namespace Services
{
    public interface IGameFactory
    {
        ITile CreateTile(TileType tileType, int x, int y);

        GamePiece CreateNormalGamePieceWithRandomColor(string levelName, int x, int y, int offsetY = 0);
        GamePiece CreateBombGamePiece(int x, int y, BombType bombType, GamePieceColor color);
        GamePiece CreateRandomCollectibleGamePiece(int x, int y, int offsetY = 0);
        GamePiece CreateGamePiece(GamePieceType gamePieceType, GamePieceColor color, int x, int y, int offsetY = 0);
    }
}