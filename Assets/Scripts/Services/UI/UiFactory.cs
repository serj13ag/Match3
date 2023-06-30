using Constants;
using UI;
using UnityEngine;

namespace Services.UI
{
    public class UiFactory : IUiFactory
    {
        private readonly IAssetProviderService _assetProviderService;

        private Transform _uiRootTransform;

        private MessageWindow _messageWindow;

        public UiFactory(IAssetProviderService assetProviderService)
        {
            _assetProviderService = assetProviderService;
        }

        public void CreateUiRootCanvas()
        {
            GameObject uiRoot = _assetProviderService.Instantiate<GameObject>(AssetPaths.UiRootCanvasPath);
            _uiRootTransform = uiRoot.transform;
        }

        public MainMenu GetMainMenu()
        {
            return _assetProviderService.Instantiate<MainMenu>(AssetPaths.MainMenuPath, _uiRootTransform);
        }

        public LevelsWindow GetLevelsWindow()
        {
            return _assetProviderService.Instantiate<LevelsWindow>(AssetPaths.LevelsWindowPath, _uiRootTransform);
        }

        public LevelButton GetLevelButton(Transform parentTransform)
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