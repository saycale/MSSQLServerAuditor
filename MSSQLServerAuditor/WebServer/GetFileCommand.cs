using System.Collections.Generic;
using System.IO;
using System.Text;
using log4net;
using MSSQLServerAuditor.Model;

namespace MSSQLServerAuditor.WebServer
{
	internal class GetFileCommand : BaseRequest
	{
		private static readonly log4net.ILog log         = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		const string                         CommandName = "file";

		public GetFileCommand(MsSqlAuditorModel model) : base(model)
		{
		}

		public override bool CanProcessCommand(string command)
		{
			return command.ToLower() == CommandName;
		}

		public override WebAnswer GetRequest(string path, Dictionary<string, string> parameters)
		{
			string   fileName = Path.Combine(Program.Model.FilesProvider.GetJsFolder(), path.Replace('/', Path.DirectorySeparatorChar));
			FileInfo fi       = new FileInfo(fileName);
			byte[]   result   = new byte[fi.Length];

			using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				fileStream.Read(result, 0, result.Length);
			}

			return new WebAnswer("", result);
		}

		public static string GetWebPath()
		{
			string strUrl = string.Format(
				"http://localhost:{0}/{1}",
				WebServerManager.WebServerPort,
				CommandName
			);

			log.DebugFormat("url:{0}", strUrl);

			return strUrl;
		}
	}
}
