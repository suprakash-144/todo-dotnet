using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace todo_backend.Models
{
    public class User
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("name"), BsonRepresentation(BsonType.String), BsonRequired]
        public required string Name { get; set; }
        [BsonElement("email"), BsonRepresentation(BsonType.String), BsonRequired]
        public required string Email { get; set; }
        [BsonElement("password"), BsonRepresentation(BsonType.String), BsonRequired] 
        
        public required string Password { get; set; }
        [BsonElement("token"), BsonRepresentation(BsonType.String)] 
        
        public  string? Token { get; set; }

    }
}
