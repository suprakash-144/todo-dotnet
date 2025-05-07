using MongoDB.Bson;

namespace todo_backend.Validator
{
    public static class MongoValidation
    {
        public static bool IsValidMongoId(string id)
        {
            return ObjectId.TryParse(id, out _);
        }
    }
}
