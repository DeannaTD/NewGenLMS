﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NovoePokolenie.Helpers;
using NovoePokolenie.Models;
using NovoePokolenie.Services;
using NovoePokolenie.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IronSoftware.Drawing;

//0551 20 47 10 Бурмат Султановна Шукурова Московская-Логвиненко, со стороны Московской, 2 этаж, Эндокринология
namespace NovoePokolenie.Controllers
{
    public class AccountController : Controller
    {
        AccountService _service;
        UserManager<NPUser> _userManager;
        IWebHostEnvironment _webHost;

        //конструктор контроллера
        public AccountController(AccountService accountService, UserManager<NPUser> userManager, IWebHostEnvironment webHost)
        {
            _service = accountService;
            _userManager = userManager;
            _webHost = webHost;
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string NewPassword, string CurrentPassword, string CheckPassword)
        {
            NPUser user = await _userManager.GetUserAsync(User);
            PasswordHasher<NPUser> hasher = new PasswordHasher<NPUser>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, CurrentPassword);
            if (result == PasswordVerificationResult.Success)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, NewPassword);
                return Ok();
            }
            else
            {
                return new StatusCodeResult(400);
            }
        }

        //todo: добавить ограничение доступа для менеджера
        public async Task ResetPasswordManager(string userId)
        {
            //NPUser user = await _userManager.FindByNameAsync("kamaneicha");
            NPUser user = await _userManager.FindByIdAsync(userId);
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, user.UserName);
        }

        //todo: что это
        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View("RegisterUser");
        }

        //todo: что это
        [HttpPost]
        public async Task<IActionResult> Register(StaffRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Register(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Staff");
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return RedirectToAction("Staff");
        }

        //todo: что это
        [HttpGet]
        public IActionResult RegisterStudent(int groupId)
        {
            return View(new StudentRegistrationViewModel() { GroupId = groupId });
        }

        public async Task RegisterUsers()
        {
            await _service.RegisterUser();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterStudent(StudentRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.RegisterStudent(model);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterStudentTrial(StudentCardViewModel model, string trialId)
        {
            if (ModelState.IsValid)
            {
                string newLogin = StHel.CreateLogin(model.FirstName, model.LastName);
                StudentRegistrationViewModel registrationModel = new StudentRegistrationViewModel()
                {
                    GroupId = model.GroupId ?? 0,
                    Login = newLogin,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = newLogin,
                    PasswordConfirm = newLogin,
                    PhoneNumber = model.PhoneNumber,
                    StudentPhoneNumber = model.StudentPhoneNumber,
                    DateOfBirth = model.DateOfBirth,
                    DateOfTrial = model.DateOfTrial,
                    DateCreated = model.DateCreated,
                    DateStarted = DateTime.Now,
                    Price = 0,
                    ParentName = model.ParentName,
                    Note = model.Note,
                    Role = "student"
                };
                var result = await _service.RegisterStudent(registrationModel);
                if (result.Succeeded)
                {
                    await _service.DeleteTrialStudent(trialId);
                    return RedirectToAction("RegisterStudent", "Account", new { groupId = model.GroupId });
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Login/Logout
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return View("_Index");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //ModelState.AddModelError("", "В данный момент вход невозможен. Попробуйте позже");
            //return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                var result = await _service.SignPassword(model);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        NPUser user = await _userManager.FindByNameAsync(model.Login);
                        bool isStudent = await _userManager.IsInRoleAsync(user, "Student");
                        if (!user.EmailConfirmed && !isStudent)
                        {
                            ModelState.AddModelError("", "Ваш аккаунт заблокирован");
                            return View(model);
                        }
                        StreamWriter sw = new StreamWriter(Path.Combine(_webHost.WebRootPath, "img/lilog.txt"), true);
                        string udata = DateTime.UtcNow.ToString("G") + " - " + model.Login;
                        sw.WriteLine(udata);
                        sw.Close();
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _service.Logout();
            return RedirectToAction("Login", "Account");
        }
        #endregion

        #region Settings


        [HttpPost]
        public async Task<IActionResult> Settings(UserSettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _service.ChangeUserPassword(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Settings");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        #endregion

        public async Task<IActionResult> InitializeRoles()
        {
            await _service.InitializeRoles();
            ////NPUser u = await _userManager.FindByNameAsync("azaripova");
            ////_service.
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Staff()
        {
            List<StaffViewModel> staffList = new List<StaffViewModel>();

            staffList.AddRange((await _service.GetAllUsersInRole("Mentor"))
                .Select(user => new StaffViewModel(
                    user.Id, user.FirstName, user.Lastname, user.UserName, "Mentor", !user.EmailConfirmed
                    )).ToList());

            staffList.AddRange((await _service.GetAllUsersInRole("Manager"))
                .Select(user => new StaffViewModel(
                    user.Id, user.FirstName, user.Lastname, user.UserName, "Manager", !user.EmailConfirmed
                    )).ToList());

            staffList.AddRange((await _service.GetAllUsersInRole("Admin"))
                .Select(user => new StaffViewModel(
                    user.Id, user.FirstName, user.Lastname, user.UserName, "Admin", !user.EmailConfirmed
                    )).ToList());

            return View("InnerUsers", staffList);
        }

        public async Task<IActionResult> BlockStaffUser(string id)
        {
            await _service.SetEmailConfirm(id, false);
            return RedirectToAction("Staff");
        }

        public async Task<IActionResult> UnblockStaffUser(string id)
        {
            await _service.SetEmailConfirm(id, true);
            return RedirectToAction("Staff");
        }
        public async Task DeleteUser(string login)
        {
            //NPUser user = await _userManager.FindByIdAsync(login);
            NPUser user = await _userManager.FindByNameAsync(login);
            await _userManager.DeleteAsync(user);
        }

        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var user = await _userManager.GetUserAsync(User);
            UserSettingsViewModel model = new UserSettingsViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.Lastname,
                Login = user.UserName,
                UserPictureAdress = user.AvatarLink
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SettingsInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.FirstName = user.FirstName;
            ViewBag.LastName = user.Lastname;
            ViewBag.Login = user.UserName;
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeLogin(string Login, string Password)
        {
            NPUser user = await _userManager.GetUserAsync(User);
            PasswordHasher<NPUser> hasher = new PasswordHasher<NPUser>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, Password);
            if (result == PasswordVerificationResult.Success)
            {
                user.UserName = Login;
                user.Email = Login + @"@pokolenie.kg";
                await _userManager.UpdateAsync(user);
                await _userManager.UpdateNormalizedUserNameAsync(user);
                await _userManager.UpdateNormalizedEmailAsync(user);
                return Ok();
            }
            else
            {
                return new StatusCodeResult(400);
            }
        }

        [HttpGet]
        public IActionResult SettingsAccount()
        {
            //NPUser user = await _userManager.GetUserAsync(User);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SettingsAccount(IFormFile image)
        {
            var user = await _userManager.GetUserAsync(User);

            string path = Path.Combine(_webHost.WebRootPath, "img");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = user.Id + "." + Path.GetExtension(image.FileName);
            AnyBitmap b = AnyBitmap.FromStream(image.OpenReadStream());
            int k = b.Width / 100;
            AnyBitmap bitmap = new AnyBitmap(b, b.Width / k, b.Height / k);
            bitmap.SaveAs(Path.Combine(path, fileName));
            user.AvatarLink = fileName;
            await _userManager.UpdateAsync(user);
            return new EmptyResult();
        }

        [HttpGet]
        public async Task<IActionResult> BlockedUsers()
        {
            var students = await _service.GetAllUsersInRole("Student");
            return View(students.Where(s => s.StatusId == (int)Helpers.ActivityStatus.Blocked).ToList());
        }

        public async Task<string> GetUserFullName(string id)
        {
            NPUser user = await _service.GetUserByIdAsync(id);
            return user.FirstName + " " + user.Lastname;
        }

        public IActionResult SessionExpired()
        {
            return View("LoginError");
        }

        public IActionResult AllUserSearch(string name, string phone, int isAjax)
        {
            if (name == "" || name is null) name = " ";
            if (phone == "" || phone is null) phone = "null";

            List<NPUser> users = new List<NPUser>();
            foreach (NPUser user in _userManager.Users)
            {
                if (IsTrue(user, name, phone))
                {
                    users.Add(user);
                }
            }

            ViewBag.useLayout = isAjax == 1;
            return View("AllUser", users);
        }
        //TODO: sense сделал много упрощений
        public bool IsTrue(NPUser user, string name, string phone)
        {
            name = name.ToUpper();
            string fn = user.FirstName.ToUpper();
            string ln = user.Lastname.ToUpper();
            bool bname = (fn + " " + ln).Contains(name) || (ln + " " + fn).Contains(name);
            bool bp1 = user.PhoneNumber != null && user.PhoneNumber.Contains(phone);
            bool bp2 = user.StudentPhoneNumber != null && user.StudentPhoneNumber.Contains(phone);
            return bname && (phone == "null" || (bp1 || bp2));
        }
        public IActionResult AllUserMainPage()
        {
            return View();
        }

        public async Task DeleteUserById(string id)
        {
            NPUser user = await _service.GetUserByIdAsync(id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<IActionResult> GetDuplicatedUsers()
        {
            var users = await _service.GetAllUsersInRole("Student");
            //Для поиска дупликатов
            var duplicates = new List<NPUser>();
            var all = new List<NPUser>();
            foreach (var user in users)
            {
                if (Char.IsDigit(user.UserName.Last()))
                {
                    duplicates.Add(user);
                }
            }
            foreach (var user in duplicates)
            {
                string name = System.Text.RegularExpressions.Regex.Replace(user.UserName, @"[0-9]", "");
                all.AddRange(users.Where(x => x.UserName.Contains(name)).ToList());
            }

            //Для поиска студентов без групп
            //var all = new List<NPUser>();
            //foreach(var user in users)
            //{
            //    if (user.GroupId != null && user.GroupId != 0)
            //        if (user.StatusId != (int)ActivityStatus.Archive) all.Add(user);
            //}
            return View("Duplicates", all);
        }


        public async Task<JsonResult> StudentsInGroup(int groupId)
        {
            var students = await _service.GetAllUsersInRole("Student"); 
            var result = students.Where(s => s.GroupId == groupId).ToList();
            return Json(result);
        }
    }
}
