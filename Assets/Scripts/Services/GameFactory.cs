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

        private readonly string _levelName;
        private readonly IRandomService _randomService;
        private readonly IStaticDataService _staticDataService;
        private readonly IParticleService _particleService;

        private readonly Transform _tilesContainerTransform;
        private readonly Transform _gamePiecesContainerTransform;

        public GameFactory(string levelName, IRandomService randomService, IStaticDataService staticDataService,
            IParticleService particleService)
        {
            _levelName = levelName;
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

        public GamePiece CreateNormalGamePieceWithRandomColor(string levelName, int x, int y, int offsetY = 0)
        {
            return CreateGamePiece(GamePieceType.Normal, GetRandomGamePieceColor(levelName), x, y, offsetY);
        }

        public GamePiece CreateBombGamePiece(int x, int y, BombType bombType, GamePieceColor color)
        {
            if (bombType == BombType.Color)
            {
                color = GamePieceColor.Undefined;
            }

            return CreateGamePiece(GetGamePieceType(bombType), color, x, y);
        }

        public GamePiece CreateRandomCollectibleGamePiece(int x, int y, int offsetY = 0)
        {
            return CreateGamePiece(GetRandomCollectibleGamePieceType(), GamePieceColor.Undefined, x, y, offsetY);
        }

        public GamePiece CreateGamePiece(GamePieceType gamePieceType, GamePieceColor color, int x, int y, int offsetY = 0)
        {
            GamePieceStaticData gamePieceData = _staticDataService.GetDataForGamePiece(gamePieceType);
            GamePiece gamePiece = Instantiate(gamePieceData.Prefab, Vector3.zero);
            gamePiece.Init(_levelName, gamePieceData, color, x, y, offsetY, _gamePiecesContainerTransform, _staticDataService);
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