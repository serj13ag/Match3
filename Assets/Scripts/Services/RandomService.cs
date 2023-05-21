using System;

namespace Services
{
    public class RandomService
    {
        private readonly Random _random;

        public RandomService()
        {
            _random = new Random();
        }

        public int Next(int maxValue)
        {
            return _random.Next(maxValue);
        }

        public float Next(float minValue, float maxValue)
        {
            return (float)(_random.NextDouble() * (maxValue - minValue) + minValue);
        }
    }
}