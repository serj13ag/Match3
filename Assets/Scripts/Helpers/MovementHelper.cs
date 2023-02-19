using System;
using Enums;
using UnityEngine;

namespace Helpers
{
    public static class MovementHelper
    {
        public static float ApplyInterpolation(float t, MoveInterpolationType moveInterpolationType)
        {
            return moveInterpolationType switch
            {
                MoveInterpolationType.Linear => t,
                MoveInterpolationType.SmoothStep => Mathf.SmoothStep(0f, 1f, t),
                MoveInterpolationType.SmootherStep => t * t * t * (t * (t * 6 - 15) + 10),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}