using System;

namespace Weather_library{

    public static class ForecastUtilities{ 

        public static Weather FindWeatherWithLargestWindchill(Weather[] weathers){
            Weather maxWindChillWeather = weathers[0];
            foreach (Weather tempWeather in weathers)
                if (tempWeather.CalculateWindChill() > maxWindChillWeather.CalculateWindChill())
                    maxWindChillWeather = tempWeather;
            return maxWindChillWeather;
        }

        public static string GetAsString(this Weather weather){
            return $"T={weather.GetTemperature()}°C, w={weather.GetWindSpeed()}km/h, h={weather.GetHumidity() * 100}%";
        }

        public static string GetAsString(this DailyForecast dailyForecast){
            return $"{dailyForecast.DateTime.ToString()}: {dailyForecast.Weather.GetAsString()}";
        }

        public static string GetAsString(this WeeklyForecast weeklyForecast){
            string temp="";
            foreach (DailyForecast dailyForecast in weeklyForecast.DailyForecasts)
                temp += $"{dailyForecast.GetAsString()}\n";
            return temp;
        }

        public static DailyForecast Parse(string dailyForecast){
            string[] data = dailyForecast.Split(',');
            if (data[1].Contains('.')) data[1] = data[1].Replace('.', ',');
            if (data[2].Contains('.')) data[2] = data[2].Replace('.', ',');
            if (data[3].Contains('.')) data[3] = data[3].Replace('.', ',');
            return new DailyForecast(DateTime.Parse(data[0]),
                new Weather(double.Parse(data[1]), double.Parse(data[3]), double.Parse(data[2])));
        }
    }
}
