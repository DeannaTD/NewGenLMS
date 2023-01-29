using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Models
{
    public class NPUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfTrial { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateStarted { get; set; }
        public string ParentName { get; set; }
        public int? GroupId { get; set; }
        public int? CurrentProjectId { get; set; }
        public int PaymentCharge { get; set; }
        public string ThemeLink { get; set; }
        public string AvatarLink { get; set; }
        public int StatusId { get; set; }
        public string Note { get; set; }
        public string StudentPhoneNumber { get; set; }

        public virtual IdentityRole Access { get; set; }
        public virtual ICollection<UserProject> UserProjects { get; set; }
        public virtual Status Status { get; set; }
        public virtual Group Group { get; set; }
        public virtual ICollection<StatusHistory> StatusHistories { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<PaymentPeriod> PaymentPeriods { get; set; }

        /*regenerated and restructed*/
    }
}
