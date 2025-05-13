using MongoDB.Driver;
using ProductCatalog.models;
using ProductCatalog.utils;

namespace ProductCatalog.services
{
    public class CategoryService
    {
        private readonly IMongoCollection<Category> _categoriesCollection;
        private readonly string _logFilePath = "error_log.txt";

        private readonly DBMongo _dbMongo;

        public CategoryService(DBMongo dbMongo)
        {
            _dbMongo = dbMongo;
            _categoriesCollection = dbMongo.GetCollection<Category>("categories");
        }
        public int AddCategory(Category category)
        {
            try
            {
                _categoriesCollection.InsertOne(category);
                return 1;
            }
            catch (Exception ex)
            {
                // Hata mesajını kullanıcıya program.cs'de gösteriyoruz burada sadece log dosyasına yazıyoruz.
                //Log dosyası olduğu için UTC kullandım. Kullanıcıdan bağımsız global bir zaman damgası.
                LogError(ex.Message, nameof(AddCategory), DateTime.UtcNow);
                return -1;
            }
        }


        private void LogError(string message, string methodName, DateTime timeStamp)
        {
            string logMessage = $"{timeStamp}: {methodName} - {message}";
            File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
        }

        public bool CategoryExists(string categoryId) //Kategori id'si kategori listesinde var mı kontrolü
        {
            var filter = Builders<Category>.Filter.Eq(c => c.Id, categoryId);
            var category = _categoriesCollection.Find(filter).FirstOrDefault();
            return category != null;
        }
    }
}