using System;

namespace MSSQLServerAuditor.Graph
{
    /// <summary>
    /// Operating an item ganta.
    /// </summary>
    public class GanttJobItem
    {
        /// <summary>
        /// The field is the name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Start time.
        /// </summary>
        public DateTime Start { get; set; }
        /// <summary>
        /// End time.
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// Initializes the object GanttJobItem.
        /// </summary>
        public GanttJobItem()
        {
        }

        /// <summary>
        /// Initializes the object GanttJobItem.
        /// </summary>
        /// <param name="name">Name object</param>
        /// <param name="start">Start time</param>
        /// <param name="end">End time</param>
        public GanttJobItem(string name, DateTime start, DateTime end)
        {
            Name = name;
            Start = start;
            End = end;
        }
    }
}