using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovoePokolenie.Services;

namespace NovoePokolenie.Controllers
{
    public class ManagerController : Controller
    {
        BranchService _branchService;
        StudentService _studentService;

        public ManagerController(BranchService branchService, StudentService studentService)
        {
            _branchService = branchService;
            _studentService = studentService;
        }

        public IActionResult Index()
        {
            if (!User.IsInRole("Manager"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public IActionResult Branches()
        {
            if (!User.IsInRole("Manager"))
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Branch");
        }

        public IActionResult Comments()
        {
            if (!User.IsInRole("Manager"))
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Comment");
        }
        
        public async Task<IActionResult> AttendanceMenu()
        {
            if (!User.IsInRole("Manager"))
            {
                return RedirectToAction("Index", "Home");
            }
            var branches = await _branchService.GetBranchesAsync();
            return View(branches);
        }
        public async Task<IActionResult> PaymentMenu()
        {
            if (!User.IsInRole("Manager"))
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Payment");
        }
        public async Task<IActionResult> UsersMenu()
        {
            return View();
        }

    }
}
