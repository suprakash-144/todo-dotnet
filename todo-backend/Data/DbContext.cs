using Microsoft.Extensions.Options;
using MongoDB.Driver;
using todo_backend.Models;

namespace todo_backend.Data
{
    public class DbContext
    {
        public IMongoCollection<User> Users { get; }
        public IMongoCollection<Todo> Todos { get; }


        public DbContext(IOptions<MongoDBSettings> settings)
        {
            // This is given us connection string 
            MongoClient client = new MongoClient(settings.Value.ConnectionURI);

            // Connecting to the database name 
            IMongoDatabase database = client.GetDatabase(settings.Value.DatabaseName);


            // Connecting to the collection name 
            Users = database.GetCollection<User>("User");
            Todos = database.GetCollection<Todo>("Todo");
        }
    }
}
