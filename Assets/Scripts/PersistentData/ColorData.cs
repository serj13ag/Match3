using System.Collections.Generic;
using UnityEngine;

namespace PersistentData
{
    [CreateAssetMenu(fileName = "ColorData", menuName = "Create Color Data")]
    public class ColorData : ScriptableObject
    {
        public List<ColorDataEntry> GamePieceColors;
    }
}