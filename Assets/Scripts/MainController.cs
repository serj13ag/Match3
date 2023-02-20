using Data;
using UnityEngine;
using Random = System.Random;

public class MainController : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Board _board;

    [SerializeField] private ColorData _colorData;
    [SerializeField] private MoveData _moveData;

    private Random _random;
    private GameDataRepository _gameDataRepository;

    private void Start()
    {
        _random = new Random();
        _gameDataRepository = new GameDataRepository(_colorData, _moveData);

        _board.Init(_gameDataRepository, _random);
        
        _board.SetupTiles();
        _cameraController.SetupCamera(_board.Width, _board.Height);
        _board.FillBoard();
    }
}