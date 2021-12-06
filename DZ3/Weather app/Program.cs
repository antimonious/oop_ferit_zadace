using System;
using System.IO;
using Weather_library;

namespace Weather_app{

    class Program
    {

        static void Main(string[] args)
        {

            const int weatherCount = 10;
            double minTemperature = -25.00, maxTemperature = 43.00;
            double minHumidity = 0.00, maxHumidity = 100.00;
            double minWindSpeed = 0.00, maxWindSpeed = 10.00;
            Random generator = new Random();

            IRandomGenerator randomGenerator = new UniformGenerator(generator);
            WeatherGenerator weatherGenerator = new WeatherGenerator(
                minTemperature, maxTemperature,
                minHumidity, maxHumidity,
                minWindSpeed, maxWindSpeed,
                randomGenerator
            );

            Weather[] uniformWeathers = new Weather[weatherCount];
            for (int i = 0; i < uniformWeathers.Length; i++)
            {
                uniformWeathers[i] = weatherGenerator.Generate();
            }

            randomGenerator = new BiasedGenerator(generator);
            weatherGenerator.SetGenerator(randomGenerator);
            Weather[] winterWeathers = new Weather[weatherCount];
            for (int i = 0; i < winterWeathers.Length; i++)
            {
                winterWeathers[i] = weatherGenerator.Generate();
            }

            IPrinter[] uniformPrinters = new IPrinter[]
            {
        new ConsolePrinter(ConsoleColor.DarkYellow),
        new FilePrinter(@"uniformWeathers.txt"),
            };
            ForecastUtilities.PrintWeathers(uniformPrinters, uniformWeathers);

            IPrinter[] winterPrinters = new IPrinter[]
            {
        new ConsolePrinter(ConsoleColor.Green),
        new FilePrinter(@"winterWeathers.txt"),
            };
            ForecastUtilities.PrintWeathers(winterPrinters, winterWeathers);
        }
    }
}
