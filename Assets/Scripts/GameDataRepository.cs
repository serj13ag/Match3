using System.Collections.Generic;
using Enums;
using PersistentData;
using UnityEngine;

public class GameDataRepository
{
    public Dictionary<TileType, TileModel> Tiles { get; private set; }
    public Dictionary<GamePieceColor, Color> Colors { get; private set; }

    public MoveInterpolationType MoveInterpolationType { get; }

    public GameDataRepository(TilesData tilesData, ColorData colorData, MoveData moveData)
    {
        SetupTiles(tilesData);
        SetupColors(colorData);

        MoveInterpolationType = moveData.MoveInterpolationType;
    }

    private void SetupColors(ColorData colorData)
    {
        Colors = new Dictionary<GamePieceColor, Color>();

        foreach (ColorDataEntry colorDataEntry in colorData.GamePieceColors)
        {
            Colors.Add(colorDataEntry.Type, colorDataEntry.Color);
        }
    }

    private void SetupTiles(TilesData tilesData)
    {
        Tiles = new Dictionary<TileType, TileModel>();

        foreach (TileModel tileModel in tilesData.Tiles)
        {
            Tiles.Add(tileModel.Type, tileModel);
        }
    }
}