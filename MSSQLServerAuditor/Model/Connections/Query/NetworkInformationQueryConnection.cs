using System;
using System.Collections.Generic;
using System.Data;
using MSSQLServerAuditor.Model.Commands;
using MSSQLServerAuditor.Utils;
using MSSQLServerAuditor.Utils.Network;

namespace MSSQLServerAuditor.Model.Connections.Query
{
	internal class NetworkInformationQueryConnection : IQueryConnection
	{
		private readonly NetworkInformationConnection _networkInformationConnection;

		public NetworkInformationQueryConnection(NetworkInformationConnection connection)
		{
			this._networkInformationConnection = connection;
		}

		public void Dispose()
		{
		}

		public void Open()
		{
		}

		public void ChangeDatabase(string database)
		{
		}

		public void Close()
		{
		}

		public ConnectionState? State
		{
			get { return ConnectionState.Open; }
		}

		public IQueryCommand GetCommand(
			string                          sqlText,
			int                             commandTimeout,
			IEnumerable<QueryParameterInfo> parameters,
			out List<Tuple<int, string>>    parametersQueueForODBC
		)
		{
			parametersQueueForODBC = new List<Tuple<int, string>>();

			return new NetworkInformationCommand(this._networkInformationConnection);
		}

		private class NetworkInformationCommand : IQueryCommand
		{
			private const string ColHost          = "Host";
			private const string ColAddress       = "Address";
			private const string ColStatus        = "NetworkStatus";
			private const string ColRoundtripTime = "RoundtripTime";
			private const string ColEventTime     = "EventTime";

			private readonly NetworkInformationConnection _networkInformationConnection;
			private readonly IPinger                      _pinger;

			public NetworkInformationCommand(
				NetworkInformationConnection networkInformationConnection
			)
			{
				this._networkInformationConnection = networkInformationConnection;

				this._pinger = CreatePinger(networkInformationConnection);
			}

			public void Dispose()
			{
			}

			public void AssignParameters(
				IEnumerable<QueryParameterInfo> parameters,
				IEnumerable<ParameterValue>     parameterValues,
				List<Tuple<int, string>>        parametersQueueForOdbc
			)
			{
			}

			public IAsyncResult BeginExecuteReader(AsyncCallback callback)
			{
				return this._pinger.BeginPing(
					callback,
					this
				);
			}

			public IDataReader EndExecuteReader(IAsyncResult asyncResult)
			{
				PingResult pingResult = this._pinger.EndPing(asyncResult);

				DataTable dt = new DataTable();

				dt.Columns.Add(new DataColumn(ColHost,          typeof(string)));
				dt.Columns.Add(new DataColumn(ColAddress,       typeof(string)));
				dt.Columns.Add(new DataColumn(ColStatus,        typeof(string)));
				dt.Columns.Add(new DataColumn(ColRoundtripTime, typeof(string)));
				dt.Columns.Add(new DataColumn(ColEventTime,     typeof(DateTime)));

				DataRow row           = dt.NewRow();
				row[ColHost]          = this._networkInformationConnection.Host;
				row[ColAddress]       = pingResult.IpAddress.ToSafeString();
				row[ColStatus]        = pingResult.Status.ToSafeString();
				row[ColRoundtripTime] = pingResult.ElapsedMillis.ToSafeString();
				row[ColEventTime]     = DateTime.Now;

				dt.Rows.Add(row);

				return dt.CreateDataReader();
			}

			public void Cancel()
			{
			}

			private IPinger CreatePinger(NetworkInformationConnection connection)
			{
				switch (connection.Protocol)
				{
					case ProtocolType.Tcp:
						return new TcpPinger(connection.Host, connection.Port, connection.Timeout);

					case ProtocolType.Udp:
						return new UdpPinger(connection.Host, connection.Port, connection.Timeout);

					default:
						return new HostPinger(connection.Host, connection.Timeout);
				}
			}
		}
	}
}
