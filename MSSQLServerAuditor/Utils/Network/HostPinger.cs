using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace MSSQLServerAuditor.Utils.Network
{
	internal class HostPinger : IPinger
	{
		public const int  DefaultTtl           = 32;
		public const int  DefaultTimeout       = 100;
		public const bool DefaultFragmentation = false;
		public const int  DefaultBufferSize    = 32;

		private readonly string          _host;
		private readonly int             _timeout;
		private readonly HostPingHandler _hostPingHandler;

		private delegate PingResult HostPingHandler(string host, int timeout);

		public HostPinger(string host, int timeoutMillis = 0)
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

			this._host            = host;
			this._hostPingHandler = PingHost;
		}

		public HostPinger(string host) : this(host, DefaultTimeout)
		{
		}

		public IAsyncResult BeginPing(AsyncCallback callback, object state)
		{
			return this._hostPingHandler.BeginInvoke(
				this._host,
				this._timeout,
				callback,
				state
			);
		}

		public PingResult EndPing(IAsyncResult asyncResult)
		{
			return this._hostPingHandler.EndInvoke(asyncResult);
		}

		private PingResult PingHost(string host, int timeout)
		{
			PingOptions options = new PingOptions(DefaultTtl, DefaultFragmentation);
			byte[]      buffer  = Enumerable.Repeat(Convert.ToByte('&'), DefaultBufferSize).ToArray();

			using (Ping pinger = new Ping())
			{
				try
				{
					PingReply reply = pinger.Send(
						host,
						timeout,
						buffer,
						options
					);

					if (reply.Status == IPStatus.Success)
					{
						return PingResult.Succeeded(
							IPStatus.Success.ToString(),
							reply.Address.ToString(),
							reply.RoundtripTime
						);
					}

					return PingResult.Failed(
						reply.Status.ToString(),
						string.Empty,
						-1
					);
				}
				catch (PingException exc)
				{
					SocketException socketException = exc.InnerException as SocketException;

					if (socketException != null)
					{
						return PingResult.Failed(
							socketException.SocketErrorCode.ToString(),
							socketException.Message,
							-1
						);
					}

					throw;
				}
				catch (Exception exc)
				{
					return PingResult.Failed(
						IPStatus.Unknown.ToString(),
						exc.Message,
						-1
					);
				}
			}
		}
	}
}
