using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NovoePokolenie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Data
{
    public class NewGenContext : IdentityDbContext<NPUser, IdentityRole, string>
    {
        public DbSet<NPUser> NPUser { get; set; }
        public DbSet<TimeTable> TimeTable { get; set; }
        public DbSet<StatusHistory> StatusHistory { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<PaymentPeriod> PaymentPeriod { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<UserProject> UserProject { get; set; }
        public DbSet<Level> Level { get; set; }
        public DbSet<Lead> Lead { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public NewGenContext(DbContextOptions<NewGenContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
