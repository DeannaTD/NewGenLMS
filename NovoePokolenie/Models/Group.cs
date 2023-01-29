using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Models
{
    public class Group
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int TimeTableId { get; set; }
        public string MentorId { get; set; }

        public virtual ICollection<NPUser> Users { get; set; }
        public virtual TimeTable TimeTable { get; set; }
    }
}
