using System;
using UnityEngine;

namespace UI.Windows
{
    public abstract class BaseWindow : MonoBehaviour
    {
        [SerializeField] private RectTransformMover _rectTransformMover;

        public event EventHandler<EventArgs> OnHided;

        public void Show()
        {
            _rectTransformMover.MoveIn();
        }

        protected void Hide()
        {
            _rectTransformMover.MoveOut(OnMoveEnded);
        }

        private void OnMoveEnded()
        {
            OnHided?.Invoke(this, EventArgs.Empty);

            Destroy(gameObject); // TODO use pooling
        }
    }
}