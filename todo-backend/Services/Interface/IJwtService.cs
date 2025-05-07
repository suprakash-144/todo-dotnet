namespace todo_backend.Services.Interface
{
    public interface IJwtService
    {
        string GenerateToken(string Id);
        string GenerateRefreshToken(string Id);
        Boolean ValidateToken(string token);
    }
}
