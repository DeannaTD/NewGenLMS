using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovoePokolenie.Services;

namespace NovoePokolenie.Controllers
{
    public class MentorController : Controller
    {
        BranchService _branchService;

        public MentorController(BranchService branchService)
        {
            _branchService = branchService;
        }

        public async Task<IActionResult> AttendanceMenu()
        {
            var branches = await _branchService.GetBranchesAsync();
            return View(branches);
        }

        public async Task<IActionResult> StudentsMenu()
        {
            var branches = await _branchService.GetBranchesAsync();
            return View(branches);
        }
        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> StudentDetail(int BranchId)
        //{

        //}
    }
}
