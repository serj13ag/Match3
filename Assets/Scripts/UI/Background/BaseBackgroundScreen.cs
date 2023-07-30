using Infrastructure.StateMachine;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Background
{
    public abstract class BaseBackgroundScreen : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Button _menuButton;

        private GameStateMachine _gameStateMachine;

        protected void InitInner(GameStateMachine gameStateMachine, ICameraService cameraService)
        {
            _gameStateMachine = gameStateMachine;
            _canvas.worldCamera = cameraService.MainCamera;

            _menuButton.onClick.AddListener(GoToMainMenu);
        }

        private void GoToMainMenu()
        {
            _gameStateMachine.Enter<MainMenuState>();
        }
    }
}