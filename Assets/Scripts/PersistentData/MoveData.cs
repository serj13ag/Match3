using Enums;
using UnityEngine;

namespace PersistentData
{
    [CreateAssetMenu(fileName = "MoveData", menuName = "Create Move Data")]
    public class MoveData : ScriptableObject
    {
        public MoveInterpolationType MoveInterpolationType;
    }
}