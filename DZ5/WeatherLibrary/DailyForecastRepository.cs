using System;
using System.Collections;
using System.Collections.Generic;

namespace WeatherLibrary
{
    public class DailyForecastRepository : IEnumerable<DailyForecast>
    {
        private SortedDictionary<DateTime, DailyForecast> dailyForecasts;
        public DailyForecastRepository() : this(null) { }
        public DailyForecastRepository(DailyForecastRepository original)
        {
            dailyForecasts = new SortedDictionary<DateTime, DailyForecast>();
            if (original != null)
                foreach (DailyForecast dailyForecast in original)
                    this.Add(dailyForecast);
        }
        public void Add(List<DailyForecast> dailyForecasts)
        {
            foreach (DailyForecast dailyForecast in dailyForecasts)
                Add(dailyForecast);
        }
        public void Add(DailyForecast dailyForecast)
        {
            if (dailyForecasts.ContainsKey(dailyForecast.DateTime.Date))
                dailyForecasts[dailyForecast.DateTime.Date] = dailyForecast;
            else dailyForecasts.Add(dailyForecast.DateTime.Date, dailyForecast);
        }
        public void Remove(DateTime dateTime)
        {
            if (dailyForecasts.ContainsKey(dateTime.Date))
                dailyForecasts.Remove(dateTime.Date);
            else throw new NoSuchDailyWeatherException(dateTime.Date, "No daily forecast for ");
        }
        public override string ToString()
        {
            string temp="";
            foreach (DailyForecast dailyForecast in this)
                temp += $"{dailyForecast}{Environment.NewLine}";
            return temp;
        }
        public IEnumerator<DailyForecast> GetEnumerator()
        {
            return dailyForecasts.Values.GetEnumerator();
        }
        private IEnumerator GetEnumerator1()
        {
            return this.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator1();
        }
    }
}
