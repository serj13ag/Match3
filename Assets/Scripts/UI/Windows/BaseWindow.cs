using UnityEngine;

namespace UI.Windows
{
    public abstract class BaseWindow : MonoBehaviour
    {
        [SerializeField] private RectTransformMover _rectTransformMover;

        public void Show()
        {
            _rectTransformMover.MoveIn();
        }

        protected void Hide()
        {
            _rectTransformMover.MoveOut(() => Destroy(gameObject));
        }
    }
}