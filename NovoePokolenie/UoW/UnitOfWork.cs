using NovoePokolenie.Data;
using NovoePokolenie.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.UoW
{
    public class UnitOfWork
    {
        private NewGenContext _context;
        public StudentRepository Students { get; set; }
        public BranchRepository Branches { get; set; }
        public GroupRepository Groups { get; set; }
        public TimeTableRepository TimeTables { get; set; }
        public AttendanceRepository Attendances { get; set; }
        public PaymentRepository Payments { get; set; }
        public PaymentPeriodRepository PaymentPeriods { get; set; }
        public ProjectRepository Projects { get; set; }
        public LevelRepository Levels { get; set; }
        public UserProjectRepository UsersProjects { get; set; }
        public CommentRepository Comments { get; set; }
        public StatusHistoryRepository StatusHistories { get; set; }
        public LeadRepository Leads { get; set; }

        public UnitOfWork(NewGenContext context)
        {
            _context = context;
            Students = new StudentRepository(_context);
            Branches = new BranchRepository(_context);
            Groups = new GroupRepository(_context);
            TimeTables = new TimeTableRepository(_context);
            Attendances = new AttendanceRepository(_context);
            Payments = new PaymentRepository(_context);
            PaymentPeriods = new PaymentPeriodRepository(_context);
            Projects = new ProjectRepository(_context);
            Levels = new LevelRepository(_context);
            UsersProjects = new UserProjectRepository(_context);
            Comments = new CommentRepository(_context);
            StatusHistories = new StatusHistoryRepository(_context);
            Leads = new LeadRepository(_context);
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
