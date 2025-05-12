

using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductCatalog.models
{
    public class Category
    {

        public Category(string name)
        {
            Name = name;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        [Required]
        public string Name { get; set; } // Benzsersizlik database tarafında index oluşturarak sağlanacak.
    }
}