using System;
using System.Collections;
using Constants;
using UnityEngine;
using UnityEngine.UI;

namespace Services.Mono
{
    public class LoadingCurtainMonoService : MonoBehaviour, ILoadingCurtainMonoService
    {
        [SerializeField] private MaskableGraphic _fadeMaskableGraphic;
        [SerializeField] private GameObject _rotatingCoinsContainer;

        private Action _onFadeEndedCallback;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void FadeOnInstantly()
        {
            _fadeMaskableGraphic.CrossFadeAlpha(Settings.ScreenFader.SolidAlpha, 0f, true);
            _rotatingCoinsContainer.gameObject.SetActive(true);
        }

        public void FadeOffWithDelay(Action onFadeEndedCallback = null)
        {
            _onFadeEndedCallback = onFadeEndedCallback;
            StartCoroutine(FadeRoutine(Settings.ScreenFader.ClearAlpha));
        }

        private IEnumerator FadeRoutine(float alpha)
        {
            yield return new WaitForSeconds(Settings.ScreenFader.Delay);

            _fadeMaskableGraphic.CrossFadeAlpha(alpha, Settings.ScreenFader.TimeToFade, true);
            _rotatingCoinsContainer.gameObject.SetActive(false);

            // wait until cross fade ended
            yield return new WaitForSeconds(Settings.ScreenFader.TimeToFade);
            _onFadeEndedCallback?.Invoke();
            _onFadeEndedCallback = null;
        }
    }
}