using Enums;
using Services;
using Services.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Background
{
    public abstract class BaseBackgroundScreen : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Button _menuButton;

        private IWindowService _windowService;

        protected void InitInner(ICameraService cameraService, IWindowService windowService)
        {
            _windowService = windowService;
            _canvas.worldCamera = cameraService.MainCamera;

            _menuButton.onClick.AddListener(GoToMainMenu);
        }

        private void GoToMainMenu()
        {
            _windowService.ShowWindow(WindowType.Settings);
        }
    }
}