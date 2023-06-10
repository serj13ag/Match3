using System.Collections;
using Constants;
using UnityEngine;
using UnityEngine.UI;

namespace Services.Mono
{
    public class LoadingCurtainMonoService : MonoBehaviour, ILoadingCurtainMonoService
    {
        [SerializeField] private MaskableGraphic _fadeMaskableGraphic;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void FadeOnInstantly()
        {
            _fadeMaskableGraphic.CrossFadeAlpha(Settings.ScreenFader.SolidAlpha, 0f, true);
        }

        public void FadeOnWithDelay()
        {
            StartCoroutine(FadeRoutine(Settings.ScreenFader.SolidAlpha));
        }

        public void FadeOffWithDelay()
        {
            StartCoroutine(FadeRoutine(Settings.ScreenFader.ClearAlpha));
        }

        private IEnumerator FadeRoutine(float alpha)
        {
            yield return new WaitForSeconds(Settings.ScreenFader.Delay);

            _fadeMaskableGraphic.CrossFadeAlpha(alpha, Settings.ScreenFader.TimeToFade, true);
        }
    }
}