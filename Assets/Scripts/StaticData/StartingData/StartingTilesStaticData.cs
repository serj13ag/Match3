using UnityEngine;

namespace StaticData.StartingData
{
    [CreateAssetMenu(fileName = "StartingTilesData", menuName = "StaticData/StartingTiles")]
    public class StartingTilesStaticData : ScriptableObject
    {
        public StartingTileStaticData[] StartingTiles;
    }
}