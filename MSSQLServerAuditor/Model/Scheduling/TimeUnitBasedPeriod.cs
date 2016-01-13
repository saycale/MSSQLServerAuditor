using System;

namespace MSSQLServerAuditor.Model.Scheduling
{
    [Serializable]
    public class TimeUnitBasedPeriod
    {
        public ReccurPeriodTimeUnit TimeUnit { get; set; }
        public int TimeUnitCount { get; set; }

        public TimeUnitBasedPeriod()
        {
            TimeUnitCount = 1;
            TimeUnit = ReccurPeriodTimeUnit.Daily;
        }

        public TimeUnitBasedPeriod(ReccurPeriodTimeUnit timeUnit, int timeUnitCount)
        {
            TimeUnit = timeUnit;
            TimeUnitCount = timeUnitCount;
        }
    }
}