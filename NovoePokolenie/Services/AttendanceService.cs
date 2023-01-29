using Microsoft.EntityFrameworkCore.Migrations.Operations;
using NovoePokolenie.Helpers;
using NovoePokolenie.Models;
using NovoePokolenie.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Services
{
    public class AttendanceService
    {
        private UnitOfWork _unitOfWork;
        public AttendanceService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Create(string student_Id, DateTime date, AttendanceStatus status = AttendanceStatus.Unknown)
        {
            Attendance attendance = new Attendance()
            {
                UserId = student_Id,
                Date = date,
                AttendanceStatus = (int)status
            };
            await _unitOfWork.Attendances.CreateAsync(attendance);
            await _unitOfWork.Save();
        }

        public async Task CheckAttendance(string AttendanceId, AttendanceStatus status)
        {
            Attendance attendance = await _unitOfWork.Attendances.GetByIdAsync(AttendanceId);
            attendance.AttendanceStatus = (int)status;
            await _unitOfWork.Save();
        }

        public async Task<List<Attendance>> GetAttendances(string studentId)
        {
            var result = await _unitOfWork.Attendances.GetCollectionAsync();
            return result.Where(x => x.UserId == studentId).ToList();
        }

        public async Task<string> AttendanceExists(string studentId, DateTime date)
        {
            var attendances = await _unitOfWork.Attendances.GetCollectionAsync();
            var result = attendances.Where(x => x.UserId == studentId).ToList().FindAll(att => att.Date.Date.Equals(date.Date));
            return result.Count == 0 ? "" : result[0].Id;
        }

        public async Task DeleteAttendanceByIdAsync(string attendanceId)
        {
            Attendance attendace = await _unitOfWork.Attendances.GetByIdAsync(attendanceId);
            _unitOfWork.Attendances.Delete(attendace);
            await _unitOfWork.Save();
        }

        public async Task<List<Attendance>> GetAllAtendances()
        {
            return await _unitOfWork.Attendances.GetCollectionAsync();
        }
    }
}
