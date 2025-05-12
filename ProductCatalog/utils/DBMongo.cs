using MongoDB.Driver;

namespace ProductCatalog.utils
{
    public class DBMongo
    {
        private readonly string _connectionString = "mongodb://localhost:27017";
        // Hata almak için
        // private readonly string _connectionString = "mongodb://localhost:00000"; 
        private readonly string _databaseName = "ProductCategory";
        private readonly IMongoDatabase _database;

        public DBMongo()
        {
            var client = new MongoClient(_connectionString);
            _database = client.GetDatabase(_databaseName);
        }
        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}