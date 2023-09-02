using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ToggleButton : MonoBehaviour
    {
        private const float SoundButtonImageInactiveAlpha = 0.3f;

        [SerializeField] private Button _button;
        [SerializeField] private Image _image;

        private float _initialImageAlpha;

        public Button Button => _button;

        public void Init(bool isEnabled)
        {
            _initialImageAlpha = _image.color.a;

            UpdateSoundButtonColor(isEnabled);
        }

        public void UpdateSoundButtonColor(bool isEnabled)
        {
            Color newColor = _image.color;
            newColor.a = isEnabled ? _initialImageAlpha : SoundButtonImageInactiveAlpha;
            _image.color = newColor;
        }
    }
}