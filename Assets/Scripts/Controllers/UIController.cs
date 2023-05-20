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

        private LevelLoadingCurtain _levelLoadingCurtain;

        public void Init(LevelLoadingCurtain levelLoadingCurtain)
        {
            _levelLoadingCurtain = levelLoadingCurtain;
        }

        public void FadeOn()
        {
            _levelLoadingCurtain.FadeOnWithDelay();
        }

        public void FadeOff()
        {
            _levelLoadingCurtain.FadeOffWithDelay();
        }

        public void ShowStartGameMessageWindow(int scoreGoal, Action onButtonClickCallback)
        {
            _messageWindow.ShowMessage(_goalIcon, $"score goal\n{scoreGoal}", "start", onButtonClickCallback);
        }

        public void ShowGameOverMessageWindow(Action onButtonClickCallback)
        {
            _messageWindow.ShowMessage(_loseIcon, "you lose!", "ok", onButtonClickCallback);
        }

        public void ShowGameWinMessageWindow(Action onButtonClickCallback)
        {
            _messageWindow.ShowMessage(_winIcon, "you win!", "ok", onButtonClickCallback);
        }
    }
}