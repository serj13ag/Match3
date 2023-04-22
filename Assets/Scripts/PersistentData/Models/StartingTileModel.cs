using System;
using Enums;

namespace PersistentData.Models
{
    [Serializable]
    public class StartingTileModel : StartingObjectModel
    {
        public TileType TileType;
    }
}