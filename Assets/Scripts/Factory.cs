using System;
using Data;
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

    private Random _random;
    private GameDataRepository _gameDataRepository;

    public void Init(Random random, GameDataRepository gameDataRepository)
    {
        _gameDataRepository = gameDataRepository;
        _random = random;
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

    public GamePiece CreateBombGamePiece(int x, int y, Transform parentTransform, MatchType matchType,
        GamePieceColor color)
    {
        GamePiece gamePiece = Instantiate(GetBombPrefabOnMatch(matchType), Vector3.zero, Quaternion.identity);
        gamePiece.Init(color, x, y, _gameDataRepository, parentTransform);
        return gamePiece;
    }

    private GamePiece GetBombPrefabOnMatch(MatchType matchType)
    {
        return matchType switch
        {
            MatchType.Horizontal => _rowBombPrefab,
            MatchType.Vertical => _columnBombPrefab,
            MatchType.Corner => _adjacentBombPrefab,
            _ => throw new ArgumentOutOfRangeException(nameof(matchType), matchType, null)
        };
    }

    private GamePieceColor GetRandomGamePieceColor()
    {
        int randomColorIndex = _random.Next(_gameDataRepository.Colors.Count - 1);
        return _gamePieceColors[randomColorIndex];
    }
}