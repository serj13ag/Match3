using System;
using EventArguments;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PuzzleLevelButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Text _text;

        private string _levelName;

        public event EventHandler<PuzzleLevelButtonClickedEventArgs> OnButtonClicked;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void Init(string levelName)
        {
            _levelName = levelName;

            UpdateText(levelName);
        }

        private void UpdateText(string levelName)
        {
            _text.text = levelName;
        }

        private void OnButtonClick()
        {
            OnButtonClicked?.Invoke(this, new PuzzleLevelButtonClickedEventArgs(_levelName));
        }
    }
}