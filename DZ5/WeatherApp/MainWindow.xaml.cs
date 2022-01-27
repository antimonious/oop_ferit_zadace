using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WeatherLibrary;

namespace WeatherApp
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            string initAddress = null;
            try
            {
                using (StreamReader reader = new StreamReader(".\\lastAddressRepos.txt"))
                {
                    initAddress = reader.ReadLine();
                }
                if (initAddress == string.Empty) throw new EndOfStreamException();
            }
            catch
            {
                initAddress = "Osijek, Hrvatska";
                using (StreamWriter writer = new StreamWriter(".\\lastAddressRepos.txt"))
                {
                    writer.WriteLine(string.Empty);
                }
            }

            if(initAddress != null)
                Reaction(initAddress);

            Cursor = Cursors.AppStarting;
            Date1.Text = "Danas";
            Date2.Text = "Sutra";
            List<TextBlock> dates = new List<TextBlock>() { Date3, Date4, Date5, Date6, Date7, Date8 };
            for (int i = 0; i < dates.Count; i++)
                dates[i].Text = DateTime.Now.AddDays(i+2).ToString("dd/MM");
            Cursor = Cursors.Arrow;
        }

        private void SearchBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) { SearchBar.Text = ""; }

        private void SearchResults_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Cursor = Cursors.Wait;
            WarningSign.Visibility = Visibility.Hidden;
            WarningDescription.Visibility = Visibility.Hidden;
            WarningDescription.Text = null;

            Reaction(SearchResults.SelectedItem.ToString());

            try
            {
                using (StreamWriter writer = new StreamWriter(".\\lastAddressRepos.txt", false))
                {
                    writer.WriteLine(SearchResults.SelectedItem.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            SearchResults.Visibility = Visibility.Hidden;
            SearchResults.ItemsSource = null;
            SearchResults.Items.Clear();

            Credits.Visibility = Visibility.Visible;
            Cursor = Cursors.Arrow;
        }

        private void SearchBar_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                Cursor = Cursors.Wait;
                SearchResults.ItemsSource = LocationUtilities.GetLocation(SearchBar.Text);
                SearchResults.Visibility = Visibility.Visible;
                Cursor = Cursors.Arrow;
            }
        }

        private void Reaction(string address)
        {
            int i;

            OpenWeather openWeather = WeatherUtilities.GetWeather(address);

            List<BitmapImage> weatherIcons = WeatherUtilities.GetIcons(openWeather);
            List<Image> icons = new List<Image>() { CurrentIcon, Icon1, Icon2, Icon3, Icon4, Icon5, Icon6, Icon7, Icon8 };
            for (i = 0; i < icons.Count; i++) icons[i].Source = weatherIcons[i];

            Weather currentWeather = new Weather(openWeather.current.temp, openWeather.current.humidity, openWeather.current.wind_speed * 3.6);

            CurrentCity.Text = LocationUtilities.GetCity(address);
            CurrentTemp.Text = currentWeather.GetTemperature().ToString("F0");
            CurrentHumidity.Text = (currentWeather.GetHumidity() * 100).ToString("F0");
            CurrentWindSpeed.Text = currentWeather.GetWindSpeed().ToString("F2");
            CurrentFeel.Text = currentWeather.CalculateFeelsLikeTemperature().ToString("F0");
            CurrentDescription.Text = openWeather.current.weather[0].description;
            SearchBar.Text = "Unesite adresu ovdje...";

            if (openWeather.alerts != null)
            {
                string temp = string.Empty;
                foreach (Alert alert in openWeather.alerts)
                {
                    if (alert.description != string.Empty)
                        temp += $"Od {new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(alert.start).ToLocalTime().ToString("g")}" +
                            $" do {new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(alert.end).ToLocalTime().ToString("g")}: " +
                            $"{alert.description}.{Environment.NewLine}";
                }
                if (temp != string.Empty)
                {
                    WarningSign.Visibility = Visibility.Visible;
                    WarningDescription.Text = temp;
                    WarningDescription.Visibility = Visibility.Visible;
                }
            }

            DailyForecastRepository repository = WeatherUtilities.ConvertWeather(openWeather);

            List<TextBlock> dailyTemps = new List<TextBlock>() { T1, T2, T3, T4, T5, T6, T7, T8 };
            List<TextBlock> dailyHumidity = new List<TextBlock>() { H1, H2, H3, H4, H5, H6, H7, H8 };
            List<TextBlock> dailyWindSpeeds = new List<TextBlock> { W1, W2, W3, W4, W5, W6, W7, W8 };
            List<TextBlock> dailyFeel = new List<TextBlock>() { F1, F2, F3, F4, F5, F6, F7, F8 };

            i = 0;
            foreach (DailyForecast daily in repository)
            {
                dailyTemps[i].Text = daily.Weather.GetTemperature().ToString("F0");
                dailyHumidity[i].Text = (daily.Weather.GetHumidity() * 100).ToString("F0");
                dailyWindSpeeds[i].Text = daily.Weather.GetWindSpeed().ToString("F2");
                dailyFeel[i].Text = daily.Weather.CalculateFeelsLikeTemperature().ToString("F0");
                i++;
            }
        }
    }
}