using MongoDB.Driver;
using todo_backend.Data;
using todo_backend.Dto;
using todo_backend.Models;
using todo_backend.Services.Interface;

namespace todo_backend.Services
{
    public class TodoService(DbContext dbContext) : ITodoService
    {
        private readonly IMongoCollection<Todo> _dbContext = dbContext.Todos;

        public async Task<Todo> CreateTodo(CreateTodoRequest dto, string userId)
        {
            Todo newtodo = new() { Title = dto.Title, Description = dto.Description, By = userId, Completion=false };
            await _dbContext.InsertOneAsync(newtodo);
            
            return newtodo;
        }

        public async Task<Todo> DeleteTodo(String Id)
        {
            var filter = Builders<Todo>.Filter.Eq(x => x.Id, Id);
            return await _dbContext.FindOneAndDeleteAsync(filter);
        }

        public async Task<IEnumerable<Todo>> GetTodos(string userId)
        {

            var filter = Builders<Todo>.Filter.Eq(x => x.By, userId);
            return await _dbContext.Find(filter).ToListAsync();
        }

        public async Task<Todo> UpdateTodo(String Id, UpdateTodoRequest dto)
        {
            var filter = Builders<Todo>.Filter.Eq(x => x.Id, Id);
            var update = Builders<Todo>.Update.Set(x => x.Completion,dto.Completion);
            var result = await _dbContext.FindOneAndUpdateAsync(filter ,update);
            result.Completion = dto.Completion;
            return result; 
        }
    }
}
