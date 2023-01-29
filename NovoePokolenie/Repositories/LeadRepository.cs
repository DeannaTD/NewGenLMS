using NovoePokolenie.Data;
using NovoePokolenie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Repositories
{
    public class LeadRepository : BaseRepository<Lead>
    {
        public LeadRepository(NewGenContext context) : base(context) { }
    }
}
