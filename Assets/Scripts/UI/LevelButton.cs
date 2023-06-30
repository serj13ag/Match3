using System;
using EventArguments;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Text _text;

        private string _levelName;

        public event EventHandler<LevelButtonClickedEventArgs> OnButtonClicked;

        public void Init(string levelName)
        {
            _levelName = levelName;

            UpdateText(levelName);

            _button.onClick.AddListener(OnButtonClick);
        }

        private void UpdateText(string levelName)
        {
            _text.text = levelName;
        }

        private void OnButtonClick()
        {
            OnButtonClicked?.Invoke(this, new LevelButtonClickedEventArgs(_levelName));
        }
    }
}