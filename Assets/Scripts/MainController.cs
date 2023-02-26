using Data;
using UnityEngine;
using Random = System.Random;

public class MainController : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private ParticleController _particleController;
    [SerializeField] private Board _board;

    [SerializeField] private ColorData _colorData;
    [SerializeField] private MoveData _moveData;

    private Random _random;
    private GameDataRepository _gameDataRepository;

    private void Start()
    {
        _random = new Random();
        _gameDataRepository = new GameDataRepository(_colorData, _moveData);

        _board.Init(_gameDataRepository, _random, _particleController);

        _board.SetupTiles();
        _cameraController.SetupCamera(_board.BoardSize);
        _board.SetupGamePieces();
    }
}