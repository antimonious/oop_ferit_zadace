using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WeatherLibrary
{
    // OpenWeather myDeserializedClass = JsonConvert.DeserializeObject<OpenWeather>(myJsonResponse);
    public static class WeatherUtilities
    {
        public static OpenWeather GetWeather(string address)
        {
            List<string> coordinates;
            try { coordinates = LocationUtilities.GetLatLon(address); }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                throw new HttpRequestException();
            }

            HttpClient client = new HttpClient();

            string key = null;
            string keyUri = "https://gist.githubusercontent.com/leotumbas/e9d224c9033aa6783d1759aef58d61ca/raw/ba24c4a25961116490cca39be9018af7ae6ead62/gistfile1.txt";

            try { key = client.GetStringAsync(keyUri).Result; }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                throw new HttpRequestException();
            }

            string uri = $"https://api.openweathermap.org/data/2.5/onecall?lat={coordinates[0]}&lon={coordinates[1]}&exclude=minutely,hourly&appid={key}&lang=hr&units=metric";
            string requestResult = null;

            try { requestResult = client.GetStringAsync(uri).Result; }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                throw new HttpRequestException();
            }

            return JsonConvert.DeserializeObject<OpenWeather>(requestResult);
        }

        public static DailyForecastRepository ConvertWeather(OpenWeather openWeather)
        {
            DailyForecastRepository dailyForecasts = new DailyForecastRepository();

            foreach (Daily daily in openWeather.daily)
                dailyForecasts.Add(
                    new DailyForecast(
                        new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(daily.dt).ToLocalTime(),
                        new Weather(daily.temp.day, daily.humidity, daily.wind_speed * 3.6)));

            return dailyForecasts;
        }

        public static List<BitmapImage> GetIcons(OpenWeather openWeather)
        {
            List<BitmapImage> icons = new List<BitmapImage>();
            string uri = "http://openweathermap.org/img/wn/";

            try
            {
                icons.Add(
                    new BitmapImage(
                        new Uri(uri + $"{openWeather.current.weather[0].icon}@2x.png", UriKind.RelativeOrAbsolute)));

                foreach (Daily daily in openWeather.daily)
                    icons.Add(
                        new BitmapImage(
                            new Uri(uri + $"{daily.weather[0].icon}.png", UriKind.RelativeOrAbsolute)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw new HttpRequestException();
            }

            return icons;
        }
    }
}