using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace WeatherLibrary
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Point
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Address
    {
        public string adminDistrict { get; set; }
        public string countryRegion { get; set; }
        public string formattedAddress { get; set; }
        public string locality { get; set; }
    }

    public class GeocodePoint
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
        public string calculationMethod { get; set; }
        public List<string> usageTypes { get; set; }
    }

    public class Resource
    {
        public string __type { get; set; }
        public List<double> bbox { get; set; }
        public string name { get; set; }
        public Point point { get; set; }
        public Address address { get; set; }
        public string confidence { get; set; }
        public string entityType { get; set; }
        public List<GeocodePoint> geocodePoints { get; set; }
        public List<string> matchCodes { get; set; }
    }

    public class ResourceSet
    {
        public int estimatedTotal { get; set; }
        public List<Resource> resources { get; set; }
    }

    public class Location
    {
        public string authenticationResultCode { get; set; }
        public string brandLogoUri { get; set; }
        public string copyright { get; set; }
        public List<ResourceSet> resourceSets { get; set; }
        public int statusCode { get; set; }
        public string statusDescription { get; set; }
        public string traceId { get; set; }
    }

    public static class LocationUtilities
    {
        public static List<double> GetLatLon(string address)
        {
            HttpClient client = new HttpClient();
            string uri = $"http://dev.virtualearth.net/REST/v1/Locations/query={address}?maxResults=1&key=z7AbpdoLBt8nqpYryo2N~c6tlIiRrlFg76IlH-QcK4A~AsqFyXRBcegV2vSPSY1a4u6HfUfV9Aas4_YmG1NIrMu-TszR0F1PpRFHjfXGrWSD";

            string requestResult = null;
            try
            {
                requestResult = client.GetStringAsync(uri).Result;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            if (requestResult is null) throw new HttpRequestException();

            WeatherLibrary.Location myDeserializedClass = JsonConvert.DeserializeObject<WeatherLibrary.Location>(requestResult);
            if (myDeserializedClass.statusCode != 200) throw new HttpRequestException();

            return myDeserializedClass.resourceSets[0].resources[0].point.coordinates;
        }
    }
}
