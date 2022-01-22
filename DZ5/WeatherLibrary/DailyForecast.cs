using System;

namespace WeatherLibrary{

    public class DailyForecast{

        private DateTime dateTime;
        private Weather weather;

        public DateTime DateTime{
            get { return this.dateTime; }
            set { this.dateTime = value; }
        }

        public Weather Weather{
            get { return this.weather; }
            set { this.weather = value; }
        }

        public DailyForecast(DateTime dateTime, Weather weather){
            this.dateTime = dateTime;
            this.weather = weather;
        }

        public static bool operator >(DailyForecast leftForecast, DailyForecast rightForecast){
            if (leftForecast is null) return rightForecast is null;
            return leftForecast.Weather.GetTemperature() > rightForecast.Weather.GetTemperature();
        }

        public static bool operator <(DailyForecast leftForecast, DailyForecast rightForecast){
            if (leftForecast is null) return rightForecast is null;
            return leftForecast.Weather.GetTemperature() < rightForecast.Weather.GetTemperature();
        }
        public override string ToString()
        {
            return $"{this.dateTime.ToString()}: {this.weather.ToString()}";
        }
    }
}
