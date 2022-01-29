using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Windows;

namespace WeatherLibrary
{
    // GoogleMaps myDeserializedClass = JsonConvert.DeserializeObject<GoogleMaps>(myJsonResponse);
    public static class LocationUtilities
    {
        public static List<string> GetLatLon(string address)
        {
            HttpClient client = new HttpClient();

            string key = null;
            string keyUri = "https://gist.githubusercontent.com/leotumbas/719e03ee221968162a3f0871705ad1ff/raw/174fa36da25d0d146918bcf54288e681129a66a3/gistfile1.txt";

            try { key = client.GetStringAsync(keyUri).Result; }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                throw new HttpRequestException();
            }

            string uri = Uri.EscapeUriString($"https://maps.googleapis.com/maps/api/geocode/json?address={address}&key={key}&lang=hr");
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

            string key = null;
            string keyUri = "https://gist.githubusercontent.com/leotumbas/719e03ee221968162a3f0871705ad1ff/raw/174fa36da25d0d146918bcf54288e681129a66a3/gistfile1.txt";

            try { key = client.GetStringAsync(keyUri).Result; }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                throw new HttpRequestException();
            }

            string uri = Uri.EscapeUriString($"https://maps.googleapis.com/maps/api/geocode/json?address={search}&key={key}&lang=hr");
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

            string key = null;
            string keyUri = "https://gist.githubusercontent.com/leotumbas/719e03ee221968162a3f0871705ad1ff/raw/174fa36da25d0d146918bcf54288e681129a66a3/gistfile1.txt";

            try { key = client.GetStringAsync(keyUri).Result; }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                throw new HttpRequestException();
            }

            string uri = Uri.EscapeUriString($"https://maps.googleapis.com/maps/api/geocode/json?address={address}&key={key}&lang=hr");
            string requestResult = null;

            try { requestResult = client.GetStringAsync(uri).Result; }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                throw new HttpRequestException();
            }

            GoogleMaps myDeserializedClass = JsonConvert.DeserializeObject<GoogleMaps>(requestResult);
            if (!myDeserializedClass.status.Equals("OK")) throw new HttpRequestException();

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
