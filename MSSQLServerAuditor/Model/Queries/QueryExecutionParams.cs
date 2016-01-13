using System.Collections.Generic;
using System.Linq;

namespace MSSQLServerAuditor.Model.Queries
{
	public class QueryExecutionParams
	{
		private readonly List<ParameterValue> _values;
		public string    DefaultDatabase { get; private set; }

		private QueryExecutionParams(IEnumerable<ParameterValue> values, string defaultDatabase)
		{
			this._values         = new List<ParameterValue>(values);
			this.DefaultDatabase = defaultDatabase;
		}

		public static QueryExecutionParams CreateFrom(TemplateNodeQueryInfo templateNodeInfo)
		{
			return new QueryExecutionParams(
				templateNodeInfo.ParameterValues,
				templateNodeInfo.TemplateNode.GetDefaultDatabase()
			);
		}

		public IEnumerable<ParameterValue> Values
		{
			get { return this._values.AsReadOnly(); }
		}

		public void AddValues(IEnumerable<ParameterValue> paramValues)
		{
			this._values.AddRange(paramValues);
		}

		public QueryExecutionParams Clone()
		{
			return new QueryExecutionParams(
				this._values.Select(v => v.Clone()),
				DefaultDatabase
			);
		}
	}
}
