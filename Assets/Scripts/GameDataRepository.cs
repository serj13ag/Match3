﻿using System.Collections.Generic;
using Enums;
using Infrastructure;
using PersistentData;
using PersistentData.Models;
using UnityEngine;

public class GameDataRepository
{
    public Dictionary<TileType, TileModel> Tiles { get; private set; }
    public Dictionary<GamePieceType, GamePieceModel> GamePieces { get; private set; }
    public Dictionary<GamePieceColor, Color> Colors { get; private set; }

    public LevelData LevelData { get; }
    public MoveInterpolationType MoveInterpolationType { get; }

    public GameDataRepository(GameData gameData)
    {
        SetupTiles(gameData.TilesData);
        SetupGamePieces(gameData.GamePiecesData);
        SetupColors(gameData.ColorData);

        LevelData = gameData.LevelData;
        MoveInterpolationType = gameData.MoveData.MoveInterpolationType;
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

        foreach (ColorDataModel colorDataEntry in colorData.GamePieceColors)
        {
            Colors.Add(colorDataEntry.Type, colorDataEntry.Color);
        }
    }
}