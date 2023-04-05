using System;

namespace NovoePokolenie.Helpers
{
    public class DateHelper
    {
        private static string[] months = new string[] { "Января", "Февраля", "Марта", "Апреля", "Мая", "Июня", "Июля", "Августа", "Сентября", "Октября", "Ноября", "Декабря" };
        private static string[] monthsDefault = new string[] { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
        private static string[] daysOfWeek = new string[] { "Воскресенье", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота" };
        private static string[] daysOfWeekShort = new string[] { "ВС", "ПН", "ВТ", "СР", "ЧТ", "ПТ", "СБ" };

        public static string GetNormalizedDateString(DateTime date, bool withTime = false)
        {
            //TODO: добавить возврат со временем
            return date.ToString("dd/MM/yy");
        }

        public static string GetMonthName(int monthIndex, bool inCase = false)
        {
            monthIndex--;
            if (monthIndex < 0 || monthIndex > months.Length) monthIndex = 0;
            return inCase ? months[monthIndex] : monthsDefault[monthIndex];
        }

        //todo: почему у функции есть перегрузка
        public static string GetDayOfWeekName(DayOfWeek dayOfWeek, bool isShort = false)
        {
            int dayIndex = (int)dayOfWeek;
            return isShort ? daysOfWeekShort[dayIndex] : daysOfWeek[dayIndex];
        }

        public static string GetDayOfWeekName(int dayOfWeek, bool isShort = false)
        {
            dayOfWeek = dayOfWeek == 6 ? 0 : dayOfWeek + 1;
            return isShort ? daysOfWeekShort[dayOfWeek] : daysOfWeek[dayOfWeek];
        }

        public static int GetDayOfWeekIndex(DayOfWeek dayOfWeek)
        {
            if (dayOfWeek == DayOfWeek.Sunday) return 6;
            else return (int)dayOfWeek - 1;
        }

        //todo: удалить
        public static string GetDateId(DateTime date, string prefix = "")
        {
            return prefix + date.Year + "-" + date.Month + "-" + date.Day;
        }

        //todo: удалить
        public static string GetShowDateName(DateTime date)
        {
            return date.Day + " " + months[date.Month - 1] + ", " + date.Year + "г.";
        }
    }
}
