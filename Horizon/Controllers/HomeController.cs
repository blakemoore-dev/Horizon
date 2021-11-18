using Horizon.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Horizon.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration config)
        {
            _configuration = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Weather(string city)
        //{
        //    var key = _configuration.GetSection("APIKey").Value;

        //    // Current weather data
        //    var client = new RestClient($"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={key}&units=imperial");

        //    var request = new RestRequest(Method.GET);
        //    IRestResponse responseCurrent = client.Execute(request);

        //    var data = JObject.Parse(responseCurrent.Content);

        //    Weather weather = new Weather()
        //    {
        //        CityName = data["name"].ToString(),
        //        Temp = data["main"]["temp"].ToString(),
        //        TempMin = data["main"]["temp_min"].ToString(),
        //        TempMax = data["main"]["temp_max"].ToString(),
        //        // Concatenate icon URL string for current weather icon
        //        Wicon = @"http://openweathermap.org/img/w/" + data["weather"][0]["icon"].ToString() + ".png",
        //        Description = data["weather"][0]["description"].ToString()
        //    };

        //    return View(weather);
        //}

        [HttpPost]
        public IActionResult ExtendedWeather(string city)
        {
            // Abstract API Key away to appsettings.json (included in .gitignore)
            var key = _configuration.GetSection("APIKey").Value;
            // API through RestSharp
            var client = new RestClient($"http://api.openweathermap.org/data/2.5/forecast?q={city}&appid={key}&units=imperial");
            var request = new RestRequest(Method.GET);
            IRestResponse responseCurrent = client.Execute(request);
            // Extended weather data
            var data = JObject.Parse(responseCurrent.Content);
            // Count of items in parsed JSON list
            var count = Convert.ToInt32(data["cnt"]);

            var list = new List<ExtendedWeather>();

            // Loop through parsed JSON data from API and store each individual section into it's own object
            for (int i = 0; i < count; i++)
            {
                list.Add(new ExtendedWeather()
                {
                    CityName = data["city"]["name"].ToString(),
                    TempMin = data["list"][i]["main"]["temp_min"].ToString(),
                    TempMax = data["list"][i]["main"]["temp_min"].ToString(),
                    Wicon = @"http://openweathermap.org/img/w/" + data["list"][i]["weather"][0]["icon"].ToString() + ".png",
                    Description = data["list"][i]["weather"][0]["description"].ToString(),
                    TimeStamp = data["list"][i]["dt_txt"].ToString()
                });
            }

            //ExtendedWeather extWeather = new ExtendedWeather()
            //{
            //    CityName = data["city"]["name"].ToString(),
            //    TempMin = data["list"][0]["main"]["temp_min"].ToString(),
            //    TempMax = data["list"][0]["main"]["temp_max"].ToString(),
            //    // Concatenate icon URL string for current weather icon
            //    Wicon = @"http://openweathermap.org/img/w/" + data["list"][0]["weather"][0]["icon"].ToString() + ".png",
            //    Description = data["list"][0]["weather"][0]["description"].ToString(),
            //    TimeStamp = data["list"][0]["dt_txt"].ToString()
            //};

            //return View(extWeather);

            ViewBag.Data = list;
            return View("ExtendedWeather");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
