using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Models
{
    public class StatusHistory
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public int FromStatusId { get; set; }
        public int ToStatusId { get; set; }
        public DateTime StatusChanged { get; set; }
        public string UserChangedId { get; set; }

        public virtual NPUser User { get; set; }
    }
}
