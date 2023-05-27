using Enums;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "SettingsData", menuName = "StaticData/Settings")]
    public class SettingsStaticData : ScriptableObject
    {
        public int BoardWidth;
        public int BoardHeight;
        public MoveInterpolationType MoveInterpolationType;
    }
}