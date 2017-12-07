using System;
using System.Globalization;

namespace BSUIR_Scheduler.Services
{
    class CalendarService
    {
        public string TranslateDay(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Friday: return new CultureInfo("ru-RU").DateTimeFormat.GetDayName(DayOfWeek.Friday);
                case DayOfWeek.Monday: return new CultureInfo("ru-RU").DateTimeFormat.GetDayName(DayOfWeek.Monday);
                case DayOfWeek.Tuesday: return new CultureInfo("ru-RU").DateTimeFormat.GetDayName(DayOfWeek.Tuesday);
                case DayOfWeek.Wednesday: return new CultureInfo("ru-RU").DateTimeFormat.GetDayName(DayOfWeek.Wednesday);
                case DayOfWeek.Thursday: return new CultureInfo("ru-RU").DateTimeFormat.GetDayName(DayOfWeek.Thursday);
                case DayOfWeek.Saturday: return new CultureInfo("ru-RU").DateTimeFormat.GetDayName(DayOfWeek.Saturday);
                case DayOfWeek.Sunday: return new CultureInfo("ru-RU").DateTimeFormat.GetDayName(DayOfWeek.Sunday);
                default: return "";
            }
        }

        public string NextDay(string day)
        {
            switch (day)
            {
                case "понедельник": return "вторник";
                case "вторник": return "среда";
                case "среда": return "четверг";
                case "четверг": return "пятница";
                case "пятница": return "суббота";
                case "суббота": return "понедельник";
                default: return "";
            }
        }

        public string PrevDay(string day)
        {
            switch (day)
            {
                case "понедельник": return "суббота";
                case "вторник": return "понедельник";
                case "среда": return "вторник";
                case "четверг": return "среда";
                case "пятница": return "четверг";
                case "суббота": return "пятница";
                default: return "";
            }
        }
    }
}
