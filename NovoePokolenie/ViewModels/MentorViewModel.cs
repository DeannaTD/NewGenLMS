using NovoePokolenie.Models;
using System.Collections.Generic;

namespace NovoePokolenie.ViewModels
{
    public class MentorViewModel
    {
        public string StudentId { get; set; }
        public string StudentName { get; set;}
        public string StudentLogin { get; set; }
        public int ProjectId { get; set; }
        public List<Attendance> Attendances { get; set; }
    }
}
