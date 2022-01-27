using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WeatherLibrary
{
    // OpenWeather myDeserializedClass = JsonConvert.DeserializeObject<OpenWeather>(myJsonResponse);
    public class WeatherData
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Current
    {
        public int dt { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
        public double temp { get; set; }
        public double feels_like { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public double dew_point { get; set; }
        public double uvi { get; set; }
        public int clouds { get; set; }
        public int visibility { get; set; }
        public double wind_speed { get; set; }
        public int wind_deg { get; set; }
        public List<WeatherData> weather { get; set; }
    }

    public class Temp
    {
        public double day { get; set; }
        public double min { get; set; }
        public double max { get; set; }
        public double night { get; set; }
        public double eve { get; set; }
        public double morn { get; set; }
    }

    public class FeelsLike
    {
        public double day { get; set; }
        public double night { get; set; }
        public double eve { get; set; }
        public double morn { get; set; }
    }

    public class Daily
    {
        public int dt { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
        public int moonrise { get; set; }
        public int moonset { get; set; }
        public double moon_phase { get; set; }
        public Temp temp { get; set; }
        public FeelsLike feels_like { get; set; }
        public int pressure { get; set; }
        public double humidity { get; set; }
        public double dew_point { get; set; }
        public double wind_speed { get; set; }
        public int wind_deg { get; set; }
        public double wind_gust { get; set; }
        public List<WeatherData> weather { get; set; }
        public int clouds { get; set; }
        public double pop { get; set; }
        public double uvi { get; set; }
        public double? rain { get; set; }
        public double? snow { get; set; }
    }

    public class Alert
    {
        public string sender_name { get; set; }
        public string @event { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public string description { get; set; }
        public List<string> tags { get; set; }
    }

    public class OpenWeather
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public string timezone { get; set; }
        public int timezone_offset { get; set; }
        public Current current { get; set; }
        public List<Daily> daily { get; set; }
        public List<Alert> alerts { get; set; }
    }


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
            string uri = $"https://api.openweathermap.org/data/2.5/onecall?lat={coordinates[0]}&lon={coordinates[1]}&exclude=minutely,hourly&appid=89537f08008dde526a6488234c381ca9&lang=hr&units=metric";
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

            foreach(Daily daily in openWeather.daily)
                dailyForecasts.Add(
                    new DailyForecast(
                        new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(daily.dt-openWeather.timezone_offset).ToLocalTime(),
                        new Weather(daily.temp.day, daily.humidity, daily.wind_speed*3.6)));

            return dailyForecasts;
        }

        public static List<BitmapImage> GetIcons(OpenWeather openWeather)
        {
            List<BitmapImage> icons = new List<BitmapImage>();
            string uri = "http://openweathermap.org/img/wn/";

            icons.Add(
                new BitmapImage(
                    new Uri(uri + $"{openWeather.current.weather[0].icon}@2x.png", UriKind.RelativeOrAbsolute)));

            foreach(Daily daily in openWeather.daily)
                icons.Add(
                    new BitmapImage(
                        new Uri(uri + $"{daily.weather[0].icon}.png", UriKind.RelativeOrAbsolute)));

            return icons;
        }
    }
}