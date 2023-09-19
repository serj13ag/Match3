using StaticData.Shop;

namespace Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IPersistentDataService _persistentDataService;
        private readonly ICoinService _coinService;

        public PurchaseService(IStaticDataService staticDataService, IPersistentDataService persistentDataService,
            ICoinService coinService)
        {
            _staticDataService = staticDataService;
            _persistentDataService = persistentDataService;
            _coinService = coinService;
        }

        public void PurchaseItemAndSave(string itemCode)
        {
            BackgroundShopItemStaticData backgroundShopItemData = _staticDataService.GetBackgroundShopItem(itemCode);
            _coinService.SpendCoins(backgroundShopItemData.CoinPrice);

            _persistentDataService.Purchases.BackgroundItemCodes.Add(itemCode);
            _persistentDataService.Save();
        }

        public bool ItemIsPurchased(string itemCode)
        {
            return _persistentDataService.Purchases.BackgroundItemCodes.Contains(itemCode);
        }
    }
}