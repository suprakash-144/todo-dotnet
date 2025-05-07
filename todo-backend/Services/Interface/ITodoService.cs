using todo_backend.Dto;
using todo_backend.Models;

namespace todo_backend.Services.Interface
{
    public interface ITodoService
    {
        Task<IEnumerable<Todo>> GetTodos(string userId);
        Task<Todo> CreateTodo(CreateTodoRequest dto, string userId);
        Task<Todo> UpdateTodo(string Id, UpdateTodoRequest dto);
        Task<Todo>DeleteTodo(string Id);
    }
}
