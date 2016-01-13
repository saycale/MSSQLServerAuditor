using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MSSQLServerAuditor.Utils.Network
{
	public class UdpPinger : IPinger
	{
		public const int DefaultTimeout = 2000;

		private readonly string _machineName;
		private readonly int    _port;
		private readonly int    _timeout;

		private readonly UdpPingHandler _pingHandler;

		private delegate PingResult UdpPingHandler(string host, int port, int timeout);

		public UdpPinger(
			string machineName,
			int    port,
			int    timeoutMillis = 0
		)
		{
			if (timeoutMillis < 0)
			{
				throw new ArgumentOutOfRangeException(
					"timeoutMillis", "Timeout is less than zero."
				);
			}

			this._timeout = timeoutMillis == 0
				? DefaultTimeout
				: timeoutMillis;

			this._machineName = machineName;
			this._port        = port;
			this._pingHandler = PingPort;
		}

		public UdpPinger(string machineName, int port)
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

			using (UdpClient udpClient = new UdpClient())
			{
				udpClient.Client.ReceiveTimeout = timeout;
				stopwatch.Start();

				try
				{
					udpClient.Connect(hostName, port);
					IPEndPoint ipAddress = (IPEndPoint) udpClient.Client.RemoteEndPoint;

					Byte[] sendBytes = Encoding.ASCII.GetBytes("?");
					udpClient.Send(sendBytes, sendBytes.Length);
					IPEndPoint remoteIpEndPoint = new IPEndPoint(ipAddress.Address, port);
					udpClient.Receive(ref remoteIpEndPoint);

					stopwatch.Stop();

					string status = SocketError.Success.ToString();

					return PingResult.Succeeded(
						status,
						ipAddress.Address.ToString(),
						stopwatch.ElapsedMilliseconds
					);
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
					udpClient.Close();
				}
			}
		}
	}
}
