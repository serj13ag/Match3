using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MessageWindow : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Text _messageText;
        [SerializeField] private Text _buttonText;

        public void ShowMessage(Sprite icon = null, string message = "", string buttonMessage = "")
        {
            if (icon != null)
            {
                _image.sprite = icon;
            }

            if (string.IsNullOrEmpty(message))
            {
                _messageText.text = message;
            }

            if (string.IsNullOrEmpty(buttonMessage))
            {
                _buttonText.text = buttonMessage;
            }
        }
    }
}