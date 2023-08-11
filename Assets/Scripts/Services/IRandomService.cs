namespace Services
{
    public interface IRandomService : IService
    {
        int Next(int maxValue);
        float Next(float minValue, float maxValue);
    }
}