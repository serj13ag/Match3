using Controllers;

namespace Infrastructure
{
    public class AllServices
    {
        private static AllServices _instance;

        private SoundController _soundController;
        private ScreenFaderController _screenFaderController;
        private SceneController _sceneController;
        private ParticleController _particleController;
        private RandomService _randomService;
        private GameDataRepository _gameDataRepository;
        private CameraService _cameraService;

        public static AllServices Instance => _instance ??= new AllServices();

        public IFactory Factory { get; private set; }

        public SceneController SceneController => _sceneController;
        public RandomService RandomService => _randomService;
        public GameDataRepository GameDataRepository => _gameDataRepository;
        public SoundController SoundController => _soundController;
        public CameraService CameraService => _cameraService;
        public ParticleController ParticleController => _particleController;
        public ScreenFaderController ScreenFaderController => _screenFaderController;

        public void Register(ParticleController particleController, SoundController soundController,
            ScreenFaderController screenFaderController, SceneController sceneController)
        {
            _sceneController = sceneController;
            _screenFaderController = screenFaderController;
            _soundController = soundController;
            _particleController = particleController;
        }

        public void Init(GameData gameData)
        {
            _randomService = new RandomService();
            _gameDataRepository = new GameDataRepository(gameData);

            Factory = new Factory(_randomService, _gameDataRepository, _particleController);

            _cameraService = new CameraService();

            _soundController.Init(_randomService);
        }
    }
}