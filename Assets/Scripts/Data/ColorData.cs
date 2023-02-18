using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "ColorData", menuName = "Create Color Data")]
    public class ColorData : ScriptableObject
    {
        public List<ColorDataEntry> GamePieceColors;
    }
}