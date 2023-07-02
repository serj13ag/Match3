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
        public GameStateMachine GameStateMachine { get; private set; }
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
            GameStateMachine = gameStateMachine;

            RandomService = new RandomService();
            AssetProviderService = new AssetProviderService();
            StaticDataService = new StaticDataService();
            PersistentProgressService = new PersistentProgressService();
            SaveLoadService = new SaveLoadService(PersistentProgressService);

            UiFactory = new UiFactory(gameStateMachine, AssetProviderService, StaticDataService);
            WindowService = new WindowService(UiFactory, AssetProviderService);

            LoadingCurtainMonoService = AssetProviderService.Instantiate<LoadingCurtainMonoService>(AssetPaths.LoadingCurtainMonoServicePath);

            SoundMonoService = AssetProviderService.Instantiate<SoundMonoService>(AssetPaths.SoundMonoServicePath);
            SoundMonoService.Init(RandomService);

            UpdateMonoService = AssetProviderService.Instantiate<UpdateMonoService>(AssetPaths.UpdateMonoServicePath);
            UpdateMonoService.Init();
        }
    }
}