using Horizon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Diagnostics;

namespace Horizon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _configuration = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Weather()
        {
            var key = _configuration.GetSection("APIKey").Value;

            var city = "alabaster";

            // Current weather data
            var client = new RestClient($"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={key}&units=imperial");

            var request = new RestRequest(Method.GET);
            IRestResponse responseCurrent = client.Execute(request);

            var data = JObject.Parse(responseCurrent.Content);

            Weather weather = new Weather()
            {
                CityName = data["name"].ToString(),
                Temp = data["main"]["temp"].ToString(),
                TempMin = data["main"]["temp_min"].ToString(),
                TempMax = data["main"]["temp_max"].ToString(),
                // Concatenate icon URL string for current weather icon
                Wicon = @"http://openweathermap.org/img/w/" + data["weather"][0]["icon"].ToString() + ".png",
                Description = data["weather"][0]["description"].ToString()
            };

            return View(weather);
        }
        public IActionResult ExtendedWeather()
        {
            var key = _configuration.GetSection("APIKey").Value;
            var city = "alabaster";

            // Current weather data
            var client = new RestClient($"http://api.openweathermap.org/data/2.5/forecast?q={city}&appid={key}&units=imperial");

            var request = new RestRequest(Method.GET);
            IRestResponse responseCurrent = client.Execute(request);

            var data = JObject.Parse(responseCurrent.Content);

            ExtendedWeather extWeather = new ExtendedWeather()
            {
                CityName = data["city"]["name"].ToString(),
                TempMin = data["list"][0]["main"]["temp_min"].ToString(),
                TempMax = data["list"][0]["main"]["temp_max"].ToString(),
                // Concatenate icon URL string for current weather icon
                Wicon = @"http://openweathermap.org/img/w/" + data["list"][0]["weather"][0]["icon"].ToString() + ".png",
                Description = data["list"][0]["weather"][0]["description"].ToString(),
                TimeStamp = data["list"][0]["dt_txt"].ToString()
            };

            return View(extWeather);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
