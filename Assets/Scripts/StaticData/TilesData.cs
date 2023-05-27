using StaticData.Models;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "TilesData", menuName = "Create Tiles")]
    public class TilesData : ScriptableObject
    {
        public TileModel[] Tiles;
    }
}