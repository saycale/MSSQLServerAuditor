using System;
using MSSQLServerAuditor.SQLite.Common;

namespace MSSQLServerAuditor.SQLite.Databases
{
	public class HistoryStorage : Database
	{
		private string _alias;
		private string _buildScript;

		public HistoryStorage(
			string fileName,
			string alias,
			string buildScript,
			bool   readOnly = false
		) : base(
				fileName,
				readOnly
			)
		{
			this._alias       = alias;
			this._buildScript = buildScript;
		}

		public string Alias
		{
			get
			{
				return this._alias;
			}
		}

		public string BuildScript
		{
			get
			{
				return this._buildScript;
			}
		}

		public override void Initialize()
		{
			this.RunBuildScripts();
		}

		private void RunBuildScripts()
		{
			//
			// ticket #390: service requirements
			// Program.Model is NULL for a service
			//
			if (Program.Model != null)
			{
				if (Program.Model.Settings != null)
				{
					if (Program.Model.Settings.SystemSettings != null)
					{
						if (!string.IsNullOrEmpty(BuildScript))
						{
							RunBuildScripts(BuildScript);
						}
					}
				}
			}
		}
	}
}
