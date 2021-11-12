using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Horizon.Models
{
    public class Weather
    {
        public string Message { get; set; }
        public string CityName { get; set; }
        public string Temp { get; set; }
        public string TempMin { get; set; }
        public string TempMax { get; set; }
        public string Humidity { get; set; }
    }
}
