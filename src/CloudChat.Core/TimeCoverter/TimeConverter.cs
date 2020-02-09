using System;

namespace CloudChat
{
    public static class TimeConverter
    {
        public static string GetShortTime(DateTimeOffset date)
        {
            string time;
            int currentDayOfYear = DateTimeOffset.UtcNow.DayOfYear;
            if (DateTimeOffset.UtcNow.Year - date.Year > 1)
            {
                if (DateTime.DaysInMonth(date.Year, 2) == 28)
                    currentDayOfYear += 365;
                else
                    currentDayOfYear += 366;
            }
            if (currentDayOfYear - date.DayOfYear == 0)
                time = date.LocalDateTime.ToShortTimeString();
            else
                if (currentDayOfYear - date.DayOfYear == 1)
                time = "Yesterday at " + date.LocalDateTime.ToShortTimeString();
            else
                time = date.LocalDateTime.ToString("dd/mm/yy");

            return time;
        }
    }
}
