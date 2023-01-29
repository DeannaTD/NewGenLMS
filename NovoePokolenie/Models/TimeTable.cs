using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Models
{
    public class TimeTable
    {
        public int Id { get; set; }
        public DayOfWeek Day1 { get; set; }
        public DayOfWeek Day2 { get; set; }
        public string TimeName { get; set; }

        //TODO: какая тут должна быть связь?
        
        public virtual ICollection<Group> Groups { get; set; }
    }
}
