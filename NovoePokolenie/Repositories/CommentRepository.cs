using NovoePokolenie.Data;
using NovoePokolenie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Repositories
{
    public class CommentRepository : BaseRepository<Comment>
    {
        public CommentRepository(NewGenContext context) : base(context) { }
    }
}
