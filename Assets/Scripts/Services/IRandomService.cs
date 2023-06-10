namespace Services
{
    public interface IRandomService
    {
        int Next(int maxValue);
        float Next(float minValue, float maxValue);
    }
}