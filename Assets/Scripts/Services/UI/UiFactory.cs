using Constants;
using UI;
using UI.Windows;
using UnityEngine;

namespace Services.UI
{
    public class UiFactory : IUiFactory
    {
        private readonly IAssetProviderService _assetProviderService;
        private readonly IStaticDataService _staticDataService;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISettingsService _settingsService;

        private Transform _uiRootTransform;

        private MessageInGameWindow _messageWindow;

        public UiFactory(IAssetProviderService assetProviderService, IStaticDataService staticDataService,
            IPersistentProgressService persistentProgressService, ISettingsService settingsService)
        {
            _assetProviderService = assetProviderService;
            _staticDataService = staticDataService;
            _persistentProgressService = persistentProgressService;
            _settingsService = settingsService;
        }

        public void CreateUiRootCanvas()
        {
            GameObject uiRoot = _assetProviderService.Instantiate<GameObject>(AssetPaths.UiRootCanvasPath);
            _uiRootTransform = uiRoot.transform;
        }

        public MainMenu CreateMainMenu()
        {
            return _assetProviderService.Instantiate<MainMenu>(AssetPaths.MainMenuPath, _uiRootTransform);
        }

        public BackgroundBlocker CreateBackgroundBlocker()
        {
            return _assetProviderService.Instantiate<BackgroundBlocker>(AssetPaths.BackgroundBlockerPath, _uiRootTransform);
        }

        public SettingsWindow CreateSettingsWindow()
        {
            SettingsWindow settingsWindow = _assetProviderService.Instantiate<SettingsWindow>(AssetPaths.SettingsWindowPath, _uiRootTransform);
            settingsWindow.Init(_persistentProgressService, _settingsService);
            return settingsWindow;
        }

        public PuzzleLevelsWindow CreatePuzzleLevelsWindow()
        {
            PuzzleLevelsWindow puzzleLevelsWindow = _assetProviderService.Instantiate<PuzzleLevelsWindow>(AssetPaths.PuzzleLevelsWindowPath, _uiRootTransform);
            puzzleLevelsWindow.Init(this, _staticDataService);
            return puzzleLevelsWindow;
        }

        public PuzzleLevelButton CreateLevelButton(Transform parentTransform)
        {
            return _assetProviderService.Instantiate<PuzzleLevelButton>(AssetPaths.PuzzleLevelButtonPath, parentTransform);
        }

        public MessageInGameWindow GetMessageWindow()
        {
            if (_messageWindow == null)
            {
                _messageWindow = _assetProviderService.Instantiate<MessageInGameWindow>(AssetPaths.MessageWindowPath, _uiRootTransform);
            }

            return _messageWindow;
        }

        public void Cleanup()
        {
            _uiRootTransform = null;
            _messageWindow = null;
        }
    }
}