using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsWindow : MonoBehaviour
    {
        [SerializeField] private RectTransformMover _rectTransformMover;

        [SerializeField] private Button _resetButton;
        [SerializeField] private Button _backButton;

        private IPersistentProgressService _persistentProgressService;
        private ISaveLoadService _saveLoadService;

        public void Init(IPersistentProgressService persistentProgressService, ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            _persistentProgressService = persistentProgressService;

            _resetButton.onClick.AddListener(ResetProgressAndSave);
            _backButton.onClick.AddListener(Back);
        }

        public void Show()
        {
            _rectTransformMover.MoveIn();
        }

        private void ResetProgressAndSave()
        {
            _persistentProgressService.ResetProgress();
            _saveLoadService.SaveProgress();
        }

        private void Back()
        {
            _rectTransformMover.MoveOut();
        }
    }
}