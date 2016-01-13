using System;

namespace MSSQLServerAuditor.Model.Scheduling
{
    [Serializable]
    public enum ReccurPeriodTimeUnit
    {
        Daily = 1,
        Weekly = 2,
        Monthly = 3
    };
}