using Ardalis.Result;
using BlogApp.AuthenticationApi.Data;
using BlogApp.AuthenticationApi.Entities;
using BlogApp.AuthenticationApi.Repositories.Interfaces;
using BlogApp.AuthenticationApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BlogApp.AuthenticationApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<UserEntity>> CreateAsync(UserEntity entity)
        {
            try
            {
                await _context.Users.AddAsync(entity);
                await _context.SaveChangesAsync();
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                return Result.Error($"Error creating user: {ex.Message}");
            }
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return Result.NotFound("User not found.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<IEnumerable<UserEntity>>> GetAllAsync()
        {
            IEnumerable<UserEntity> users = await _context.Users.ToListAsync();
            return Result.Success(users);
        }

        public async Task<Result<UserEntity>> GetAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user != null ? Result.Success(user) : Result.NotFound("User not found.");
        }

        public async Task<Result<UserEntity>> GetByAsync(Expression<Func<UserEntity, bool>> predicate)
        {
            var user = await _context.Users.FirstOrDefaultAsync(predicate);
            return user != null ? Result.Success(user) : Result.NotFound("User not found.");
        }

        public async Task<Result<UserEntity>> GetByUsernameAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            return user != null ? Result.Success(user) : Result.NotFound("User not found.");
        }

        public async Task<Result<UserEntity>> UpdateAsync(UserEntity entity)
        {
            var existingUser = await _context.Users.FindAsync(entity.Id);
            if (existingUser == null)
            {
                return Result.NotFound("User not found.");
            }

            existingUser.Username = entity.Username;
            existingUser.PasswordHash = entity.PasswordHash;
            existingUser.Role = entity.Role;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            return Result.Success(existingUser);
        }
    }
}
