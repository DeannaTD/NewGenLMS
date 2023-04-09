using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NovoePokolenie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Helpers
{
    public class StHel
    {
        public static string[] months = new string[] { "Января ", "Февраля ", "Марта ", "Апреля ", "Мая ", "Июня ", "Июля ", "Августа ", "Сентября ", "Октября ", "Ноября ", "Декабря " };
        public static string[] monthsDefault = new string[] { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
        public static Dictionary<char, string> letters = new Dictionary<char, string>();
        public static List<DayOfWeek> daysOrder = new List<DayOfWeek>() { 
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday,
            DayOfWeek.Saturday,
            DayOfWeek.Sunday
        };
        public static int CompareTimeTables(TimeTable time1, TimeTable time2)
        {
            int d1 = daysOrder.IndexOf(time1.Day1);
            int d2 = daysOrder.IndexOf(time2.Day1);
            if (d1 > d2)
            {
                return 1;
            }
            else if(d1 < d2)
            {
                return -1;
            }
            else
            {
                return time1.TimeName.CompareTo(time2.TimeName);
            }
        }
        public static string TimeTableToString(TimeTable time)
        {
            return
                GetDayOfWeekName(time.Day1) + "-" + GetDayOfWeekName(time.Day2) + "," + time.TimeName;
        }

        public static string TimeTableToShortString(TimeTable time)
        {
            return
                GetDayOfWeekShortName(time.Day1) + "-" + GetDayOfWeekShortName(time.Day2) + "," + time.TimeName;
        }

        public static string ScheduleDays(TimeTable time)
        {
            return
                (int)time.Day1 + ":" + (int)time.Day2;
        }

        public static DateTime GetFirstDay(int year, int month)
        {
            return new DateTime(year, month, 1);
        }

        public static List<DateTime> GetTimeTableDays(int year, int month, (DayOfWeek, DayOfWeek) days)
        {
            var InitialDate = GetFirstDay(year, month);
            var FlagDate = GetFirstDay(year, month);
            List<DateTime> Dates = new List<DateTime>();

            while(InitialDate.Month == FlagDate.Month)
            {
                if (InitialDate.DayOfWeek == days.Item1 || InitialDate.DayOfWeek == days.Item2)
                    Dates.Add(InitialDate);
                InitialDate = InitialDate.AddDays(1);
            }

            return Dates;
        }

        public static string MonthName(int month) => (month < 13 && month > 0) ? monthsDefault[month - 1] : string.Empty;

        public static string GetDayOfWeekName(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Sunday: return "Воскресенье";
                case DayOfWeek.Monday: return "Понедельник";
                case DayOfWeek.Tuesday: return "Вторник";
                case DayOfWeek.Wednesday: return "Среда";
                case DayOfWeek.Thursday: return "Четверг";
                case DayOfWeek.Friday: return "Пятница";
                case DayOfWeek.Saturday: return "Суббота";
                default:
                    return string.Empty;
            }
        }

        public static string GetDayOfWeekShortName(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Sunday: return "ВС";
                case DayOfWeek.Monday: return "ПН";
                case DayOfWeek.Tuesday: return "ВТ";
                case DayOfWeek.Wednesday: return "СР";
                case DayOfWeek.Thursday: return "ЧТ";
                case DayOfWeek.Friday: return "ПТ";
                case DayOfWeek.Saturday: return "СБ";
                default:
                    return string.Empty;
            }
        }
        public static string GetFormatedDate(DateTime date, bool withWeekDay = true)
        {
            //return date.Day + " " + months[date.Month - 1] + date.Year + "г." + (withWeekDay? ", " + GetDayOfWeekName(date.DayOfWeek):"");
            string result = "";
            result += date.Day + "/";
            if (date.Month.ToString().Length == 1) result += "0";
            result += date.Month + "/";
            result += date.Year;
            result += withWeekDay ? ", " + GetDayOfWeekName(date.DayOfWeek) : "";

            return result;
        }

        public static string GetGreetingDate(DateTime date)
        {
            string result = string.Join(' ', new string[] { date.Day.ToString(), months[date.Month - 1], date.Year.ToString() });
            result += ", " + GetDayOfWeekName(date.DayOfWeek);
            return result;
        }

        public static IEnumerable<SelectListItem> DaysOfWeek()
        {
            return new List<SelectListItem>
            {
                new SelectListItem("Понедельник", "1"),
                new SelectListItem("Вторник", "2"),
                new SelectListItem("Среда", "3"),
                new SelectListItem("Четверг", "4"),
                new SelectListItem("Пятница", "5"),
                new SelectListItem("Суббота", "6"),
                new SelectListItem("Воскресенье", "0")
            };
        }
        public static string GetLatinFromCyrillic(char symbol) => letters.ContainsKey(symbol) ? letters.GetValueOrDefault(symbol) : string.Empty;

        public static string CreateLogin(string firstName, string lastName)
        {
            if (letters.Count == 0)
            {
                letters.Add('а', "a");
                letters.Add('б', "b");
                letters.Add('в', "v");
                letters.Add('г', "g");
                letters.Add('д', "d");
                letters.Add('е', "e");
                letters.Add('ё', "e");
                letters.Add('ж', "zh");
                letters.Add('з', "z");
                letters.Add('и', "i");
                letters.Add('й', "y");
                letters.Add('к', "k");
                letters.Add('л', "l");
                letters.Add('м', "m");
                letters.Add('н', "n");
                letters.Add('о', "o");
                letters.Add('п', "p");
                letters.Add('р', "r");
                letters.Add('с', "s");
                letters.Add('т', "t");
                letters.Add('у', "u");
                letters.Add('ф', "f");
                letters.Add('х', "h");
                letters.Add('ц', "c");
                letters.Add('ч', "ch");
                letters.Add('ш', "sh");
                letters.Add('щ', "sh");
                letters.Add('ъ', "");
                letters.Add('ы', "y");
                letters.Add('ь', "");
                letters.Add('э', "e");
                letters.Add('ю', "u");
                letters.Add('я', "ya");
                letters.Add('a', "a");
                letters.Add('b', "b");
                letters.Add('c', "c");
                letters.Add('d', "d");
                letters.Add('e', "e");
                letters.Add('f', "f");
                letters.Add('g', "g");
                letters.Add('h', "h");
                letters.Add('i', "i");
                letters.Add('j', "j");
                letters.Add('k', "k");
                letters.Add('l', "l");
                letters.Add('m', "m");
                letters.Add('n', "n");
                letters.Add('o', "o");
                letters.Add('p', "p");
                letters.Add('q', "q");
                letters.Add('r', "r");
                letters.Add('s', "s");
                letters.Add('t', "t");
                letters.Add('u', "u");
                letters.Add('v', "v");
                letters.Add('w', "w");
                letters.Add('x', "x");
                letters.Add('y', "y");
                letters.Add('z', "z");
            }
            string result = "";
            firstName = firstName.ToLower();
            lastName = lastName.ToLower();
            result += GetLatinFromCyrillic(firstName[0]);
            foreach(char c in lastName)
            {
                result += GetLatinFromCyrillic(c);
            }
            return result;
        }

    }
}
