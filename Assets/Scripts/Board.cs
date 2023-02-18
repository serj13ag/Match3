using Data;
using Entities;
using Enums;
using UnityEngine;
using Random = System.Random;

public class Board : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private GamePiece _gamePiecePrefab;

    [SerializeField] private GamePieceColor[] _gamePieceColors;

    private Tile[,] _tiles;
    private GamePiece[,] _gamePieces;

    public int Width => _width;
    public int Height => _height;

    public void SetupTiles()
    {
        _tiles = new Tile[_width, _height];

        for (var i = 0; i < _width; i++)
        {
            for (var j = 0; j < _height; j++)
            {
                Tile tile = Instantiate(_tilePrefab, new Vector3(i, j, 0), Quaternion.identity);
                tile.Init(i, j, this);

                _tiles[i, j] = tile;
            }
        }
    }

    public void FillBoardWithRandomGamePieces(GameDataRepository gameDataRepository, Random random)
    {
        _gamePieces = new GamePiece[_width, _height];

        for (var i = 0; i < _width; i++)
        {
            for (var j = 0; j < _height; j++)
            {
                GamePiece gamePiece = Instantiate(_gamePiecePrefab, Vector3.zero, Quaternion.identity);
                gamePiece.Init(GetRandomGamePieceColor(random, gameDataRepository));
                gamePiece.MoveToPosition(i, j);

                _gamePieces[i, j] = gamePiece;
            }
        }
    }

    private Color GetRandomGamePieceColor(Random random, GameDataRepository gameDataRepository)
    {
        int randomColorIndex = random.Next(gameDataRepository.Colors.Count - 1);
        GamePieceColor color = _gamePieceColors[randomColorIndex];
        return gameDataRepository.Colors[color];
    }
}