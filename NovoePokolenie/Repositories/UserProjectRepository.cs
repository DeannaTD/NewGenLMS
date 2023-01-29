using NovoePokolenie.Data;
using NovoePokolenie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Repositories
{
    public class UserProjectRepository : BaseRepository<UserProject>
    {
        public UserProjectRepository(NewGenContext context) : base(context) { }

    }
}
