using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MSSQLServerAuditor.Entities
{
    internal class BuildDateAttribute : Attribute
    {
        public DateTime BuildDate { get; private set; }

        public BuildDateAttribute(string buildDate)
        {
            DateTime datetime = DateTime.Now;
            if (!string.IsNullOrEmpty(buildDate))
            {
                if (DateTime.TryParseExact(buildDate, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out datetime))
                {
                    datetime = datetime.Date.AddSeconds(-1).AddDays(1);
                }
                BuildDate = datetime;
            }
        }
    }
}
