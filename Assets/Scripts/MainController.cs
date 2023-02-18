using Data;
using UnityEngine;
using Random = System.Random;

public class MainController : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Board _board;

    [SerializeField] private ColorData _colorData;

    private Random _random;
    private GameDataRepository _gameDataRepository;

    private void Start()
    {
        _random = new Random();
        _gameDataRepository = new GameDataRepository(_colorData);

        _board.SetupTiles();
        _cameraController.SetupCamera(_board.Width, _board.Height);
        _board.FillBoardWithRandomGamePieces(_gameDataRepository, _random);
    }
}