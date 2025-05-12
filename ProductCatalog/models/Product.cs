using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductCatalog.models
{
    public class Product
    {
        public Product(string name, string categoryId)
        {
            Name = name;
            CategoryId = categoryId;
            CreatedAt = DateTime.UtcNow;
        }
        public Product(string name, string description, decimal price, int stock, string categoryId)
        {
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            CategoryId = categoryId;
            CreatedAt = DateTime.UtcNow;
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("name")]
        [Required]
        public string Name { get; set; }
        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("price")]
        [Required]
        [BsonRepresentation(BsonType.Decimal128)]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal Price { get; set; }
        [BsonElement("stock")]
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative number.")]
        public int Stock { get; set; }
        [BsonElement("categoryId")]
        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}