using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Utils;
using log4net;
using MSSQLServerAuditor.BusinessLogic;
using MSSQLServerAuditor.WebServer;

namespace MSSQLServerAuditor.Preprocessor
{
	/// <summary>
	/// Xsl preprocessor for inject graph image in HTML report
	/// </summary>
	public class HtmlPreprocessorDialog : Preprocessor
	{
		private static readonly ILog          log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly string               _pathToHtml;
		private readonly XslPreprocessManager _preprocessManager;

		private class HtmlContentFactory : ContentFactory
		{
			private readonly string _htmlPath;

			public HtmlContentFactory(
				IPreprocessor preprocessor,
				string        id,
				string        configuration,
				string        htmlPath
			) : base(
				preprocessor,
				id,
				configuration
			)
			{
				this._htmlPath = htmlPath;
			}

			public override Control CreateControl()
			{
				return CreateControl(this._htmlPath);
			}

			private Control CreateControl(string htmlFileName)
			{
				WebBrowser control = new WebBrowser();

				//
				// disable JavaScript error in WebBrowser control
				//
				control.ScriptErrorsSuppressed = true;

				control.Navigate(GetResultCommand.GetWebPath(htmlFileName));

				return control;
			}

			public override MailContent CreateMailContent()
			{
				MailContent mailContent = new MailContent();

				string fullHtmlPath  = GetResultCommand.GetWebPath(this._htmlPath);
				string htmlStr       = DownloadHtml(fullHtmlPath);
				string innerBodyHtml = GetInnerBodyHtml(htmlStr);

				mailContent.Message = string.Format(
					"<br>Html Report:<br><div>{0}</div>",
					innerBodyHtml
				);

				return mailContent;
			}

			private string GetInnerBodyHtml(string htmlStr)
			{
				int    p1 = htmlStr.IndexOf("<body>");
				int    p2 = htmlStr.IndexOf("</body>");
				string rv = htmlStr.Substring(p1 + 6, p2 - p1 - 6);

				return rv;
			}

			private string DownloadHtml(string url)
			{
				WebClient client = new WebClient();

				client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

				using (Stream data = client.OpenRead(url))
				{
					using (StreamReader reader = new StreamReader(data))
					{
						return reader.ReadToEnd();
					}
				}
			}

			public override bool CanCreateMailContent
			{
				get { return true; }
			}
		}

		/// <summary>
		/// Html preprocessor dialog.
		/// </summary>
		public HtmlPreprocessorDialog(
			NodeDataProvider     dataProvider,
			GraphicsInfo         graphicsInfo,
			XslPreprocessManager preprocessManager
		) : base(
				dataProvider,
				graphicsInfo
			)
		{
			this._pathToHtml        = GetDbfsFolderName();
			this._preprocessManager = preprocessManager;
		}

		public override IContentFactory CreateContentFactory(string id, string configuration)
		{
			string htmlPath = Path.Combine(this._pathToHtml, id + ".html");

			SaveHtmlContent(htmlPath, configuration);

			return new HtmlContentFactory(
				this,
				id,
				configuration,
				htmlPath
			);
		}

		private void SaveHtmlContent(string htmlFileName, string configuration)
		{
			XslCompiledTransform xsl = new XslCompiledTransform();

			using (XmlReader xmlReader = this._preprocessManager.PreprocessXslString(
				configuration.Replace("$JS_FOLDER$", GetFileCommand.GetWebPath()))
			)
			{
				xsl.Load(xmlReader);
			}

			try
			{
				XsltArgumentList list    = new XsltArgumentList();
				XmlDocument      xmlData = this._dataProvider.XmlDocument;

				using (MemoryStream stream = new MemoryStream())
				{
					XmlNode docEmenent = null;

					if (xmlData != null)
					{
						docEmenent = xmlData.DocumentElement;
					}
					else
					{
						docEmenent = new XmlDocument();
					}

					//
					// #248 - fix memory leaks during XML files processing
					//
					// XmlReader reader = new XmlNodeReader(docEmenent);
					// xsl.Transform(reader, list, stream);
					using (XmlReader reader = new XmlNodeReader(docEmenent))
					{
						xsl.Transform(reader, list, stream);
					}

					stream.Position = 0;

					this._preprocessManager.DbFs.WriteStream(htmlFileName, stream);
				}
			}
			catch (PathTooLongException ex)
			{
				log.ErrorFormat("PathTooLongException:htmlFileName:{0};Exception:{1}",
					htmlFileName,
					ex
				);

				throw;
			}
		}

		private string GetDbfsFolderName()
		{
			ConcreteTemplateNodeDefinition nodeDefinition = this._dataProvider.NodeDefinition;

			string dbfsFolderName = Path.GetDirectoryName(
				GetFileName(
					nodeDefinition.TemplateNode,
					nodeDefinition.Connection
				)
			);

			return dbfsFolderName;
		}

		private string ComposeFileName(
			string              directory,
			ConnectionGroupInfo connectionGroup
		)
		{
			string strFilePath = string.Format("MSSQLServerAuditor.{0}",
				connectionGroup.Identity
			);

			return Path.Combine(
				directory,
				strFilePath
			);
		}

		private string GetFileName(
			TemplateNodeInfo    node,
			ConnectionGroupInfo connectionGroup
		)
		{
			string rootFolder = string.Empty;

			do
			{
				Debug.Assert(node.IsInstance);

				string strFilePath = string.Format("{0}",
					node.TemplateNodeId
				);

				rootFolder = Path.Combine(
					strFilePath.GetValidFileName(),
					rootFolder
				);

				node = node.Parent;

			} while (node != null);

			rootFolder = Path.Combine(connectionGroup.Identity.ToSafeString(), rootFolder);

			return ComposeFileName(rootFolder, connectionGroup);
		}
	}
}
