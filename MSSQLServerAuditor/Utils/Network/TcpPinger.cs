using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MSSQLServerAuditor.Utils.Network
{
	public class TcpPinger : IPinger
	{
		public const int DefaultTimeout = 2000;

		private readonly string         _machineName;
		private readonly int            _port;
		private readonly int            _timeout;
		private readonly TcpPingHandler _pingHandler;

		private delegate PingResult TcpPingHandler(string host, int port, int timeout);

		public TcpPinger(
			string machineName,
			int    port,
			int    timeoutMillis = 0)
		{
			if (timeoutMillis < 0)
			{
				throw new ArgumentOutOfRangeException(
					"timeoutMillis", "Timeout is less than zero.");
			}

			this._timeout = timeoutMillis == 0
				? DefaultTimeout
				: timeoutMillis;

			this._machineName = machineName;
			this._port        = port;
			this._pingHandler = PingPort;
		}

		public TcpPinger(string machineName, int port)
			: this(machineName, port, DefaultTimeout)
		{
		}

		public IAsyncResult BeginPing(AsyncCallback callback, object state)
		{
			return this._pingHandler.BeginInvoke(
				this._machineName,
				this._port,
				this._timeout,
				callback,
				state
			);
		}

		public PingResult EndPing(IAsyncResult asyncResult)
		{
			return this._pingHandler.EndInvoke(asyncResult);
		}

		private PingResult PingPort(string hostName, int port, int timeout)
		{
			Stopwatch stopwatch = new Stopwatch();

			using (TcpClient tcpClient = new TcpClient())
			{
				stopwatch.Start();

				IAsyncResult asyncResult = tcpClient.BeginConnect(hostName, port, null, null);
				WaitHandle   waitHandle  = asyncResult.AsyncWaitHandle;

				try
				{
					if (!waitHandle.WaitOne(timeout, false))
					{
						tcpClient.Close();
						throw new SocketException(10060); // timeout error
					}

					tcpClient.EndConnect(asyncResult);
				}
				catch (SocketException exc)
				{
					stopwatch.Stop();

					string errorStatus = exc.SocketErrorCode.ToString();

					return PingResult.Failed(
						errorStatus,
						exc.Message,
						-1
					);
				}
				finally
				{
					waitHandle.Close();
				}

				stopwatch.Stop();

				string status    = SocketError.Success.ToString();
				string ipAddress = ((IPEndPoint) tcpClient.Client.RemoteEndPoint).Address.ToString();

				return PingResult.Succeeded(
					status,
					ipAddress,
					stopwatch.ElapsedMilliseconds
				);
			}
		}
	}
}
