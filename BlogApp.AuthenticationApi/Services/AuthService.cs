using Ardalis.Result;
using BlogApp.AuthenticationApi.DTOs;
using BlogApp.AuthenticationApi.Entities;
using BlogApp.AuthenticationApi.Repositories.Interfaces;
using BlogApp.AuthenticationApi.Services.Interfaces;
using BlogApp.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BlogApp.AuthenticationApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepository, IPasswordHasher<UserEntity> passwordHasher, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<Result<UserEntity>> RegisterAsync(string username, string password)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(username);
            if (existingUser.IsSuccess)
            {
                return Result<UserEntity>.Conflict("Username already taken.");
            }

            var user = new UserEntity
            {
                Username = username,
                PasswordHash = _passwordHasher.HashPassword(null, password),
                Role = "commenter"
            };

            return await _userRepository.CreateAsync(user);
        }

        public async Task<Result<string>> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (!user.IsSuccess)
            {
                return Result.Unauthorized("Invalid username or password.");
            }

            var verifyPassword = _passwordHasher.VerifyHashedPassword(null, user.Value.PasswordHash, password);
            if (verifyPassword == PasswordVerificationResult.Failed)
            {
                return Result.Unauthorized("Invalid username or password.");
            }

            var token = _tokenService.GenerateToken(user.Value);
            return Result.Success(token);
        }
        public async Task<Result> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var userResult = await _userRepository.GetByUsernameAsync(request.Username);
            if (!userResult.IsSuccess)
            {
                return Result.NotFound("User not found.");
            }

            var user = userResult.Value;
            var verifyPassword = _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, request.CurrentPassword);

            if (verifyPassword == PasswordVerificationResult.Failed)
            {
                return Result.Unauthorized("Current password is incorrect.");
            }

            user.PasswordHash = _passwordHasher.HashPassword(null, request.NewPassword);
            await _userRepository.UpdateAsync(user);

            return Result.SuccessWithMessage("Password changed successfully.");
        }
    }
}
