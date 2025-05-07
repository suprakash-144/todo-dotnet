using todo_backend.Dto;
using todo_backend.Models;

namespace todo_backend.Services.Interface
{
    public interface IUserService
    {
        Task<LoginUserResponse?> LoginUser(LoginUserRequest dto);
        Task<User?> CreateUser(CreateUserRequest dto);
        String HashedPassword(String Password);
        Task<String?> HandleRefresh(String Id);

    }
}
