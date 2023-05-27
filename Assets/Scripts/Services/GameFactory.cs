using System;
using Entities;
using Entities.Tiles;
using Enums;
using Interfaces;
using Services.Mono;
using StaticData;
using StaticData.Models;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Services
{
    public class GameFactory : IGameFactory
    {
        private const string TilesContainerName = "Tiles";
        private const string GamePiecesContainerName = "GamePieces";

        private readonly RandomService _randomService;
        private readonly StaticDataService _staticDataService;
        private readonly ParticleMonoService _particleMonoService;

        private readonly Transform _tilesContainerTransform;
        private readonly Transform _gamePiecesContainerTransform;

        public GameFactory(RandomService randomService, StaticDataService staticDataService,
            ParticleMonoService particleMonoService)
        {
            _randomService = randomService;
            _staticDataService = staticDataService;
            _particleMonoService = particleMonoService;

            GameObject tilesContainer = new GameObject(TilesContainerName);
            _tilesContainerTransform = tilesContainer.transform;

            GameObject gamePiecesContainer = new GameObject(GamePiecesContainerName);
            _gamePiecesContainerTransform = gamePiecesContainer.transform;
        }

        public ITile CreateTile(TileType tileType, int x, int y)
        {
            TileModel tileModel = _staticDataService.Tiles[tileType];
            BaseTile tile = Object.Instantiate(tileModel.TilePrefab, new Vector3(x, y, 0), Quaternion.identity);
            tile.Init(x, y, _tilesContainerTransform, _particleMonoService, tileModel);
            return tile;
        }

        public GamePiece CreateNormalGamePieceWithRandomColor(int x, int y)
        {
            return CreateGamePiece(GamePieceType.Normal, GetRandomGamePieceColor(), x, y);
        }

        public GamePiece CreateBombGamePiece(int x, int y, BombType bombType, GamePieceColor color)
        {
            if (bombType == BombType.Color)
            {
                color = GamePieceColor.Undefined;
            }

            return CreateGamePiece(GetGamePieceType(bombType), color, x, y);
        }

        public GamePiece CreateRandomCollectibleGamePiece(int x, int y)
        {
            return CreateGamePiece(GetRandomCollectibleGamePieceType(), GamePieceColor.Undefined, x, y);
        }

        public GamePiece CreateGamePiece(GamePieceType gamePieceType, GamePieceColor color, int x, int y)
        {
            GamePieceStaticData gamePieceData = _staticDataService.GamePieces[gamePieceType];
            GamePiece gamePiece = Object.Instantiate(gamePieceData.Prefab, Vector3.zero, Quaternion.identity);
            gamePiece.Init(gamePieceData, color, x, y, _gamePiecesContainerTransform, _staticDataService);
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
            int randomIndex = _randomService.Next(_staticDataService.LevelData.AvailableColors.Length);
            return _staticDataService.LevelData.AvailableColors[randomIndex];
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
}