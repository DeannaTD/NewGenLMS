using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateToNotify { get; set; }

        public virtual NPUser User { get; set; }
    }
}
