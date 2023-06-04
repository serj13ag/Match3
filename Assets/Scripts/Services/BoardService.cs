using System;
using System.Collections.Generic;
using System.Linq;
using Commands;
using Data;
using DTO;
using Entities;
using Enums;
using Helpers;
using Interfaces;
using Services.Mono;
using StaticData.StartingData;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Services
{
    public class BoardService : IUpdatable
    {
        private readonly ParticleService _particleService;
        private readonly IGameFactory _gameFactory;
        private readonly RandomService _randomService;
        private readonly ScoreService _scoreService;
        private readonly SoundMonoService _soundMonoService;
        private readonly StaticDataService _staticDataService;
        private readonly GameRoundService _gameRoundService;

        private readonly int _width;
        private readonly int _height;
        private readonly string _levelName;

        private ITile[,] _tiles;
        private GamePiece[,] _gamePieces;

        private ITile _clickedTile;
        private ITile _targetTile;
        private readonly Stack<GamePiece> _movedPieces;
        private int _collapsedGamePieces;

        private readonly CommandBlock _commandBlock;
        private Direction _playerSwitchGamePiecesDirection;
        private int _collectibleGamePieces;

        private int _completedBreakIterationsAfterSwitchedGamePieces;

        public Vector2Int BoardSize => new Vector2Int(_width, _height);

        public event Action OnGamePiecesSwitched;

        public BoardService(string levelName, RandomService randomService, StaticDataService staticDataService,
            SoundMonoService soundMonoService, UpdateMonoService updateMonoService,
            PersistentProgressService persistentProgressService, IGameFactory gameFactory, ScoreService scoreService,
            GameRoundService gameRoundService, ParticleService particleService)
        {
            _staticDataService = staticDataService;
            _scoreService = scoreService;
            _particleService = particleService;
            _gameFactory = gameFactory;
            _randomService = randomService;
            _soundMonoService = soundMonoService;
            _gameRoundService = gameRoundService;

            _movedPieces = new Stack<GamePiece>();

            _commandBlock = new CommandBlock();

            _levelName = levelName;
            _width = staticDataService.Settings.BoardWidth;
            _height = staticDataService.Settings.BoardHeight;

            updateMonoService.Register(this);

            Setup(persistentProgressService.Progress);
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_gameRoundService.RoundIsActive)
            {
                return;
            }
            
            _commandBlock.Update(deltaTime);
        }

        private void Setup(PlayerProgress progress)
        {
            LevelBoardData levelBoardData = progress.BoardData.LevelBoardData;
            if (SceneManager.GetActiveScene().name == levelBoardData.LevelName)
            {
                //_savedTiles = levelBoardData.Tiles; TODO add load
            }

            SetupTiles();
            SetupGamePieces();
        }

        private void SetupTiles()
        {
            _tiles = new ITile[_width, _height];

            foreach (StartingTileStaticData startingTile in _staticDataService.Levels[_levelName].StartingTiles.StartingTiles)
            {
                SpawnTile(startingTile.Type, startingTile.X, startingTile.Y);
            }

            for (var i = 0; i < _width; i++)
            {
                for (var j = 0; j < _height; j++)
                {
                    if (!TryGetTileAt(i, j, out _))
                    {
                        SpawnTile(TileType.Normal, i, j);
                    }
                }
            }
        }

        private void SetupGamePieces()
        {
            _gamePieces = new GamePiece[_width, _height];

            foreach (StartingGamePieceStaticData startingGamePieceEntry in _staticDataService.Levels[_levelName].StartingGamePieces
                         .StartingGamePieces)
            {
                SpawnCustomGamePiece(startingGamePieceEntry.X, startingGamePieceEntry.Y,
                    startingGamePieceEntry.Type, startingGamePieceEntry.Color);
            }

            FillBoardWithRandomGamePieces();
        }

        private void SpawnTile(TileType tileType, int x, int y)
        {
            ITile tile = _gameFactory.CreateTile(tileType, x, y);
            RegisterTile(x, y, tile);
        }

        private void RegisterTile(int x, int y, ITile tile)
        {
            tile.OnClicked += OnTileClicked;
            tile.OnMouseEntered += OnTileMouseEntered;
            tile.OnMouseReleased += OnTileMouseReleased;

            _tiles[x, y] = tile;
        }

        private void FillBoardWithRandomGamePieces()
        {
            for (var i = 0; i < _width; i++)
            {
                for (var j = 0; j < _height; j++)
                {
                    if (_gamePieces[i, j] != null || _tiles[i, j].IsObstacle)
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
        }

        private bool TrySpawnCollectibleGamePiece(int x, int y)
        {
            if (y == _height - 1
                && _collectibleGamePieces < Constants.MaxCollectibles
                && _randomService.Next(100) <= Constants.PercentChanceToSpawnCollectible)
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

        private void SpawnBombGamePiece(int x, int y, BombType bombType, GamePieceColor color)
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
                Assert.IsTrue(_collectibleGamePieces <= Constants.MaxCollectibles);
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

            if (gamePiece.IsLastMoveMadeByPlayer)
            {
                HandlePieceMovedByPlayer(gamePiece);
            }
            else
            {
                if (_collapsedGamePieces > 0)
                {
                    HandlePieceCollapsed(gamePiece);
                }
            }
        }

        private void OnTileClicked(ITile tile)
        {
            _clickedTile ??= tile;
        }

        private void OnTileMouseEntered(ITile tile)
        {
            if (_clickedTile != null && TileHelper.IsNeighbours(_clickedTile, tile))
            {
                _targetTile = tile;
            }
        }

        private void OnTileMouseReleased()
        {
            if (!_commandBlock.IsActive
                && _clickedTile != null
                && _targetTile != null
                && TryGetGamePieceAt(_clickedTile.Position, out _)
                && TryGetGamePieceAt(_targetTile.Position, out _))
            {
                SwitchGamePieces(_clickedTile, _targetTile);
            }

            _clickedTile = null;
            _targetTile = null;
        }

        private void SwitchGamePieces(ITile clickedTile, ITile targetTile)
        {
            GamePiece clickedGamePiece = _gamePieces[clickedTile.Position.x, clickedTile.Position.y];
            GamePiece targetGamePiece = _gamePieces[targetTile.Position.x, targetTile.Position.y];

            _playerSwitchGamePiecesDirection = clickedGamePiece.Position.x != targetGamePiece.Position.x
                ? Direction.Horizontal
                : Direction.Vertical;

            _completedBreakIterationsAfterSwitchedGamePieces = 0;

            clickedGamePiece.Move(targetTile.Position, true);
            targetGamePiece.Move(clickedTile.Position, true);
        }

        private void HandlePieceMovedByPlayer(GamePiece gamePiece)
        {
            _movedPieces.Push(gamePiece);

            if (_movedPieces.Count == 2)
            {
                GamePiece[] movedGamePieces =
                {
                    _movedPieces.Pop(),
                    _movedPieces.Pop(),
                };

                if (PlayerMovedColorBomb(movedGamePieces[1], movedGamePieces[0], out var gamePiecesToClear))
                {
                    OnGamePiecesSwitched?.Invoke(); // TODO one invocation

                    ClearAndCollapseAndRefill(gamePiecesToClear);
                }
                else if (HasMatches(movedGamePieces, out HashSet<GamePiece> allMatches))
                {
                    OnGamePiecesSwitched?.Invoke(); // TODO one invocation

                    CreateBombAndClearAndCollapseAndRefill(movedGamePieces, allMatches);
                }
                else
                {
                    RevertMovedGamePieces(movedGamePieces);
                }
            }
        }

        private void HandlePieceCollapsed(GamePiece gamePiece)
        {
            _movedPieces.Push(gamePiece);

            if (_movedPieces.Count == _collapsedGamePieces)
            {
                _collapsedGamePieces = 0;

                var movedGamePieces = new List<GamePiece>();
                while (_movedPieces.Count > 0)
                {
                    movedGamePieces.Add(_movedPieces.Pop());
                }

                if (HasMatches(movedGamePieces, out HashSet<GamePiece> allMatches))
                {
                    ClearAndCollapseAndRefill(allMatches);
                }
                else if (HasCollectiblesToBreak(out HashSet<GamePiece> collectiblesToBreak))
                {
                    ClearAndCollapseAndRefill(collectiblesToBreak);
                }
                else
                {
                    AddFillBoardCommand();
                }
            }
        }

        private void RevertMovedGamePieces(GamePiece[] movedGamePieces)
        {
            movedGamePieces[0].Move(movedGamePieces[1].Position);
            movedGamePieces[1].Move(movedGamePieces[0].Position);
        }

        private void CreateBombAndClearAndCollapseAndRefill(GamePiece[] movedGamePieces, HashSet<GamePiece> allMatches)
        {
            HashSet<GamePiece> gamePiecesToBreak = GetGamePiecesToBreak(allMatches);

            var clickedGamePiece = movedGamePieces[1];
            if (allMatches.Count >= Constants.MatchesToSpawnBomb && allMatches.Contains(clickedGamePiece))
            {
                var bombType = GamePieceMatchHelper.GetBombTypeOnMatch(allMatches, _playerSwitchGamePiecesDirection);
                ClearGamePieceAt(clickedGamePiece.Position);
                SpawnBombGamePiece(clickedGamePiece.Position.x, clickedGamePiece.Position.y, bombType,
                    clickedGamePiece.Color);

                gamePiecesToBreak.Remove(clickedGamePiece);
            }

            AddBreakGamePiecesCommand(gamePiecesToBreak);
            AddCollapseColumnsCommand(gamePiecesToBreak);
        }

        private void ClearAndCollapseAndRefill(HashSet<GamePiece> allMatches)
        {
            HashSet<GamePiece> gamePiecesToBreak = GetGamePiecesToBreak(allMatches);

            AddBreakGamePiecesCommand(gamePiecesToBreak);
            AddCollapseColumnsCommand(gamePiecesToBreak);
        }

        private HashSet<GamePiece> GetGamePiecesToBreak(HashSet<GamePiece> matchedGamePieces)
        {
            var gamePiecesToBreak = new HashSet<GamePiece>();

            foreach (GamePiece matchedGamePiece in matchedGamePieces)
            {
                if (TryGetBombedGamePieces(matchedGamePiece, out HashSet<GamePiece> bombedGamePieces))
                {
                    _soundMonoService.PlaySound(SoundType.BombGamePieces);
                    gamePiecesToBreak.UnionWith(bombedGamePieces);
                }
                else
                {
                    gamePiecesToBreak.Add(matchedGamePiece);
                }
            }

            return gamePiecesToBreak;
        }

        private bool TryGetBombedGamePieces(GamePiece matchedGamePiece, out HashSet<GamePiece> bombedGamePieces,
            HashSet<GamePiece> gamePiecesToExclude = null)
        {
            bombedGamePieces = new HashSet<GamePiece>();

            if (matchedGamePiece is not BombGamePiece bombGamePiece)
            {
                return false;
            }

            bombedGamePieces = GetBombedGamePieces(bombGamePiece.BombType, matchedGamePiece);

            if (bombedGamePieces == null)
            {
                return false;
            }

            // TODO: fix later
            foreach (GamePiece bombedGamePiece in bombedGamePieces)
            {
                bombedGamePiece.Bombed = true;
            }

            if (gamePiecesToExclude != null)
            {
                bombedGamePieces.ExceptWith(gamePiecesToExclude);
            }

            foreach (var bombedGamePiece in bombedGamePieces.ToArray())
            {
                if (TryGetBombedGamePieces(bombedGamePiece, out var pieces, bombedGamePieces))
                {
                    bombedGamePieces.UnionWith(pieces);
                }
            }

            return true;
        }

        private HashSet<GamePiece> GetBombedGamePieces(BombType bombType, GamePiece matchedGamePiece)
        {
            return bombType switch
            {
                BombType.Column => GetBombedColumnGamePieces(matchedGamePiece.Position.x),
                BombType.Row => GetBombedRowGamePieces(matchedGamePiece.Position.y),
                BombType.Adjacent => GetBombedAdjacentGamePieces(matchedGamePiece.Position,
                    Constants.BombAdjacentGamePiecesRange),
                BombType.Color => null,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private void AddBreakGamePiecesCommand(HashSet<GamePiece> gamePiecesToBreak)
        {
            Command breakCommand =
                new Command(() => BreakGamePieces(gamePiecesToBreak), Constants.Commands.ClearGamePiecesTimeout);
            _commandBlock.AddCommand(breakCommand);
        }

        private void AddCollapseColumnsCommand(HashSet<GamePiece> gamePiecesToBreak)
        {
            HashSet<int> columnIndexes = BoardHelper.GetColumnIndexes(gamePiecesToBreak);
            Command collapseCommand =
                new Command(() => CollapseColumns(columnIndexes), Constants.Commands.CollapseColumnsTimeout);
            _commandBlock.AddCommand(collapseCommand);
        }

        private void AddFillBoardCommand()
        {
            Command fillBoardCommand = new Command(FillBoardWithRandomGamePieces, Constants.Commands.FillBoardTimeout);
            _commandBlock.AddCommand(fillBoardCommand);
        }

        private void BreakGamePieces(HashSet<GamePiece> gamePieces)
        {
            foreach (GamePiece gamePiece in gamePieces)
            {
                _scoreService.AddScore(gamePiece.Score, gamePieces.Count,
                    _completedBreakIterationsAfterSwitchedGamePieces);

                ClearGamePieceAt(gamePiece.Position, true);
                ProcessTileMatchAt(gamePiece.Position);
            }

            _soundMonoService.PlaySound(SoundType.BreakGamePieces);
            if (_completedBreakIterationsAfterSwitchedGamePieces > 1)
            {
                _soundMonoService.PlaySound(SoundType.Bonus);
            }

            _completedBreakIterationsAfterSwitchedGamePieces++;
        }

        private void ClearGamePieceAt(Vector2Int position, bool breakOnMatch = false)
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
                var particleEffectType = gamePiece.Bombed
                    ? ParticleEffectType.Bomb
                    : ParticleEffectType.Clear;
                _particleService.PlayParticleEffectAt(position, particleEffectType);
            }
        }

        private void ProcessTileMatchAt(Vector2Int position)
        {
            if (TryGetTileAt(position.x, position.y, out ITile tile))
            {
                tile.ProcessMatch();
            }
        }

        private void CollapseColumns(HashSet<int> columnIndexes)
        {
            var gamePiecesToMoveData = new List<GamePieceMoveData>();

            foreach (int columnIndex in columnIndexes)
            {
                gamePiecesToMoveData.AddRange(GetGamePiecesToCollapseMoveData(columnIndex));
            }

            foreach (GamePieceMoveData gamePieceMoveData in gamePiecesToMoveData)
            {
                gamePieceMoveData.GamePiece.Move(gamePieceMoveData.Direction, gamePieceMoveData.Distance);
            }

            _collapsedGamePieces = gamePiecesToMoveData.Count;

            if (gamePiecesToMoveData.Count == 0)
            {
                AddFillBoardCommand();
            }
        }

        private IEnumerable<GamePieceMoveData> GetGamePiecesToCollapseMoveData(int column)
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
                else if (!_tiles[column, row].IsObstacle)
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

        private bool TryGetTileAt(int x, int y, out ITile tile)
        {
            tile = _tiles[x, y];

            return tile != null;
        }

        private bool HasMatches(IEnumerable<GamePiece> gamePieces, out HashSet<GamePiece> allMatches)
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

        private bool HasCollectiblesToBreak(out HashSet<GamePiece> collectiblesToBreak)
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

        private bool PlayerMovedColorBomb(GamePiece clickedGamePiece, GamePiece targetGamePiece,
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

        private HashSet<GamePiece> GetBombedRowGamePieces(int row)
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

        private HashSet<GamePiece> GetBombedColumnGamePieces(int column)
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

        private HashSet<GamePiece> GetBombedAdjacentGamePieces(Vector2Int position, int range)
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
    }
}