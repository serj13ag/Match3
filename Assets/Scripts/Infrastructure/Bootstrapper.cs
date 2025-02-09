﻿using Constants;
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
            bool isYaGamesEnvironment = false;

            ServiceLocator serviceLocator = new ServiceLocator();
            InitAndRegisterGlobalServices(serviceLocator, isYaGamesEnvironment);

            _gameStateMachine = new GameStateMachine(serviceLocator, isYaGamesEnvironment);

            if (isYaGamesEnvironment)
            {
                _gameStateMachine.Enter<LoadYaSaveDataState>();
            }
            else
            {
                _gameStateMachine.Enter<LoadLocalSaveDataState>();
            }

            serviceLocator.Register(_gameStateMachine);

            DontDestroyOnLoad(this);
        }

        private void InitAndRegisterGlobalServices(ServiceLocator serviceLocator, bool isYaGamesEnvironment)
        {
            ISceneLoader sceneLoader = new SceneLoader(this);
            IRandomService randomService = new RandomService();
            IAssetProviderService assetProviderService = new AssetProviderService();
            IStaticDataService staticDataService = new StaticDataService();

            ISaveService saveService;
            IAdsService adsService;
            IYaGamesMonoService yaGamesMonoService;
            if (isYaGamesEnvironment)
            {
                yaGamesMonoService = assetProviderService.Instantiate<YaGamesMonoService>(AssetPaths.YaGamesMonoServicePath);
                saveService = new YaGamesSaveService(yaGamesMonoService);
                adsService = new YaGamesAdsService(yaGamesMonoService);
            }
            else
            {
                yaGamesMonoService = new NullYaGamesMonoService();
                saveService = new LocalSaveService();
                adsService = new EmptyAdsService();
            }

            IPersistentDataService persistentDataService = new PersistentDataService(saveService);
            ICustomizationService customizationService = new CustomizationService(staticDataService, persistentDataService);
            ISettingsService settingsService = new SettingsService(persistentDataService);
            ICoinService coinService = new CoinService(persistentDataService);
            IPurchaseService purchaseService = new PurchaseService(staticDataService, persistentDataService, coinService);
            ILocalizationService localizationService = new LocalizationService(staticDataService, settingsService);

            ILoadingCurtainMonoService loadingCurtainMonoService = assetProviderService.Instantiate<LoadingCurtainMonoService>(AssetPaths.LoadingCurtainMonoServicePath);

            ISoundMonoService soundMonoService = assetProviderService.Instantiate<SoundMonoService>(AssetPaths.SoundMonoServicePath);
            soundMonoService.Init(randomService, settingsService);

            IUpdateMonoService updateMonoService = assetProviderService.Instantiate<UpdateMonoService>(AssetPaths.UpdateMonoServicePath);
            updateMonoService.Init();

            IUiFactory uiFactory = new UiFactory(assetProviderService);
            IWindowService windowService = new WindowService(uiFactory, localizationService);

            serviceLocator.Register(sceneLoader);
            serviceLocator.Register(randomService);
            serviceLocator.Register(assetProviderService);
            serviceLocator.Register(staticDataService);
            serviceLocator.Register(saveService);
            serviceLocator.Register(adsService);
            serviceLocator.Register(persistentDataService);
            serviceLocator.Register(customizationService);
            serviceLocator.Register(purchaseService);
            serviceLocator.Register(settingsService);
            serviceLocator.Register(localizationService);
            serviceLocator.Register(coinService);

            serviceLocator.Register(yaGamesMonoService);
            serviceLocator.Register(loadingCurtainMonoService);
            serviceLocator.Register(soundMonoService);
            serviceLocator.Register(updateMonoService);
            serviceLocator.Register(uiFactory);
            serviceLocator.Register(windowService);
        }
    }
}