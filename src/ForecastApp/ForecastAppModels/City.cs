﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ForecastApp.ForecastAppModels
{
    public class City
    {
        [Display(Name = "City:")]
        public string Name { get; set; }

        [Display(Name = "Temperature:")]
        public float Temp { get; set; }

        [Display(Name = "Humidity:")]
        public int Humidity { get; set; }

        [Display(Name = "Pressure:")]
        public int Pressure { get; set; }

        [Display(Name = "Wind Speed:")]
        public float Wind { get; set; }

        [Display(Name = "Weather Condition:")]
        public string Weather { get; set; }

        public string WeatherImage { get; set; }

        public string Country { get; set; }

        [JsonIgnore]        
        public string ErrorMessage { get; set; }
    }
}
