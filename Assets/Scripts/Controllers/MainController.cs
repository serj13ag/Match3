using PersistentData;
using UnityEngine;
using Random = System.Random;

namespace Controllers
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private ParticleController _particleController;
        [SerializeField] private ScreenFaderController _screenFaderController;
        [SerializeField] private ScoreController _scoreController;
        [SerializeField] private Factory _factory;
        [SerializeField] private Board _board;

        [SerializeField] private TilesData _tilesData;
        [SerializeField] private GamePiecesData _gamePiecesData;
        [SerializeField] private ColorData _colorData;
        [SerializeField] private MoveData _moveData;

        [SerializeField] private LevelData _levelData;

        private Random _random;
        private GameDataRepository _gameDataRepository;

        private void Start()
        {
            _screenFaderController.FadeOff();

            _random = new Random();
            _gameDataRepository = new GameDataRepository(_tilesData, _gamePiecesData, _colorData, _moveData, _levelData);
            _factory.Init(_random, _gameDataRepository, _particleController);

            _board.Init(_particleController, _factory, _random, _scoreController, _gameDataRepository);

            _board.SetupTiles();
            _cameraController.SetupCamera(_board.BoardSize);
            _board.SetupGamePieces();
        }
    }
}