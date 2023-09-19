using System;
using System.Collections.Generic;
using Constants;

namespace Data
{
    [Serializable]
    public class Purchases
    {
        public List<string> BackgroundItemCodes { get; set; }

        public Purchases()
        {
            BackgroundItemCodes = new List<string> { Settings.InitiallyPurchasedBackgroundItemCode };
        }
    }
}