using Services;
using StaticData.Shop;
using TMPro;
using UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BackgroundShopItem : MonoBehaviour
    {
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Image _lockImage;
        [SerializeField] private Image _checkmarkImage;

        [SerializeField] private Button _button;
        [SerializeField] private Image _buttonCoinImage;
        [SerializeField] private TMP_Text _buttonText;

        private IPurchaseService _purchaseService;
        private ICoinService _coinService;
        private ICustomizationService _customizationService;

        private BackgroundShopItemStaticData _backgroundShopItem;
        private ShopWindow _shopWindow;
        private bool _isPurchased;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        public void Init(ShopWindow shopWindow, BackgroundShopItemStaticData backgroundShopItem,
            ICoinService coinService, IPurchaseService purchaseService, ICustomizationService customizationService)
        {
            _customizationService = customizationService;
            _coinService = coinService;
            _purchaseService = purchaseService;
            _shopWindow = shopWindow;
            _backgroundShopItem = backgroundShopItem;

            _backgroundImage.color = backgroundShopItem.Color;

            UpdateView();
        }

        public void UpdateView()
        {
            string itemCode = _backgroundShopItem.Code;

            if (_purchaseService.ItemIsPurchased(itemCode))
            {
                _isPurchased = true;

                bool isCurrentlySelectedBackgroundItem = itemCode == _customizationService.CurrentBackgroundItemCode;

                _lockImage.gameObject.SetActive(false);
                _checkmarkImage.gameObject.SetActive(isCurrentlySelectedBackgroundItem);

                _buttonCoinImage.gameObject.SetActive(false);
                _buttonText.text = string.Empty;
                _buttonText.color = Color.white;
                _button.interactable = !isCurrentlySelectedBackgroundItem;
            }
            else
            {
                _isPurchased = false;

                _lockImage.gameObject.SetActive(true);
                _checkmarkImage.gameObject.SetActive(false);

                _buttonCoinImage.gameObject.SetActive(true);
                
                bool canBuyItem = _coinService.Coins >= _backgroundShopItem.CoinPrice;
                _buttonText.text = _backgroundShopItem.CoinPrice.ToString();
                _buttonText.color = canBuyItem ? Color.white : Color.red;
                _button.interactable = canBuyItem;
            }
        }

        private void OnButtonClicked()
        {
            if (_isPurchased)
            {
                _shopWindow.SelectPurchasedBackgroundItem(_backgroundShopItem.Code);
            }
            else
            {
                _shopWindow.PurchaseBackgroundItem(_backgroundShopItem.Code);
            }
        }
    }
}