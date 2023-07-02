using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MessageWindow : MonoBehaviour
    {
        [SerializeField] private RectTransformMover _rectTransformMover;

        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        [SerializeField] private Text _messageText;
        [SerializeField] private Text _buttonText;

        private Action _onButtonClickCallback;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void ShowMessage(Sprite icon = null, string message = "", string buttonMessage = "",
            Action onButtonClickCallback = null)
        {
            if (icon != null)
            {
                _image.sprite = icon;
            }

            if (!string.IsNullOrEmpty(message))
            {
                _messageText.text = message;
            }

            if (!string.IsNullOrEmpty(buttonMessage))
            {
                _buttonText.text = buttonMessage;
            }

            _onButtonClickCallback = onButtonClickCallback;

            _rectTransformMover.MoveIn();
        }

        private void OnButtonClick()
        {
            _onButtonClickCallback?.Invoke();

            _rectTransformMover.MoveOut();
        }
    }
}