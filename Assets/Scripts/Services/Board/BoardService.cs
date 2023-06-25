using System;
using System.Collections.Generic;
using Constants;
using Data;
using DTO;
using Entities;
using Enums;
using EventArguments;
using Helpers;
using Interfaces;
using Services.Board.States;
using Services.Mono;
using Services.Mono.Sound;
using StaticData;
using StaticData.StartingData;
using UnityEngine;
using UnityEngine.Assertions;

namespace Services.Board
{
    public class BoardService : IUpdatable, IBoardService
    {
        private readonly IParticleService _particleService;
        private readonly ITileService _tileService;
        private readonly IGameFactory _gameFactory;
        private readonly IRandomService _randomService;
        private readonly IScoreService _scoreService;
        private readonly ISoundMonoService _soundMonoService;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IGameRoundService _gameRoundService;

        private readonly int _width;
        private readonly int _height;
        private readonly string _levelName;

        private readonly GamePiece[,] _gamePieces;

        private int _collapsedGamePieces;

        private Direction _playerSwitchGamePiecesDirection;
        private int _collectibleGamePieces;

        private IBoardState _boardState;

        public Vector2Int BoardSize => new Vector2Int(_width, _height);

        public event Action OnGamePiecesSwitched;

        public BoardService(string levelName, IRandomService randomService, IStaticDataService staticDataService,
            ISoundMonoService soundMonoService, IUpdateMonoService updateMonoService,
            IPersistentProgressService persistentProgressService, ISaveLoadService saveLoadService,
            IGameFactory gameFactory, IScoreService scoreService, IGameRoundService gameRoundService,
            IParticleService particleService, ITileService tileService)
        {
            _scoreService = scoreService;
            _particleService = particleService;
            _tileService = tileService;
            _gameFactory = gameFactory;
            _randomService = randomService;
            _soundMonoService = soundMonoService;
            _persistentProgressService = persistentProgressService;
            _saveLoadService = saveLoadService;
            _gameRoundService = gameRoundService;

            _levelName = levelName;
            _width = staticDataService.Settings.BoardWidth;
            _height = staticDataService.Settings.BoardHeight;

            _gamePieces = new GamePiece[_width, _height];

            LevelBoardData levelBoardData = persistentProgressService.Progress.BoardData.LevelBoardData;
            if (_levelName == levelBoardData.LevelName && levelBoardData.GamePieces != null)
            {
                SetupFromLoadData(levelBoardData);
            }
            else
            {
                SetupNewBoard(staticDataService.GetDataForLevel(_levelName));
            }

            updateMonoService.Register(this);

            tileService.OnMoveRequested += OnMoveRequested;

            ChangeStateToWaiting();
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_gameRoundService.RoundIsActive)
            {
                return;
            }

            _boardState.Update(deltaTime);
        }

        public void ChangeStateToCollapse(HashSet<int> columnIndexesToCollapse)
        {
            ChangeState(new CollapseColumnsTimeoutBoardState(this, columnIndexesToCollapse));
        }

        public void ChangeStateToFill()
        {
            ChangeState(new FillTimeoutBoardState(this));
        }

        public void ChangeStateToWaiting()
        {
            ChangeState(new WaitingBoardState());
        }

        public void ChangeStateToBreak(HashSet<GamePiece> gamePiecesToBreak)
        {
            ChangeState(new BreakGamePiecesTimeoutBoardState(this, _scoreService, _tileService, _soundMonoService, gamePiecesToBreak));
        }

        private void ChangeStateToHandlePlayerSwitchGamePieces(GamePiece clickedGamePiece, GamePiece targetGamePiece)
        {
            ChangeState(new HandlePlayerSwitchGamePiecesBoardState(this, _soundMonoService, clickedGamePiece, targetGamePiece));
        }

        private void ChangeState(IBoardState newBoardState)
        {
            _boardState = newBoardState;
        }

        private void SetupFromLoadData(LevelBoardData levelBoardData)
        {
            foreach (GamePieceSaveData gamePiece in levelBoardData.GamePieces)
            {
                SpawnCustomGamePiece(gamePiece.Position.x, gamePiece.Position.y, gamePiece.Type, gamePiece.Color);
            }
        }

        private void SetupNewBoard(LevelStaticData levelStaticData)
        {
            foreach (StartingGamePieceStaticData startingGamePieceEntry in levelStaticData.StartingGamePieces
                         .StartingGamePieces)
            {
                SpawnCustomGamePiece(startingGamePieceEntry.X, startingGamePieceEntry.Y, startingGamePieceEntry.Type,
                    startingGamePieceEntry.Color);
            }

            FillBoardWithRandomGamePieces();
        }

        public void FillBoardWithRandomGamePieces()
        {
            for (var i = 0; i < _width; i++)
            {
                for (var j = 0; j < _height; j++)
                {
                    if (_gamePieces[i, j] != null || _tileService.IsObstacleAt(i, j))
                    {
                        continue;
                    }

                    if (TrySpawnCollectibleGamePiece(i, j))
                    {
                        continue;
                    }

                    GamePiece gamePiece = SpawnBasicGamePieceWithRandomColor(i, j);

                    while (GamePieceMatchHelper.HasMatchAtFillBoard(new Vector2Int(i, j), _gamePieces, BoardSize))
                    {
                        ClearGamePieceAt(gamePiece.Position);
                        gamePiece = SpawnBasicGamePieceWithRandomColor(i, j);
                    }
                }
            }

            UpdateProgressAndSave();
        }

        private bool TrySpawnCollectibleGamePiece(int x, int y)
        {
            if (y == _height - 1
                && _collectibleGamePieces < Settings.MaxCollectibles
                && _randomService.Next(100) <= Settings.PercentChanceToSpawnCollectible)
            {
                SpawnRandomCollectibleGamePiece(x, y);
                return true;
            }

            return false;
        }

        private GamePiece SpawnBasicGamePieceWithRandomColor(int x, int y)
        {
            GamePiece gamePiece = _gameFactory.CreateNormalGamePieceWithRandomColor(_levelName, x, y);
            RegisterGamePiece(gamePiece, x, y);
            return gamePiece;
        }

        private void SpawnCustomGamePiece(int x, int y, GamePieceType gamePieceType, GamePieceColor gamePieceColor)
        {
            GamePiece gamePiece = _gameFactory.CreateGamePiece(gamePieceType, gamePieceColor, x, y);
            RegisterGamePiece(gamePiece, x, y);
        }

        public void SpawnBombGamePiece(int x, int y, BombType bombType, GamePieceColor color)
        {
            GamePiece gamePiece = _gameFactory.CreateBombGamePiece(x, y, bombType, color);
            RegisterGamePiece(gamePiece, x, y);
        }

        private void SpawnRandomCollectibleGamePiece(int x, int y)
        {
            GamePiece gamePiece = _gameFactory.CreateRandomCollectibleGamePiece(x, y);
            RegisterGamePiece(gamePiece, x, y);
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
            _gamePieces[gamePiece.Position.x, gamePiece.Position.y] = null;
        }

        private void OnGamePiecePositionChanged(GamePiece gamePiece)
        {
            _gamePieces[gamePiece.Position.x, gamePiece.Position.y] = gamePiece;
        }

        private void OnMoveRequested(object sender, MoveRequestedEventArgs e)
        {
            if (_boardState is WaitingBoardState
                && TryGetGamePieceAt(e.FromPosition, out GamePiece clickedGamePiece)
                && !clickedGamePiece.IsCollectible()
                && TryGetGamePieceAt(e.ToPosition, out GamePiece targetGamePiece))
            {
                _scoreService.ResetBreakStreakIterations();

                ChangeStateToHandlePlayerSwitchGamePieces(clickedGamePiece, targetGamePiece);
            }
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
                _particleService.PlayParticleEffectAt(position, particleEffectType);
            }
        }

        public IEnumerable<GamePieceMoveData> GetGamePiecesToCollapseMoveData(int column)
        {
            var availableRows = new Queue<int>();
            var moveDataEntries = new List<GamePieceMoveData>();

            for (var row = 0; row < _height; row++)
            {
                var position = new Vector2Int(column, row);
                if (TryGetGamePieceAt(position, out GamePiece gamePiece))
                {
                    int distanceToMove = availableRows.Count > 0
                        ? row - availableRows.Dequeue()
                        : 0;

                    if (distanceToMove > 0)
                    {
                        var gamePieceMoveData = new GamePieceMoveData(gamePiece, Vector2Int.down, distanceToMove);
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

        private bool TryGetGamePieceAt(Vector2Int position, out GamePiece gamePiece)
        {
            gamePiece = null;

            if (BoardHelper.IsOutOfBounds(position, new Vector2Int(_width, _height)))
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
                if (GamePieceMatchHelper.TryFindMatches(gamePiece.Position, 3, _gamePieces,
                        BoardSize, out HashSet<GamePiece> matches))
                {
                    allMatches.UnionWith(matches);
                }
            }

            return allMatches.Count > 0;
        }

        public bool HasCollectiblesToBreak(out HashSet<GamePiece> collectiblesToBreak)
        {
            collectiblesToBreak = new HashSet<GamePiece>();

            for (int column = 0; column < _width; column++)
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

        public bool PlayerMovedColorBomb(GamePiece clickedGamePiece, GamePiece targetGamePiece,
            out HashSet<GamePiece> gamePiecesToClear)
        {
            gamePiecesToClear = new HashSet<GamePiece>();

            if (targetGamePiece.Color == GamePieceColor.Undefined)
            {
                return false;
            }

            if (clickedGamePiece is BombGamePiece { BombType: BombType.Color })
            {
                if (targetGamePiece is BombGamePiece { BombType: BombType.Color })
                {
                    gamePiecesToClear = GetAllGamePieces();
                }
                else
                {
                    gamePiecesToClear = GetGamePiecesByColor(targetGamePiece.Color);
                    gamePiecesToClear.Add(clickedGamePiece);
                }

                return true;
            }

            return false;
        }

        public void InvokeGamePiecesSwitched()
        {
            OnGamePiecesSwitched?.Invoke();
        }

        public HashSet<GamePiece> GetBombedRowGamePieces(int row)
        {
            var rowGamePieces = new HashSet<GamePiece>();

            for (var column = 0; column < _width; column++)
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
            var rowGamePieces = new HashSet<GamePiece>();

            for (var row = 0; row < _height; row++)
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
            var rowGamePieces = new HashSet<GamePiece>();

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

        private bool CanBombGamePiece(GamePiece gamePiece)
        {
            return gamePiece is not CollectibleGamePiece collectibleGamePiece
                   || collectibleGamePiece.CollectibleType == CollectibleType.ClearedByBomb;
        }

        private HashSet<GamePiece> GetGamePiecesByColor(GamePieceColor color)
        {
            HashSet<GamePiece> result = new HashSet<GamePiece>();

            foreach (var gamePiece in _gamePieces)
            {
                if (gamePiece != null && gamePiece.Color == color)
                {
                    result.Add(gamePiece);
                }
            }

            return result;
        }

        private HashSet<GamePiece> GetAllGamePieces()
        {
            HashSet<GamePiece> result = new HashSet<GamePiece>();

            foreach (var gamePiece in _gamePieces)
            {
                if (gamePiece != null)
                {
                    result.Add(gamePiece);
                }
            }

            return result;
        }

        private void UpdateProgressAndSave()
        {
            List<GamePieceSaveData> gamePieceSaveData = new List<GamePieceSaveData>();

            foreach (GamePiece gamePiece in _gamePieces)
            {
                if (gamePiece != null)
                {
                    gamePieceSaveData.Add(new GamePieceSaveData(gamePiece.Type, gamePiece.Position, gamePiece.Color));
                }
            }

            _persistentProgressService.Progress.BoardData.LevelBoardData.GamePieces = gamePieceSaveData;
            _persistentProgressService.Progress.BoardData.LevelBoardData.LevelName = _levelName;

            _tileService.UpdateProgress();
            _saveLoadService.SaveProgress();
        }
    }
}