using Constants;
using Infrastructure.StateMachine;
using Services;
using Services.Mono;
using Services.Mono.Sound;
using Services.UI;
using UnityEngine;

namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private IGameStateMachine _gameStateMachine;

        private void Awake()
        {
            ServiceLocator serviceLocator = new ServiceLocator();

            InitAndRegisterGlobalServices(serviceLocator);

            _gameStateMachine = new GameStateMachine(serviceLocator);
            _gameStateMachine.Enter<LoadProgressState>();

            serviceLocator.Register(_gameStateMachine);

            DontDestroyOnLoad(this);
        }

        private void InitAndRegisterGlobalServices(ServiceLocator serviceLocator)
        {
            ISceneLoader sceneLoader = new SceneLoader(this);
            IRandomService randomService = new RandomService();
            IAssetProviderService assetProviderService = new AssetProviderService();
            IStaticDataService staticDataService = new StaticDataService();
            ISaveLoadService saveLoadService = new SaveLoadService();
            IPersistentProgressService persistentProgressService = new PersistentProgressService(saveLoadService);
            ISettingsService settingsService = new SettingsService(saveLoadService);

            ILoadingCurtainMonoService loadingCurtainMonoService = assetProviderService.Instantiate<LoadingCurtainMonoService>(AssetPaths.LoadingCurtainMonoServicePath);

            ISoundMonoService soundMonoService = assetProviderService.Instantiate<SoundMonoService>(AssetPaths.SoundMonoServicePath);
            soundMonoService.Init(randomService, settingsService);

            IUpdateMonoService updateMonoService = assetProviderService.Instantiate<UpdateMonoService>(AssetPaths.UpdateMonoServicePath);
            updateMonoService.Init();

            IUiFactory uiFactory = new UiFactory(assetProviderService, staticDataService, persistentProgressService, settingsService);
            IWindowService windowService = new WindowService(uiFactory, assetProviderService);

            serviceLocator.Register(sceneLoader);
            serviceLocator.Register(randomService);
            serviceLocator.Register(assetProviderService);
            serviceLocator.Register(staticDataService);
            serviceLocator.Register(saveLoadService);
            serviceLocator.Register(persistentProgressService);
            serviceLocator.Register(settingsService);

            serviceLocator.Register(loadingCurtainMonoService);
            serviceLocator.Register(soundMonoService);
            serviceLocator.Register(updateMonoService);
            serviceLocator.Register(uiFactory);
            serviceLocator.Register(windowService);
        }
    }
}