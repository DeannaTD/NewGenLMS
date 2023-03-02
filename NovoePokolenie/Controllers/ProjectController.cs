﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NovoePokolenie.Models;
using NovoePokolenie.Services;
using System.IO.Compression;
using Microsoft.VisualBasic;

namespace NovoePokolenie.Controllers
{
    public class ProjectController : Controller
    {
        LevelService _levelService;
        ProjectService _projectService;
        IWebHostEnvironment _webHost;
        public ProjectController(LevelService levelService, ProjectService projectService, IWebHostEnvironment webHost)
        {
            _levelService = levelService;
            _projectService = projectService;
            _webHost = webHost;
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

        public async Task<bool> RenameProjectFile(int projectId, string newLink)
        {
            var project = await _projectService.GetProjectAsync(projectId);
            string newPath = Path.Combine(_webHost.WebRootPath, "projects", newLink);
            string oldLink = project.ProjectLink.Split("/").Last();
            string oldPath = Path.Combine(_webHost.WebRootPath, "projects", oldLink);
            if(System.IO.File.Exists(newPath))
            {
                return false;
            }
            System.IO.File.Move(oldPath, newPath);
            return true;
        }

        public async Task LoadProject(int projectId, IFormFile file)
        {
            var project = await _projectService.GetProjectAsync(projectId);
            //var level = await _levelService.
            string zipLink = project.Level.Name + project.IndexNumber + "_id" + projectId;
            //AI4_id57 - name of dir
            string path = Path.Combine(_webHost.WebRootPath, "projects", file.Name);
            //path to zip file
            using FileStream stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);
            stream.Close();
            UnzipFile(path, zipLink);
            /*
             * Архив должен содержать файл index.html и всё необходимое для его работы
             */
        }

        private void UnzipFile(string filePath, string zipLink)
        {
            string extractPath = Path.Combine(_webHost.WebRootPath, "projects", zipLink);
            ZipFile.ExtractToDirectory(filePath, extractPath);
        }
    }
}
