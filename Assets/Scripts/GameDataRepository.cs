using System.Collections.Generic;
using Enums;
using PersistentData;
using UnityEngine;

public class GameDataRepository
{
    public Dictionary<TileType, TileModel> Tiles { get; private set; }
    public Dictionary<GamePieceType, GamePieceModel> GamePieces { get; private set; }
    public Dictionary<GamePieceColor, Color> Colors { get; private set; }

    public MoveInterpolationType MoveInterpolationType { get; }

    public GameDataRepository(TilesData tilesData, GamePiecesData gamePiecesData, ColorData colorData,
        MoveData moveData)
    {
        SetupTiles(tilesData);
        SetupGamePieces(gamePiecesData);
        SetupColors(colorData);

        MoveInterpolationType = moveData.MoveInterpolationType;
    }

    private void SetupTiles(TilesData tilesData)
    {
        Tiles = new Dictionary<TileType, TileModel>();

        foreach (TileModel tileModel in tilesData.Tiles)
        {
            Tiles.Add(tileModel.Type, tileModel);
        }
    }

    private void SetupGamePieces(GamePiecesData gamePiecesData)
    {
        GamePieces = new Dictionary<GamePieceType, GamePieceModel>();

        foreach (GamePieceModel gamePieceModel in gamePiecesData.GamePieces)
        {
            GamePieces.Add(gamePieceModel.Type, gamePieceModel);
        }
    }

    private void SetupColors(ColorData colorData)
    {
        Colors = new Dictionary<GamePieceColor, Color>();

        foreach (ColorDataEntry colorDataEntry in colorData.GamePieceColors)
        {
            Colors.Add(colorDataEntry.Type, colorDataEntry.Color);
        }
    }
}