using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using SqlCodeGuardAPI;
using log4net;

namespace MSSQLServerAuditor.Utils
{
	internal class CodeGuardAnalyzer
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private readonly API CodeGuard;

		internal event EventHandler<CodeGuardProcessEventArgs>  OnReadObject;
		internal event EventHandler<CodeGuardProcessEventArgs>  OnModuleProcessed;
		internal event EventHandler<CodeGuardCompleteEventArgs> OnAnalyseComplete;
		internal event EventHandler                             OnLoadComplete;

		public CodeGuardAnalyzer()
		{
			CodeGuard = new API();
		}

		internal void ProcessAllDatabase(string server, string database)
		{
			// log.Debug("SQLCodeGuard");
			// log.Debug(server);
			// log.Debug(database);

			try
			{
				CodeGuard.Reset();
				CodeGuard.TabSize = 4;

				CodeGuard.Connect(server, database);

				CodeGuard.OnReadObject = InnerOnReadObject;
				CodeGuard.OnModuleProcessed = InnerOnModuleProcessed;
				CodeGuard.OnAnalyseComplete = InnerOnAnalyseComplete;
				CodeGuard.OnLoadComplete = InnerOnLoadComplete;

				CodeGuard.LoadObjects();

				if (!CodeGuard.AbortRequested)
				{
					CodeGuard.Execute();
				}
			}
			catch (Exception ex)
			{
				log.Error(ex);
			}
		}

		private void InnerOnReadObject(DatabaseObject obj, int total, int current)
		{
			if(OnReadObject != null)
			{
				OnReadObject(CodeGuard, new CodeGuardProcessEventArgs(obj, total, current));
			}
		}

		private void InnerOnModuleProcessed(DatabaseObject obj, int current, int total)
		{
			if (OnModuleProcessed != null)
			{
				OnModuleProcessed(CodeGuard, new CodeGuardProcessEventArgs(obj, total, current));
			}
		}

		private void InnerOnAnalyseComplete(object sender, EventArgs e)
		{
			if (OnAnalyseComplete != null)
			{
				OnAnalyseComplete(CodeGuard, new CodeGuardCompleteEventArgs(CodeGuard.FoundIssues()));
			}
		}

		private void InnerOnLoadComplete()
		{
			if (OnLoadComplete != null)
			{
				OnLoadComplete(CodeGuard, new EventArgs());
			}
		}

		internal string ProcessSqlQuery(string sql, string include = "ALL", string exclude = "", bool includeComplexity = false)
		{
			if (string.IsNullOrEmpty(include))
			{
				include = "ALL";
			}

			CodeGuard.IncludeIssue(include);

			if (!string.IsNullOrEmpty(exclude))
			{
				CodeGuard.ExcludeIssue(exclude);
			}

			var result = new StringBuilder();

			bool unparsed;
			var issues = CodeGuard.GetIssues(sql, out unparsed);

			if (unparsed)
			{
				log.ErrorFormat("SQLCodeGuard: Unparsed! Issue list may be incomplete or wrong. Sql starts with: {0}", sql.Take(30));
			}

			foreach (var i in issues)
			{
				result.AppendLine(string.Format("({0}){1} at {2}:{3} ({4})", i.ErrorCode, i.ErrorText, i.Column, i.Line, i.ErrorMessage));
			}

			if (includeComplexity)
			{
				double complexity     = 0.0;
				int    statementCount = 0;

				CodeGuard.GetComplexity(sql, out unparsed, out complexity, out statementCount);
				result.AppendLine(string.Format("\nComplexity:{0}, Statement Count:{1}", complexity, statementCount));
			}

			return result.ToString();
		}
	}
}
