using UnityEngine;

namespace Services
{
    public class CustomizationService : ICustomizationService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IPersistentDataService _persistentDataService;

        public string CurrentBackgroundItemCode => _persistentDataService.Customizations.BackgroundItemCode;

        public CustomizationService(IStaticDataService staticDataService, IPersistentDataService persistentDataService)
        {
            _staticDataService = staticDataService;
            _persistentDataService = persistentDataService;
        }

        public void SetBackgroundItem(string itemCode)
        {
            _persistentDataService.Customizations.BackgroundItemCode = itemCode;
            _persistentDataService.Save();
        }

        public Color GetCurrentBackgroundColor()
        {
            string currentBackgroundItem = _persistentDataService.Customizations.BackgroundItemCode;
            return _staticDataService.GetBackgroundShopItem(currentBackgroundItem).Color;
        }
    }
}