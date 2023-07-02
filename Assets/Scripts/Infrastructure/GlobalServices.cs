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

        public ISoundMonoService SoundMonoService { get; private set; }
        public ILoadingCurtainMonoService LoadingCurtainMonoService { get; private set; }
        public IUpdateMonoService UpdateMonoService { get; private set; }

        public IUiFactory UiFactory { get; private set; }
        public IWindowService WindowService { get; private set; }

        public GlobalServices(SceneLoader sceneLoader)
        {
            SceneLoader = sceneLoader;
        }

        public void InitGlobalServices(GameStateMachine gameStateMachine)
        {
            IRandomService randomService = new RandomService();
            IAssetProviderService assetProviderService = new AssetProviderService();
            IStaticDataService staticDataService = new StaticDataService();
            ISaveLoadService saveLoadService = new SaveLoadService();

            IPersistentProgressService persistentProgressService = new PersistentProgressService(saveLoadService);
            ISettingsService settingsService = new SettingsService(saveLoadService);

            ILoadingCurtainMonoService loadingCurtainMonoService =
                assetProviderService.Instantiate<LoadingCurtainMonoService>(AssetPaths.LoadingCurtainMonoServicePath);

            ISoundMonoService soundMonoService =
                assetProviderService.Instantiate<SoundMonoService>(AssetPaths.SoundMonoServicePath);
            soundMonoService.Init(randomService, settingsService);

            IUpdateMonoService updateMonoService =
                assetProviderService.Instantiate<UpdateMonoService>(AssetPaths.UpdateMonoServicePath);
            updateMonoService.Init();

            IUiFactory uiFactory = new UiFactory(gameStateMachine, assetProviderService, staticDataService,
                persistentProgressService, soundMonoService, settingsService);
            IWindowService windowService = new WindowService(uiFactory, assetProviderService);

            RandomService = randomService;
            AssetProviderService = assetProviderService;
            StaticDataService = staticDataService;
            SaveLoadService = saveLoadService;

            PersistentProgressService = persistentProgressService;

            LoadingCurtainMonoService = loadingCurtainMonoService;
            SoundMonoService = soundMonoService;
            UpdateMonoService = updateMonoService;

            UiFactory = uiFactory;
            WindowService = windowService;
        }
    }
}