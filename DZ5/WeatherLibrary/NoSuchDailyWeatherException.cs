using System;

namespace WeatherLibrary
{
    public class NoSuchDailyWeatherException : Exception
    {
        public NoSuchDailyWeatherException(DateTime dateTime, string message) : base($"{message}{dateTime}") { }
    }
}
