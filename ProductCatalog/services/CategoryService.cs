using MongoDB.Driver;
using ProductCatalog.models;
using ProductCatalog.utils;

namespace ProductCatalog.services
{
    public class CategoryService
    {
        private readonly IMongoCollection<Category> _categoriesCollection;
        private readonly DBMongo _dbMongo;

        private readonly FileService _fileService;

        public CategoryService(DBMongo dbMongo, FileService fileService)
        {
            _dbMongo = dbMongo;
            _categoriesCollection = dbMongo.GetCollection<Category>("categories");
            _fileService = fileService;
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
                _fileService.LogError(ex.Message, nameof(AddCategory), DateTime.UtcNow);
                return -1;
            }
        }

        public int UpdateCategory(string categoryId, string categoryName)
        {
            try
            {
                var filter = Builders<Category>.Filter.Eq(c => c.Id, categoryId);
                var update = Builders<Category>.Update.Set(c => c.Name, categoryName);
                var result = _categoriesCollection.UpdateOne(filter, update);
                return 1;
            }
            catch (Exception ex)
            {
                _fileService.LogError(ex.Message, nameof(AddCategory), DateTime.UtcNow);
                return -1;
            }
        }

        public int DeleteCategory(string categoryId)
        {
            try
            {
                if (!CategoryExists(categoryId))
                {
                    _fileService.LogError("Silinmek istenen kategori bulunamadı.", nameof(DeleteCategory), DateTime.UtcNow);
                    return 0;
                }

                var filter = Builders<Category>.Filter.Eq(c => c.Id, categoryId);
                var result = _categoriesCollection.DeleteOne(filter);

                return result.DeletedCount > 0 ? 1 : -1;
            }
            catch (Exception ex)
            {
                _fileService.LogError(ex.Message, nameof(DeleteCategory), DateTime.UtcNow);
                return -1;
            }
        }

        public List<Category> GetAllCategories()
        {
            try
            {
                return _categoriesCollection.Find(_ => true).ToList();
            }
            catch (Exception ex)
            {
                _fileService.LogError(ex.Message, nameof(GetAllCategories), DateTime.UtcNow);
                return new List<Category>();
            }
        }


        public bool CategoryExists(string categoryId) //Kategori id'si kategori listesinde var mı kontrolü
        {
            var filter = Builders<Category>.Filter.Eq(c => c.Id, categoryId);
            var category = _categoriesCollection.Find(filter).FirstOrDefault();
            return category != null;
        }
        public string? GetCategoryNameById(string categoryId)
        {
            try
            {
                var filter = Builders<Category>.Filter.Eq(c => c.Id, categoryId);
                var category = _categoriesCollection.Find(filter).FirstOrDefault();

                if (category == null)
                {
                    _fileService.LogError("Kategori bulunamadı.", nameof(GetCategoryNameById), DateTime.UtcNow);
                    return null;
                }

                return category.Name;
            }
            catch (Exception ex)
            {
                _fileService.LogError(ex.Message, nameof(GetCategoryNameById), DateTime.UtcNow);
                return null;
            }
        }

    }
}