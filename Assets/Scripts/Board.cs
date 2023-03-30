using System;
using System.Collections.Generic;
using System.Linq;
using Commands;
using Data;
using DTO;
using Entities;
using Enums;
using Helpers;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    [SerializeField] private StartingTilesData _startingTilesData;
    [SerializeField] private StartingGamePiecesData _startingGamePiecesData;

    private ParticleController _particleController;
    private Factory _factory;

    private Tile[,] _tiles;
    private GamePiece[,] _gamePieces;

    private Tile _clickedTile;
    private Tile _targetTile;
    private Stack<GamePiece> _movedPieces;
    private int _collapsedGamePieces;

    private CommandBlock _commandBlock;

    public Vector2Int BoardSize => new Vector2Int(_width, _height);

    public void Init(ParticleController particleController, Factory factory)
    {
        _particleController = particleController;
        _factory = factory;

        _movedPieces = new Stack<GamePiece>();

        _commandBlock = new CommandBlock();
    }

    private void Update()
    {
        _commandBlock.Update(Time.deltaTime);
    }

    public void SetupTiles()
    {
        _tiles = new Tile[_width, _height];

        foreach (StartingTileEntry startingTile in _startingTilesData.StartingTiles)
        {
            SpawnCustomTile(startingTile.TilePrefab, startingTile.X, startingTile.Y, startingTile.Z);
        }

        for (var i = 0; i < _width; i++)
        {
            for (var j = 0; j < _height; j++)
            {
                if (!TryGetTileAt(i, j, out _))
                {
                    SpawnBasicTile(i, j);
                }
            }
        }
    }

    public void SetupGamePieces()
    {
        _gamePieces = new GamePiece[_width, _height];

        foreach (StartingGamePieceEntry startingGamePieceEntry in _startingGamePiecesData.StartingGamePieces)
        {
            SpawnCustomGamePiece(startingGamePieceEntry.X, startingGamePieceEntry.Y,
                startingGamePieceEntry.GamePiecePrefab, startingGamePieceEntry.GamePieceColor);
        }

        FillBoardWithRandomGamePieces();
    }

    private void SpawnBasicTile(int x, int y)
    {
        Tile tile = _factory.CreateBasicTile(x, y, transform);
        RegisterTile(x, y, tile);
    }

    private void SpawnCustomTile(Tile tilePrefab, int x, int y, int z)
    {
        Tile tile = _factory.CreateCustomTile(tilePrefab, x, y, z, transform);
        RegisterTile(x, y, tile);
    }

    private void RegisterTile(int x, int y, Tile tile)
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
                if (_gamePieces[i, j] != null || _tiles[i, j].TileType == TileType.Obstacle)
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

    private GamePiece SpawnBasicGamePieceWithRandomColor(int x, int y)
    {
        GamePiece gamePiece = _factory.CreateBasicGamePieceWithRandomColor(x, y, transform);
        RegisterGamePiece(gamePiece, x, y);
        return gamePiece;
    }

    private void SpawnCustomGamePiece(int x, int y, GamePiece gamePiecePrefab, GamePieceColor gamePieceColor)
    {
        GamePiece gamePiece = _factory.CreateCustomGamePiece(x, y, transform, gamePiecePrefab, gamePieceColor);
        RegisterGamePiece(gamePiece, x, y);
    }

    private void SpawnBombGamePiece(int x, int y, MatchType matchType, GamePieceColor color)
    {
        GamePiece gamePiece = _factory.CreateBombGamePiece(x, y, transform, matchType, color);
        RegisterGamePiece(gamePiece, x, y);
    }

    private void RegisterGamePiece(GamePiece gamePiece, int x, int y)
    {
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

    private void OnTileClicked(Tile tile)
    {
        if (_clickedTile == null)
        {
            _clickedTile = tile;
        }
    }

    private void OnTileMouseEntered(Tile tile)
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

    private void SwitchGamePieces(Tile clickedTile, Tile targetTile)
    {
        GamePiece clickedGamePiece = _gamePieces[clickedTile.Position.x, clickedTile.Position.y];
        GamePiece targetGamePiece = _gamePieces[targetTile.Position.x, targetTile.Position.y];

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

            if (HasMatches(movedGamePieces, out HashSet<GamePiece> allMatches))
            {
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

        if (allMatches.Count >= Constants.MatchesToSpawnBomb)
        {
            MatchType matchType = GamePieceMatchHelper.GetMatchType(allMatches);
            List<GamePiece> matchedMovedGamePieces = movedGamePieces
                .Where(allMatches.Contains)
                .ToList();

            foreach (GamePiece matchedMovedGamePiece in matchedMovedGamePieces)
            {
                ClearGamePieceAt(matchedMovedGamePiece.Position);
                SpawnBombGamePiece(matchedMovedGamePiece.Position.x, matchedMovedGamePiece.Position.y,
                    matchType, matchedMovedGamePiece.Color);

                gamePiecesToBreak.Remove(matchedMovedGamePiece);
            }
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
            if (matchedGamePiece is BombGamePiece bombGamePiece)
            {
                switch (bombGamePiece.BombType)
                {
                    case BombType.Column:
                        gamePiecesToBreak.UnionWith(GetColumnGamePieces(matchedGamePiece.Position.x));
                        break;
                    case BombType.Row:
                        gamePiecesToBreak.UnionWith(GetRowGamePieces(matchedGamePiece.Position.y));
                        break;
                    case BombType.Adjacent:
                        gamePiecesToBreak.UnionWith(GetAdjacentGamePieces(matchedGamePiece.Position,
                            Constants.BombAdjacentGamePiecesRange));
                        break;
                    case BombType.Color:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                gamePiecesToBreak.Add(matchedGamePiece);
            }
        }

        return gamePiecesToBreak;
    }

    private void AddBreakGamePiecesCommand(HashSet<GamePiece> gamePiecesToBreak)
    {
        var breakCommand = new Command(() => BreakGamePieces(gamePiecesToBreak), Constants.ClearGamePiecesTimeout);
        _commandBlock.AddCommand(breakCommand);
    }

    private void AddCollapseColumnsCommand(HashSet<GamePiece> gamePiecesToBreak)
    {
        HashSet<int> columnIndexes = BoardHelper.GetColumnIndexes(gamePiecesToBreak);
        var collapseCommand = new Command(() => CollapseColumns(columnIndexes), Constants.CollapseColumnsTimeout);
        _commandBlock.AddCommand(collapseCommand);
    }

    private void AddFillBoardCommand()
    {
        var fillBoardCommand = new Command(FillBoardWithRandomGamePieces, Constants.FillBoardTimeout);
        _commandBlock.AddCommand(fillBoardCommand);
    }

    private void BreakGamePieces(IEnumerable<GamePiece> gamePieces)
    {
        foreach (GamePiece gamePiece in gamePieces)
        {
            ClearGamePieceAt(gamePiece.Position, true);
            ProcessTileMatchAt(gamePiece.Position);
        }
    }

    private void ClearGamePieceAt(Vector2Int position, bool breakOnMatch = false)
    {
        GamePiece gamePiece = _gamePieces[position.x, position.y];

        _gamePieces[position.x, position.y] = null;

        gamePiece.OnStartMoving -= OnGamePieceStartMoving;
        gamePiece.OnPositionChanged -= OnGamePiecePositionChanged;

        Destroy(gamePiece.gameObject);

        if (breakOnMatch)
        {
            _particleController.PlayParticleEffectAt(position, ParticleEffectType.Clear);
        }
    }

    private void ProcessTileMatchAt(Vector2Int position)
    {
        if (TryGetTileAt(position.x, position.y, out Tile tile))
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
            else if (_tiles[column, row].TileType != TileType.Obstacle)
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

    private bool TryGetTileAt(int x, int y, out Tile tile)
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

    private IEnumerable<GamePiece> GetRowGamePieces(int row)
    {
        var rowGamePieces = new List<GamePiece>();

        for (var column = 0; column < _width; column++)
        {
            if (TryGetGamePieceAt(new Vector2Int(column, row), out GamePiece gamePiece))
            {
                rowGamePieces.Add(gamePiece);
            }
        }

        return rowGamePieces;
    }

    private IEnumerable<GamePiece> GetColumnGamePieces(int column)
    {
        var rowGamePieces = new List<GamePiece>();

        for (var row = 0; row < _height; row++)
        {
            if (TryGetGamePieceAt(new Vector2Int(column, row), out GamePiece gamePiece))
            {
                rowGamePieces.Add(gamePiece);
            }
        }

        return rowGamePieces;
    }

    private IEnumerable<GamePiece> GetAdjacentGamePieces(Vector2Int position, int range)
    {
        var rowGamePieces = new List<GamePiece>();

        int startColumn = position.x - range;
        int endColumn = position.x + range;
        int startRow = position.y - range;
        int endRow = position.y + range;

        for (int column = startColumn; column <= endColumn; column++)
        {
            for (int row = startRow; row <= endRow; row++)
            {
                if (TryGetGamePieceAt(new Vector2Int(column, row), out GamePiece gamePiece))
                {
                    rowGamePieces.Add(gamePiece);
                }
            }
        }

        return rowGamePieces;
    }
}