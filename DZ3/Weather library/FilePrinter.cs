using System;
using System.IO;

namespace Weather_library
{
    public class FilePrinter : IPrinter
    {
        private string fileName;
        public string FileName { set { this.fileName = value; } }
        public FilePrinter(string fileName)
        {
            this.fileName = fileName;
        }
        public void PrintWeather(Weather[] weathers)
        {
            using(StreamWriter streamWriter = new StreamWriter(fileName))
            {
                foreach(Weather weather in weathers)
                    streamWriter.WriteLine(weather.ToString());
            }
        }
    }
}
