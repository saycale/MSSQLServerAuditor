using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using log4net;

namespace MSSQLServerAuditor.WebServer
{
	internal class WebServerFrontend
	{
		private static readonly log4net.ILog     log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private readonly WebServerAnswerDelegate _answerDelegate;
		private readonly TcpListener             _myListener;
		private readonly int                     _port;

		public WebServerFrontend(int port, WebServerAnswerDelegate answerDelegate)
		{
			this._port           = port;
			this._answerDelegate = answerDelegate;
			this._myListener     = new TcpListener(IPAddress.Loopback, _port);

			this._myListener.Start();

			Thread th = new Thread(StartListen);

			th.IsBackground = true;
			th.Start();
		}

		private string GetDefaultMimeType(string extension)
		{
			switch (extension)
			{
				case ".htm":
				case ".html":
					return "text/html";

				case ".css":
					return "text/css";

				case ".js":
					return "text/javascript";

				case ".jpg":
					return "image/jpeg";

				case ".jpeg":
				case ".png":
				case ".gif":
					return "image/" + extension.Substring(1);

				default:
					if (extension.Length > 1)
					{
						return "application/" + extension.Substring(1);
					}
					else
					{
						return "application/unknown";
					}
			}
		}

		private void SendHeader(
			string     extension,
			string     sHttpVersion,
			string     sMIMEHeader,
			int        iTotBytes,
			string     sStatusCode,
			ref Socket mySocket
		)
		{
			String sBuffer = "";

			if (sMIMEHeader.Length == 0)
			{
				sMIMEHeader = GetDefaultMimeType(extension);
			}

			sBuffer = sBuffer + sHttpVersion + sStatusCode + "\r\n";
			sBuffer = sBuffer + "Server: cx1193719-b\r\n";
			sBuffer = sBuffer + "Content-Type: " + sMIMEHeader + "\r\n";
			sBuffer = sBuffer + "Accept-Ranges: bytes\r\n";
			sBuffer = sBuffer + "Content-Length: " + iTotBytes + "\r\n\r\n";

			Byte[] bSendData = Encoding.ASCII.GetBytes(sBuffer);

			SendToBrowser(bSendData, ref mySocket);
		}

		private void SendToBrowser(String sData, ref Socket mySocket)
		{
			SendToBrowser(Encoding.UTF8.GetBytes(sData), ref mySocket);
		}

		private void SendToBrowser(Byte[] bSendData, ref Socket mySocket)
		{
			try
			{
				if (mySocket.Connected)
				{
					mySocket.Send(bSendData, bSendData.Length, 0);
				}
			}
			catch (Exception ex)
			{
				log.Error("Webserver error at send", ex);
			}
		}

		private void StartListen()
		{
			while (true)
			{
				Socket mySocket = this._myListener.AcceptSocket();

				mySocket.ReceiveTimeout = 2000;

				try
				{
					if (mySocket.Connected)
					{
						Byte[] bReceive = new Byte[1024];
						int    i        = mySocket.Receive(bReceive, bReceive.Length, 0);
						string sBuffer  = Encoding.UTF8.GetString(bReceive);

						if (sBuffer.Substring(0, 3).CompareTo("GET") == 0)
						{
							int                        iStartPos    = sBuffer.IndexOf("HTTP", 1);
							string                     sHttpVersion = sBuffer.Substring(iStartPos, 8);
							String                     url          = GetUrl(sBuffer.Substring(0, iStartPos - 1));
							string                     request      = GetRequest(url);
							string                     path         = GetPath(url);
							Dictionary<string, string> parameters   = GetParameters(url);
							string                     extension    = url.Substring(url.LastIndexOf('.'));
							WebAnswer                  answer       = _answerDelegate(request, path, parameters);

							SendHeader(
								extension,
								sHttpVersion,
								answer.MimeType,
								answer.Data.Length,
								" 200 OK",
								ref mySocket
							);

							SendToBrowser(
								answer.Data,
								ref mySocket
							);
						}
					}
				}
				catch (Exception ex)
				{
					log.Error(ex);
				}
				finally
				{
					mySocket.Close();
				}
			}
		}

		private static string GetUrl(string request)
		{
			int    space1 = request.IndexOf(" ");
			string url    = request.Substring(space1 + 2, request.Length - space1 - 2);

			return url;
		}

		private string GetRequest(string url)
		{
			string[] results = url.Split('?');

			return results[0].Split('/')[0];
		}

		private string GetPath(string url)
		{
			string[] results  = url.Split('?');
			var      cmdIndex = results[0].IndexOf('/');

			return results[0].Substring(cmdIndex + 1);
		}

		private Dictionary<string, string> GetParameters(string url)
		{
			string[] strings = url.Split('?');
			var      result  = new Dictionary<string, string>();

			if (strings.Length > 1)
			{
				string[] prms = strings[1].Split('&');

				foreach (string prm in prms)
				{
					string[] pair = prm.Split('=');

					result.Add(pair[0], pair[1]);
				}
			}

			return result;
		}
	}
}
