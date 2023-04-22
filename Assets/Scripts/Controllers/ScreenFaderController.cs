using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class ScreenFaderController : MonoBehaviour
    {
        private const float SolidAlpha = 1;
        private const float ClearAlpha = 0;

        private const float Delay = 1f;
        private const float TimeToFade = 1f;

        [SerializeField] private MaskableGraphic _fadeMaskableGraphic;

        public void FadeOn()
        {
            StartCoroutine(FadeRoutine(SolidAlpha));
        }

        public void FadeOff()
        {
            StartCoroutine(FadeRoutine(ClearAlpha));
        }

        private IEnumerator FadeRoutine(float alpha)
        {
            yield return new WaitForSeconds(Delay);

            _fadeMaskableGraphic.CrossFadeAlpha(alpha, TimeToFade, true);
        }
    }
}