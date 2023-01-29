using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.ViewModels
{
    public class StudentRegistrationViewModel
    {
        [Required]
        [HiddenInput]
        public int GroupId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Номер родителя")]
        [DisplayFormat(DataFormatString ="{0:###-##-##-##", ApplyFormatInEditMode = true)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Номер ученика")]
        [DisplayFormat(DataFormatString = "{0:###-##-##-##", ApplyFormatInEditMode = true)]
        public string StudentPhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата пробного занятия")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfTrial { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата начала занятий")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateStarted { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата обращения")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Оплата")]
        public int Price { get; set; }

        [Required]
        [Display(Name = "Имя родителя")]
        public string ParentName { get; set; }

        [Display(Name = "Комментарий")]
        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public string Role { get; set; }



        public StudentRegistrationViewModel()
        {
            Role = "Student";
        }

    }
}