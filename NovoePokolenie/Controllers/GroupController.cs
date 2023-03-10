using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovoePokolenie.Helpers;
using NovoePokolenie.Models;
using NovoePokolenie.Services;
using NovoePokolenie.ViewModels;

namespace NovoePokolenie.Controllers
{
    public class GroupController : Controller
    {
        GroupService _groupService;
        AccountService _accountService;
        TimeTableService _timeTableService;
        public GroupController(GroupService groupService, AccountService accountService, TimeTableService timeTableService)
        {
            _groupService = groupService;
            _accountService = accountService;
            _timeTableService = timeTableService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create(int branchId)
        {
            CreateGroupViewModel model = new CreateGroupViewModel()
            {
                Mentors = await _accountService.GetAllUsersInRole("Mentor"),
                BranchId = branchId,
                MentorId = "0"
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGroupViewModel model)
        {
            TimeTable groupTimeTable = new TimeTable
            {
                Day1 = model.Day1,
                Day2 = model.Day2,
                TimeName = model.Time
            };
            await _timeTableService.CreateAsync(groupTimeTable);
            
            Group group = new Group()
            {
                BranchId = model.BranchId,
                MentorId = model.MentorId,
                TimeTableId = groupTimeTable.Id
            };
            await _groupService.CreateAsync(group);
            
            return RedirectToAction("Index", "Branch");
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Group group = await _groupService.GetByIdAsync(id);
            TimeTable timeTable = await _timeTableService.GetByIdAsync(group.TimeTableId);
            CreateGroupViewModel model = new CreateGroupViewModel()
            {
                Mentors = await _accountService.GetAllUsersInRole("Mentor"),
                BranchId = group.Id,
                MentorId = group.MentorId,
                Day1 = timeTable.Day1,
                Day2 = timeTable.Day2
            };
            ViewBag.TimeName = timeTable.TimeName;
            ViewBag.Id = id;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateGroupViewModel model, int Id)
        {
            Group group = await _groupService.GetByIdAsync(Id);
            await _groupService.EditGroup(Id, model);
            return RedirectToAction("Index", "Branch");
        }

        [HttpPost]
        public async Task<StatusCodeResult> Delete(string groupId)
        {
            int id = 0;
            try
            {
                id = Convert.ToInt32(groupId);
            }
            catch
            {
                return StatusCode(500);
            }
            try
            {
                await _groupService.DeleteGroup(id);
            }
            catch
            {
                return StatusCode(500);
            }
            return Ok();
        }
    }
}
