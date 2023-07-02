using Constants;
using Infrastructure.StateMachine;
using Services;
using Services.Mono;
using Services.Mono.Sound;
using Services.UI;

namespace Infrastructure
{
    public class GlobalServices
    {
        public SceneLoader SceneLoader { get; }

        public IRandomService RandomService { get; private set; }
        public IAssetProviderService AssetProviderService { get; private set; }
        public IStaticDataService StaticDataService { get; private set; }
        public IPersistentProgressService PersistentProgressService { get; private set; }
        public ISaveLoadService SaveLoadService { get; private set; }

        public IUiFactory UiFactory { get; private set; }
        public IWindowService WindowService { get; private set; }

        public ISoundMonoService SoundMonoService { get; private set; }
        public ILoadingCurtainMonoService LoadingCurtainMonoService { get; private set; }
        public IUpdateMonoService UpdateMonoService { get; private set; }

        public GlobalServices(SceneLoader sceneLoader)
        {
            SceneLoader = sceneLoader;
        }

        public void InitGlobalServices(GameStateMachine gameStateMachine)
        {
            RandomService randomService = new RandomService();
            AssetProviderService assetProviderService = new AssetProviderService();
            StaticDataService staticDataService = new StaticDataService();
            PersistentProgressService persistentProgressService = new PersistentProgressService();
            SaveLoadService saveLoadService = new SaveLoadService(persistentProgressService);

            UiFactory uiFactory = new UiFactory(gameStateMachine, assetProviderService, staticDataService);
            WindowService windowService = new WindowService(uiFactory, assetProviderService);

            LoadingCurtainMonoService loadingCurtainMonoService =
                assetProviderService.Instantiate<LoadingCurtainMonoService>(AssetPaths.LoadingCurtainMonoServicePath);

            SoundMonoService soundMonoService =
                assetProviderService.Instantiate<SoundMonoService>(AssetPaths.SoundMonoServicePath);
            soundMonoService.Init(randomService);

            UpdateMonoService updateMonoService =
                assetProviderService.Instantiate<UpdateMonoService>(AssetPaths.UpdateMonoServicePath);
            updateMonoService.Init();

            RandomService = randomService;
            AssetProviderService = assetProviderService;
            StaticDataService = staticDataService;
            PersistentProgressService = persistentProgressService;
            SaveLoadService = saveLoadService;
            UiFactory = uiFactory;
            WindowService = windowService;
            LoadingCurtainMonoService = loadingCurtainMonoService;
            SoundMonoService = soundMonoService;
            UpdateMonoService = updateMonoService;
        }
    }
}