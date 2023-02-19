using Enums;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "MoveData", menuName = "Create Move Data")]
    public class MoveData : ScriptableObject
    {
        public MoveInterpolationType MoveInterpolationType;
    }
}