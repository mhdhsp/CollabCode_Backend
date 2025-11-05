using CollabCode.CollabCode.Infrastructure.Persistense;
using System.Collections;
using System.Linq.Expressions;

namespace CollabCode.CollabCode.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T :class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T item);
        Task<T> UpdateAsync(T item);
        Task<T> DeleteAsync(T item);

        IQueryable<T> Query();

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAllByCondition(Expression<Func<T, bool>> predicate);
        public AppDbContext GetDbContext();
    }
}
