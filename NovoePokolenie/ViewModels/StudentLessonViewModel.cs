using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.ViewModels
{
    public class StudentLessonViewModel
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public int LevelId { get; set; }
        public int ProjectId { get; set; }
        public string LinkString { get; set; }
    }
}
