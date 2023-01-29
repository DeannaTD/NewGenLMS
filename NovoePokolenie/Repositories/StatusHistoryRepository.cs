using NovoePokolenie.Data;
using NovoePokolenie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Repositories
{
    public class StatusHistoryRepository : BaseRepository<StatusHistory>
    {
        public StatusHistoryRepository(NewGenContext context) : base(context) { }
    }
}
