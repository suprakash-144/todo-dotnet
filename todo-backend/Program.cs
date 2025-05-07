using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using todo_backend;
using todo_backend.Data;
using todo_backend.Exceptions;
using todo_backend.Middlewares;
using todo_backend.Services;
using todo_backend.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Enter Jwt Access Token",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSecurityScheme,Array.Empty<string>() } });
});
//builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
//Jwt Configurations 
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;


}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = true;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])),
        ClockSkew = TimeSpan.Zero

    };
});
builder.Services.AddAuthorization();
builder.Services.AddCors(builder =>
{
    builder.AddPolicy(name: "CorsPolicy", policy =>
        {
            policy.WithOrigins("http://localhost:3000", "https://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials();

        });
});

builder.Services.AddProblemDetails();
 //To  Add external Cors files
//builder.Services.ConfigureCors();


//configuring of Mongodb Server
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("Mongodb"));
builder.Services.AddSingleton<DbContext>();

//Middleware Service 
builder.Services.AddSingleton<AuthMiddleware>();

//Custom servies
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddSingleton<IPasswordService, PasswordService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseExceptionHandler();

app.UseCors("CorsPolicy");
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
//app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/Todo"),
//    customApi =>
//    { customApi.UseMiddleware<AuthMiddleware>(); }
//    );

//app.Map("/api/User/Logout", customApi =>
//{ customApi.UseMiddleware<AuthMiddleware>(); });

app.Run();
