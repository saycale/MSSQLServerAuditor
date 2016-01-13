using System;
using System.Collections.Generic;
using System.Linq;

namespace MSSQLServerAuditor.Model.Connections.Parameters
{
	public class ParametersParser
	{
		private readonly Dictionary<string, string> _keyValuePairs;

		public ParametersParser(string connectionString)
		{
			this.ConnectionString = connectionString;

			this._keyValuePairs = connectionString.Split(';')
				.Where(kvp => kvp.Contains('='))
				.Select(kvp => kvp.Split(new[] { '=' }, 2))
				.ToDictionary(
					kvp => kvp[0].Trim(),
					kvp => kvp[1].Trim(),
					StringComparer.InvariantCultureIgnoreCase
				);
		}

		public string ConnectionString { get; private set; }

		public string GetValue(params string[] keyAliases)
		{
			foreach (string alias in keyAliases)
			{
				string value = string.Empty;

				if (this._keyValuePairs.TryGetValue(alias, out value))
				{
					return value;
				}
			}

			return string.Empty;
		}
	}
}
