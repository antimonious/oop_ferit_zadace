using System;
using System.IO;
using Weather_library;

namespace Weather_app{

    class Program{

        static void Main(string[] args){

            DateTime monday = new DateTime(2021, 11, 8);
            Weather mondayWeather = new Weather(6.17, 56.13, 4.9);
            DailyForecast mondayForecast = new DailyForecast(monday, mondayWeather);
            Console.WriteLine(monday.ToString());
            Console.WriteLine(mondayWeather.GetAsString());
            Console.WriteLine(mondayForecast.GetAsString());

            // Assume a valid input file (correct format).
            // Assume that the number of rows in the text file is always 7. 
            string fileName = "weather.forecast";
            if (File.Exists(fileName) == false){
                Console.WriteLine("The required file does not exist. Please create it, or change the path.");
                return;
            }

            string[] dailyWeatherInputs = File.ReadAllLines(fileName);
            DailyForecast[] dailyForecasts = new DailyForecast[dailyWeatherInputs.Length];
            for (int i = 0; i < dailyForecasts.Length; i++){
                dailyForecasts[i] = ForecastUtilities.Parse(dailyWeatherInputs[i]);
            }
            WeeklyForecast weeklyForecast = new WeeklyForecast(dailyForecasts);
            Console.WriteLine(weeklyForecast.GetAsString());
            Console.WriteLine("Maximal weekly temperature:");
            Console.WriteLine(weeklyForecast.GetMaxTemperature());
            Console.WriteLine(weeklyForecast[0].GetAsString());
        }
    }
}
