using System;

namespace WeatherLibrary
{
    public class WeatherGenerator
    {
        private double minTemperature;
        private double maxTemperature;
        private double minHumidity;
        private double maxHumidity;
        private double minWindSpeed;
        private double maxWindSpeed;
        private IRandomGenerator generator;
        public WeatherGenerator(double minTemperature, double maxTemperature,
            double minHumidity, double maxHumidity,
            double minWindSpeed, double maxWindSpeed,
            IRandomGenerator generator)
        {
            this.minTemperature = minTemperature;
            this.maxTemperature = maxTemperature;
            this.minHumidity = minHumidity;
            this.maxHumidity = maxHumidity;
            this.minWindSpeed = minWindSpeed;
            this.maxWindSpeed = maxWindSpeed;
            this.generator = generator;
        }
        public Weather Generate()
        {
            return new Weather(generator.GenerateDouble(minTemperature, maxTemperature),
                generator.GenerateDouble(minHumidity, maxHumidity),
                generator.GenerateDouble(minWindSpeed, maxWindSpeed));
        }
        public void SetGenerator(IRandomGenerator generator)
        {
            this.generator=generator;
        }
    }
}
