using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NovoePokolenie.Helpers;
using NovoePokolenie.Models;
using NovoePokolenie.UoW;
using NovoePokolenie.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;

namespace NovoePokolenie.Services
{
    public class AccountService
    {
        private UserManager<NPUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private SignInManager<NPUser> _signInManager;
        private IHttpContextAccessor _contextAccessor;
        private UnitOfWork _unitOfWork;

        public AccountService(UserManager<NPUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<NPUser> signInManager, IHttpContextAccessor contextAccessor, UnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _contextAccessor = contextAccessor;
            _unitOfWork = unitOfWork;
        }

        public async Task<IdentityResult> Register(StaffRegistrationViewModel model)
        {
            NPUser user = new NPUser
            {
                FirstName = model.FirstName,
                Lastname = model.LastName,
                UserName = model.Login,
                Email = model.Login + "@pokolenie.kg",
                EmailConfirmed = true,
                StatusId = (int)ActivityStatus.Staff
            };

            var CreationResult = await _userManager.CreateAsync(user, model.Password);
            if (CreationResult.Succeeded)
            {
                var res = await AssingToRole(user, model.Role);
                return res;
            }
            return IdentityResult.Failed();
        }
        public async Task RegisterUser()
        {
            List<NPUser> users = new List<NPUser>();
            users.Add(new NPUser()
            {
                FirstName = "Роман",
                Lastname = "Красноштанов",
                UserName = "kure",
                Email = "kure@pokolenie.kg",
                EmailConfirmed = true,
                StatusId = (int)ActivityStatus.Staff
            });
            foreach (var user in users)
            {
                var CreationResult = await _userManager.CreateAsync(user, "kurekure");
                if (CreationResult.Succeeded)
                    await AssingToRole(user, "Mentor");
            }
        }

        public async Task<IdentityResult> RegisterStudent(StudentRegistrationViewModel model)
        {
            model.Login = StHel.CreateLogin(model.FirstName, model.LastName);
            NPUser userNameExisted = await _userManager.FindByNameAsync(model.Login);
            int accountNumber = 1;
            string tempLogin = model.Login;
            while(userNameExisted != null)
            {
                tempLogin = model.Login + accountNumber++;
                userNameExisted = await _userManager.FindByNameAsync(tempLogin);
            }
            model.Login = tempLogin;
            model.Password = model.Login;

            NPUser user = new NPUser
            {
                FirstName = model.FirstName,
                Lastname = model.LastName,
                UserName = model.Login,
                ParentName = model.ParentName,
                Email = model.Login + "@pokolenie.kg",
                EmailConfirmed = true,
                PhoneNumber = model.PhoneNumber ?? "",
                StudentPhoneNumber = model.StudentPhoneNumber ?? "",
                PhoneNumberConfirmed = true,
                DateOfBirth = model.DateOfBirth,
                DateOfTrial = model.DateOfTrial,
                DateStarted = model.DateStarted,
                GroupId = model.GroupId,
                CurrentProjectId = 1,
                StatusId = (int)ActivityStatus.Active,
                PaymentCharge = model.Price
            };

            var CreationResult = await _userManager.CreateAsync(user, model.Password);
            if (CreationResult.Succeeded)
                return await AssingToRole(user, model.Role);
            else
                return IdentityResult.Failed();
        }

        public async Task<IdentityResult> Register(StudentRegistrationViewModel model)
        {
            NPUser user = new NPUser
            {
                FirstName = model.FirstName,
                Lastname = model.LastName,
                UserName = model.Login,
                Email = model.Login + "@pokolenie.kg",
                EmailConfirmed = true,
                StatusId = (int)ActivityStatus.Staff,
                PhoneNumber = model.PhoneNumber,
                DateOfBirth = model.DateOfBirth,
                DateOfTrial = model.DateOfTrial,
                DateCreated = DateTime.Today
            };
            var CreationResult = await _userManager.CreateAsync(user, model.Password);
            if (CreationResult.Succeeded)
                return await AssingToRole(user, "Student");
            else return IdentityResult.Failed();
        }

        private async Task<IdentityResult> AssingToRole(NPUser user, string Role)
        {
            return await _userManager.AddToRoleAsync(user, Role);
        }

        public async Task<Microsoft.AspNetCore.Identity.SignInResult> SignPassword(LoginViewModel model)
        {
            //todo: Add remember me
            return await _signInManager.PasswordSignInAsync(model.Login, model.Password, true, false);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ChangeUserPassword(UserSettingsViewModel model)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);
            return await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        }

        public async Task<IList<NPUser>> GetAllUsersInRole(string Role)
        {
            return await _userManager.GetUsersInRoleAsync(Role);
        }

        public async Task<NPUser> GetUserByIdAsync(string Id)
        {
            NPUser user = await _unitOfWork.Students.GetByIdAsync(Id);
            return user;
        }

        public async Task DeleteTrialStudent(string id)
        {
            Lead lead = await _unitOfWork.Leads.GetByIdAsync(id);
            _unitOfWork.Leads.Delete(lead);
            await _unitOfWork.Save();
        }
        public async Task<IdentityResult> InitializeRoles()
        {
            var result = await _roleManager.CreateAsync(new IdentityRole("dev"));
            result = await _roleManager.CreateAsync(new IdentityRole("Student"));
            //if (!result.Succeeded) return IdentityResult.Failed();
            result = await _roleManager.CreateAsync(new IdentityRole("Lead"));
            //if (!result.Succeeded) return IdentityResult.Failed();
            result = await _roleManager.CreateAsync(new IdentityRole("Administrator"));
            //if (!result.Succeeded) return IdentityResult.Failed();
            result = await _roleManager.CreateAsync(new IdentityRole("Manager"));
            //if (!result.Succeeded) return IdentityResult.Failed();
            result = await _roleManager.CreateAsync(new IdentityRole("Mentor"));
            //if (!result.Succeeded) return IdentityResult.Failed();
            result = await _roleManager.CreateAsync(new IdentityRole("Staff"));
            if (!result.Succeeded) return IdentityResult.Failed();
            return result;
        }

    }
}
