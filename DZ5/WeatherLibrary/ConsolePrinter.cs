using System;

namespace WeatherLibrary
{
    public class ConsolePrinter : IPrinter
    {
        private ConsoleColor consoleColor;
        public ConsoleColor ConsoleColor { set { this.consoleColor = value; } }
        public ConsolePrinter(ConsoleColor consoleColor)
        {
            this.consoleColor = consoleColor;
        }
        public void PrintWeather(Weather[] weathers)
        {
            Console.ForegroundColor = consoleColor;
            foreach (Weather weather in weathers)
                Console.WriteLine(weather.ToString());
        }
    }
}
