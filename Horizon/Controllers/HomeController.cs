using Horizon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Horizon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public ActionResult FirstTab()
        {
            return PartialView("_FirstTab");
        }
        public ActionResult SecondTab()
        {
            return PartialView("_SecondTab");
        }

        public IActionResult Weather()
        {
            // Using Open Weather Map from RapidAPI.com ***** 5 Day / 3 Hour Forecast Data *****

            // Current weather data
            var clientCurrent = new RestClient("https://community-open-weather-map.p.rapidapi.com/weather?q=Alabaster%2C%20USA&lang=null&units=imperial");

            // 5 Day weather data
            var client = new RestClient("https://community-open-weather-map.p.rapidapi.com/forecast?q=alabaster%2Cus&units=imperial");

            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "community-open-weather-map.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "602dc747acmsh8ff549b33d98aebp10b439jsnef8a8404c4da");
            IRestResponse response5Day = client.Execute(request);
            IRestResponse responseCurrent = client.Execute(request);

            var dataCurrent = JObject.Parse(responseCurrent.Content);
            var data5Day = JObject.Parse(response5Day.Content);

            Weather weather = new Weather()
            {
                Message = data5Day["message"].ToString(),
                CityName = data5Day["city"]["name"].ToString(),
                Temp = dataCurrent["list"][0]["main"]["temp"].ToString(),
                TempMin = data5Day["list"][0]["main"]["temp_min"].ToString(),
                TempMax = data5Day["list"][0]["main"]["temp_max"].ToString(),
                Humidity = data5Day["list"][0]["main"]["humidity"].ToString()
            };

            return View(weather);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
