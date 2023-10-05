using System;
using System.Collections;
using Infrastructure;
using Services;
using Services.Mono.Sound;
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

        private ISoundMonoService _soundMonoService;
        private IAdsService _adsService;

        private Action _onButtonClickCallback;
        private Coroutine _adCoroutine;

        private void Awake()
        {
            _soundMonoService = ServiceLocator.Instance.Get<ISoundMonoService>();
            _adsService = ServiceLocator.Instance.Get<IAdsService>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void ShowMessage(string message = "", string buttonMessage = "", Action onButtonClickCallback = null,
            bool showAd = false)
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

            if (showAd)
            {
                _adCoroutine = StartCoroutine(ShowAdCoroutine());
            }
        }

        private void OnButtonClick()
        {
            _onButtonClickCallback?.Invoke();

            Hide();

            if (_adCoroutine != null)
            {
                StopCoroutine(_adCoroutine);
            }
        }

        private IEnumerator ShowAdCoroutine()
        {
            yield return new WaitForSeconds(1f);
            _soundMonoService.AdStarted();
            _adsService.ShowFullAd(Unmute);
            _adCoroutine = null;
        }

        private void Unmute()
        {
            _soundMonoService.AdEnded();
        }
    }
}