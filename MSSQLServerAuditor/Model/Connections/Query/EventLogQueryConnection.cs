using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using MSSQLServerAuditor.Model.Commands;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Model.Connections.Query
{
	internal class EventLogQueryConnection : IQueryConnection
	{
		private readonly EventLogConnection _eventLogConnection;

		public EventLogQueryConnection(EventLogConnection connection)
		{
			this._eventLogConnection = connection;
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
			return new EventLogCommand(this._eventLogConnection, sqlText);
		}

		private class EventLogCommand : IQueryCommand
		{
			private const string                               ColumnParamPrefix = "@col";
			private static readonly Dictionary<string, string> DefaultMappings;

			static EventLogCommand()
			{
				DefaultMappings = new Dictionary<string, string>
					{
						{"EventID",     "//System/EventID/text()"},
						{"TimeCreated", "//System/TimeCreated/@SystemTime"},
						{"Channel",     "//System/Channel/text()"},
						{"Computer",    "//System/Computer/text()"},
						{"Provider",    "//System/Provider/@Name"},
						{"Level",       "//System/Level/text()"}
					};
			}

			private readonly EventLogConnection         _eventLogConnection;
			private readonly string                     _eventLogQuery;
			private readonly Dictionary<string, string> _mappings;

			public EventLogCommand()
			{
				this._eventLogConnection = null;
				this._eventLogQuery      = null;
				this._mappings           = new Dictionary<string, string>();
			}

			public EventLogCommand(
				EventLogConnection eventLogConnection,
				string             eventLogQuery
			) : this()
			{
				this._eventLogConnection = eventLogConnection;
				this._eventLogQuery      = eventLogQuery;
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
				this._mappings.Clear();

				foreach (ParameterValue parameterValue in parameterValues)
				{
					if (parameterValue.Name.StartsWith(ColumnParamPrefix))
					{
						string columnName = parameterValue.Name.Substring(ColumnParamPrefix.Length);
						string columnPath = parameterValue.StringValue;

						this._mappings.Add(columnName, columnPath);
					}
				}
			}

			public IAsyncResult BeginExecuteReader(AsyncCallback callback)
			{
				AsyncResult<IDataReader> asyncResult = new AsyncResult<IDataReader>(callback, this);

				Task.Factory.StartNew(() =>
				{
					try
					{
						IDataReader dataReader = ExecureEventLogReader();
						asyncResult.Complete(dataReader, false);
					}
					catch (Exception exc)
					{
						asyncResult.HandleException(exc, false);
					}
				});

				return asyncResult;
			}

			public IDataReader EndExecuteReader(IAsyncResult result)
			{
				AsyncResult<IDataReader> asyncResult = (AsyncResult<IDataReader>)result;

				if (asyncResult.Exception != null)
				{
					throw asyncResult.Exception;
				}

				return asyncResult.Result;
			}

			private IDataReader ExecureEventLogReader()
			{
				EventLogQuery eventLogQuery = new EventLogQuery(
					null,
					PathType.LogName,
					this._eventLogQuery.TrimmedOrEmpty()
				);

				EventLogSession session = new EventLogSession(this._eventLogConnection.MachineName);
				eventLogQuery.Session   = session;

				EventLogReader eventLogReader = new EventLogReader(eventLogQuery);

				Dictionary<string, string> mappings = _mappings.Count > 0
					? new Dictionary<string, string>(_mappings)
					: new Dictionary<string, string>(DefaultMappings);

				DataTable dt = new DataTable();
				foreach (KeyValuePair<string, string> mapping in mappings)
				{
					dt.Columns.Add(mapping.Key, typeof(string));
				}

				EventRecord eventInstance = eventLogReader.ReadEvent();

				while (eventInstance != null)
				{
					XmlDocument    eventDoc  = RemoveXmlns(eventInstance.ToXml());
					XPathNavigator navigator = eventDoc.CreateNavigator();
					DataRow        row       = dt.NewRow();

					foreach (KeyValuePair<string, string> mapping in mappings)
					{
						string         column = mapping.Key;
						string         xpath  = mapping.Value;
						XPathNavigator node   = navigator.SelectSingleNode(xpath);

						row[column] = node == null || node.Value == null
							? string.Empty
							: node.Value;
					}

					dt.Rows.Add(row);
					eventInstance = eventLogReader.ReadEvent();
				}

				return dt.CreateDataReader();
			}

			private static XmlDocument RemoveXmlns(String xml)
			{
				XDocument doc = XDocument.Parse(xml);
				Debug.Assert(doc.Root != null, "d.Root != null");

				doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

				foreach (XElement elem in doc.Descendants())
				{
					elem.Name = elem.Name.LocalName;
				}

				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(doc.CreateReader());

				return xmlDocument;
			}

			public void Cancel()
			{
			}
		}
	}
}
