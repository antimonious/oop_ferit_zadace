using System;

namespace WeatherLibrary{

    public class WeeklyForecast{

        private DailyForecast[] dailyForecasts;

        public WeeklyForecast(DailyForecast[] dailyForecasts){
            this.dailyForecasts = dailyForecasts;
        }

        public DailyForecast[] DailyForecasts{
            get { return this.dailyForecasts; }
            set { this.dailyForecasts = value; }
        }

        public double GetMaxTemperature(){
            DailyForecast maxForecast = dailyForecasts[0];
            foreach (DailyForecast dailyForecast in dailyForecasts)
                if (dailyForecast > maxForecast)
                    maxForecast = dailyForecast;
            return maxForecast.Weather.GetTemperature();
        }

        public DailyForecast this[int i] => this.dailyForecasts[i];
        public override string ToString()
        {
            string temp = "";
            foreach (DailyForecast dailyForecast in this.dailyForecasts)
                temp += $"{dailyForecast.ToString()}\n";
            return temp;
        }
    }
}
