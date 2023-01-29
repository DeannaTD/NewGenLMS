using Microsoft.EntityFrameworkCore;
using NovoePokolenie.Data;
using NovoePokolenie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Repositories
{
    public class PaymentPeriodRepository : BaseRepository<PaymentPeriod>
    {
        public PaymentPeriodRepository(NewGenContext context) : base(context) { }
        public override async Task<List<PaymentPeriod>> GetCollectionAsync()
        {
            return await _dbSet
                            .Include(x => x.Payments)
                            .ToListAsync();
        }
    }
}
