using Constants;
using UI;
using UI.Windows;
using UnityEngine;

namespace Services.UI
{
    public class UiFactory : IUiFactory
    {
        private readonly IAssetProviderService _assetProviderService;

        private Transform _uiRootTransform;

        private MessageInGameWindow _messageWindow;

        public UiFactory(IAssetProviderService assetProviderService)
        {
            _assetProviderService = assetProviderService;
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

        public BaseWindow CreateShopWindow()
        {
            return _assetProviderService.Instantiate<ShopWindow>(AssetPaths.ShopWindowPath, _uiRootTransform);
        }

        public SettingsWindow CreateSettingsWindow()
        {
            return _assetProviderService.Instantiate<SettingsWindow>(AssetPaths.SettingsWindowPath, _uiRootTransform);
        }

        public PuzzleLevelsWindow CreatePuzzleLevelsWindow()
        {
            return _assetProviderService.Instantiate<PuzzleLevelsWindow>(AssetPaths.PuzzleLevelsWindowPath, _uiRootTransform);
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