using System.Collections.Generic;
using Data;
using Entities;
using Enums;
using Helpers;
using UnityEngine;
using Random = System.Random;

public class Board : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private GamePiece _gamePiecePrefab;

    [SerializeField] private GamePieceColor[] _gamePieceColors;

    private GameDataRepository _gameDataRepository;
    private Random _random;

    private Tile[,] _tiles;
    private GamePiece[,] _gamePieces;

    private Tile _clickedTile;
    private Tile _targetTile;
    private Stack<GamePiece> _movedPieces;
    private bool _revertingPieces;

    private Vector2Int BoardSize => new Vector2Int(_width, _height);

    public int Width => _width;
    public int Height => _height;

    public void Init(GameDataRepository gameDataRepository, Random random)
    {
        _random = random;
        _gameDataRepository = gameDataRepository;

        _movedPieces = new Stack<GamePiece>();
    }

    public void SetupTiles()
    {
        _tiles = new Tile[_width, _height];

        for (var i = 0; i < _width; i++)
        {
            for (var j = 0; j < _height; j++)
            {
                Tile tile = Instantiate(_tilePrefab, new Vector3(i, j, 0), Quaternion.identity);
                tile.Init(i, j, transform);

                tile.OnClicked += OnTileClicked;
                tile.OnMouseEntered += OnTileMouseEntered;
                tile.OnMouseReleased += OnTileMouseReleased;

                _tiles[i, j] = tile;
            }
        }
    }

    public void FillBoard()
    {
        _gamePieces = new GamePiece[_width, _height];

        for (var i = 0; i < _width; i++)
        {
            for (var j = 0; j < _height; j++)
            {
                GamePiece gamePiece = CreateRandomGamePieceAt(i, j);

                while (GamePieceMatchHelper.HasMatchAtFillBoard(new Vector2Int(i, j), _gamePieces, BoardSize))
                {
                    ClearGamePieceAt(gamePiece.Position);
                    gamePiece = CreateRandomGamePieceAt(i, j);
                }
            }
        }
    }

    private GamePiece CreateRandomGamePieceAt(int x, int y)
    {
        GamePiece gamePiece = Instantiate(_gamePiecePrefab, Vector3.zero, Quaternion.identity);
        gamePiece.Init(GetRandomGamePieceColor(), x, y, _gameDataRepository, transform);

        gamePiece.OnPositionChanged += OnGamePiecePositionChanged;

        _gamePieces[x, y] = gamePiece;

        return gamePiece;
    }

    private void OnGamePiecePositionChanged(GamePiece gamePiece)
    {
        _gamePieces[gamePiece.Position.x, gamePiece.Position.y] = gamePiece;

        HandleMovedPieces(gamePiece);
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
        if (_clickedTile != null
            && _targetTile != null
            && HasGamePieceAt(_clickedTile.Position)
            && HasGamePieceAt(_targetTile.Position))
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

        clickedGamePiece.Move(targetTile.Position);
        targetGamePiece.Move(clickedTile.Position);
    }

    private void HandleMovedPieces(GamePiece gamePiece)
    {
        _movedPieces.Push(gamePiece);

        if (_movedPieces.Count == 2)
        {
            if (_revertingPieces)
            {
                _movedPieces.Clear();
                _revertingPieces = false;
                return;
            }

            var movedGamePieces = new GamePiece[]
            {
                _movedPieces.Pop(),
                _movedPieces.Pop(),
            };

            if (HasMatches(movedGamePieces, out HashSet<GamePiece> allMatches))
            {
                ClearGamePieces(allMatches);
            }
            else
            {
                RevertMovedGamePieces(movedGamePieces);
            }
        }
    }

    private void RevertMovedGamePieces(GamePiece[] movedGamePieces)
    {
        _revertingPieces = true;
        movedGamePieces[0].Move(movedGamePieces[1].Position);
        movedGamePieces[1].Move(movedGamePieces[0].Position);
    }

    private void ClearGamePieces(IEnumerable<GamePiece> gamePieces)
    {
        foreach (GamePiece gamePiece in gamePieces)
        {
            ClearGamePieceAt(gamePiece.Position);
        }
    }

    private void ClearGamePieceAt(Vector2Int position)
    {
        GamePiece gamePiece = _gamePieces[position.x, position.y];
        _gamePieces[position.x, position.y] = null;
        Destroy(gamePiece.gameObject);
    }

    private GamePieceColor GetRandomGamePieceColor()
    {
        int randomColorIndex = _random.Next(_gameDataRepository.Colors.Count - 1);
        return _gamePieceColors[randomColorIndex];
    }

    private bool HasGamePieceAt(Vector2Int position)
    {
        return _gamePieces[position.x, position.y] != null;
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
}