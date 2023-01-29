using NovoePokolenie.Models;
using NovoePokolenie.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Services
{
    public class LevelService
    {
        private UnitOfWork _unitOfWork;
        public LevelService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Level>> GetCollectionAsync()
        {
            return await _unitOfWork.Levels.GetCollectionAsync();
        }

        public async Task Create(Level level)
        {
            await _unitOfWork.Levels.CreateAsync(level);
            await _unitOfWork.Save();
        }
    }
}
