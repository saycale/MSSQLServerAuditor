using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using log4net;
using MSSQLServerAuditor.Model;

namespace MSSQLServerAuditor.WebServer
{
	internal struct WebAnswer
	{
		private string _mimeType;
		private byte[] _data;

		public WebAnswer(string mimeType, byte[] answer)
		{
			this._mimeType = mimeType;
			this._data     = answer;
		}

		public string MimeType
		{
			get { return this._mimeType; }
		}

		public byte[] Data
		{
			get { return this._data; }
		}
	}

	internal delegate WebAnswer WebServerAnswerDelegate(string request, string path, Dictionary<string, string> parameters);

	/// <summary>
	/// Web server manager
	/// </summary>
	public class WebServerManager
	{
		private static readonly log4net.ILog log            = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private static int                   _webServerPort;
		private WebServerFrontend            _frontend;
		private MsSqlAuditorModel            _model;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="model">MSSQLAuditor model</param>
		public WebServerManager(MsSqlAuditorModel model)
		{
			int    tryCount         = 10;
			bool   webServerStarted = false;
			Random rnd              = new Random();

			this._model    = model;
			_webServerPort = Consts.DefaultWebServerPort;

			for (int i = 0; i < tryCount; i++)
			{
				try
				{
					this._frontend = new WebServerFrontend(_webServerPort, ProcessRequest);

					log.InfoFormat("Web-server started at port:'{0}'",
						_webServerPort
					);

					webServerStarted = true;

					break;
				}
				catch (Exception ex)
				{
					log.ErrorFormat("Unable to start web-server at port:'{0}'.Error:'{1}'",
						_webServerPort,
						ex
					);
				}

				_webServerPort = 10000 + rnd.Next(50000);
			}

			if (!webServerStarted)
			{
				log.ErrorFormat(
					"Unable to start web-server for '{0}' times",
					tryCount
				);
			}
		}

		/// <summary>
		/// Port for web server
		/// </summary>
		public static int WebServerPort
		{
			get { return _webServerPort; }
		}

		WebAnswer ProcessRequest(string request, string path, Dictionary<string, string> parameters)
		{
			try
			{
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					foreach (Type type in assembly.GetTypes())
					{
						if ((type.IsSubclassOf(typeof(BaseRequest))) && (!type.IsAbstract))
						{
							ConstructorInfo constructor = type.GetConstructor(new Type[] {typeof(MsSqlAuditorModel) });

							if (constructor != null)
							{
								BaseRequest data = (BaseRequest)constructor.Invoke(
									new object[]
									{
										this._model
									}
								);

								if (data.CanProcessCommand(request))
								{
									return data.GetRequest(path, parameters);
								}
							}
						}
					}
				}

				return new WebAnswer("text/html", Encoding.Default.GetBytes("Unknown command."));
			}
			catch (Exception ex)
			{
				log.Error(ex);

				return new WebAnswer();
			}
		}
	}
}
