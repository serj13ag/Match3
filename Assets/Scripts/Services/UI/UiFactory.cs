using Constants;
using Infrastructure.StateMachine;
using UI;
using UnityEngine;

namespace Services.UI
{
    public class UiFactory : IUiFactory
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IAssetProviderService _assetProviderService;
        private readonly IStaticDataService _staticDataService;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISaveLoadService _saveLoadService;

        private Transform _uiRootTransform;

        private MessageWindow _messageWindow;

        public UiFactory(GameStateMachine gameStateMachine, IAssetProviderService assetProviderService,
            IStaticDataService staticDataService, IPersistentProgressService persistentProgressService,
            ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _assetProviderService = assetProviderService;
            _staticDataService = staticDataService;
            _persistentProgressService = persistentProgressService;
            _saveLoadService = saveLoadService;
        }

        public void CreateUiRootCanvas()
        {
            GameObject uiRoot = _assetProviderService.Instantiate<GameObject>(AssetPaths.UiRootCanvasPath);
            _uiRootTransform = uiRoot.transform;
        }

        public MainMenu CreateMainMenu()
        {
            MainMenu mainMenu = _assetProviderService.Instantiate<MainMenu>(AssetPaths.MainMenuPath, _uiRootTransform);
            mainMenu.Init(this);
            return mainMenu;
        }

        public SettingsWindow CreateSettingsWindow()
        {
            SettingsWindow settingsWindow = _assetProviderService.Instantiate<SettingsWindow>(AssetPaths.SettingsWindowPath, _uiRootTransform);
            settingsWindow.Init(_persistentProgressService, _saveLoadService);
            return settingsWindow;
        }

        public LevelsWindow CreateLevelsWindow()
        {
            LevelsWindow levelsWindow = _assetProviderService.Instantiate<LevelsWindow>(AssetPaths.LevelsWindowPath, _uiRootTransform);
            levelsWindow.Init(_gameStateMachine, this, _staticDataService);
            return levelsWindow;
        }

        public LevelButton CreateLevelButton(Transform parentTransform)
        {
            return _assetProviderService.Instantiate<LevelButton>(AssetPaths.LevelButtonPath, parentTransform);
        }

        public MessageWindow GetMessageWindow()
        {
            if (_messageWindow != null)
            {
                return _messageWindow;
            }

            _messageWindow = _assetProviderService.Instantiate<MessageWindow>(AssetPaths.MessageWindowPath, _uiRootTransform);
            return _messageWindow;
        }

        public void Cleanup()
        {
            _uiRootTransform = null;
            _messageWindow = null;
        }
    }
}