using System;

namespace Weather_library
{
    public class WeeklyForecast
    {
        private DailyForecast[] dailyForecasts;

        public WeeklyForecast(DailyForecast[] dailyForecasts)
        {
            this.dailyForecasts = dailyForecasts;
        }

        public DailyForecast[] DailyForecasts { get; set; }

        public double GetMaxTemperature()
        {
            double maxTemperature = dailyForecasts[0].Weather.GetTemperature();
            foreach (DailyForecast dailyForecast in dailyForecasts)
                if (dailyForecast.Weather.GetTemperature() > maxTemperature)
                    maxTemperature = dailyForecast.Weather.GetTemperature();
            return maxTemperature;
        }
    }
}
