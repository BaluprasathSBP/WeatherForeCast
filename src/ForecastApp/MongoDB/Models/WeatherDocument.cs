using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ForecastApp.MongoDB.Models
{
    public class WeatherDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string SearchName { get; set; }

        public float Temp { get; set; }

        public int Humidity { get; set; }

        public int Pressure { get; set; }

        public float Wind { get; set; }

        public string Weather { get; set; }

        public string WeatherImage { get; set; }

        public string Country { get; set; }
    }
}
