using System;

namespace WeatherLibrary
{
    public class BiasedGenerator : IRandomGenerator
    {
        private Random generator;
        public BiasedGenerator(Random generator)
        {
            this.generator = generator;
        }
        public double GenerateDouble(double minValue, double maxValue)
        {
            return Math.Abs(generator.NextDouble() - generator.NextDouble()) * (1 + maxValue - minValue) + minValue;
        }
    }
}
