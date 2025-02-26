using BlogApp.AuthenticationApi.Entities;

namespace BlogApp.AuthenticationApi.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(UserEntity user);
    }
}
