using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NovoePokolenie.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.ViewModels
{
    public class BranchViewModel
    {
        public int Id { get; set; }
        public string BranchName { get; set; }
        public List<Group> Groups { get; set; }
    }
}
