using Controllers;

namespace Infrastructure
{
    public class GlobalServices
    {
        private const string SoundControllerPath = "Prefabs/Infrastructure/Global/SoundController";
        private const string LevelLoadingCurtainPath = "Prefabs/Infrastructure/Global/LevelLoadingCurtain";

        public SceneLoader SceneLoader { get; }

        // Global
        public RandomService RandomService { get; private set; }
        public AssetProviderService AssetProviderService { get; private set; }
        public SoundController SoundController { get; private set; }
        public LevelLoadingCurtain LevelLoadingCurtain { get; private set; }
        public GameDataRepository GameDataRepository { get; private set; }

        public GlobalServices(SceneLoader sceneLoader)
        {
            SceneLoader = sceneLoader;
        }

        public void InitGlobalServices(GameData gameData)
        {
            RandomService = new RandomService();
            AssetProviderService = new AssetProviderService();
            GameDataRepository = new GameDataRepository(gameData);

            SoundController = AssetProviderService.Instantiate<SoundController>(SoundControllerPath);
            SoundController.Init(RandomService);

            LevelLoadingCurtain = AssetProviderService.Instantiate<LevelLoadingCurtain>(LevelLoadingCurtainPath);
        }
    }
}