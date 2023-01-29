using Microsoft.EntityFrameworkCore;
using NovoePokolenie.Data;
using NovoePokolenie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Repositories
{
    public class LevelRepository : BaseRepository<Level>
    {
        public LevelRepository(NewGenContext context) : base(context) { }
        public async override Task<List<Level>> GetCollectionAsync()
        {
            return await _dbSet.Include(x => x.Projects).ToListAsync();
        }
    }
}
