using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Humraaah.Models
{
    public class DistanceCalculator
    {
        public static double DistanceCalculate(Address ad1, Address ad2)
        {
            string endpoint = "http://localhost:59116/"; //ConfigurationManager.AppSettings["ENDPOINTAPI"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(endpoint);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                string requestQuery = string.Format(
                    "api/DistanceCalculator/CalculateDistance?lat1_={0}&lat2_={1}&lon1_={2}&lon2_={3}",
                    ad1.Lat, ad2.Lat, ad1.Lng, ad2.Lng);
                HttpResponseMessage response = client.GetAsync(requestQuery).Result;
                if (response.IsSuccessStatusCode)
                {
                    var resultObject = response.Content.ReadAsAsync<double>().Result;
                    return resultObject;
                }

            }
            return 100000.0;
        }
    }
}