using Ardalis.Result;
using BlogApp.Common.GenericRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Common.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<Result<IEnumerable<T>>> GetAllAsync()
        {
            try
            {
                var items = await _dbSet.ToListAsync();
<<<<<<< Updated upstream
                return Result.Success(items);
            }
            catch (Exception ex)
            {
                return Result.Error<IEnumerable<T>>(new[] { ex.Message });
=======
                return Result<IEnumerable<T>>.Success(items);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<T>>.Error(ex.Message);
>>>>>>> Stashed changes
            }
        }

        public async Task<Result<T>> GetAsync(int id)
        {
            try
            {
                var item = await _dbSet.FindAsync(id);
<<<<<<< Updated upstream
                return item is null ? Result.NotFound<T>() : Result.Success(item);
            }
            catch (Exception ex)
            {
                return Result.Error<T>(new[] { ex.Message });
=======
                return item is null 
                    ? Result<T>.NotFound() 
                    : Result<T>.Success(item);
            }
            catch (Exception ex)
            {
                return Result<T>.Error(ex.Message);
>>>>>>> Stashed changes
            }
        }

        public async Task<Result<T>> GetByAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var item = await _dbSet.FirstOrDefaultAsync(predicate);
<<<<<<< Updated upstream
                return item is null ? Result.NotFound<T>() : Result.Success(item);
            }
            catch (Exception ex)
            {
                return Result.Error<T>(new[] { ex.Message });
=======
                return item is null 
                    ? Result<T>.NotFound() 
                    : Result<T>.Success(item);
            }
            catch (Exception ex)
            {
                return Result<T>.Error(ex.Message);
>>>>>>> Stashed changes
            }
        }

        public async Task<Result<T>> CreateAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
<<<<<<< Updated upstream
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                return Result.Error<T>(new[] { ex.Message });
=======
                return Result<T>.Success(entity);
            }
            catch (Exception ex)
            {
                return Result<T>.Error(ex.Message);
>>>>>>> Stashed changes
            }
        }

        public async Task<Result<T>> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
<<<<<<< Updated upstream
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                return Result.Error<T>(new[] { ex.Message });
=======
                return Result<T>.Success(entity);
            }
            catch (Exception ex)
            {
                return Result<T>.Error(ex.Message);
>>>>>>> Stashed changes
            }
        }

        public async Task<Result> DeleteAsync(int id)
        {
            try
            {
                var item = await _dbSet.FindAsync(id);
                if (item is null)
                    return Result.NotFound();

                _dbSet.Remove(item);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
<<<<<<< Updated upstream
                return Result.Error(new[] { ex.Message });
=======
                return Result.Error(ex.Message);
>>>>>>> Stashed changes
            }
        }
    }
}
