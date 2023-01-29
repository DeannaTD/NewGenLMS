using Microsoft.EntityFrameworkCore;
using NovoePokolenie.Data;
using NovoePokolenie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Repositories
{
    public class GroupRepository : BaseRepository<Group>
    {
        public GroupRepository(NewGenContext context) : base(context) { }

        public async Task<List<Group>> GetGroupsWithTimeTableAsync(int BranchId)
        {
            return await _dbSet
                            .Where(x => x.BranchId == BranchId)
                            .Include(g => g.TimeTable)
                            .Include(u => u.Users)
                            .ToListAsync();
        }

        public async Task<List<Group>> GetGroupsWithTimeTableAsync()
        {
            return await _dbSet
                            .Include(g => g.TimeTable)
                            .ToListAsync();
        }

        public async Task<Group> GetByIdWithTimeTableAsync(int groupId)
        {
            return await _dbSet.
                            Where(x => x.Id == groupId)
                            .Include(g => g.TimeTable)
                            .FirstOrDefaultAsync();
        }
    }
}
