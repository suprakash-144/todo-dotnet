namespace todo_backend.Services.Interface
{
    public interface IPasswordService
    {
        public string HashedPassword(string password);
        public Boolean VerifyPassowrd(string password,string passwordHash);
    }
}
