using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.Services
{
    public static class ApiService
    {
        public static async Task<Root> Getweather(double latitude , double longitude)
        { 
            var httpClient = new HttpClient();
            var resposta = await httpClient.GetStringAsync(string.Format("https://api.openweathermap.org/data/2.5/forecast?lat={0}&lon={1}&units=metric&appid=7807d9a311ce2f61cb5bb9c716d16700",latitude,longitude));
            return JsonConvert.DeserializeObject<Root>(resposta);  
        }

        public static async Task<Root> GetweatherByCity(String city)
        {
            var httpClient = new HttpClient();
            var resposta = await httpClient.GetStringAsync(string.Format("https://api.openweathermap.org/data/2.5/forecast?q={0}&units=metric&appid=7807d9a311ce2f61cb5bb9c716d16700", city));
            return JsonConvert.DeserializeObject<Root>(resposta);
        }
    }
}
