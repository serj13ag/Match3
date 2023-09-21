using UnityEngine;

namespace StaticData.Shop
{
    [CreateAssetMenu(fileName = "BackgroundShopItem", menuName = "StaticData/Shop/BackgroundShopItem")]
    public class BackgroundShopItemStaticData : ScriptableObject
    {
        public string Code;
        public Color Color;
        public int CoinPrice;
    }
}