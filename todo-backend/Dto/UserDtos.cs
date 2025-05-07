
using System.ComponentModel.DataAnnotations;

namespace todo_backend.Dto
{
    public class CreateUserRequest
    {
        public required String Name { get; set; }
        public required String Email { get; set; }
        public required String Password { get; set; }
    }
    public class CreateUserResponse
    {

        public required String Name { get; set; }
        public required String Email { get; set; }
        public required String Password { get; set; }
    }
    public class LoginUserRequest
    {

        public required String Email { get; set; }
        public required String Password { get; set; }
    }
    public class LoginUserResponse
    {

        public required String Id { get; set; }
        public required String Token { get; set; }
    }

}
