using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovoePokolenie.Models;
using NovoePokolenie.Services;

namespace NovoePokolenie.Controllers
{
    public class ProjectController : Controller
    {
        LevelService _levelService;
        ProjectService _projectService;
        public ProjectController(LevelService levelService, ProjectService projectService)
        {
            _levelService = levelService;
            _projectService = projectService;
        }

        public IActionResult CreateLevel()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateLevel(Level level)
        {
            await _levelService.Create(level);
            return new EmptyResult();
        }

        public IActionResult CreateProject(int levelId)
        {
            ViewBag.LevelId = levelId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(Project project)
        {
            await _projectService.Create(project);
            return new EmptyResult();
        }

        public async Task<IActionResult> StudentProjectInfo()
        {
            var levels = await _levelService.GetCollectionAsync();
            List<UserProject> taskLinks = new List<UserProject>();

            return PartialView(levels);
        }
        public async Task<IActionResult> Index()
        {
            var levels = await _levelService.GetCollectionAsync();
            foreach(var level in levels)
                level.Projects.OrderBy(x => x.ProjectLink);
            return View(levels);
        }
    }
}
