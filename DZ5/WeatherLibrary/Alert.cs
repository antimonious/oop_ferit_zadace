using System.Collections.Generic;

namespace WeatherLibrary
{
    public class Alert
    {
        public string sender_name { get; set; }
        public string @event { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public string description { get; set; }
        public List<string> tags { get; set; }
    }
}
