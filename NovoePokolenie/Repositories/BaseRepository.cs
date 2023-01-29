using Microsoft.EntityFrameworkCore;
using NovoePokolenie.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Repositories
{
    public class BaseRepository<T> : IRepository<T>
        where T : class
    {
        protected NewGenContext _context;
        protected DbSet<T> _dbSet;

        public BaseRepository(NewGenContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task<List<T>> GetCollectionAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual async ValueTask<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }
    }
}
