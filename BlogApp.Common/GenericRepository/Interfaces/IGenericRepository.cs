using Ardalis.Result;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BlogApp.Common.GenericRepository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<Result<IEnumerable<T>>> GetAllAsync();
        Task<Result<T>> GetAsync(int id);
        Task<Result<T>> GetByAsync(Expression<Func<T, bool>> predicate);
        Task<Result<T>> CreateAsync(T entity);
        Task<Result<T>> UpdateAsync(T entity);
        Task<Result> DeleteAsync(int id);
    }
}
