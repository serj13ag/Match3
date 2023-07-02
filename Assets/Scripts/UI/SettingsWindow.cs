using Services;
using Services.Mono.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsWindow : MonoBehaviour
    {
        [SerializeField] private RectTransformMover _rectTransformMover;

        [SerializeField] private Button _soundButton;
        [SerializeField] private Button _resetButton;
        [SerializeField] private Button _backButton;

        private IPersistentProgressService _persistentProgressService;
        private ISaveLoadService _saveLoadService;
        private ISoundMonoService _soundMonoService;

        private void OnEnable()
        {
            _soundButton.onClick.AddListener(SwitchSoundMode);
            _resetButton.onClick.AddListener(ResetProgressAndSave);
            _backButton.onClick.AddListener(Back);
        }

        private void OnDisable()
        {
            _soundButton.onClick.RemoveListener(SwitchSoundMode);
            _resetButton.onClick.RemoveListener(ResetProgressAndSave);
            _backButton.onClick.RemoveListener(Back);
        }

        public void Init(IPersistentProgressService persistentProgressService, ISaveLoadService saveLoadService,
            ISoundMonoService soundMonoService)
        {
            _soundMonoService = soundMonoService;
            _saveLoadService = saveLoadService;
            _persistentProgressService = persistentProgressService;
        }

        private void SwitchSoundMode()
        {
            _soundMonoService.SwitchSoundMode();
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