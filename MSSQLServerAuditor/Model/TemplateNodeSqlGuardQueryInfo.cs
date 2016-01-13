using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Model
{
    /// <summary>
    /// Template node query definition
    /// </summary>
    [Serializable]
    public class TemplateNodeSqlGuardQueryInfo: TemplateNodeQueryInfo
    {
        private string _sqlQueryId;
        private string _queryCodeColumn;
        private string _queryObjectColumn;
        private bool _addSummary;
        private string _includedIssue;
        private string _excludedIssue;

        /// <summary>
        /// Id of sql-select query which will be analyzed
        /// </summary>
        [XmlAttribute(AttributeName = "sql-select-id")]
        public string SqlQueryId
        {
            get { return _sqlQueryId; }
            set { _sqlQueryId = value; }
        }

        /// <summary>
        /// Column in sql-select query result which contains code for analyzing
        /// </summary>
        [XmlAttribute(AttributeName = "code-column")]
        public string QueryCodeColumn
        {
            get { return _queryCodeColumn; }
            set { _queryCodeColumn = value; }
        }

        public string UserQueryCodeColumn { get; set; }

        public string GetQueryCodeColumn()
        {
            return UserQueryCodeColumn ?? QueryCodeColumn;
        }

        /// <summary>
        /// Column in sql-select query result which contains name of analyzing object
        /// </summary>
        [XmlAttribute(AttributeName = "object-column")]
        public string QueryObjectColumn
        {
            get { return _queryObjectColumn; }
            set { _queryObjectColumn = value; }
        }

        public string UserObjectColumn { get; set; }

        public string GetObjectColumn()
        {
            return UserObjectColumn ?? QueryObjectColumn;
        }

        /// <summary>
        /// Column in sql-select query result which contains name of analyzing object
        /// </summary>
        [XmlAttribute(AttributeName = "add-summary")]
        public bool AddSummary
        {
            get { return _addSummary; }
            set { _addSummary = value; }
        }

        public bool? UserAddSummary { get; set; }

        public bool GetAddSummary()
        {
            return UserAddSummary ?? AddSummary;
        }

        /// <summary>
        /// Type of sqlcodeguard-select report
        /// </summary>
        [XmlAttribute(AttributeName = "include")]
        public string IncludedIssue
        {
            get { return _includedIssue; }
            set { _includedIssue = value; }
        }

        public string UserIncludedIssue { get; set; }

        public string GetIncludedIssue()
        {
            return UserIncludedIssue ?? IncludedIssue;
        }

        /// <summary>
        /// Type of sqlcodeguard-select report
        /// </summary>
        [XmlAttribute(AttributeName = "exclude")]
        public string ExcludedIssue
        {
            get { return _excludedIssue; }
            set { _excludedIssue = value; }
        }

        public string UserExcludedIssue { get; set; }

        public string GetExcludedIssue()
        {
            return UserExcludedIssue ?? ExcludedIssue;
        }

        /// <summary>
        /// Objects to string.
        /// </summary>
        /// <returns>String objects.</returns>
        public override string ToString()
        {
            return string.Format("QueryName={0} SqlQueryId={1} QueryCodeColumn={2}", QueryName, SqlQueryId, QueryCodeColumn)
                + string.Join(" ", ParameterValues.Select(pv => pv.ToString()));
        }
    }
}