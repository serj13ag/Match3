using UI;
using UnityEngine;

namespace Services.UI
{
    public class UiFactory
    {
        private const string UiRootCanvasPath = "Prefabs/UI/UIRootCanvas";
        private const string MessageWindowPath = "Prefabs/UI/MessageWindow";

        private readonly AssetProviderService _assetProviderService;

        private Transform _uiRootTransform;

        private MessageWindow _messageWindow;

        public UiFactory(AssetProviderService assetProviderService)
        {
            _assetProviderService = assetProviderService;
        }

        public void CreateUiRootCanvas()
        {
            GameObject uiRoot = _assetProviderService.Instantiate<GameObject>(UiRootCanvasPath);
            _uiRootTransform = uiRoot.transform;
        }

        public MessageWindow GetMessageWindow()
        {
            if (_messageWindow != null)
            {
                return _messageWindow;
            }

            _messageWindow = _assetProviderService.Instantiate<MessageWindow>(MessageWindowPath, _uiRootTransform);
            return _messageWindow;
        }
    }
}