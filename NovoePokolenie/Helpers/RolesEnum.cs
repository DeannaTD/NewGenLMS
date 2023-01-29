using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Helpers
{
    public class RolesEnum
    {
        public static Dictionary<string, string> RolesNameRus(IQueryable<IdentityRole> roles)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            foreach (IdentityRole role in roles)
            {
                string roleName = NormalizedToRus(role.Name);
                if(roleName != string.Empty)
                    values.Add(role.Name, roleName);
            }
            return values;
        }

        public static string NormalizedToRus(string role)
        {
            if (role == "Student") return "Ученик";
            if (role == "Mentor") return "Учитель";
            if (role == "Lead") return "Лид";
            if (role == "Manager") return "Менеджер";
            if (role == "Administrator") return "Администратор";
            return string.Empty;
        }
    }

    

    public enum AttendanceStatus
    {
        Unknown = 0,
        Attended = 1,
        Absent = 2
    }

    public enum ActivityStatus
    {
        Lead = 1,
        Trial = 2,
        Active = 3,
        Archive = 4,
        Blocked = 5,
        Staff = 6
    }

    public enum DefaultValues
    {
        TrialLevel = 7
    }

    public enum FilterOption
    {
        None,
        Status,
        Branch
    }

    public enum SortOption
    {
        None,
        Debt,
        Firstname,
        Lastname,
        Date
    }

    public class StSort
    {
        public SortOption Option { get; private set; }
        public string Parameter { get; private set; }

        public StSort(SortOption option, string parameter)
        {
            Option = option;
            Parameter = parameter;
        }
    }

    public class StFilter
    {
        public FilterOption Option { get; set; }
        public int Parameter { get; set; }

        public StFilter()
        {
            Option = FilterOption.None;
            Parameter = 0;
        }

        public StFilter(FilterOption option, int parameter)
        {
            Option = option;
            Parameter = parameter;
        }
    }
}


//1 - interested
//2 - trial
//3 - active
//4 - Staff
//5 - blocked
//6 - archived