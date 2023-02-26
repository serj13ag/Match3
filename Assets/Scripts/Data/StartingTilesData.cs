using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "StartingTilesData", menuName = "Create Starting Tiles")]
    public class StartingTilesData : ScriptableObject
    {
        public StartingTileEntry[] StartingTiles;
    }
}