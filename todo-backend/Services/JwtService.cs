﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using todo_backend.Services.Interface;

namespace todo_backend.Services
{
    public class JwtService(IConfiguration config) : IJwtService
    {

        private readonly IConfiguration _config = config;


        public string GenerateToken(string Id)
        {
            var issuer = _config["JwtSettings:Issuer"];
            var audience = _config["JwtSettings:Audience"];
            var key = _config["JwtSettings:SecretKey"];
            var validity = _config.GetValue<int>("JwtSettings:Valid");
            var expiryTime = DateTime.UtcNow.AddDays(validity);
            var Descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, Id),
                }),
                Issuer = issuer,
                Audience = audience,
                Expires = expiryTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256),
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(Descriptor);
            var accestoken = tokenHandler.WriteToken(securityToken);
            return accestoken;
        }
        public String GenerateRefreshToken(string Id)
        {
            var issuer = _config["JwtSettings:Issuer"];
            var audience = _config["JwtSettings:Audience"];
            var key = _config["JwtSettings:SecretKey"];
            var validity = _config.GetValue<int>("JwtSettings:Valid");
            var expiryTime = DateTime.UtcNow.AddDays(validity + 2);
            var Descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, Id),
                }),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256),
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(Descriptor);
            var accestoken = tokenHandler.WriteToken(securityToken);
            return accestoken;
        }
        public Boolean ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var issuer = _config["JwtSettings:Issuer"];
            var audience = _config["JwtSettings:Audience"];
            var key = Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]);
            try
            {

             tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer=true,
                ValidateAudience=true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidIssuer = issuer,
                ValidAudience = audience,
            }, out SecurityToken validatedToken);

                //var jwtToken = (JwtSecurityToken) validatedToken;
                var jwtToken = validatedToken as JwtSecurityToken;
                return true;
            }
            catch (Exception ex) {

                return false;
            }
        }
    }
}
