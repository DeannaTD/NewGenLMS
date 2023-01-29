using NovoePokolenie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.ViewModels
{
    public class AttendanceViewModel
    {
        public NPUser Student { get; set; }
        public List<Attendance> Attendances { get; set; }
    }
}
