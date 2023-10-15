using System.Collections.Generic;
using System.Linq;
using Constants;
using EventArguments;
using Infrastructure;
using Services;
using Services.Mono.Sound;
using StaticData.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class ShopWindow : BaseWindow
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _showRewardedAdButton;
        [SerializeField] private TMP_Text _coinsCountText;

        [SerializeField] private BackgroundShopItem _backgroundShopItemPrefab;
        [SerializeField] private Transform _backgroundShopItemsContainer;

        private IStaticDataService _staticDataService;
        private ICoinService _coinService;
        private IPurchaseService _purchaseService;
        private ICustomizationService _customizationService;
        private IAdsService _adsService;
        private ISoundMonoService _soundMonoService;

        private List<BackgroundShopItem> _prefabs;
        private float _rewardedAdButtonTimeout;

        private void OnEnable()
        {
            _backButton.onClick.AddListener(Back);
            _showRewardedAdButton.onClick.AddListener(ShowRewardedAd);

            _coinService.OnCoinsChanged += OnCoinsChanged;
        }

        private void OnDisable()
        {
            _backButton.onClick.RemoveListener(Back);
            _showRewardedAdButton.onClick.RemoveListener(ShowRewardedAd);

            _coinService.OnCoinsChanged -= OnCoinsChanged;
        }

        private void Awake()
        {
            _staticDataService = ServiceLocator.Instance.Get<IStaticDataService>();
            _coinService = ServiceLocator.Instance.Get<ICoinService>();
            _purchaseService = ServiceLocator.Instance.Get<IPurchaseService>();
            _customizationService = ServiceLocator.Instance.Get<ICustomizationService>();
            _adsService = ServiceLocator.Instance.Get<IAdsService>();
            _soundMonoService = ServiceLocator.Instance.Get<ISoundMonoService>();

            _prefabs = new List<BackgroundShopItem>();

            UpdateCoinsCount();

            CreateBackgroundShopItems();

            _rewardedAdButtonTimeout = 0f;
        }

        private void Update()
        {
            if (_rewardedAdButtonTimeout > 0)
            {
                _rewardedAdButtonTimeout -= Time.deltaTime;
            }
        }

        public void PurchaseBackgroundItem(string itemCode)
        {
            _purchaseService.PurchaseItemAndSave(itemCode);

            UpdateView();
        }

        public void SelectPurchasedBackgroundItem(string itemCode)
        {
            _customizationService.SetBackgroundItem(itemCode);

            UpdateView();
        }

        private void CreateBackgroundShopItems()
        {
            foreach (BackgroundShopItemStaticData backgroundShopItem in _staticDataService.BackgroundShopItems.OrderBy(x => x.CoinPrice))
            {
                BackgroundShopItem prefab = Instantiate(_backgroundShopItemPrefab, _backgroundShopItemsContainer);
                prefab.Init(this, backgroundShopItem, _coinService, _purchaseService, _customizationService);
                _prefabs.Add(prefab);
            }
        }

        private void UpdateView()
        {
            foreach (BackgroundShopItem backgroundShopItem in _prefabs)
            {
                backgroundShopItem.UpdateView();
            }
        }

        private void UpdateCoinsCount()
        {
            _coinsCountText.text = _coinService.Coins.ToString();
        }

        private void ShowRewardedAd()
        {
            if (_rewardedAdButtonTimeout > 0f)
            {
                return;
            }

            _rewardedAdButtonTimeout = Settings.RewardedAdButtonTimeout;

            _soundMonoService.AdStarted();
            _adsService.ShowRewardedAd(AddCoins);
        }

        private void AddCoins()
        {
            _coinService.IncrementCoins();
            _soundMonoService.AdEnded();
        }

        private void OnCoinsChanged(object sender, CoinsChangedEventArgs e)
        {
            UpdateView();
        }

        private void Back()
        {
            Hide();
        }
    }
}