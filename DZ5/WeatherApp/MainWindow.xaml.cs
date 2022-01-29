using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WeatherLibrary;

namespace WeatherApp
{
    public partial class MainWindow
    {
        private OpenWeather openWeather;
        private DailyForecastRepository repository;
        private Weather currentWeather;
        private List<BitmapImage> weatherIcons;
        private List<Image> icons;
        private List<TextBlock> dates;
        private List<TextBlock> dailyTemps;
        private List<TextBlock> dailyHumidity;
        private List<TextBlock> dailyWindSpeeds;
        private List<TextBlock> dailyFeel;
        private string address;
        private const string fileName = ".\\lastAddressRepos.txt";
        private DispatcherTimer refreshTimer;
        private DispatcherTimer dateTimer;
        public MainWindow()
        {
            Cursor = Cursors.AppStarting;
            refreshTimer = new DispatcherTimer();
            dateTimer = new DispatcherTimer();
            refreshTimer.Tick += OnTimedEvent;
            dateTimer.Tick += DateTimedEvent;
            refreshTimer.Interval = TimeSpan.FromMinutes(15);
            dateTimer.Interval = DateTime.Today.AddDays(1).AddSeconds(228) - DateTime.Now;
            dateTimer.Start();

            InitializeComponent();

            dates = new List<TextBlock>() { Date3, Date4, Date5, Date6, Date7, Date8 };
            icons = new List<Image>() { CurrentIcon, Icon1, Icon2, Icon3, Icon4, Icon5, Icon6, Icon7, Icon8 };
            dailyTemps = new List<TextBlock>() { T1, T2, T3, T4, T5, T6, T7, T8 };
            dailyHumidity = new List<TextBlock>() { H1, H2, H3, H4, H5, H6, H7, H8 };
            dailyWindSpeeds = new List<TextBlock> { W1, W2, W3, W4, W5, W6, W7, W8 };
            dailyFeel = new List<TextBlock>() { F1, F2, F3, F4, F5, F6, F7, F8 };

            string initAddress = null;
            try
            {
                using (StreamReader reader = new StreamReader(fileName)) { initAddress = reader.ReadLine(); }
                if (string.IsNullOrWhiteSpace(initAddress)) throw new EndOfStreamException();
            }
            catch
            {
                initAddress = "Osijek, Hrvatska";
                using (StreamWriter writer = new StreamWriter(fileName)) { writer.WriteLine(initAddress); }
            }

            if (!string.IsNullOrWhiteSpace(initAddress)) address = initAddress;
            else address = "Osijek, Hrvatska";

            Date1.Text = "Danas";
            Date2.Text = "Sutra";

            SetDates();
            Reaction();

            refreshTimer.Start();
            Cursor = Cursors.Arrow;
        }
        private void DateTimedEvent(object sender, EventArgs e)
        {
            refreshTimer.Stop();
            dateTimer.Stop();
            SetDates();
            Reaction();
            dateTimer.Interval = DateTime.Today.AddDays(1) - DateTime.Now;
            dateTimer.Start();
            refreshTimer.Start();
        }
        private void OnTimedEvent(object sender, EventArgs e) { Reaction(); }
        private void SetDates() { for (int i = 0; i < dates.Count; i++) dates[i].Text = DateTime.Now.AddDays(i + 2).ToString("dd/MM"); }
        private void SearchBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) { SearchBar.Text = ""; }
        private void SearchResults_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            refreshTimer.Stop();

            try
            {
                WarningSign.Visibility = Visibility.Hidden;
                WarningDescription.Visibility = Visibility.Hidden;
                WarningDescription.Text = null;

                address = SearchResults.SelectedItem.ToString();
                Reaction();

                using (StreamWriter writer = new StreamWriter(fileName, false)) { writer.WriteLine(address); }

                SearchResults.Visibility = Visibility.Hidden;
                SearchResults.ItemsSource = null;
                SearchResults.Items.Clear();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            refreshTimer.Start();
        }
        private void SearchBar_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.Equals(System.Windows.Input.Key.Enter))
            {
                Cursor = Cursors.Wait;
                try
                {
                    SearchResults.ItemsSource = LocationUtilities.GetLocation(SearchBar.Text);
                    SearchResults.Visibility = Visibility.Visible;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                Cursor = Cursors.Arrow;
            }
        }
        private void Reaction()
        {
            Cursor = Cursors.Wait;
            int i;

            try
            {
                openWeather = WeatherUtilities.GetWeather(address);
                weatherIcons = WeatherUtilities.GetIcons(openWeather);

                for (i = 0; i < icons.Count; i++) icons[i].Source = weatherIcons[i];

                currentWeather = new Weather(openWeather.current.temp, openWeather.current.humidity, openWeather.current.wind_speed * 3.6);

                CurrentCity.Text = LocationUtilities.GetCity(address);
                CurrentTemp.Text = currentWeather.GetTemperature().ToString("F0");
                CurrentHumidity.Text = (currentWeather.GetHumidity() * 100).ToString("F0");
                CurrentWindSpeed.Text = currentWeather.GetWindSpeed().ToString("F2");
                CurrentFeel.Text = currentWeather.CalculateFeelsLikeTemperature().ToString("F0");
                CurrentDescription.Text = openWeather.current.weather[0].description;
                SearchBar.Text = "Unesite adresu ovdje...";

                if (!(openWeather.alerts is null))
                {
                    string temp = string.Empty;
                    foreach (Alert alert in openWeather.alerts)
                    {
                        if (!string.IsNullOrWhiteSpace(alert.description))
                            temp += $"Od {new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(alert.start).ToLocalTime().ToString("g")}" +
                                $" do {new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(alert.end).ToLocalTime().ToString("g")}: " +
                                $"{alert.description}.{Environment.NewLine}";
                    }
                    if (!string.IsNullOrWhiteSpace(temp))
                    {
                        WarningSign.Visibility = Visibility.Visible;
                        WarningDescription.Text = temp;
                        WarningDescription.Visibility = Visibility.Visible;
                    }
                }

                repository = WeatherUtilities.ConvertWeather(openWeather);

                i = 0;
                foreach (DailyForecast daily in repository)
                {
                    dailyTemps[i].Text = daily.Weather.GetTemperature().ToString("F0");
                    dailyHumidity[i].Text = (daily.Weather.GetHumidity() * 100).ToString("F0");
                    dailyWindSpeeds[i].Text = daily.Weather.GetWindSpeed().ToString("F2");
                    dailyFeel[i].Text = daily.Weather.CalculateFeelsLikeTemperature().ToString("F0");
                    i++;
                }

                RefreshInfo.Text = $"Podaci ažurirani u: {DateTime.Now.ToString("G")}";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            Cursor = Cursors.Arrow;
        }
        private void ForceRefresh_Click(object sender, RoutedEventArgs e)
        {
            refreshTimer.Stop();
            SetDates();
            Reaction();
            refreshTimer.Start();
        }
    }
}