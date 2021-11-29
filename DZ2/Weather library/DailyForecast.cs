using System;

namespace Weather_library
{
    public class DailyForecast
    {
        private DateTime dateTime;
        private Weather weather;

        public DateTime DateTime { get; set; }
        public Weather Weather { get; set; }

        public DailyForecast(DateTime dateTime, Weather weather)
        {
            this.dateTime = dateTime;
            this.weather = weather;
        }
    }
}
