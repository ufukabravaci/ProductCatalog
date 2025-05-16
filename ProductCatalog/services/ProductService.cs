using System.IO.Pipelines;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductCatalog.models;
using ProductCatalog.utils;

namespace ProductCatalog.services
{
    public class ProductService
    {
        private readonly IMongoCollection<Product> _productsCollection;
        private readonly CategoryService _categoryService;

        private readonly FileService _fileService;
        private readonly DBMongo _dbMongo;

        public ProductService(CategoryService categoryService, DBMongo dBMongo, FileService fileService)
        {
            _dbMongo = dBMongo;
            _productsCollection = _dbMongo.GetCollection<Product>("products");
            _categoryService = categoryService;
            _fileService = fileService;
        }

        public int AddProduct(Product product)
        {
            try
            {
                // 1. Kategori var mı kontrolü
                bool categoryExists = _categoryService.CategoryExists(product.CategoryId);
                if (!categoryExists)
                {
                    string errorMessage = $"Geçersiz Kategori Id: {product.CategoryId}. Ürün eklenemedi.";
                    _fileService.LogError(errorMessage, nameof(AddProduct), DateTime.UtcNow);
                    Console.WriteLine(errorMessage); // Kullanıcıya da gösterdik
                    return -2; // özel kod: kategori yok
                }
                _productsCollection.InsertOne(product);
                return 1;
            }

            catch (Exception ex)
            {
                // Hata mesajını kullanıcıya program.cs'de gösteriyoruz burada sadece log dosyasına yazıyoruz.
                //Log dosyası olduğu için UTC kullandım. Kullanıcıdan bağımsız global bir zaman damgası.
                _fileService.LogError(ex.Message, nameof(AddProduct), DateTime.UtcNow);
                return -1;
            }
        }

        public int UpdateProduct(Product updatedProduct)
        {
            try
            {
                // Kategori kontrolü
                if (!_categoryService.CategoryExists(updatedProduct.CategoryId))
                {
                    _fileService.LogError("CategoryId bulunamadı.", nameof(UpdateProduct), DateTime.UtcNow);
                    return -2;
                }

                // Güncellenecek ürünü bul ve değiştir
                var filter = Builders<Product>.Filter.Eq(p => p.Id, updatedProduct.Id);
                var update = Builders<Product>.Update
                    .Set(p => p.Name, updatedProduct.Name)
                    .Set(p => p.Description, updatedProduct.Description)
                    .Set(p => p.Price, updatedProduct.Price)
                    .Set(p => p.Stock, updatedProduct.Stock)
                    .Set(p => p.CategoryId, updatedProduct.CategoryId);

                var result = _productsCollection.UpdateOne(filter, update);

                if (result.ModifiedCount == 0)
                {
                    Console.WriteLine("Belirtilen ID ile eşleşen bir ürün bulunamadı.");
                    return 0;
                }

                return 1;
            }
            catch (Exception ex)
            {
                _fileService.LogError(ex.Message, nameof(UpdateProduct), DateTime.UtcNow);
                return -1;
            }
        }

        public int DeleteProduct(string productId)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq(p => p.Id, productId);
                var result = _productsCollection.DeleteOne(filter);
                return 1;
            }
            catch (Exception ex)
            {
                _fileService.LogError(ex.Message, nameof(DeleteProduct), DateTime.UtcNow);
                return 0;
            }
        }

        public Product? GetProductById(string productId)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq(p => p.Id, productId);
                var product = _productsCollection.Find(filter).FirstOrDefault();
                return product;
            }
            catch (Exception ex)
            {
                _fileService.LogError(ex.Message, nameof(GetProductById), DateTime.UtcNow);
                return null;
            }
        }

        public List<Product> GetProducts(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;

                return _productsCollection.Find(_ => true).Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToList();
            }
            catch (Exception ex)
            {
                _fileService.LogError(ex.Message, nameof(GetProducts), DateTime.UtcNow);
                return new List<Product>();
            }
        }

        public List<Product> GetSearchProducts(string keyword)
        {
            try
            {
                var filter = Builders<Product>.Filter.Or(
                    Builders<Product>.Filter.Regex(p => p.Name, new BsonRegularExpression(keyword, "i")),
                    Builders<Product>.Filter.Regex(p => p.Description, new BsonRegularExpression(keyword, "i"))
                );
                return _productsCollection.Find(filter).ToList();
            }
            catch (Exception ex)
            {
                _fileService.LogError(ex.Message, nameof(GetSearchProducts), DateTime.UtcNow);
                return new List<Product>();
            }
        }

        public List<Product> FilterProducts(decimal? minFiyat = null, decimal? maxFiyat = null, string? kategoriId = null)
        {
            try
            {
                var builder = Builders<Product>.Filter; //FilterDefinitionBuilder nesnesi
                var filter = Builders<Product>.Filter.Empty; //boş filter nesnesi
                if(minFiyat.HasValue)
                {
                    filter = builder.And(filter, builder.Gte(p => p.Price, minFiyat.Value));
                }
                if(maxFiyat.HasValue)
                {
                    filter = builder.And(filter, builder.Lte(p => p.Price, maxFiyat.Value));
                }
                if(!string.IsNullOrWhiteSpace(kategoriId))
                {
                    filter = builder.And(filter, builder.Eq(p => p.CategoryId, kategoriId));
                }
                return _productsCollection.Find(filter).ToList();
            }
            catch (Exception ex)
            {
                _fileService.LogError(ex.Message, nameof(FilterProducts), DateTime.UtcNow);
                return new List<Product>();
            }
        }

    }
}