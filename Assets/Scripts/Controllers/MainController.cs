using PersistentData;
using UnityEngine;

namespace Controllers
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private ParticleController _particleController;
        [SerializeField] private SoundController _soundController;
        [SerializeField] private ScreenFaderController _screenFaderController;
        [SerializeField] private SceneController _sceneController;
        [SerializeField] private UIController _uiController;
        [SerializeField] private GameStateController _gameStateController;
        [SerializeField] private ScoreController _scoreController;

        [SerializeField] private Factory _factory;
        [SerializeField] private Board _board;

        [SerializeField] private TilesData _tilesData;
        [SerializeField] private GamePiecesData _gamePiecesData;
        [SerializeField] private ColorData _colorData;
        [SerializeField] private LevelData _levelData;
        [SerializeField] private MoveData _moveData;

        private RandomService _randomService;
        private GameDataRepository _gameDataRepository;

        private void Start()
        {
            _sceneController.UpdateSceneNameText();

            _randomService = new RandomService();
            _gameDataRepository =
                new GameDataRepository(_tilesData, _gamePiecesData, _colorData, _moveData, _levelData);

            _soundController.Init(_randomService);
            _factory.Init(_randomService, _gameDataRepository, _particleController);
            _board.Init(_particleController, _factory, _randomService, _scoreController, _gameDataRepository);
            _uiController.Init(_screenFaderController);
            _gameStateController.Init(_uiController, _board, _cameraController, _sceneController, _scoreController);

            _gameStateController.InitializeLevel(3, 100); // TODO move to levelData
        }
    }
}