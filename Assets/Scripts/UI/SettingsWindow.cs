using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsWindow : MonoBehaviour
    {
        [SerializeField] private RectTransformMover _rectTransformMover;

        [SerializeField] private Button _backButton;

        public void Init()
        {
            _backButton.onClick.AddListener(Back);
        }

        public void Show()
        {
            _rectTransformMover.MoveIn();
        }

        private void Back()
        {
            _rectTransformMover.MoveOut();
        }
    }
}