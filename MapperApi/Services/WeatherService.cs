using System;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Mapper_Api.Services
{
    public class WeatherService
    {
        string AppKey;
        public WeatherService(string appKey)
        {
            this.AppKey = appKey;
        }

        public async Task<string> GetWeatherInLatLng(double Lat, double Lng)
        {
            // todo override
            double 
            lat = -25.768926, 
            lng = 28.242805;

            string baseUrl = $"http://api.openweathermap.org/data/2.5/weather?lat={lat.ToString()}&lon={lng.ToString()}&appid={this.AppKey}";
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage res = await client.GetAsync(baseUrl))
            using (HttpContent content = res.Content)
            {
                string data = await content.ReadAsStringAsync();
                if (data != null)
                {
                    return data;
                }
                return "";
            }
        }
    }
}
