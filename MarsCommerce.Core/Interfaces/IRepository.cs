using MarsCommerce.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarsCommerce.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetAsync(int? id);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter);
        Task<List<T>> GetAllByAsync(Expression<Func<T,bool>> filter);
        Task<T> AddAsync(T entity);
        Task<List<T>> AddRangeAsync(List<T> entities);
        Task<T> UpdateAsync(T entity);
        Task<List<T>> UpdateRangeAsync(List<T> entities);
        Task<T> DeleteAsync(T entity);
        Task<T> DeleteCartAsync(int id);
    }
}
