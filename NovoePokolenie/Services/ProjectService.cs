using NovoePokolenie.Models;
using NovoePokolenie.UoW;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Services
{
    public class ProjectService
    {
        private UnitOfWork _unitOfWork;
        public ProjectService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Project>> GetCollectionAsync()
        {
            List<Project> collection = await _unitOfWork.Projects.GetCollectionAsync();
            return collection;
        }

        public async Task Create(Project project)
        {
            await _unitOfWork.Projects.CreateAsync(project);
            await _unitOfWork.Save();
        }

        public async Task<Project> GetProjectAsync(int id)
        {
            return await _unitOfWork.Projects.GetByIdAsync(id);
        }

        public async Task<List<Project>> GetProjectsByLevelIdAsync(int id)
        {
            var projects = (await _unitOfWork.Projects.GetCollectionAsync()).Where(p => p.LevelId == id).ToList();
            projects.Sort((x, y) => { return x.ProjectLink.CompareTo(y.ProjectLink); });
            return projects;
        }

        public async Task<Project> GetProjectByLevelIdAsync(int id)
        {
            int projectId = await _unitOfWork.Projects.GetFirstProjectIdAsync(id);
            return await _unitOfWork.Projects.GetByIdAsync(projectId);
        }

        public async Task Update(Project project)
        {
            _unitOfWork.Projects.Update(project);
            await _unitOfWork.Save();
        }
    }
}
