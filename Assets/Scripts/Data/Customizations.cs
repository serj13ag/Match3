using System;
using Constants;

namespace Data
{
    [Serializable]
    public class Customizations
    {
        public string BackgroundItemCode { get; set; }

        public Customizations()
        {
            BackgroundItemCode = Settings.InitiallyPurchasedBackgroundItemCode;
        }
    }
}