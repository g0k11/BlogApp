using BlogApp.AuthenticationApi.Data;
using BlogApp.AuthenticationApi.Entities;
using BlogApp.AuthenticationApi.Repositories.Interfaces;
using BlogApp.AuthenticationApi.Repositories;
using BlogApp.AuthenticationApi.Services;
using BlogApp.AuthenticationApi.Services.Interfaces;
using BlogApp.Common.DependencyInjections;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BlogApp.AuthenticationApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

// **Database Connection**
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// **Authentication & JWT**
builder.Services.AddJWTAuthScheme(builder.Configuration);

// **Dependency Injection**
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
