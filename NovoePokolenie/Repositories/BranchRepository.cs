using Microsoft.EntityFrameworkCore;
using NovoePokolenie.Data;
using NovoePokolenie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Repositories
{
    public class BranchRepository : BaseRepository<Branch>
    {
        public BranchRepository(NewGenContext context) : base(context) { }

        public async ValueTask<Branch> GetByIdWithGroupsAsync(object id)
        {
            int Id = Convert.ToInt32(id);
            return await _dbSet.Include(b => b.Groups).SingleOrDefaultAsync(g => g.Id == Id);
        }

        public async ValueTask<List<Branch>> GetBranchesWithGroupsAsync()
        {
            return await _dbSet.Include(b => b.Groups).ToListAsync();
        }
    }
}
