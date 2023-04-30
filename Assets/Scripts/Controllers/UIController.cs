using System;
using UI;
using UnityEngine;

namespace Controllers
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private MessageWindow _messageWindow;

        [SerializeField] private Sprite _winIcon;
        [SerializeField] private Sprite _loseIcon;
        [SerializeField] private Sprite _goalIcon;

        private ScreenFaderController _screenFaderController;

        public void Init(ScreenFaderController screenFaderController)
        {
            _screenFaderController = screenFaderController;
        }

        public void FadeOn()
        {
            _screenFaderController.FadeOn();
        }

        public void FadeOff()
        {
            _screenFaderController.FadeOff();
        }

        public void ShowStartGameMessageWindow(int scoreGoal, Action onButtonClickCallback)
        {
            _messageWindow.ShowMessage(_goalIcon, $"score goal\n{scoreGoal}", "start", onButtonClickCallback);
        }

        public void ShowGameOverMessageWindow(Action onButtonClickCallback)
        {
            _messageWindow.ShowMessage(_loseIcon, "you lose!", "ok", onButtonClickCallback);
        }
    }
}