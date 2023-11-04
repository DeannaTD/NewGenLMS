using Microsoft.AspNetCore.Identity;
using NovoePokolenie.Helpers;
using NovoePokolenie.Models;
using NovoePokolenie.UoW;
using NovoePokolenie.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Services
{
    public class StudentService
    {
        UnitOfWork _unitOfWork;
        UserManager<NPUser> _userManager;

        public StudentService(UnitOfWork unitOfWork, UserManager<NPUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<List<NPUser>> GetStudentsByGroupIdAsync(int groupId)
        {
            var students = await _userManager.GetUsersInRoleAsync("Student");
            return students.Where(s => s.GroupId == groupId).ToList();
        }

        public async Task<List<NPUser>> GetStudentsAndAttendances(int groupId)
        {
            var students = await _unitOfWork.Students.GetCollectionWithAttendance();
            return students.Where(s => s.GroupId == groupId).ToList();
        }

        public async Task<List<NPUser>> GetStudentsAndPayments(int groupId)
        {
            var students = await _unitOfWork.Students.GetCollectionWithPayments();
            return students.Where(s => s.GroupId == groupId).ToList();
        }

        public async Task<List<NPUser>> GetStudentsAndPaymentsByBranch(int branchId)
        {
            var students = await _unitOfWork.Students.GetCollectionWithPayments();
            var groups = (await _unitOfWork.Groups.GetCollectionAsync()).Where(x => x.BranchId == branchId);
            var groupsId = new List<int>();
            foreach (var group in groups) groupsId.Add(group.Id);
            return students.Where(s => groupsId.Contains(s.GroupId ?? 0)).ToList();
        }

        public async Task<List<NPUser>> GetStudentsByBranchAsync(int branchId)
        {
            var students = await _unitOfWork.Students.GetCollectionAsync();
            var groups = (await _unitOfWork.Groups.GetCollectionAsync()).Where(x => x.BranchId == branchId);
            var groupsId = new List<int>();
            foreach (var group in groups) groupsId.Add(group.Id);
            return students.Where(s => groupsId.Contains(s.GroupId ?? 0)).ToList();
        }

        public async Task<NPUser> GetStudentById(string id)
        {
            return await _unitOfWork.Students.GetByIdAsync(id);
        }

        public async Task<List<NPUser>> GetStudentsByStatus(ActivityStatus status)
        {
            var students =  await _unitOfWork.Students.GetAllStatusStudentsAsync();
            return students.Where(student => student.StatusId == (int)status).ToList();
        }

        public async Task<List<NPUser>> GetStudents()
        {
            return await _unitOfWork.Students.GetCollectionAsync();
        }

        public async Task<Level> GetStudentLevelByProjectId(int projectId)
        {
            if (projectId == 0)
            {
                return await _unitOfWork.Levels.GetByIdAsync((int)DefaultValues.TrialLevel);
            }
            Project project = await _unitOfWork.Projects.GetByIdAsync(projectId);
            return await _unitOfWork.Levels.GetByIdAsync(project.LevelId);
        }

        public async Task<Level> GetStudentLevelById(int levelId)
        {
            Level level = await _unitOfWork.Levels.GetByIdAsync(levelId);
            return level;
        }
        public async Task<string> GetStudentGroupNameAsync(int groupId)
        {
            var group = await _unitOfWork.Groups.GetByIdWithTimeTableAsync(groupId);
            return StHel.TimeTableToString(group.TimeTable);
        }

        public async Task<string> GetStudentMentorNameAsync(int? groupId)
        {
            if (groupId == null) return "";
            var group = await _unitOfWork.Groups.GetByIdAsync(groupId);
            var mentor = await _userManager.FindByIdAsync(group.MentorId);
            return mentor.FirstName;
        }

        public async Task<string> GetGroupMentorIdAsync(int? groupId)
        {
            if (groupId == null) return "";
            var group = await _unitOfWork.Groups.GetByIdAsync(groupId);
            return group.MentorId;
        }

        public async Task ChageStudentStatus(string studentId, ActivityStatus status)
        {
            NPUser student = await _unitOfWork.Students.GetByIdAsync(studentId);

            student.StatusId = (int)status;
            student.GroupId = null;
            await _unitOfWork.Save();
        }

        //TODO: replace by GetByStatus
        public async Task<List<NPUser>> GetByStatus(ActivityStatus status)
        {
            var students = await _unitOfWork.Students.GetAllStatusStudentsAsync();
            return students.Where(st => st.StatusId == (int)status).ToList();
        }
        public async Task<List<NPUser>> GetArchivedAsync()
        {
            var students = await _unitOfWork.Students.GetAllStatusStudentsAsync();
            return students.Where(st => st.StatusId == (int)ActivityStatus.Archive).ToList();
        }

        public async Task ChangeStudentDetails(StudentCardViewModel model)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(model.StudentId);
            student.FirstName = model.FirstName;
            student.Lastname = model.LastName;
            student.PhoneNumber = model.PhoneNumber;
            student.StudentPhoneNumber = model.StudentPhoneNumber;
            student.DateOfBirth = model.DateOfBirth;
            student.DateOfTrial = model.DateCreated;
            student.DateStarted = model.DateStarted;
            student.ParentName = model.ParentName;
            student.PaymentCharge = model.PaymentCharge;
            student.Note = model.Note;
            student.GroupId = model.GroupId;
            student.CurrentProjectId = await _unitOfWork.Projects.GetFirstProjectIdAsync(model.LevelId);
            /*
               int StatusId { get; set; }
             */
            await _unitOfWork.Save();
        }

        public async Task ChangeStudentProjectAsync(string studentId, int projectId)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(studentId);
            student.CurrentProjectId = projectId;
            await _unitOfWork.Save();
        }

        public async Task<int> GetStudentsBranchByGrouIdAsync(int groupId)
        {
            Group group = await _unitOfWork.Groups.GetByIdAsync(groupId);
            return group.Id;
        }

    }
}
