using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Common.Interfaces
{
    interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetAsync(int id);
        Task<T?> GetByAsync(Expression<Func<T, bool>> predicate);
        Task<Responses.Response> CreateAsync(T entity);
        Task<Responses.Response> UpdateAsync(T entity);
        Task<Responses.Response> DeleteAsync(int id);
    }
}