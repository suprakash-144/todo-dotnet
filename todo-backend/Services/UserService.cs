using MongoDB.Driver;
using todo_backend.Data;
using todo_backend.Dto;
using todo_backend.Models;
using todo_backend.Services.Interface;

namespace todo_backend.Services
{
    public class UserService(DbContext dbContext, IPasswordService passwordService , IJwtService jwService , IHttpContextAccessor context) : IUserService
    {
        private readonly DbContext _dbContext = dbContext;
        private readonly IJwtService _jwtService = jwService;
        private readonly IPasswordService _passwordService= passwordService;

        private readonly IHttpContextAccessor _contextAccessor = context;
        public async Task<User?> CreateUser(CreateUserRequest dto)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Email, dto.Email);
            var exist = await _dbContext.Users.Find(filter).FirstOrDefaultAsync();

            if (exist != null)
            {
                throw new Exception("User Already exist");
            }
            string HashedPassword = _passwordService.HashedPassword(dto.Password);
            
            User newuser = new() { Email = dto.Email,Name= dto.Name , Password=HashedPassword }; 
            await _dbContext.Users.InsertOneAsync(newuser);
            return newuser;
        }

        public async Task<LoginUserResponse?> LoginUser(LoginUserRequest dto)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Email, dto.Email);
            var exist = await _dbContext.Users.Find(filter).FirstOrDefaultAsync();
            if (exist == null || !_passwordService.VerifyPassowrd(dto.Password , exist.Password))
            {
                return null;
            }
            else
            {
                string token = _jwtService.GenerateToken(exist.Id);
                string Refreshtoken =  _jwtService.GenerateRefreshToken(exist.Id);
                //update the token in the database
                //exist.Token= Refreshtoken;
                var updateDef = Builders<User>.Update.Set(x=>x.Token, Refreshtoken);
                await _dbContext.Users.UpdateOneAsync(filter, updateDef);
                _contextAccessor.HttpContext.Response.Cookies.Append("token", Refreshtoken, new CookieOptions {
                    SameSite = SameSiteMode.None,
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(3),
                    Secure = true
                });
                LoginUserResponse response = new() {Token = token , Id = exist.Id };
                return response;
            }
        }
        public String HashedPassword(String Password)
        {
            return Password;
        }
        public async Task<String?> HandleRefresh(String Id)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Token, Id);
            var exist = await _dbContext.Users.Find(filter).FirstOrDefaultAsync();
            if (exist == null)
            {
                return null;
            }
            bool validtoken = _jwtService.ValidateToken(exist.Token);
            if (validtoken) { 
                string token = _jwtService.GenerateToken(exist.Id);
                return token;
            }
            return null;
        }
    }
}
