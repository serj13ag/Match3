using Constants;
using UI;
using UnityEngine;

namespace Services.UI
{
    public class UiFactory
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

        public MessageWindow GetMessageWindow()
        {
            if (_messageWindow != null)
            {
                return _messageWindow;
            }

            _messageWindow = _assetProviderService.Instantiate<MessageWindow>(AssetPaths.MessageWindowPath, _uiRootTransform);
            return _messageWindow;
        }
    }
}