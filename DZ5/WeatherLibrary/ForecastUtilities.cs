using System;

namespace WeatherLibrary{

    public static class ForecastUtilities{ 

        public static Weather FindWeatherWithLargestWindchill(Weather[] weathers){
            Weather maxWindChillWeather = weathers[0];
            foreach (Weather tempWeather in weathers)
                if (tempWeather.CalculateWindChill() > maxWindChillWeather.CalculateWindChill())
                    maxWindChillWeather = tempWeather;
            return maxWindChillWeather;
        }
        public static DailyForecast Parse(string dailyForecast){
            string[] data = dailyForecast.Split(',');
            if (data[1].Contains('.'.ToString())) data[1] = data[1].Replace('.', ',');
            if (data[2].Contains('.'.ToString())) data[2] = data[2].Replace('.', ',');
            if (data[3].Contains('.'.ToString())) data[3] = data[3].Replace('.', ',');
            return new DailyForecast(DateTime.Parse(data[0]),
                new Weather(double.Parse(data[1]), double.Parse(data[3]), double.Parse(data[2])));
        }
        public static void PrintWeathers(IPrinter[] printers, Weather[] weathers){
            foreach (IPrinter printer in printers)
            {
                printer.PrintWeather(weathers);
            }
        }
    }
}
