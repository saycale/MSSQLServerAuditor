using System.Collections.Generic;
using System.Linq;
using MSSQLServerAuditor.Model;

namespace MSSQLServerAuditor.WebServer
{
	abstract class BaseRequest
	{
		private readonly MsSqlAuditorModel _model;

		public BaseRequest(MsSqlAuditorModel model)
		{
			this._model = model;
		}

		public abstract bool CanProcessCommand(string command);

		public MsSqlAuditorModel Model
		{
			get { return _model; }
		}

		public abstract WebAnswer GetRequest(string path, Dictionary<string, string> parameters);

		protected string FormatArray(string[] headers, string[][] data)
		{
			string result = "<table width=\"100%\" border=\"1\">";

			result += "<tr>";
			result = headers.Aggregate(result, (current, h) => current + ("<th>" + h + "</th>"));
			result += "</tr>";

			foreach (string[] row in data)
			{
				result += "<tr>";
				result = row.Aggregate(result, (current, d) => current + ("<td>" + d + "</td>"));
				result += "</tr>";
			}

			result += "</table>";

			return result;
		}
	}
}
