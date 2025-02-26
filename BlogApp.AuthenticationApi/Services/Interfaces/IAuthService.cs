using Ardalis.Result;
using BlogApp.AuthenticationApi.DTOs;
using BlogApp.AuthenticationApi.Entities;
using System.Threading.Tasks;

namespace BlogApp.AuthenticationApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<UserEntity>> RegisterAsync(string username, string password);
        Task<Result<string>> LoginAsync(string username, string password);
        Task<Result> ChangePasswordAsync(ChangePasswordRequest request);
    }
}