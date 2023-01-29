using NovoePokolenie.Models;
using NovoePokolenie.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Services
{
    public class TimeTableService
    {
        private UnitOfWork _unitOfWork;
        public TimeTableService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(TimeTable timeTable)
        {
            await _unitOfWork.TimeTables.CreateAsync(timeTable);
            await _unitOfWork.Save();
        }

        public async Task<TimeTable> GetByIdAsync(int id)
        {
            TimeTable result = await _unitOfWork.TimeTables.GetByIdAsync(id);
            return result;
        }
    }
}
