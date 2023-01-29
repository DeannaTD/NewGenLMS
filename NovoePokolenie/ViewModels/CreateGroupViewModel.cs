using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NovoePokolenie.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.ViewModels
{
    public class CreateGroupViewModel
    {
        [Display(Name = "Преподаватель")]
        [Required]
        public IList<NPUser> Mentors { get; set; }

        [Display(Name = "День #1")]
        [Required]
        public DayOfWeek Day1 { get; set; }

        [Display(Name = "День #2")]
        [Required]
        public DayOfWeek Day2 { get; set; }

        [Display(Name = "Время занятий")]
        [Required]
        public string Time { get; set; }
        
        [HiddenInput]
        [Required]
        public int BranchId { get; set; }

        [HiddenInput]
        [Required]
        public string MentorId { get; set; }
    }
}
