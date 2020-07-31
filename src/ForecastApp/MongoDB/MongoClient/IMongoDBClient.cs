using MongoDB.Driver;

namespace ForecastApp.MongoDB.MongoConnect
{
    public interface IMongoDBClient
    {
        IMongoDatabase DB { get; set; }
        string ConnectionString { get; set; }
        void CreateConnection();
    }


}