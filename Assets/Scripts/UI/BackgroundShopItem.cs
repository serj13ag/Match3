using StaticData.Shop;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BackgroundShopItem : MonoBehaviour
    {
        [SerializeField] private Image _backgroundImage;

        public void Init(BackgroundShopItemStaticData backgroundShopItem)
        {
            _backgroundImage.color = backgroundShopItem.Color;
        }
    }
}