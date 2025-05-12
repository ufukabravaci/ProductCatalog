using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using ProductCatalog.models;
using ProductCatalog.utils;

namespace ProductCatalog.services
{
    public class ProductService
    {
        private readonly IMongoCollection<Product> _productsCollection;
        private readonly string _logFilePath = "error_log.txt";

        private readonly CategoryService _categoryService;
        private readonly DBMongo _dbMongo;

        public ProductService(CategoryService categoryService,DBMongo dBMongo )
        {
            _dbMongo = dBMongo;
            _productsCollection = _dbMongo.GetCollection<Product>("products");
            _categoryService = categoryService;
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
                    LogError(errorMessage, nameof(AddProduct), DateTime.UtcNow);
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
                LogError(ex.Message, nameof(AddProduct), DateTime.UtcNow);
                return -1;
            }
        }

        // Hata oluşan metot, hata mesajı ve zaman damgası olacak şekilde bir log metodu
        private void LogError(string message, string methodName, DateTime timeStamp)
        {
            string logMessage = $"{timeStamp}: {methodName} - {message}";
            File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
        }
    }
}