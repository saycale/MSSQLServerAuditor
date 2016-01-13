using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using MSSQLServerAuditor.Model.Connections.Parameters;

namespace MSSQLServerAuditor.Model.Connections
{
	public enum ProtocolType
	{
		[Description("ICMP (Ping)")]
		Icmp,

		[Description("TCP")]
		Tcp,

		[Description("UDP")]
		Udp
	}

	public class NetworkInformationConnection
	{
		private static readonly string[] AliasesHost     = { "host", "hostName", "machine", "machineName", "server" };
		private static readonly string[] AliasesPort     = { "port" };
		private static readonly string[] AliasesProtocol = { "prot", "type", "protocol" };
		private static readonly string[] AliasesTimeout  = { "timeout" };

		public NetworkInformationConnection(
			string       host,
			int          port,
			ProtocolType protocol,
			int          timeout
		)
		{
			if (string.IsNullOrWhiteSpace(host))
			{
				throw new ArgumentNullException("host");
			}

			if (port < 0 || port > short.MaxValue)
			{
				throw new ArgumentOutOfRangeException("port");
			}

			if (timeout < 0)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}

			this.Host     = host;
			this.Port     = port;
			this.Protocol = protocol;
			this.Timeout  = timeout;
		}

		public static NetworkInformationConnection Parse(string connectionString)
		{
			ParametersParser parser = new ParametersParser(connectionString);
			ProtocolType     protocol;
			int              port = 0;
			int              timeout = 0;

			if (!Enum.TryParse(parser.GetValue(AliasesProtocol), true, out protocol))
			{
				protocol = ProtocolType.Tcp;
			}

			if (!int.TryParse(parser.GetValue(AliasesPort), out port))
			{
				switch (protocol)
				{
					case ProtocolType.Tcp:
					case ProtocolType.Udp:
						port = 80; // assing default port for tcp/udp
						break;

					case ProtocolType.Icmp:
						port = 1;
						break;
				}
			}

			if (!int.TryParse(parser.GetValue(AliasesTimeout), out timeout))
			{
				// timeout is not defined explicitly
				// default value will be used
				timeout = 0;
			}

			string host = parser.GetValue(AliasesHost);

			if (string.IsNullOrEmpty(host))
			{
				host = connectionString; // assume that connection string is a host name
			}

			return new NetworkInformationConnection(
				host,
				port,
				protocol,
				timeout
			);
		}

		/// <summary>
		/// Remote host
		/// </summary>
		public string Host { get; private set; }

		/// <summary>
		/// Remote server port
		/// </summary>
		public int Port { get; private set; }

		/// <summary>
		/// Specifies network protocol to check availability of the remote server
		/// </summary>
		public ProtocolType Protocol { get; private set; }

		/// <summary>
		/// Timeout (in milliseconds)
		/// </summary>
		public int Timeout { get; private set; }

		public string ConnectionString
		{
			get
			{
				List<string> parts = new List<string>();

				parts.Add("host="     + Host);
				parts.Add("protocol=" + Protocol.ToString().ToLower());

				if (Protocol != ProtocolType.Icmp)
				{
					parts.Add("port=" + Port);
				}

				if (Timeout != 0)
				{
					parts.Add("timeout=" + Timeout);
				}

				return string.Join(";", parts);
			}
		}
	}
}
