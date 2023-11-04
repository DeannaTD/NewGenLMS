using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NovoePokolenie.Helpers;
using NovoePokolenie.Models;
using NovoePokolenie.Services;
using NovoePokolenie.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Controllers
{
    public class StudentController : Controller
    {
        StudentService _studentService;
        StatusHistoryService _statusHistoryService;
        ProjectService _projectService;
        LeadService _leadService;
        UserManager<NPUser> _userManager;

        public StudentController(StudentService studentService, StatusHistoryService statusHistoryService, ProjectService projectService, LeadService leadService, UserManager<NPUser> userManager)
        {
            _studentService = studentService;
            _statusHistoryService = statusHistoryService;
            _projectService = projectService;
            _leadService = leadService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var student = await _userManager.GetUserAsync(User);
            var project = await _projectService.GetProjectAsync(student.CurrentProjectId ?? 0);
            ViewBag.Path = project.ProjectLink;
            return View();
        }

        public async Task<IActionResult> Archive()
        {
            //todo: remove comment if GetByStatus works
            //var archivedStudents = await _studentService.GetArchivedAsync();
            var archivedStudents = await _studentService.GetByStatus(ActivityStatus.Archive);
            archivedStudents.Sort((x, y) => x.Lastname.CompareTo(y.Lastname));
            return View("NotActiveUsers", archivedStudents);
        }

        public async Task<IActionResult> BlockList()
        {
            List<NPUser> notActiveusers = await _studentService.GetStudentsByStatus(ActivityStatus.Blocked);
            notActiveusers.Sort((x, y) => x.Lastname.CompareTo(y.Lastname));
            return View("NotActiveUsers", notActiveusers);
        }

        public async Task<IActionResult> NotActiveUsers()
        {
            List<NPUser> notActiveusers = await _studentService.GetStudentsByStatus(ActivityStatus.Blocked);
            notActiveusers.AddRange(await _studentService.GetStudentsByStatus(ActivityStatus.Archive));

            return View(notActiveusers);
        }

        public async Task Block(string studentId)
        {
            await _studentService.ChageStudentStatus(studentId, ActivityStatus.Blocked);
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUser(string studentId, int groupId)
        {
            NPUser student = await _studentService.GetStudentById(studentId);
            if(student.StatusId != (int)ActivityStatus.Active)
                await _studentService.ChageStudentStatus(studentId, ActivityStatus.Active);
            student.GroupId = groupId;
            await _userManager.UpdateAsync(student);
            return Ok();
        }

        [HttpGet]
        public IActionResult Create(int groupId)
        {
            StudentRegistrationViewModel model = new StudentRegistrationViewModel()
            {
                GroupId = groupId
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> StudentDetail(string StudentId)
        {
            var student = await _studentService.GetStudentById(StudentId);
            var model = new StudentCardViewModel(student, new List<Group>());
            var level = await _studentService.GetStudentLevelByProjectId(student.CurrentProjectId ?? 0);
            model.Level = level.Name;
            model.LevelId = level.Id;
            model.Group = await _studentService.GetStudentGroupNameAsync(student.GroupId ?? 0);
            model.MentorId = await _studentService.GetGroupMentorIdAsync(student.GroupId ?? 0);
            ViewBag.UserName = student.UserName;
            return PartialView(model);
        }

        [HttpGet]
        public async Task<IActionResult> TrialDetail(string trialId)
        {
            Lead trial = await _leadService.GetLeadByIdAsync(trialId);
            StudentCardViewModel model = new StudentCardViewModel(trial);
            Level level = await _studentService.GetStudentLevelById((int)DefaultValues.TrialLevel);
            model.Level = level.Name;
            model.LevelId = level.Id;
            model.Group = await _studentService.GetStudentGroupNameAsync(trial.GroupId ?? 0);
            model.MentorId = await _studentService.GetGroupMentorIdAsync(trial.GroupId);
            ViewBag.UserName = "student";
            return PartialView("StudentDetail", model);
        }

        public async Task<IActionResult> GroupStudents(int groupId)
        {
            var students = await _studentService.GetStudentsByGroupIdAsync(groupId);
            var trials = await _leadService.GetTrials(groupId);
            List<StudentCardViewModel> studentCards = new List<StudentCardViewModel>();
            foreach (var student in students)
            {
                studentCards.Add(new StudentCardViewModel(student, new List<Group>()));
                studentCards.Last().Level = (await _studentService.GetStudentLevelByProjectId(student.CurrentProjectId ?? 0)).Name;
                studentCards.Last().Group = await _studentService.GetStudentGroupNameAsync(student.GroupId ?? 0);
                studentCards.Last().MentorName = await _studentService.GetStudentMentorNameAsync(student.GroupId);
            }
            foreach (var trial in trials)
            {
                studentCards.Add(new StudentCardViewModel(trial));
                studentCards.Last().Level = (await _studentService.GetStudentLevelByProjectId(0)).Name;
                studentCards.Last().Group = await _studentService.GetStudentGroupNameAsync(trial.GroupId ?? 0);
                studentCards.Last().MentorName = await _studentService.GetStudentMentorNameAsync(trial.GroupId);
            }
            return PartialView("ManagerStudentList", studentCards);
        }

        public async Task<IActionResult> Trial()
        {
            List<Lead> trials = await _leadService.GetTrials();
            List<StudentCardViewModel> trialsCards = new List<StudentCardViewModel>();
            foreach (var trial in trials)
            {
                trialsCards.Add(new StudentCardViewModel(trial));
                trialsCards.Last().Level = (await _studentService.GetStudentLevelByProjectId(0)).Name;
                trialsCards.Last().Group = await _studentService.GetStudentGroupNameAsync(trial.GroupId ?? 0);
                trialsCards.Last().MentorName = await _studentService.GetStudentMentorNameAsync(trial.GroupId);
            }
            return View("TrialList", trialsCards);
        }

        public async Task<IActionResult> MakeArchived(string studentId, int? groupId = null)
        {
            //StatusHistory statusRecord = new StatusHistory()
            //{
            //    UserId = studentId,
            //    FromStatusId = (await _studentService.GetStudentById(studentId)).StatusId,
            //    ToStatusId = (int)Helpers.ActivityStatus.Archive,
            //    StatusChanged = DateTime.Now
            //};
            await _studentService.ChageStudentStatus(studentId, ActivityStatus.Archive);
            if(groupId != null)
            {
                return RedirectToAction("Edit", "Group", new { id = groupId });
            }
            else
            {
                return Ok();
            }
            //TODO: проверить что не так с созданием статуса
            //await _statusHistoryService.CreateRecordAsync(statusRecord);
        }

        public async Task<IActionResult> LessonDetail(string StudentId)
        {
            var student = await _studentService.GetStudentById(StudentId);
            var project = await _projectService.GetProjectAsync(student.CurrentProjectId ?? 0);
            var level = await _studentService.GetStudentLevelByProjectId(project.Id);
            ViewBag.ProjectId = project.Id;
            ViewBag.LevelId = level.Id;
            ViewBag.StudentId = student.Id;
            return PartialView();
        }

        public async Task<IActionResult> LessonDetailSubmit(StudentLessonViewModel model)
        {
            var student = await _studentService.GetStudentById(model.StudentId);
            var level = await _studentService.GetStudentLevelByProjectId(student.CurrentProjectId ?? 1);
            Project project;
            if (level.Id == model.LevelId)
            {
                await _studentService.ChangeStudentProjectAsync(model.StudentId, model.ProjectId);
                project = await _projectService.GetProjectAsync(model.ProjectId);
            }
            else
            {
                level = await _studentService.GetStudentLevelById(model.LevelId);
                project = await _projectService.GetProjectByLevelIdAsync(model.LevelId);
                await _studentService.ChangeStudentProjectAsync(model.StudentId, project.Id);
            }

            ViewBag.ProjectId = project.Id;
            ViewBag.LevelId = level.Id;
            ViewBag.StudentId = student.Id;
            return PartialView("LessonDetail");
        }

        public async Task<IActionResult> Lesson(string studentId)
        {
            NPUser student = await _studentService.GetStudentById(studentId);
            Project project = await _projectService.GetProjectAsync(student.CurrentProjectId ?? 1);

            ViewBag.MentorName = await _studentService.GetStudentMentorNameAsync(student.GroupId);
            ViewBag.ProjectName = project.Name;
            ViewBag.ProjectLink = project.ProjectLink;
            ViewBag.StudentId = student.Id;
            ViewBag.ProjectId = project.Id;

            return View();
        }


        [HttpPost]
        public async Task SaveStudent(StudentCardViewModel model)
        {
            await _studentService.ChangeStudentDetails(model);
            return;
        }

        [HttpPost]
        public async Task SaveTrial(StudentCardViewModel model)
        {
            await _leadService.ChangeTrialDetails(model);
            return;
        }

        public async Task<IActionResult> GroupLevelsProjects(int groupId)
        {
            var students = await _studentService.GetStudentsByGroupIdAsync(groupId);
            return PartialView(students);
        }

        [HttpGet]
        public IActionResult CreateLead(int groupId)
        {
            ViewBag.GroupId = groupId;
            return View();
        }

        [HttpPost]
        public async Task CreateLead(Lead lead)
        {
            if (lead is null) return;
            await _leadService.CreateAsync(lead);
        }
    }
}
