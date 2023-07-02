using System;
using System.Collections;
using Enums;
using Helpers;
using UnityEngine;

namespace UI
{
    public class RectTransformMover : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;

        [SerializeField] private Vector3 _startPosition;
        [SerializeField] private Vector3 _onScreenPosition;
        [SerializeField] private Vector3 _endPosition;
        [SerializeField] private float _timeToMove;

        private Coroutine _moveRoutine;
        private Action _onMoveEndedCallback;

        public void MoveIn()
        {
            Move(_startPosition, _onScreenPosition, _timeToMove);
        }

        public void MoveOut(Action onMoveEndedCallback = null)
        {
            _onMoveEndedCallback = onMoveEndedCallback;

            Move(_onScreenPosition, _endPosition, _timeToMove);
        }

        private void Move(Vector3 startPosition, Vector3 endPosition, float timeToMove)
        {
            _moveRoutine ??= StartCoroutine(MoveRoutine(startPosition, endPosition, timeToMove));
        }

        private IEnumerator MoveRoutine(Vector3 startPosition, Vector3 endPosition, float timeToMove)
        {
            _rect.anchoredPosition = startPosition;

            var elapsedTime = 0f;

            while (!EndPositionReached(endPosition))
            {
                elapsedTime += Time.deltaTime;
                var t = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f);
                t = MovementHelper.ApplyInterpolation(t, MoveInterpolationType.SmootherStep);

                _rect.anchoredPosition = Vector3.Lerp(startPosition, endPosition, t);

                yield return null;
            }

            _moveRoutine = null;

            _onMoveEndedCallback?.Invoke();
        }

        private bool EndPositionReached(Vector3 endPosition)
        {
            return Vector3.Distance(_rect.anchoredPosition, endPosition) < 0.01f;
        }
    }
}