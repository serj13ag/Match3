using System;
using UI;
using UnityEngine;

namespace Services.Mono
{
    public class UiMonoService : MonoBehaviour
    {
        [SerializeField] private MessageWindow _messageWindow;

        [SerializeField] private Sprite _winIcon;
        [SerializeField] private Sprite _loseIcon;
        [SerializeField] private Sprite _goalIcon;

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