using System;
using System.Xml;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Model.Scheduling
{
    [Serializable]
    public class DailyFrequency
    {
        public static readonly TimeSpan DayLength = new TimeSpan(
            days:    1,
            hours:   0,
            minutes: 0,
            seconds: 0
        );

        public enum TimeUnit
        {
            Hour,
            Minute,
            Second
        }

        private static TimeSpan GetDuration(TimeUnit timeUnit, int count)
        {
            switch (timeUnit)
            {
                case TimeUnit.Hour:
                    return new TimeSpan(0, count, 0, 0);
                case TimeUnit.Minute:
                    return new TimeSpan(0, 0, count, 0);
                case TimeUnit.Second:
                    return new TimeSpan(0, 0, 0, count);
                default:
                    throw new Exception("Unforeseen enum count");
            }
        }

        public bool OccursOnce { get; set; }

        [XmlIgnore]
        public TimeSpan OccursOnceTime { get; set; }

        [XmlElement("OccursOnceTime")]
        public string OccursOnceTimeAsDateTime
        {
            get
            {
                return XmlConvert.ToString(this.OccursOnceTime);
            }
            set
            {
                this.OccursOnceTime = XmlConvert.ToTimeSpan(value);
            }
        }

        public TimeUnit? PeriodTimeUnit { get; set; }

        public int? PeriodTimeUnitCount { get; set; }

        [XmlIgnore]
        public TimeSpan? StartingAt { get; set; }

        [XmlElement("StartingAt")]
        public string StartingAtAsDateTime
        {
            get
            {
                return this.StartingAt != null
                           ? XmlConvert.ToString(this.StartingAt.Value)
                           : null;
            }
            set
            {
                this.StartingAt = XmlConvert.ToTimeSpan(value);
            }
        }

        [XmlIgnore]
        public TimeSpan? EndingAt { get; set; }

        [XmlElement("EndingAt")]
        public string EndingAtAsDateTime
        {
            get
            {
                return this.EndingAt != null
                           ? XmlConvert.ToString(this.EndingAt.Value)
                           : null;
            }
            set
            {
                this.EndingAt = XmlConvert.ToTimeSpan(value);
            }
        }

        internal bool GetNextTime(TimeSpan searchStart, bool inclidingSearchStart, out TimeSpan nextTime)
        {
            if (this.OccursOnce)
            {
                nextTime = this.OccursOnceTime;
                return searchStart < this.OccursOnceTime;
            }

            if (this.PeriodTimeUnit == null || this.PeriodTimeUnitCount == null)
            {
                throw new InvalidOperationException();
            }

            if (this.StartingAt != null && searchStart < this.StartingAt)
            {
                nextTime = this.StartingAt.Value;
                return true;
            }

            var periodLength = GetDuration(this.PeriodTimeUnit.Value, this.PeriodTimeUnitCount.Value).Ticks;
            var startedAt = this.StartingAt ?? TimeSpan.Zero;
            var periodsPasssed = (searchStart - startedAt).Ticks / (double)periodLength;

            if (inclidingSearchStart)
            {
                nextTime = Math.Abs(periodsPasssed - (int) periodsPasssed) < 1e-12
                    ? searchStart
                    : startedAt + new TimeSpan(periodLength*((int) periodsPasssed + 1));
            }
            else
            {
                nextTime = startedAt + new TimeSpan(periodLength * ((int)periodsPasssed + 1));
            }

            return nextTime <= (this.EndingAt ?? DayLength);
        }

        public DailyFrequency()
        {
            this.OccursOnceTime = TimeSpan.Zero;
        }
    }
}