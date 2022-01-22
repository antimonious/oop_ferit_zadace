using System;

namespace WeatherLibrary{

    public class Weather{

        private const double c1 = -8.78469475556;
        private const double c2 = 1.61139411;
        private const double c3 = 2.33854883889;
        private const double c4 = -0.14611605;
        private const double c5 = -0.012308094;
        private const double c6 = -0.0164248277778;
        private const double c7 = 0.002211732;
        private const double c8 = 0.00072546;
        private const double c9 = -0.000003582;
        private double temperature;
        private double humidity;
        private double windSpeed;

        public Weather() : this(0.0, 0.0, 0.0) { }

        public Weather(double temperature, double humidity, double windSpeed){
            this.temperature = temperature;
            this.humidity = humidity / 100;
            this.windSpeed = windSpeed;
        }

        public void SetTemperature(double temperature) { this.temperature = temperature; }
        public void SetWindSpeed(double windSpeed) { this.windSpeed = windSpeed; }
        public void SetHumidity(double humidity) { this.humidity = humidity / 100; }

        public double GetTemperature() { return this.temperature; }
        public double GetWindSpeed() { return this.windSpeed; }
        public double GetHumidity() { return this.humidity; }

        public double CalculateFeelsLikeTemperature(){
            return c1 + c2 * temperature + c3 * humidity + c4 * temperature * humidity +
                c5 * Math.Pow(temperature, 2) + c6 * Math.Pow(humidity, 2)
                + c7 * Math.Pow(temperature, 2) * humidity +
                c8 * temperature * Math.Pow(humidity, 2) +
                c9 * Math.Pow(temperature, 2) * Math.Pow(humidity, 2);
        }
        public double CalculateWindChill(){
            if (temperature <= 10 && windSpeed > 4.8)
                return 13.12 + 0.6215 * temperature - 11.37 * Math.Pow(windSpeed, 0.16) +
                    0.3965 * temperature * Math.Pow(windSpeed, 0.16);
            else return 0;
        }
        public override string ToString()
        {
            return $"T={this.temperature}°C, w={this.windSpeed}km/h, h={this.humidity * 100}%";
        }
    }
}