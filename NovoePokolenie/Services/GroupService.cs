using NovoePokolenie.Models;
using NovoePokolenie.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NovoePokolenie.ViewModels;

namespace NovoePokolenie.Services
{
    public class GroupService
    {
        private UnitOfWork _unitOfWork;

        public GroupService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(Group group)
        {
            await _unitOfWork.Groups.CreateAsync(group);
            await _unitOfWork.Save();
        }

        public async Task<Group> GetByIdAsync(int Id)
        {
            return await _unitOfWork.Groups.GetByIdAsync(Id);
        }

        public async Task<List<Group>> GetGroupsByBranchIdAsync(int branchId)
        {
            return await _unitOfWork.Groups.GetGroupsWithTimeTableAsync(branchId);
        }

        public async Task<(DayOfWeek, DayOfWeek)> GetDaysOfWeekByIdAsync(int groupId)
        {
            var group = await _unitOfWork.Groups.GetByIdAsync(groupId);
            var timeTable = await _unitOfWork.TimeTables.GetByIdAsync(group.TimeTableId);
            return (timeTable.Day1, timeTable.Day2);
        }

        public async Task<List<Group>> GetGroupsAsync()
        {
            var a = await _unitOfWork.Groups.GetGroupsWithTimeTableAsync();
            a.Sort((x, y) => x.BranchId > y.BranchId ? 1 : x.BranchId < y.BranchId ? -1 : 0);
            return a;
        }

        public async Task DeleteGroup(int groupId)
        {
            Group group = await _unitOfWork.Groups.GetByIdAsync(groupId);
            _unitOfWork.Groups.Delete(group);
            await _unitOfWork.Save();
        }

        public async Task EditGroup(int groupId, CreateGroupViewModel model)
        {
            Group group = await _unitOfWork.Groups.GetByIdAsync(groupId);
            TimeTable timeTable = await _unitOfWork.TimeTables.GetByIdAsync(group.TimeTableId);
            timeTable.Day1 = model.Day1;
            timeTable.Day2 = model.Day2;
            timeTable.TimeName = model.Time;
            group.MentorId = model.MentorId;
            await _unitOfWork.Save();
        }

        public async Task<TimeTable> GetTimeTableAsync(int groupId)
        {
            Group group = await _unitOfWork.Groups.GetByIdAsync(groupId);
            return await _unitOfWork.TimeTables.GetByIdAsync(group.TimeTableId);
        }

        public async Task<int> GroupStudentsCount(int groupId)
        {
            Group group = await _unitOfWork.Groups.GetByIdAsync(groupId);
            return (await _unitOfWork.Students.GetAllStatusStudentsAsync()).Where(student => student.GroupId == groupId).Count();
        }
    }
}
