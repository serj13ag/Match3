namespace Services
{
    public interface ILocalizationService : IService
    {
        string GetTranslation(string key);
    }
}