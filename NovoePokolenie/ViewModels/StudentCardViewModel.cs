using NovoePokolenie.Helpers;
using NovoePokolenie.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.ViewModels
{
    public class StudentCardViewModel
    {
        public string StudentId { get; set; }

        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Номер родителя")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Номер ученика")]
        public string StudentPhoneNumber { get; set; }

        [Display(Name = "Дата рождения")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Дата пробного")]
        public DateTime DateOfTrial { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Дата начала занятий")]
        public DateTime DateStarted { get; set; }

        [Display(Name = "Имя родителя")]
        public string ParentName { get; set; }

        [Display(Name = "Стоимость")]
        public int PaymentCharge { get; set; }

        [Display(Name = "Уровень")]
        public string Level { get; set; }

        public int? GroupId { get; set; }

        [Display(Name = "Комментарий")]
        public string Note { get; set; }

        [Display(Name = "Долг")]
        public int Debt { get; set; }

        [Display(Name = "Группа")]
        public string Group { get; set; }

        [Display(Name = "Преподаватель")]
        public string MentorName { get; set; }
        public string MentorId { get; set; }

        public int StatusId { get; set; }
        public int LevelId { get; set; }

        public StudentCardViewModel(NPUser student, List<Group> groups)
        {
            StudentId = student.Id;
            FirstName = student.FirstName;
            LastName = student.Lastname;
            PhoneNumber = student.PhoneNumber;
            StudentPhoneNumber = student.StudentPhoneNumber;
            DateOfBirth = student.DateOfBirth;
            DateOfTrial = student.DateOfTrial;
            DateCreated = student.DateCreated;
            DateStarted = student.DateStarted;
            ParentName = student.ParentName;
            PaymentCharge = student.PaymentCharge;
            GroupId = student.GroupId;
            Note = student.Note;
            LevelId = 0;
        }

        public StudentCardViewModel(Lead lead)
        {
            StudentId = lead.Id;
            FirstName = lead.FirstName;
            LastName = lead.LastName;
            PhoneNumber = lead.PhoneNumber;
            StudentPhoneNumber = lead.PhoneNumber;
            DateOfBirth = lead.DateOfBirth;
            DateOfTrial = lead.DateOfTrial;
            DateCreated = lead.DateCreated;
            DateStarted = new DateTime(2000, 01, 01);
            ParentName = lead.ParentName;
            PaymentCharge = 0;
            GroupId = lead.GroupId;
            Note = lead.SocialLink;
            LevelId = (int)DefaultValues.TrialLevel;
            Debt = 0;
        }

        public StudentCardViewModel()
        {

        }
    }
}
