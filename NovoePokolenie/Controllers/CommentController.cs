using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovoePokolenie.Models;

namespace NovoePokolenie.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            List<Comment> comments = new List<Comment>();
            return View(comments);
        }
    }
}
