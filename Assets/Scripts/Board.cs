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

    public int Width => _width;
    public int Height => _height;

    public void Init(GameDataRepository gameDataRepository, Random random)
    {
        _random = random;
        _gameDataRepository = gameDataRepository;
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

    public void FillBoardWithRandomGamePieces()
    {
        _gamePieces = new GamePiece[_width, _height];

        for (var i = 0; i < _width; i++)
        {
            for (var j = 0; j < _height; j++)
            {
                GamePiece gamePiece = Instantiate(_gamePiecePrefab, Vector3.zero, Quaternion.identity);
                gamePiece.Init(GetRandomGamePieceColor(), i, j, _gameDataRepository, transform);

                gamePiece.OnPositionChanged += OnGamePiecePositionChanged;

                _gamePieces[i, j] = gamePiece;
            }
        }
    }

    private void OnGamePiecePositionChanged(GamePiece gamePiece)
    {
        _gamePieces[gamePiece.Position.x, gamePiece.Position.y] = gamePiece;
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
        if (_clickedTile != null && _targetTile != null)
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

    private GamePieceColor GetRandomGamePieceColor()
    {
        int randomColorIndex = _random.Next(_gameDataRepository.Colors.Count - 1);
        return _gamePieceColors[randomColorIndex];
    }

    private bool TryFindMatches(Vector2Int startPosition, Vector2Int searchDirection, out List<GamePiece> matches,
        int minMatchesCount = 3)
    {
        matches = new List<GamePiece>();

        GamePiece startGamePiece = _gamePieces[startPosition.x, startPosition.y];
        matches.Add(startGamePiece);

        Vector2Int nextPosition = Vector2Int.zero;

        int maxSearches = Mathf.Max(_width, _height);

        for (var i = 0; i < maxSearches - 1; i++)
        {
            nextPosition.x = startPosition.x + searchDirection.x * i;
            nextPosition.y = startPosition.y + searchDirection.y * i;

            if (IsOutOfBounds(nextPosition))
            {
                break;
            }

            GamePiece gamePieceToCheck = _gamePieces[nextPosition.x, nextPosition.y];

            if (gamePieceToCheck.Color == startGamePiece.Color)
            {
                matches.Add(gamePieceToCheck);
            }
            else
            {
                break;
            }
        }

        return matches.Count > minMatchesCount;
    }

    private bool IsOutOfBounds(Vector2Int position)
    {
        return position.x < 0 || position.x > _width ||
               position.y < 0 || position.y > _height;
    }
}