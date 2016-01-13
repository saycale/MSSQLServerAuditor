using System;

namespace MSSQLServerAuditor.Model.Scheduling
{
    [Serializable]
    public class DayOfMonthSettings
    {
        public static DateTime BeginingOfMonth(DateTime anyDateOfMonth)
        {
            return new DateTime(anyDateOfMonth.Year, anyDateOfMonth.Month, 1);
        }

        public int DayNumber { get; set; }

        public DateTime GetDateOfThisMonth(DateTime anyDateOfMonth)
        {
            return BeginingOfMonth(anyDateOfMonth).AddDays(DayNumber - 1);
        }

        public DayOfMonthSettings()
        {
            DayNumber = 1;
        }
    }
}