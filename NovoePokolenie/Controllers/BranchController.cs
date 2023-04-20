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
    public class BranchController : Controller
    {
        BranchService _branchService;
        GroupService _groupService;
        public BranchController(BranchService branchService, GroupService groupService)
        {
            _branchService = branchService;
            _groupService = groupService;
        }

        public async Task<IActionResult> Index()
        {
            var branches = await _branchService.GetBranchesAsync();
            return View(branches);
        }

        public async Task<IActionResult> Detail(int BranchId)
        {
            Branch Branch = await _branchService.GetBranchByIdAsync(BranchId);
            List<Group> groups = await _groupService.GetGroupsByBranchIdAsync(BranchId);
            groups.Sort((g1, g2) => StHel.TimeTableToString(g1.TimeTable).CompareTo(StHel.TimeTableToString(g2.TimeTable)));
            return PartialView("GroupsAttendanceList", new BranchViewModel() { Id = Branch.Id, BranchName = Branch.Name, Groups = groups });
        }

        //Is not used anymore
        public async Task<IActionResult> MentorLessonDetail(int BranchId)
        {
            Branch Branch = await _branchService.GetBranchByIdAsync(BranchId);
            List<Group> groups = await _groupService.GetGroupsByBranchIdAsync(BranchId);
            groups.Sort((g1, g2) => StHel.TimeTableToString(g1.TimeTable).CompareTo(StHel.TimeTableToString(g2.TimeTable)));
            return PartialView("GroupsLessonList", new BranchViewModel() { Id = Branch.Id, BranchName = Branch.Name, Groups = groups });
        }

        public async Task<IActionResult> PaymentDetail(int BranchId)
        {
            Branch Branch = await _branchService.GetBranchByIdAsync(BranchId);
            List<Group> groups = await _groupService.GetGroupsByBranchIdAsync(BranchId);
            groups.Sort((g1, g2) => StHel.TimeTableToString(g1.TimeTable).CompareTo(StHel.TimeTableToString(g2.TimeTable)));
            return PartialView("PaymentsList", new BranchViewModel() { Id = Branch.Id, BranchName = Branch.Name, Groups = groups });
        }

        public async Task<IActionResult> GroupBranchDetiles(int BranchId)
        {
            Branch Branch = await _branchService.GetBranchByIdAsync(BranchId);
            List<Group> groups = await _groupService.GetGroupsByBranchIdAsync(BranchId);
            groups.Sort((g1, g2) => StHel.TimeTableToString(g1.TimeTable).CompareTo(StHel.TimeTableToString(g2.TimeTable)));
            return PartialView("GroupsList", new BranchViewModel() { Id = Branch.Id, BranchName = Branch.Name, Groups = groups });
        }

        public async Task<IActionResult> LessonDetail(int BranchId)
        {
            Branch Branch = await _branchService.GetBranchByIdAsync(BranchId);
            List<Group> groups = await _groupService.GetGroupsByBranchIdAsync(BranchId);
            return PartialView("GroupsStudentList", new BranchViewModel() { Id = Branch.Id, BranchName = Branch.Name, Groups = groups });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //TODO: errors messages
        [HttpPost]
        public async Task<IActionResult> Create(Branch branch)
        {
            if (ModelState.IsValid)
            {
                bool Result = await _branchService.CreateAsync(branch);
                if (Result) return RedirectToAction("Create");
            }
            return RedirectToAction("ErrorCreate");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var branch = await _branchService.GetBranchByIdAsync(id);
            return View(branch);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Branch branch)
        {
            await _branchService.UpdateBranch(branch);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditGroups(int id)
        {
            List<Group> groups = await _groupService.GetGroupsByBranchIdAsync(id);
            var branch = await _branchService.GetBranchByIdAsync(id);
            ViewBag.BranchName = branch.Name;
            return View(groups);
        }

        public async Task<IActionResult> ChartPieTest()
        {
            var brs = await _branchService.GetBranchesAsync();
            var branches = new List<Branch>();
            foreach(var branch in brs)
            {
                branches.Add(await _branchService.GetBranchWithGroupsAsync(branch.Id));
            }

            Dictionary<string, int> data = new Dictionary<string, int>();
            foreach(var branch in branches)
            {
                int count = 0;
                foreach(var group in branch.Groups)
                    count += await _groupService.GroupStudentsCount(group.Id);
                data.Add(branch.Name, count);
            }
            return View(data);
        }
    }
}
