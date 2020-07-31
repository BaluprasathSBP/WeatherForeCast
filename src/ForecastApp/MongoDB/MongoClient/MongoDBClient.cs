using System;
using ForecastApp.Config;
using ForecastApp.MongoDB.Models;
using Mongo2Go;

using MongoDB.Driver;

namespace ForecastApp.MongoDB.MongoConnect
{
    public class MongoDBClient : IMongoDBClient
    {
        private MongoDbRunner _runner;
        private IMongoCollection<WeatherDocument> _collection;
        public IMongoDatabase DB { get; set; }
        public string ConnectionString { get; set; }

        public MongoDBClient()
        {
            CreateConnection();
        }

        public void CreateConnection()
        {
            _runner = MongoDbRunner.Start();            
            MongoClient client = new MongoClient(_runner.ConnectionString);
            ConnectionString = _runner.ConnectionString;
            DB = client.GetDatabase(Constants.DATABASE_NAME);
        }
    }
}
