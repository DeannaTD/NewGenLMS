using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LevelId { get; set; }
        public string ProjectLink { get; set; }

        public int IndexNumber { get; set; }
        public virtual ICollection<UserProject> UserProjects { get; set; }
        public virtual Level Level { get; set; }
    }
}
