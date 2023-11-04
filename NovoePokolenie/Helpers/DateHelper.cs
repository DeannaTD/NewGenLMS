using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace NovoePokolenie.Helpers
{
    public class DateHelper
    {
        private static string[] months = new string[] { "Января", "Февраля", "Марта", "Апреля", "Мая", "Июня", "Июля", "Августа", "Сентября", "Октября", "Ноября", "Декабря" };
        private static string[] monthsDefault = new string[] { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
        private static Dictionary<DayOfWeek, string> dofRuShort = new Dictionary<DayOfWeek, string>()
        {
            { DayOfWeek.Monday, "Пн" },
            { DayOfWeek.Tuesday, "Вт" },
            { DayOfWeek.Wednesday, "Ср" },
            { DayOfWeek.Thursday, "Чт" },
            { DayOfWeek.Friday, "Пт" },
            { DayOfWeek.Saturday, "Сб" },
            { DayOfWeek.Sunday, "Вс" }
        };

        private static Dictionary<DayOfWeek, string> dofRuFull = new Dictionary<DayOfWeek, string>()
        {
            { DayOfWeek.Monday, "Понедельник" },
            { DayOfWeek.Tuesday, "Вторник" },
            { DayOfWeek.Wednesday, "Среда" },
            { DayOfWeek.Thursday, "Четверг" },
            { DayOfWeek.Friday, "Пятница" },
            { DayOfWeek.Saturday, "Суббота" },
            { DayOfWeek.Sunday, "Воскресенье" }
        };

        public static string GetMonthName(int monthIndex, bool inCase = false)
        {
            monthIndex--;
            if (monthIndex < 0 || monthIndex > months.Length) monthIndex = 0;
            return inCase ? months[monthIndex] : monthsDefault[monthIndex];
        }

        public static string GetDayOfWeekName(DayOfWeek dayOfWeek, bool isShort = false)
        {
            return isShort ? dofRuShort[dayOfWeek] : dofRuFull[dayOfWeek];
        }

        public static string GetDayOfWeekName(int dayOfWeek, bool isShort = false)
        {
            DayOfWeek day = (DayOfWeek)(dayOfWeek);
            return isShort ? dofRuShort[day] : dofRuFull[day];
        }

        public static int GetDayOfWeekIndex(DayOfWeek dayOfWeek)
        {
            if (dayOfWeek == DayOfWeek.Sunday) return 6;
            else return (int)dayOfWeek - 1;
        }
    }
}
