using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using Microsoft.Maps.MapControl.WPF;
using SourceChord.FluentWPF;
using Weather_library;

namespace WeatherApp
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            img.Source = new BitmapImage(new Uri("https://static6.depositphotos.com/1004330/653/i/950/depositphotos_6532778-stock-photo-clear-blue-sky-background-with.jpg", UriKind.RelativeOrAbsolute));
        }
    }
}
