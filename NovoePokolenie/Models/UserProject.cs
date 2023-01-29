using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Models
{
    public class UserProject
    {
        [Key]
        public string Id { get; set; }
        public DateTime StartedDate { get; set; }
        public DateTime FinishedDate { get; set; }
        public string CompletedLink { get; set; }

        public string UserId { get; set; }
        public int ProjectId { get; set; }
        public virtual NPUser User { get; set; }
        public virtual Project Project { get; set; }
    }
}
