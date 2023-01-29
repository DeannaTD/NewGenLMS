using NovoePokolenie.UoW;
using NovoePokolenie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NovoePokolenie.ViewModels;

namespace NovoePokolenie.Services
{
    public class LeadService
    {
        private UnitOfWork _unitOfWork;
        public LeadService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Lead> GetLeadByIdAsync(string id)
        {
            return await _unitOfWork.Leads.GetByIdAsync(id);
        }

        public async Task<bool> CreateAsync(Lead lead)
        {
            lead.DateCreated = DateTime.Now;
            try
            {
                await _unitOfWork.Leads.CreateAsync(lead);
                await _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task ChangeTrialDetails(StudentCardViewModel model)
        {
            var student = await _unitOfWork.Leads.GetByIdAsync(model.StudentId);
            student.FirstName = model.FirstName;
            student.LastName = model.LastName;
            student.PhoneNumber = model.PhoneNumber;
            student.DateOfBirth = model.DateOfBirth;
            student.DateOfTrial = model.DateCreated;
            student.ParentName = model.ParentName;
            student.SocialLink = model.Note;
            student.GroupId = model.GroupId;
            await _unitOfWork.Save();
        }

        public async Task<List<Lead>> GetTrials(int groupId)
        {
            List<Lead> leads = await _unitOfWork.Leads.GetCollectionAsync();
            return leads.Where(lead => lead.GroupId == groupId).ToList();
        }

        public async Task<int> GetTrialsCount(int groupId)
        {
            List<Lead> leads = await _unitOfWork.Leads.GetCollectionAsync();
            if (leads is null) return 0;
            int b = leads.Where(lead => lead.GroupId == groupId).ToList().Count;
            return b;
        }

        public async Task<List<Lead>> GetTrials()
        {
            List<Lead> leads = await _unitOfWork.Leads.GetCollectionAsync();
            return leads.Where(lead => lead.GroupId != null).ToList();
        }

        public async Task<List<Lead>> GetLeads()
        {
            List<Lead> leads = await _unitOfWork.Leads.GetCollectionAsync();
            return leads.Where(lead => lead.GroupId is null).ToList();
        }
    }
}
