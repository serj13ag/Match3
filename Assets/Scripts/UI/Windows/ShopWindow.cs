using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using Services;
using StaticData.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class ShopWindow : BaseWindow
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private TMP_Text _coinsCountText;

        [SerializeField] private BackgroundShopItem _backgroundShopItemPrefab;
        [SerializeField] private Transform _backgroundShopItemsContainer;

        private IStaticDataService _staticDataService;
        private ICoinService _coinService;
        private IPurchaseService _purchaseService;

        private List<BackgroundShopItem> _prefabs;

        private void OnEnable()
        {
            _backButton.onClick.AddListener(Back);
        }

        private void OnDisable()
        {
            _backButton.onClick.RemoveListener(Back);
        }

        private void Awake()
        {
            _staticDataService = ServiceLocator.Instance.Get<IStaticDataService>();
            _coinService = ServiceLocator.Instance.Get<ICoinService>();
            _purchaseService = ServiceLocator.Instance.Get<IPurchaseService>();

            _prefabs = new List<BackgroundShopItem>();

            UpdateCoinsCount();

            CreateBackgroundShopItems();
        }

        public void PurchaseBackgroundItem(string itemCode)
        {
            _purchaseService.PurchaseItemAndSave(itemCode);

            foreach (BackgroundShopItem backgroundShopItem in _prefabs)
            {
                backgroundShopItem.UpdateView();
            }
        }

        public void SelectPurchasedBackgroundItem(string itemCode)
        {
            // TODO add
        }

        private void CreateBackgroundShopItems()
        {
            foreach (BackgroundShopItemStaticData backgroundShopItem in _staticDataService.BackgroundShopItems.OrderBy(x => x.CoinPrice))
            {
                BackgroundShopItem prefab = Instantiate(_backgroundShopItemPrefab, _backgroundShopItemsContainer);
                prefab.Init(this, backgroundShopItem, _coinService, _purchaseService);
                _prefabs.Add(prefab);
            }
        }

        private void UpdateCoinsCount()
        {
            _coinsCountText.text = _coinService.Coins.ToString();
        }

        private void Back()
        {
            Hide();
        }
    }
}