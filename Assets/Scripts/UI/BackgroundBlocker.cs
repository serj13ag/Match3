using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BackgroundBlocker : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public void Activate()
        {
            _image.enabled = true;
        }

        public void Deactivate()
        {
            _image.enabled = false;
        }
    }
}