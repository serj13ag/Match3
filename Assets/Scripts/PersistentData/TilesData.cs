using PersistentData.Models;
using UnityEngine;

namespace PersistentData
{
    [CreateAssetMenu(fileName = "TilesData", menuName = "Create Tiles")]
    public class TilesData : ScriptableObject
    {
        public TileModel[] Tiles;
    }
}