using System.Collections.Generic;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Table
{
    /// <summary>
    /// Base data source class for PlainItems
    /// </summary>
    [XmlInclude(typeof(XmlFileTableSource))]
    public abstract class TableSource
    {
        /// <summary>
        /// Extracts PlainItems for the Graph according settings
        /// </summary>
        /// <returns>List of PlainItems as a source data for Graph</returns>
        internal abstract List<Dictionary<ColumnDefinition, object>> GetPlainData(object context);

        internal abstract List<ColumnDefinition> ColumnDefinitions { get; }
    }
}