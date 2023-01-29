using Microsoft.AspNetCore.Mvc;
using NovoePokolenie.Models;
using NovoePokolenie.UoW;
using NovoePokolenie.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Services
{
    public class BranchService
    {
        private UnitOfWork _unitOfWork;
        public BranchService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Branch> GetBranchByIdAsync(int Id)
        {
            var branch = await _unitOfWork.Branches.GetByIdAsync(Id);
            return branch ?? await _unitOfWork.Branches.GetByIdAsync(0);
        }

        #region Create-Delete
        public async Task<bool> CreateAsync(Branch model)
        {
            await _unitOfWork.Branches.CreateAsync(model);

            int Counter = await _unitOfWork.Save();
            return Counter != 0;
        }
        #endregion

        public async Task<List<Branch>> GetBranchesAsync()
        {
            return await _unitOfWork.Branches.GetBranchesWithGroupsAsync();
        }

        public async Task<Branch> GetBranchWithGroupsAsync(int Id)
        {
            return await _unitOfWork.Branches.GetByIdWithGroupsAsync(Id);         
        }

        public async Task UpdateBranch(Branch branch)
        {
            _unitOfWork.Branches.Update(branch);
            await _unitOfWork.Save();
        }
    }
}
