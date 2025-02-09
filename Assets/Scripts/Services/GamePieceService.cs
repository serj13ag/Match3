﻿using System;
using System.Collections.Generic;
using Constants;
using Data;
using DTO;
using Entities;
using Enums;
using Helpers;
using Interfaces;
using Services.Mono.Sound;
using StaticData.StartingData;
using UnityEngine;
using UnityEngine.Assertions;

namespace Services
{
    public class GamePieceService : IGamePieceService, IProgressWriter
    {
        private readonly ISoundMonoService _soundMonoService;
        private readonly IParticleService _particleService;
        private readonly IGameFactory _gameFactory;
        private readonly IRandomService _randomService;
        private readonly IStaticDataService _staticDataService;
        private readonly ITileService _tileService;

        private readonly string _levelName;
        private readonly Vector2Int _boardSize;

        private readonly GamePiece[,] _gamePieces;
        private int _collectibleGamePieces;

        public GamePieceService(string levelName, IStaticDataService staticDataService,
            ISoundMonoService soundMonoService, IRandomService randomService,
            IProgressUpdateService progressUpdateService, ITileService tileService, IGameFactory gameFactory,
            IParticleService particleService)
        {
            _soundMonoService = soundMonoService;
            _particleService = particleService;
            _gameFactory = gameFactory;
            _randomService = randomService;
            _staticDataService = staticDataService;
            _tileService = tileService;

            _levelName = levelName;
            _boardSize = new Vector2Int(staticDataService.Settings.BoardWidth, staticDataService.Settings.BoardHeight);

            _gamePieces = new GamePiece[staticDataService.Settings.BoardWidth, staticDataService.Settings.BoardHeight];

            progressUpdateService.Register(this);
        }

        public void Initialize()
        {
            StartingGamePiecesStaticData startingGamePiecesStaticData = _staticDataService.GetDataForLevel(_levelName)?.StartingGamePieces;
            if (startingGamePiecesStaticData != null)
            {
                foreach (StartingGamePieceStaticData startingGamePieceEntry in startingGamePiecesStaticData.StartingGamePieces)
                {
                    SpawnCustomGamePiece(startingGamePieceEntry.X, startingGamePieceEntry.Y, startingGamePieceEntry.Type,
                        startingGamePieceEntry.Color);
                }
            }

            FillBoardWithRandomGamePieces();
        }

        public void Initialize(List<GamePieceSaveData> gamePieces)
        {
            foreach (GamePieceSaveData gamePiece in gamePieces)
            {
                SpawnCustomGamePiece(gamePiece.Position.x, gamePiece.Position.y, gamePiece.Type, gamePiece.Color);
            }
        }

        public void WriteToProgress(PlayerProgress progress)
        {
            List<GamePieceSaveData> gamePieceSaveData = new List<GamePieceSaveData>();

            foreach (GamePiece gamePiece in _gamePieces)
            {
                if (gamePiece != null)
                {
                    gamePieceSaveData.Add(new GamePieceSaveData(gamePiece.Type, gamePiece.Position, gamePiece.Color));
                }
            }

            progress.BoardData[_levelName].GamePieces = gamePieceSaveData;
        }

        public List<GamePiece> FillBoardWithRandomGamePieces(int offsetY = 0)
        {
            List<GamePiece> spawnedGamePieces = new List<GamePiece>();

            for (int i = 0; i < _gamePieces.GetWidth(); i++)
            {
                for (int j = 0; j < _gamePieces.GetHeight(); j++)
                {
                    if (_gamePieces[i, j] != null || _tileService.IsObstacleAt(i, j))
                    {
                        continue;
                    }

                    if (Settings.CollectibleGamePieceEnabled && TrySpawnCollectibleGamePiece(i, j, out GamePiece spawnedCollectableGamePiece, offsetY))
                    {
                        spawnedGamePieces.Add(spawnedCollectableGamePiece);
                        continue;
                    }

                    GamePiece gamePiece = SpawnBasicGamePieceWithRandomColor(i, j, offsetY);

                    while (GamePieceMatchHelper.HasMatchAtFillBoard(new Vector2Int(i, j), _gamePieces, _boardSize))
                    {
                        ClearGamePieceAt(gamePiece.Position);
                        gamePiece = SpawnBasicGamePieceWithRandomColor(i, j, offsetY);
                    }

                    spawnedGamePieces.Add(gamePiece);
                }
            }

            return spawnedGamePieces;
        }

        public void ClearBoard()
        {
            foreach (GamePiece gamePiece in _gamePieces)
            {
                ClearGamePieceAt(gamePiece.Position);
            }
        }

        public IEnumerable<GamePieceMoveData> GetGamePiecesToCollapseMoveData(int column)
        {
            Queue<int> availableRows = new Queue<int>();
            List<GamePieceMoveData> moveDataEntries = new List<GamePieceMoveData>();

            for (int row = 0; row < _gamePieces.GetHeight(); row++)
            {
                Vector2Int position = new Vector2Int(column, row);
                if (TryGetGamePieceAt(position, out GamePiece gamePiece))
                {
                    int distanceToMove = availableRows.Count > 0
                        ? row - availableRows.Dequeue()
                        : 0;

                    if (distanceToMove > 0)
                    {
                        GamePieceMoveData gamePieceMoveData =
                            new GamePieceMoveData(gamePiece, Vector2Int.down, distanceToMove);
                        moveDataEntries.Add(gamePieceMoveData);
                        availableRows.Enqueue(row);
                    }
                }
                else if (!_tileService.IsObstacleAt(column, row))
                {
                    availableRows.Enqueue(row);
                }
            }

            return moveDataEntries;
        }

        public bool TryGetGamePieceAt(Vector2Int position, out GamePiece gamePiece)
        {
            gamePiece = null;

            if (BoardHelper.IsOutOfBounds(position, new Vector2Int(_gamePieces.GetWidth(), _gamePieces.GetHeight())))
            {
                return false;
            }

            gamePiece = _gamePieces[position.x, position.y];

            return gamePiece != null;
        }

        public bool HasMatches(IEnumerable<GamePiece> gamePieces, out HashSet<GamePiece> allMatches)
        {
            allMatches = new HashSet<GamePiece>();

            foreach (GamePiece gamePiece in gamePieces)
            {
                if (GamePieceMatchHelper.TryFindMatches(gamePiece.Position, Settings.MinMatchesCount, _gamePieces,
                        _boardSize, out HashSet<GamePiece> matches))
                {
                    allMatches.UnionWith(matches);
                }
            }

            return allMatches.Count > 0;
        }

        public bool HasCollectiblesToBreak(out HashSet<GamePiece> collectiblesToBreak)
        {
            collectiblesToBreak = new HashSet<GamePiece>();

            for (int column = 0; column < _gamePieces.GetWidth(); column++)
            {
                GamePiece bottomGamePiece = _gamePieces[column, 0];
                if (bottomGamePiece != null
                    && bottomGamePiece is CollectibleGamePiece piece
                    && piece.CollectibleType == CollectibleType.ClearedAtBottomRow)
                {
                    collectiblesToBreak.Add(bottomGamePiece);
                }
            }

            return collectiblesToBreak.Count > 0;
        }

        public void ClearGamePieceAt(Vector2Int position, bool breakOnMatch = false)
        {
            GamePiece gamePiece = _gamePieces[position.x, position.y];

            if (gamePiece == null)
            {
                return;
            }

            if (gamePiece is CollectibleGamePiece)
            {
                _soundMonoService.PlaySound(SoundType.BreakCollectible);

                _collectibleGamePieces--;
            }

            _gamePieces[position.x, position.y] = null;

            gamePiece.OnStartMoving -= OnGamePieceStartMoving;
            gamePiece.OnPositionChanged -= OnGamePiecePositionChanged;

            gamePiece.Destroy();

            if (breakOnMatch)
            {
                ParticleEffectType particleEffectType = gamePiece.Bombed
                    ? ParticleEffectType.Bomb
                    : ParticleEffectType.Clear;
                _particleService.PlayParticleEffectAt(position, particleEffectType, 0, _staticDataService.GetColorForGamePiece(gamePiece.Color));
            }
        }

        public HashSet<GamePiece> GetGamePiecesByColor(GamePieceColor color)
        {
            HashSet<GamePiece> result = new HashSet<GamePiece>();

            foreach (GamePiece gamePiece in _gamePieces)
            {
                if (gamePiece != null && gamePiece.Color == color)
                {
                    result.Add(gamePiece);
                }
            }

            return result;
        }

        public HashSet<GamePiece> GetAllGamePieces()
        {
            HashSet<GamePiece> result = new HashSet<GamePiece>();

            foreach (GamePiece gamePiece in _gamePieces)
            {
                if (gamePiece != null)
                {
                    result.Add(gamePiece);
                }
            }

            return result;
        }

        public bool TryGetLowestRowWithEmptyGamePiece(out int lowestEmptyRow)
        {
            lowestEmptyRow = default;

            for (int row = 0; row < _boardSize.y; row++)
            {
                for (int column = 0; column < _boardSize.x; column++)
                {
                    if (_gamePieces[column, row] == null)
                    {
                        lowestEmptyRow = row;
                        return true;
                    }
                }
            }

            return false;
        }

        public bool HasAvailableMoves(out Tuple<GamePiece, GamePiece> gamePiecesForMatch)
        {
            return GamePieceMatchHelper.HasAvailableMoves(_gamePieces, _boardSize, out gamePiecesForMatch);
        }

        public void SpawnBombGamePiece(int x, int y, BombType bombType, GamePieceColor color)
        {
            GamePiece gamePiece = _gameFactory.CreateBombGamePiece(x, y, bombType, color);
            RegisterGamePiece(gamePiece, x, y);
        }

        public HashSet<GamePiece> GetBombedRowGamePieces(int row)
        {
            HashSet<GamePiece> rowGamePieces = new HashSet<GamePiece>();

            for (int column = 0; column < _gamePieces.GetWidth(); column++)
            {
                if (TryGetGamePieceAt(new Vector2Int(column, row), out GamePiece gamePiece)
                    && CanBombGamePiece(gamePiece))
                {
                    rowGamePieces.Add(gamePiece);
                }
            }

            return rowGamePieces;
        }

        public HashSet<GamePiece> GetBombedColumnGamePieces(int column)
        {
            HashSet<GamePiece> rowGamePieces = new HashSet<GamePiece>();

            for (int row = 0; row < _gamePieces.GetHeight(); row++)
            {
                if (TryGetGamePieceAt(new Vector2Int(column, row), out GamePiece gamePiece)
                    && CanBombGamePiece(gamePiece))
                {
                    rowGamePieces.Add(gamePiece);
                }
            }

            return rowGamePieces;
        }

        public HashSet<GamePiece> GetBombedAdjacentGamePieces(Vector2Int position, int range)
        {
            HashSet<GamePiece> rowGamePieces = new HashSet<GamePiece>();

            int startColumn = position.x - range;
            int endColumn = position.x + range;
            int startRow = position.y - range;
            int endRow = position.y + range;

            for (int column = startColumn; column <= endColumn; column++)
            {
                for (int row = startRow; row <= endRow; row++)
                {
                    if (TryGetGamePieceAt(new Vector2Int(column, row), out GamePiece gamePiece)
                        && CanBombGamePiece(gamePiece))
                    {
                        rowGamePieces.Add(gamePiece);
                    }
                }
            }

            return rowGamePieces;
        }

        private bool TrySpawnCollectibleGamePiece(int x, int y, out GamePiece spawnedGamePiece, int offset = 0)
        {
            spawnedGamePiece = null;

            if (y == _gamePieces.GetHeight() - 1
                && _collectibleGamePieces < Settings.MaxCollectibles
                && _randomService.Next(100) <= Settings.PercentChanceToSpawnCollectible)
            {
                spawnedGamePiece = SpawnRandomCollectibleGamePiece(x, y, offset);
                return true;
            }

            return false;
        }

        private GamePiece SpawnBasicGamePieceWithRandomColor(int x, int y, int offsetY = 0)
        {
            GamePiece gamePiece = _gameFactory.CreateNormalGamePieceWithRandomColor(_levelName, x, y, offsetY);
            RegisterGamePiece(gamePiece, x, y);
            return gamePiece;
        }

        private void SpawnCustomGamePiece(int x, int y, GamePieceType gamePieceType, GamePieceColor gamePieceColor)
        {
            GamePiece gamePiece = _gameFactory.CreateGamePiece(gamePieceType, gamePieceColor, x, y);
            RegisterGamePiece(gamePiece, x, y);
        }

        private GamePiece SpawnRandomCollectibleGamePiece(int x, int y, int offset = 0)
        {
            GamePiece gamePiece = _gameFactory.CreateRandomCollectibleGamePiece(x, y, offset);
            RegisterGamePiece(gamePiece, x, y);
            return gamePiece;
        }

        private void RegisterGamePiece(GamePiece gamePiece, int x, int y)
        {
            if (gamePiece is CollectibleGamePiece)
            {
                _collectibleGamePieces++;
                Assert.IsTrue(_collectibleGamePieces <= Settings.MaxCollectibles);
            }

            gamePiece.OnStartMoving += OnGamePieceStartMoving;
            gamePiece.OnPositionChanged += OnGamePiecePositionChanged;

            _gamePieces[x, y] = gamePiece;
        }

        private void OnGamePieceStartMoving(GamePiece gamePiece)
        {
            if (BoardHelper.IsOutOfBounds(gamePiece.Position, _boardSize))
            {
                return;
            }

            _gamePieces[gamePiece.Position.x, gamePiece.Position.y] = null;
        }

        private void OnGamePiecePositionChanged(GamePiece gamePiece)
        {
            _gamePieces[gamePiece.Position.x, gamePiece.Position.y] = gamePiece;
        }

        private bool CanBombGamePiece(GamePiece gamePiece)
        {
            return gamePiece is not CollectibleGamePiece collectibleGamePiece
                   || collectibleGamePiece.CollectibleType == CollectibleType.ClearedByBomb;
        }
    }
}