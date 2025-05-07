using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace todo_backend.Models
{
    public class Todo
    {
        [BsonId]
        [BsonElement("_id"),BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("title"), BsonRepresentation(BsonType.String)]
        public required string Title { get; set; }

        [BsonElement("description"), BsonRepresentation(BsonType.String)]
        public required string Description { get; set; }
        [BsonElement("completion"), BsonRepresentation(BsonType.Boolean)]
        public required bool Completion { get; set; } = false;

        [BsonElement("by"), BsonRepresentation(BsonType.ObjectId)]
        public required string By { get; set; }

        
    }
}
