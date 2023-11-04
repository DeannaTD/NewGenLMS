using Microsoft.AspNetCore.Mvc;
using NovoePokolenie.Services;
using System.Threading.Tasks;

namespace NovoePokolenie.Controllers
{
    public class MentorController : Controller
    {
        BranchService _branchService;

        public MentorController(BranchService branchService)
        {
            _branchService = branchService;
        }

        //TODO: remove
        public async Task<IActionResult> AttendanceMenu()
        {
            var branches = await _branchService.GetBranchesAsync();
            return View(branches);
        }

        public async Task<IActionResult> StudentsMenu()
        {
            var branches = await _branchService.GetBranchesAsync();
            return View(branches);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
