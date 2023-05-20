using System;
using Controllers;
using Entities;
using Entities.Tiles;
using Enums;
using Interfaces;
using PersistentData.Models;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameFactory : IGameFactory
{
    private readonly RandomService _randomService;
    private readonly GameDataRepository _gameDataRepository;
    private readonly ParticleController _particleController;

    public GameFactory(RandomService randomService, GameDataRepository gameDataRepository,
        ParticleController particleController)
    {
        _randomService = randomService;
        _gameDataRepository = gameDataRepository;
        _particleController = particleController;
    }

    public ITile CreateTile(TileType tileType, int x, int y, Transform parentTransform)
    {
        TileModel tileModel = _gameDataRepository.Tiles[tileType];
        BaseTile tile = Object.Instantiate(tileModel.TilePrefab, new Vector3(x, y, 0), Quaternion.identity);
        tile.Init(x, y, parentTransform, _particleController, tileModel);
        return tile;
    }

    public GamePiece CreateNormalGamePieceWithRandomColor(int x, int y, Transform parentTransform)
    {
        return CreateGamePiece(GamePieceType.Normal, GetRandomGamePieceColor(), x, y, parentTransform);
    }

    public GamePiece CreateBombGamePiece(int x, int y, Transform parentTransform, BombType bombType,
        GamePieceColor color)
    {
        if (bombType == BombType.Color)
        {
            color = GamePieceColor.Undefined;
        }

        return CreateGamePiece(GetGamePieceType(bombType), color, x, y, parentTransform);
    }

    public GamePiece CreateRandomCollectibleGamePiece(int x, int y, Transform parentTransform)
    {
        return CreateGamePiece(GetRandomCollectibleGamePieceType(), GamePieceColor.Undefined, x, y, parentTransform);
    }

    public GamePiece CreateGamePiece(GamePieceType gamePieceType, GamePieceColor color, int x, int y,
        Transform parentTransform)
    {
        GamePieceModel gamePieceModel = _gameDataRepository.GamePieces[gamePieceType];
        GamePiece gamePiece = Object.Instantiate(gamePieceModel.GamePiecePrefab, Vector3.zero, Quaternion.identity);
        gamePiece.Init(color, x, y, _gameDataRepository, parentTransform, gamePieceModel);
        return gamePiece;
    }

    private static GamePieceType GetGamePieceType(BombType bombType)
    {
        return bombType switch
        {
            BombType.Row => GamePieceType.BombRow,
            BombType.Column => GamePieceType.BombColumn,
            BombType.Adjacent => GamePieceType.BombAdjacent,
            BombType.Color => GamePieceType.BombColor,
            _ => throw new ArgumentOutOfRangeException(nameof(bombType), bombType, null)
        };
    }

    private GamePieceColor GetRandomGamePieceColor()
    {
        int randomIndex = _randomService.Next(_gameDataRepository.LevelData.AvailableColors.Length);
        return _gameDataRepository.LevelData.AvailableColors[randomIndex];
    }

    private GamePieceType GetRandomCollectibleGamePieceType()
    {
        GamePieceType[] collectibleGamePieceTypes = new[]
        {
            GamePieceType.CollectibleByBomb,
            GamePieceType.CollectibleByBottomRow,
        };

        return collectibleGamePieceTypes[_randomService.Next(collectibleGamePieceTypes.Length)];
    }
}