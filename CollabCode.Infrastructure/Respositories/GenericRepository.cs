using CollabCode.CollabCode.Application.Interfaces.Repositories;
using CollabCode.CollabCode.Infrastructure.Persistense;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CollabCode.CollabCode.Infrastructure.Respositories
{
    public class GenericRepository<T>:IGenericRepository<T> where T:class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<T> AddAsync(T item)
        {
            if (item == null)
                throw new NullReferenceException(" couldnt created");
            await _dbSet.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<T> UpdateAsync(T item)
        {
            if (item == null)
                throw new NullReferenceException("Couldnt update ");
             _dbSet.Update(item);
              await _context.SaveChangesAsync();
            return item;
        }

        public async Task<T> DeleteAsync(T item)
        {
             _dbSet.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
           return  await _dbSet.FirstOrDefaultAsync(predicate);
        }

    }
}
