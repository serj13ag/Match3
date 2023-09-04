using System;

namespace Services
{
    public interface ILocalizationService : IService
    {
        string GetTranslation(string key);

        event EventHandler<EventArgs> LocalizationChanged;
    }
}