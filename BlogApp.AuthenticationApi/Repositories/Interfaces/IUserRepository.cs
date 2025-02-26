using BlogApp.Common.Interfaces;
using BlogApp.AuthenticationApi.Entities;
using Ardalis.Result;

namespace BlogApp.AuthenticationApi.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<UserEntity>
    {
        Task<Result<UserEntity>> GetByUsernameAsync(string username);
    }
}
