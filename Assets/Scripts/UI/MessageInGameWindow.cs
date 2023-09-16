using System;
using TMPro;
using UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MessageInGameWindow : BaseWindow
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private TMP_Text _buttonText;

        private Action _onButtonClickCallback;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void ShowMessage(string message = "", string buttonMessage = "", Action onButtonClickCallback = null)
        {
            if (!string.IsNullOrEmpty(message))
            {
                _messageText.text = message;
            }

            if (!string.IsNullOrEmpty(buttonMessage))
            {
                _buttonText.text = buttonMessage;
            }

            _onButtonClickCallback = onButtonClickCallback;

            Show();
        }

        private void OnButtonClick()
        {
            _onButtonClickCallback?.Invoke();

            Hide();
        }
    }
}