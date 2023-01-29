using NovoePokolenie.Models;
using NovoePokolenie.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Services
{
    public class StatusHistoryService
    {
        private UnitOfWork _unitOfWork;
        public StatusHistoryService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateRecordAsync(StatusHistory record)
        {
            await _unitOfWork.StatusHistories.CreateAsync(record);
        }
    }
}
