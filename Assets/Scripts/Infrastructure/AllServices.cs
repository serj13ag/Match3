﻿using Controllers;

namespace Infrastructure
{
    public class AllServices
    {
        private const string SoundControllerPath = "Prefabs/Infrastructure/Global/SoundController";
        private const string ParticleControllerPath = "Prefabs/Infrastructure/Global/ParticleController";
        private const string LevelLoadingCurtainPath = "Prefabs/Infrastructure/Global/LevelLoadingCurtain";

        private static AllServices _instance;

        public static AllServices Instance => _instance ??= new AllServices();

        // Global
        public RandomService RandomService { get; private set; }
        public AssetProviderService AssetProviderService { get; private set; }
        public CameraService CameraService { get; private set; } // Global?
        public SoundController SoundController { get; private set; }
        public ParticleController ParticleController { get; private set; } // Global?
        public LevelLoadingCurtain LevelLoadingCurtain { get; private set; }
        public GameDataRepository GameDataRepository { get; private set; }
        public IFactory Factory { get; private set; }

        public void InitGlobalServices(GameData gameData)
        {
            RandomService = new RandomService();
            AssetProviderService = new AssetProviderService();
            CameraService = new CameraService();

            SoundController = AssetProviderService.Instantiate<SoundController>(SoundControllerPath);
            SoundController.Init(RandomService);

            ParticleController = AssetProviderService.Instantiate<ParticleController>(ParticleControllerPath); // Global?
            LevelLoadingCurtain = AssetProviderService.Instantiate<LevelLoadingCurtain>(LevelLoadingCurtainPath);

            GameDataRepository = new GameDataRepository(gameData);

            Factory = new Factory(RandomService, GameDataRepository, ParticleController);
        }
    }
}