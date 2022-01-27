using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Windows;

namespace WeatherLibrary
{
    // GoogleMaps myDeserializedClass = JsonConvert.DeserializeObject<GoogleMaps>(myJsonResponse);
    public class AddressComponent
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }

    public class Northeast
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Southwest
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Bounds
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }

    public class Location
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Viewport
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }

    public class Geometry
    {
        public Bounds bounds { get; set; }
        public Location location { get; set; }
        public string location_type { get; set; }
        public Viewport viewport { get; set; }
    }

    public class Result
    {
        public List<AddressComponent> address_components { get; set; }
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public string place_id { get; set; }
        public List<string> types { get; set; }
    }

    public class GoogleMaps
    {
        public List<Result> results { get; set; }
        public string status { get; set; }
    }

    public static class LocationUtilities
    {
        public static List<string> GetLatLon(string address)
        {
            HttpClient client = new HttpClient();

            string uri = Uri.EscapeUriString($"https://maps.googleapis.com/maps/api/geocode/json?address={address}&key=AIzaSyBOAYzNNJth_IEXHwWimV4CFdR_ZNmqPwg&lang=hr");
            string requestResult = null;
            
            try { requestResult = client.GetStringAsync(uri).Result; }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                throw new HttpRequestException();
            }

            GoogleMaps myDeserializedClass = JsonConvert.DeserializeObject<GoogleMaps>(requestResult);
            if (myDeserializedClass.status != "OK") throw new HttpRequestException();

            List<string> coordinates = new List<string>() {
                myDeserializedClass.results[0].geometry.location.lat.ToString("G", CultureInfo.InvariantCulture),
                myDeserializedClass.results[0].geometry.location.lng.ToString("G", CultureInfo.InvariantCulture)
            };

            return coordinates;
        }

        public static List<string> GetLocation(string search)
        {
            HttpClient client = new HttpClient();

            string uri = Uri.EscapeUriString($"https://maps.googleapis.com/maps/api/geocode/json?address={search}&key=AIzaSyBOAYzNNJth_IEXHwWimV4CFdR_ZNmqPwg&lang=hr");
            string requestResult = null;

            try { requestResult = client.GetStringAsync(uri).Result; }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                throw new HttpRequestException();
            }

            GoogleMaps myDeserializedClass = JsonConvert.DeserializeObject<GoogleMaps>(requestResult);
            if (myDeserializedClass.status != "OK" && myDeserializedClass.status != "ZERO_RESULTS") throw new HttpRequestException();

            List<string> possibleMatches = new List<string>();
            foreach(Result result in myDeserializedClass.results)
                possibleMatches.Add(result.formatted_address);

            return possibleMatches;
        }
        public static string GetCity(string address)
        {
            HttpClient client = new HttpClient();

            string uri = Uri.EscapeUriString($"https://maps.googleapis.com/maps/api/geocode/json?address={address}&key=AIzaSyBOAYzNNJth_IEXHwWimV4CFdR_ZNmqPwg&lang=hr");
            string requestResult = null;

            try { requestResult = client.GetStringAsync(uri).Result; }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                throw new HttpRequestException();
            }

            GoogleMaps myDeserializedClass = JsonConvert.DeserializeObject<GoogleMaps>(requestResult);
            if (myDeserializedClass.status != "OK") throw new HttpRequestException();

            foreach (AddressComponent addressComponent in myDeserializedClass.results[0].address_components)
                if (addressComponent.types.Contains("locality"))
                    return addressComponent.long_name;

            foreach (AddressComponent addressComponent in myDeserializedClass.results[0].address_components)
                if (addressComponent.types.Contains("administrative_area_level_2"))
                    return addressComponent.long_name;

            foreach (AddressComponent addressComponent in myDeserializedClass.results[0].address_components)
                if (addressComponent.types.Contains("administrative_area_level_1"))
                    return addressComponent.long_name;

            foreach (AddressComponent addressComponent in myDeserializedClass.results[0].address_components)
                if (addressComponent.types.Contains("country"))
                    return addressComponent.long_name;

            return null;
        }
    }
}
