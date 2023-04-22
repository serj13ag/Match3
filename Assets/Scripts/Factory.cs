using System;
using Controllers;
using Entities;
using Enums;
using UnityEngine;
using Random = System.Random;

public class Factory : MonoBehaviour
{
    [SerializeField] private GamePiece _gamePiecePrefab;
    [SerializeField] private GamePieceColor[] _gamePieceColors;

    [SerializeField] private BombGamePiece _adjacentBombPrefab;
    [SerializeField] private BombGamePiece _columnBombPrefab;
    [SerializeField] private BombGamePiece _rowBombPrefab;
    [SerializeField] private BombGamePiece _colorBombPrefab;

    [SerializeField] private CollectibleGamePiece[] _collectibleGamePieces;

    [SerializeField] private Tile _basicTilePrefab;

    private Random _random;
    private GameDataRepository _gameDataRepository;
    private ParticleController _particleController;

    public void Init(Random random, GameDataRepository gameDataRepository, ParticleController particleController)
    {
        _random = random;
        _gameDataRepository = gameDataRepository;
        _particleController = particleController;
    }

    public Tile CreateBasicTile(int x, int y, Transform parentTransform)
    {
        Tile tile = Instantiate(_basicTilePrefab, new Vector3(x, y, 0), Quaternion.identity);
        tile.Init(x, y, parentTransform, _particleController);
        return tile;
    }

    public Tile CreateCustomTile(Tile tilePrefab, int x, int y, int z, Transform parentTransform)
    {
        Tile tile = Instantiate(tilePrefab, new Vector3(x, y, z), Quaternion.identity);
        tile.Init(x, y, parentTransform, _particleController);
        return tile;
    }

    public GamePiece CreateBasicGamePieceWithRandomColor(int x, int y, Transform parentTransform)
    {
        GamePiece gamePiece = Instantiate(_gamePiecePrefab, Vector3.zero, Quaternion.identity);
        gamePiece.Init(GetRandomGamePieceColor(), x, y, _gameDataRepository, parentTransform);
        return gamePiece;
    }

    public GamePiece CreateCustomGamePiece(int x, int y, Transform parentTransform,
        GamePiece gamePiecePrefab, GamePieceColor color)
    {
        GamePiece gamePiece = Instantiate(gamePiecePrefab, Vector3.zero, Quaternion.identity);
        gamePiece.Init(color, x, y, _gameDataRepository, parentTransform);
        return gamePiece;
    }

    public GamePiece CreateBombGamePiece(int x, int y, Transform parentTransform, BombType bombType,
        GamePieceColor color)
    {
        GamePiece gamePiece = Instantiate(GetBombPrefabOnMatch(bombType), Vector3.zero, Quaternion.identity);

        GamePieceColor gamePieceColor = bombType == BombType.Color
            ? GamePieceColor.Undefined
            : color;
        gamePiece.Init(gamePieceColor, x, y, _gameDataRepository, parentTransform);
        return gamePiece;
    }

    public GamePiece CreateRandomCollectibleGamePiece(int x, int y, Transform parentTransform)
    {
        GamePiece gamePiece = Instantiate(GetRandomCollectibleGamePiecePrefab(), Vector3.zero, Quaternion.identity);
        gamePiece.Init(GamePieceColor.Undefined, x, y, _gameDataRepository, parentTransform);
        return gamePiece;
    }

    private GamePiece GetBombPrefabOnMatch(BombType bombType)
    {
        return bombType switch
        {
            BombType.Row => _rowBombPrefab,
            BombType.Column => _columnBombPrefab,
            BombType.Adjacent => _adjacentBombPrefab,
            BombType.Color => _colorBombPrefab,
            _ => throw new ArgumentOutOfRangeException(nameof(bombType), bombType, null)
        };
    }

    private GamePieceColor GetRandomGamePieceColor()
    {
        int randomColorIndex = _random.Next(_gameDataRepository.Colors.Count - 1);
        return _gamePieceColors[randomColorIndex];
    }

    private CollectibleGamePiece GetRandomCollectibleGamePiecePrefab()
    {
        int randomIndex = _random.Next(_collectibleGamePieces.Length);
        return _collectibleGamePieces[randomIndex];
    }
}