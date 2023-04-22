using PersistentData.Models;
using UnityEngine;

namespace PersistentData
{
    [CreateAssetMenu(fileName = "ColorData", menuName = "Create Color Data")]
    public class ColorData : ScriptableObject
    {
        public ColorDataModel[] GamePieceColors;
    }
}