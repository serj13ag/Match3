using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class LoadingCurtainController : MonoBehaviour
    {
        [SerializeField] private MaskableGraphic _fadeMaskableGraphic;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void FadeOnInstantly()
        {
            _fadeMaskableGraphic.CrossFadeAlpha(Constants.ScreenFader.SolidAlpha, 0f, true);
        }

        public void FadeOnWithDelay()
        {
            StartCoroutine(FadeRoutine(Constants.ScreenFader.SolidAlpha));
        }

        public void FadeOffWithDelay()
        {
            StartCoroutine(FadeRoutine(Constants.ScreenFader.ClearAlpha));
        }

        private IEnumerator FadeRoutine(float alpha)
        {
            yield return new WaitForSeconds(Constants.ScreenFader.Delay);

            _fadeMaskableGraphic.CrossFadeAlpha(alpha, Constants.ScreenFader.TimeToFade, true);
        }
    }
}