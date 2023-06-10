using System;
using Entities;
using Entities.Tiles;
using Enums;
using Interfaces;
using StaticData;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Services
{
    public class GameFactory : IGameFactory
    {
        private const string TilesContainerName = "Tiles";
        private const string GamePiecesContainerName = "GamePieces";

        private readonly IRandomService _randomService;
        private readonly IStaticDataService _staticDataService;
        private readonly ParticleService _particleService;

        private readonly Transform _tilesContainerTransform;
        private readonly Transform _gamePiecesContainerTransform;

        public GameFactory(IRandomService randomService, IStaticDataService staticDataService,
            ParticleService particleService)
        {
            _randomService = randomService;
            _staticDataService = staticDataService;
            _particleService = particleService;

            GameObject tilesContainer = new GameObject(TilesContainerName);
            _tilesContainerTransform = tilesContainer.transform;

            GameObject gamePiecesContainer = new GameObject(GamePiecesContainerName);
            _gamePiecesContainerTransform = gamePiecesContainer.transform;
        }

        public ITile CreateTile(TileType tileType, int x, int y)
        {
            TileStaticData tileData = _staticDataService.GetDataForTile(tileType);
            BaseTile tile = Instantiate(tileData.Prefab, new Vector3(x, y, 0));
            tile.Init(tileData, x, y, _tilesContainerTransform, _particleService);
            return tile;
        }

        public GamePiece CreateNormalGamePieceWithRandomColor(string levelName, int x, int y)
        {
            return CreateGamePiece(GamePieceType.Normal, GetRandomGamePieceColor(levelName), x, y);
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
            GamePieceStaticData gamePieceData = _staticDataService.GetDataForGamePiece(gamePieceType);
            GamePiece gamePiece = Instantiate(gamePieceData.Prefab, Vector3.zero);
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

        private GamePieceColor GetRandomGamePieceColor(string levelName)
        {
            int randomIndex = _randomService.Next(_staticDataService.GetDataForLevel(levelName).AvailableColors.Length);
            return _staticDataService.GetDataForLevel(levelName).AvailableColors[randomIndex];
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

        private static T Instantiate<T>(T prefab, Vector3 position) where T : Object
        {
            return Object.Instantiate(prefab, position, Quaternion.identity);
        }
    }
}