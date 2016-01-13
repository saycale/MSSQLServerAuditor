using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using log4net;
using MSSQLServerAuditor.Model;

namespace MSSQLServerAuditor.WebServer
{
	internal class GetResultCommand : BaseRequest
	{
		private static readonly log4net.ILog log         = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		const string                         CommandName = "result";

		public GetResultCommand(MsSqlAuditorModel model) : base(model)
		{
		}

		public override bool CanProcessCommand(string command)
		{
			return command.ToLower() == CommandName;
		}

		public override WebAnswer GetRequest(string path, Dictionary<string, string> parameters)
		{
			var result = Model.DbFs.ReadFile(path.Replace('/', Path.DirectorySeparatorChar));

			return new WebAnswer("", result);
		}

		public static string GetWebPath(string path)
		{
			string strUrl = string.Format(
				"http://localhost:{0}/{1}/{2}",
				WebServerManager.WebServerPort,
				CommandName,
				path.Replace(Path.DirectorySeparatorChar, '/').Replace(@"\?\", String.Empty).Replace(@"/?/", String.Empty)
			);

			log.DebugFormat("url:{0}", strUrl);

			//
			// ticket #246 - exception by XML file load
			//
			// return string.Format(
			//    "http://localhost:{0}/{1}/{2}",
			//    WebServerManager.WebServerPort,
			//    CommandName,
			//    path.Replace(Path.DirectorySeparatorChar,'/')
			// );
			//

			return strUrl;
		}
	}
}
