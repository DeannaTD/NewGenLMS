using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Models
{
    public class Lead
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } //identifier

        [Display(Name ="Имя")]
        public string FirstName { get; set; } //Имя

        [Display(Name ="Фамилия")]
        public string LastName { get; set; } //Фамилия

        [Display(Name = "Комментарий")]
        public string SocialLink { get; set; } //Комментарий

        [Display(Name = "Дата рождения")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Дата пробного")]
        public DateTime DateOfTrial { get; set; } //Дата пробника (для пробников)
        public DateTime DateCreated { get; set; } //Дата создания-контакта

        [Display(Name = "Имя родителя")]
        public string ParentName { get; set; } //Имя родителя 

        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; } //Номер телефона

        //11.02
        [Display(Name = "Группа")]
        public int? GroupId { get; set; }
    }
}

