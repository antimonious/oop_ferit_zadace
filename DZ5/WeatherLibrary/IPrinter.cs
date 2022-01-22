using System;

namespace WeatherLibrary
{
    public interface IPrinter
    {
        void PrintWeather(Weather[] weathers);
    }
}
