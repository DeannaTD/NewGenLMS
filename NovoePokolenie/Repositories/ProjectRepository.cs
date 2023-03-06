using Microsoft.EntityFrameworkCore;
using NovoePokolenie.Data;
using NovoePokolenie.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Repositories
{
    public class ProjectRepository : BaseRepository<Project>
    {
        public ProjectRepository(NewGenContext context) : base(context) { }

        public async Task<int> GetFirstProjectIdAsync(int LevelId)
        {
            return (await _dbSet.FirstOrDefaultAsync(p => p.LevelId == LevelId)).Id;
        }

        public async Task<Project> GetByIdAsync(int ProjectId)
        {
            return await _dbSet.Where(p => p.Id == ProjectId)
                                .Include(p => p.Level)
                                .FirstOrDefaultAsync();
        }

        public override async Task<List<Project>> GetCollectionAsync()
        {
            return await _dbSet.Include(p => p.Level).ToListAsync();
        }
    }
}
