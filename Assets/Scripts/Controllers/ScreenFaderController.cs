using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class ScreenFaderController : MonoBehaviour
    {
        [SerializeField] private MaskableGraphic _fadeMaskableGraphic;

        public void FadeOn()
        {
            StartCoroutine(FadeRoutine(Constants.ScreenFader.SolidAlpha));
        }

        public void FadeOff()
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