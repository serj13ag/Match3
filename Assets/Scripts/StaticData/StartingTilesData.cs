using StaticData.Models;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "StartingTilesData", menuName = "Create Starting Tiles")]
    public class StartingTilesData : ScriptableObject
    {
        public StartingTileModel[] StartingTiles;
    }
}