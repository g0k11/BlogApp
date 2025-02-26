using BlogApp.AuthenticationApi.Entities;
using Ardalis.Result;
using BlogApp.Common.GenericRepository.Interfaces;

namespace BlogApp.AuthenticationApi.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<UserEntity>
    {
        Task<Result<UserEntity>> GetByUsernameAsync(string username);
    }
}
