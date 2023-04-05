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
    public class AttendanceController : Controller
    {
        AttendanceService _attendanceService;
        GroupService _groupService;
        StudentService _studentService;
        BranchService _branchService;

        public AttendanceController(AttendanceService attendanceService, GroupService groupService, StudentService studentService, BranchService branchService)
        {
            _attendanceService = attendanceService;
            _groupService = groupService;
            _studentService = studentService;
            _branchService = branchService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task CreateAttendanceA()
        {
        }

        public async Task<List<DateTime>> CheckAndCreate(int groupId)
        {
            List<NPUser> _students = await _studentService.GetStudentsAndAttendances(groupId);
            (DayOfWeek, DayOfWeek) _daysOfWeek = await _groupService.GetDaysOfWeekByIdAsync(groupId);
            List<DateTime> _dates = StHel.GetTimeTableDays(DateTime.Now.Year, DateTime.Now.Month, _daysOfWeek);
            foreach (var student in _students)
            {
                foreach (var date in _dates)
                {
                    if (student.Attendances.Where(a => a.Date == date).Count() == 0)
                    {
                        await _attendanceService.Create(student.Id, date);
                    }
                }
            }
            return _dates;
        }

        public async Task<IActionResult> GroupAttendanceNew(int groupId, DateTime date, bool monthly = false)
        {
            //todo - manager monthly issue
            List<NPUser> students = await _studentService.GetStudentsAndAttendances(groupId);
            //ID, NAME, USERNAME, CURRENT PROJECT ID
            List<MentorViewModel> model = new List<MentorViewModel>();
            //найти прошлый понедельник
            DateTime end;
            if (!monthly)
            {
                date = new DateTime(date.Year, date.Month, date.Day);

                while (date.DayOfWeek != DayOfWeek.Monday)
                    date = date.AddDays(-1);

                end = date.AddDays(7);
            }
            else
            {
                date = new DateTime(date.Year, date.Month, 1);
                end = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            }
            foreach (NPUser student in students)
            {
                model.Add(new MentorViewModel()
                {
                    StudentId = student.Id,
                    StudentName = student.FirstName + " " + student.Lastname,
                    StudentLogin = student.UserName,
                    ProjectId = student.CurrentProjectId ?? 0,
                    Attendances = student.Attendances.Where(x => x.Date >= date && x.Date <= end).ToList()
                });
            }
            ViewBag.Date = date;
            ViewBag.groupId = groupId;
            ViewBag.Monthly = monthly;
            return View("GroupAttendance", model);
        }
        public async Task<IActionResult> GroupAttendance(int groupId, int month, int year)
        {
            //if (groupId == -1) return PartialView(new List<AttendanceViewModel>());

            List<NPUser> _students = await _studentService.GetStudentsAndAttendances(groupId);

            List<AttendanceViewModel> model = new List<AttendanceViewModel>();
            foreach (NPUser student in _students)
            {
                model.Add(new AttendanceViewModel()
                {
                    Student = student,
                    Attendances = student.Attendances.Where(a => a.Date.Month == month && a.Date.Year == year).OrderBy(x => x.Date.Day).ToList()
                });

                if (model.Last().Attendances == null) model.Last().Attendances = new List<Attendance>();
            }

            ViewBag.Date = new DateTime(year, month, 1);
            return View(model);
        }

        public async Task CheckAttendance(string AttendanceId, int AttendanceState)
        {
            await _attendanceService.CheckAttendance(AttendanceId, (AttendanceStatus)AttendanceState);
        }

        public async Task CreateCheckedAttendance(string StudentId, DateTime AttendanceDate, int AttendanceState)
        {
            DateTime date = AttendanceDate;
            string attendanceId = await _attendanceService.AttendanceExists(StudentId, date);

            if (attendanceId == "")
            {
                await _attendanceService.Create(StudentId, date, (AttendanceStatus)AttendanceState);
            }
            else
            {
                await _attendanceService.CheckAttendance(attendanceId, (AttendanceStatus)AttendanceState);
            }
        }

        public async Task<IActionResult> StudentAttendance(string studentId)
        {
            List<Attendance> model = await _attendanceService.GetAttendances(studentId);
            for (int i = 0; i < model.Count; i++)
            {
                var atts = model.FindAll(attendance => attendance.Date == model[i].Date);
                if (atts.Count > 1)
                {
                    for (int j = atts.Count - 1; j > 0; j--)
                    {
                        await _attendanceService.DeleteAttendanceByIdAsync(atts[j].Id);
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> AttendanceMenu()
        {
            var branches = await _branchService.GetBranchesAsync();
            return View(branches);
        }

        public async Task<IActionResult> AllAtendanceTest()
        {
            //List<Attendance> model = await _attendanceService.GetAllAtendances();
            //model.Sort((x,y) => x.UserId.CompareTo(y.UserId));
            //return View("Showcase", model);
            List<Attendance> model = await _attendanceService.GetAttendances("07b9887a-9740-4aa6-8b57-505715a15923");
            model.Sort((x, y) => x.Date.CompareTo(y.Date));
            return View("Showcase", model);
        }
    }
}
