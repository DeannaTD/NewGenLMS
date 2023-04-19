using Microsoft.EntityFrameworkCore;
using NovoePokolenie.Data;
using NovoePokolenie.Helpers;
using NovoePokolenie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Repositories
{
    public class StudentRepository : BaseRepository<NPUser>
    {
        public StudentRepository(NewGenContext context) : base(context) { }

        public override async Task<List<NPUser>> GetCollectionAsync()
        {
            return await _dbSet.Where(x => x.StatusId == (int)ActivityStatus.Active || x.StatusId == (int)ActivityStatus.Blocked).ToListAsync();
        }

        public async Task<List<NPUser>> GetAllStatusStudentsAsync()
        {
            return await _dbSet.Where(x => x.StatusId != (int)ActivityStatus.Staff).ToListAsync();
        }

        public async Task<List<NPUser>> GetAllStudentsAsync()
        {
            return await _dbSet.Where(x => x.StatusId == (int)ActivityStatus.Active || x.StatusId == (int)ActivityStatus.Blocked).ToListAsync();
        }

        public override async ValueTask<NPUser> GetByIdAsync(object id)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.Id == id.ToString());
        }

        public async Task<List<NPUser>> GetCollectionWithAttendance()
        {
            return await _dbSet
                            .Where(x => x.StatusId == 3 || x.StatusId == 5)
                            .Include(s => s.Attendances)
                            .ToListAsync();
        }
        public async Task<List<NPUser>> GetCollectionWithPayments()
        {
            return await _dbSet
                            .Where(x => x.StatusId == 3 || x.StatusId == 5)
                            .Include(s => s.PaymentPeriods)
                            .ToListAsync();
        }
    }
}
