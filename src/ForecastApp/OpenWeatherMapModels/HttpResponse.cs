using System;
using Newtonsoft.Json;

namespace ForecastApp.OpenWeatherMapModels
{
    public class HttpResponse
    {
        [JsonProperty("cod")]
        public string StatusCode { get; set; }

        [JsonProperty("message")]
        public string ErrorMessage { get; set; }
    }
}
