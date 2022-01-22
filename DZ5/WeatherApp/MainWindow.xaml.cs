using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using BingMapsRESTToolkit;
using BingMapsRESTToolkit.Extensions;
using BingMapsSDSToolkit;
using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SourceChord.FluentWPF;
using WeatherLibrary;

namespace WeatherApp
{
    public partial class MainWindow
    {
        public void Test(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            string urlBase = "https://api.openweathermap.org/data/2.5/onecall?";
            string appId = "&appid=89537f08008dde526a6488234c381ca9";

            string requestResult = null;
            try
            {
                requestResult = client.GetStringAsync(urlBase+appId).Result;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            if (requestResult is null) return;

            JToken json = JToken.Parse(requestResult);
            string result = json["timezone"]?.ToString();
            Console.WriteLine(result);
        }
        
        public MainWindow()
        {
            InitializeComponent();
            img.Source = new BitmapImage(new Uri("C:\\Users\\leotu\\OneDrive\\Pictures\\IMG_20190412_183202-01.jpeg", UriKind.RelativeOrAbsolute));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Console.WriteLine(City.Text);
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            //List<double> coordinates = LocationUtilities.GetLatLon(City.Text);
            //Console.WriteLine(coordinates[0]);
            //Console.WriteLine(coordinates[1]);
        }
    }
}
