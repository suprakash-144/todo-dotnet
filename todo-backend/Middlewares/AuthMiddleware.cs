
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace todo_backend.Middlewares
{
    public class AuthMiddleware(IConfiguration config) : IMiddleware
    {
        private readonly IConfiguration _config = config;
        public async Task InvokeAsync(HttpContext context, RequestDelegate next )
        {



        var scheme = "Bearer";
            var tokenWithScheme = context.Request.Headers[HeaderNames.Authorization].ToString();
            if (!String.IsNullOrEmpty(tokenWithScheme))
            {
            string token = tokenWithScheme[$"{scheme} ".Length..].Trim();
                if (!String.IsNullOrEmpty(token))
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var issuer = _config["JwtSettings:Issuer"];
                    var audience = _config["JwtSettings:Audience"];
                    var key = Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]);

                        tokenHandler.ValidateToken(token, new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidIssuer = issuer,
                            ValidAudience = audience,
                        }, out SecurityToken validatedToken);

                        var jwtToken = validatedToken as JwtSecurityToken;
                        context.User = (System.Security.Claims.ClaimsPrincipal)jwtToken.Claims;
                        await next(context);
                    
                }
                else
                {

                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Token Not Attached");
                }
            }
            else
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Headers Not Provided");
            }
        }
    }
}
