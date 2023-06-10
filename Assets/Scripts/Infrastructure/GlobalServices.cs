using Constants;
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
        public PersistentProgressService PersistentProgressService { get; private set; }
        public ISaveLoadService SaveLoadService { get; private set; }

        public IUiFactory UiFactory { get; private set; }
        public IWindowService WindowService { get; private set; }

        public ISoundMonoService SoundMonoService { get; private set; }
        public ILoadingCurtainMonoService LoadingCurtainMonoService { get; private set; }
        public UpdateMonoService UpdateMonoService { get; private set; }

        public GlobalServices(SceneLoader sceneLoader)
        {
            SceneLoader = sceneLoader;
        }

        public void InitGlobalServices()
        {
            RandomService = new RandomService();
            AssetProviderService = new AssetProviderService();
            StaticDataService = new StaticDataService();
            PersistentProgressService = new PersistentProgressService();
            SaveLoadService = new SaveLoadService(PersistentProgressService);

            UiFactory = new UiFactory(AssetProviderService);
            WindowService = new WindowService(UiFactory, AssetProviderService);

            LoadingCurtainMonoService = AssetProviderService.Instantiate<LoadingCurtainMonoService>(AssetPaths.LoadingCurtainMonoServicePath);

            SoundMonoService = AssetProviderService.Instantiate<SoundMonoService>(AssetPaths.SoundMonoServicePath);
            SoundMonoService.Init(RandomService);

            UpdateMonoService = AssetProviderService.Instantiate<UpdateMonoService>(AssetPaths.UpdateMonoServicePath);
            UpdateMonoService.Init();
        }
    }
}