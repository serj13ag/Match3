using Controllers;

namespace Infrastructure
{
    public class GlobalServices
    {
        private const string SoundControllerPath = "Prefabs/Infrastructure/Global/SoundController";
        private const string LevelLoadingCurtainPath = "Prefabs/Infrastructure/Global/LevelLoadingCurtain";
        private const string UpdateControllerPath = "Prefabs/Infrastructure/Global/UpdateController";

        public SceneLoader SceneLoader { get; }

        public RandomService RandomService { get; private set; }
        public AssetProviderService AssetProviderService { get; private set; }
        public GameDataRepository GameDataRepository { get; private set; }

        public SoundController SoundController { get; private set; }
        public LoadingCurtainController LoadingCurtainController { get; private set; }
        public UpdateController UpdateController { get; private set; }

        public GlobalServices(SceneLoader sceneLoader)
        {
            SceneLoader = sceneLoader;
        }

        public void InitGlobalServices(GameData gameData)
        {
            RandomService = new RandomService();
            AssetProviderService = new AssetProviderService();
            GameDataRepository = new GameDataRepository(gameData);

            LoadingCurtainController = AssetProviderService.Instantiate<LoadingCurtainController>(LevelLoadingCurtainPath);

            SoundController = AssetProviderService.Instantiate<SoundController>(SoundControllerPath);
            SoundController.Init(RandomService);

            UpdateController = AssetProviderService.Instantiate<UpdateController>(UpdateControllerPath);
            UpdateController.Init();
        }
    }
}