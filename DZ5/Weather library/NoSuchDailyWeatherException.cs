using System;

namespace Weather_library
{
    public class NoSuchDailyWeatherException : Exception
    {
        public NoSuchDailyWeatherException(DateTime dateTime, string message) : base($"{message}{dateTime}") { }
    }
}
