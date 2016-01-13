using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using log4net;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Model.Loaders
{
	/// <summary>
	/// Loader for queries
	/// </summary>
	public class QueriesLoader
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// Loader root wrapper.
		/// </summary>
		[XmlRoot(ElementName = "root")]
		public class LoaderRootWrapper
		{
			private List<LoaderMSSQLQueryWrapper> _sqlInfos;

			/// <summary>
			/// Records for sql queries.
			/// </summary>
			[XmlElement(ElementName = "sqlquery")]
			public List<LoaderMSSQLQueryWrapper> SqlInfos
			{
				get { return this._sqlInfos; }
				set { this._sqlInfos = value; }
			}

			/// <summary>
			/// Records for MS SQL queries
			/// </summary>
			public List<LoaderMSSQLQueryWrapper> MSSQLInfos
			{
				get { return GetQueriesOfType(QuerySource.MSSQL); }
			}

			/// <summary>
			/// Records for TeraData SQL queries
			/// </summary>
			public List<LoaderMSSQLQueryWrapper> TDSQLInfos
			{
				get { return GetQueriesOfType(QuerySource.TDSQL); }
			}

			/// <summary>
			/// Records for SQLite queries
			/// </summary>
			[XmlElement(ElementName = "SQLiteQuery")]
			public List<LoaderMSSQLQueryWrapper> SQLiteInfos
			{
				get { return GetQueriesOfType(QuerySource.SQLite); }
			}

			/// <summary>
			/// Records for ActiveDirectory queries
			/// </summary>
			[XmlElement(ElementName = "ActiveDirectoryQuery")]
			public List<LoaderMSSQLQueryWrapper> ActiveDirectoryInfos
			{
				get { return GetQueriesOfType(QuerySource.ActiveDirectory); }
			}

			/// <summary>
			/// Records for SQLite external queries
			/// </summary>
			[XmlElement(ElementName = "SQLiteExternalQuery")]
			public List<LoaderMSSQLQueryWrapper> SQLiteExternalInfos
			{
				get { return GetQueriesOfType(QuerySource.SQLiteExternal); }
			}

			/// <summary>
			/// Records for EventLog queries
			/// </summary>
			[XmlElement(ElementName = "EventLogQuery")]
			public List<LoaderMSSQLQueryWrapper> EventLogInfos
			{
				get { return GetQueriesOfType(QuerySource.EventLog); }
			}

			/// <summary>
			/// Records for NetworkInformation queries
			/// </summary>
			[XmlElement(ElementName = "NetworkInformationQuery")]
			public List<LoaderMSSQLQueryWrapper> NetworkInformationInfos
			{
				get { return GetQueriesOfType(QuerySource.NetworkInformation); }
			}

			public List<LoaderMSSQLQueryWrapper> GetQueriesOfType(QuerySource querySource)
			{
				return _sqlInfos.Where(x => x.DBType == querySource).ToList();
			}
		}

		/// <summary>
		/// Loader MS SQL query wrapper.
		/// </summary>
		public class LoaderMSSQLQueryWrapper
		{
			private List<QueryInfo> _infos;
			private string          _type;

			/// <summary>
			/// Query database type string.
			/// </summary>
			[XmlAttribute(AttributeName = "type")]
			public string Type
			{
				get { return this._type; }
				set { this._type = value; }
			}

			/// <summary>
			/// Query database type.
			/// </summary>
			public QuerySource DBType
			{
				get
				{
					QuerySource type = QuerySource.MSSQL;

					if (Enum.TryParse(Type, true, out type))
					{
						return type;
					}

					return QuerySource.MSSQL;
				}
			}

			/// <summary>
			/// Informations.
			/// </summary>
			[XmlElement(ElementName = "sql-select")]
			public List<QueryInfo> Infos
			{
				get { return this._infos; }
				set { this._infos = value; }
			}
		}

		/// <summary>
		/// Load queries from xml file
		/// </summary>
		/// <param name="fileName">xml file name</param>
		/// <returns>List of queries</returns>
		public static List<QueryInfo> LoadFromXml(string fileName)
		{
			// log.DebugFormat("fileName:'{0}'", fileName);

			XmlSerializer s = new XmlSerializer(typeof(LoaderRootWrapper));

			using (XmlReader xmlReader = XmlReader.Create(fileName, XmlUtils.GetXmlReaderSettings()))
			{
				var loaded = ((LoaderRootWrapper) s.Deserialize(xmlReader));
				return GetQueries(loaded);
			}
		}

		private static List<QueryInfo> GetQueries(LoaderRootWrapper wrapper)
		{
			List<QueryInfo> queries = new List<QueryInfo>();

			queries.AddRange(GetQueries(wrapper, QuerySource.MSSQL));
			queries.AddRange(GetQueries(wrapper, QuerySource.TDSQL));
			queries.AddRange(GetQueries(wrapper, QuerySource.SQLite));
			queries.AddRange(GetQueries(wrapper, QuerySource.SQLiteExternal));
			queries.AddRange(GetQueries(wrapper, QuerySource.ActiveDirectory));
			queries.AddRange(GetQueries(wrapper, QuerySource.EventLog));
			queries.AddRange(GetQueries(wrapper, QuerySource.NetworkInformation));

			return queries;
		}

 		private static IEnumerable<QueryInfo> GetQueries(LoaderRootWrapper wrapper, QuerySource querySource)
 		{
			List<QueryInfo> queryInfos = wrapper.GetQueriesOfType(querySource).SelectMany(w => w.Infos).ToList();

			queryInfos.ForEach(q =>
			{
				q.OnDesialized();
				q.Source = querySource;
			});

			return queryInfos;
		}

		/// <summary>
		/// Load queries from Xml-file
		/// </summary>
		/// <param name="fileName">Xml-file name</param>
		/// <param name="cryptoProcessor">Encoder</param>
		/// <param name="isExternal">Is opened from user file template</param>
		/// <returns>List of queries</returns>
		public static List<QueryInfo> LoadFromXml(string fileName, CryptoProcessor cryptoProcessor, bool isExternal)
		{
			if (AppVersionHelper.IsDebug() || isExternal)
			{
				if (cryptoProcessor == null)
				{
					return LoadFromXml(fileName);
				}
			}

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.PreserveWhitespace = true;
				doc.Load(fileName);

				if (AppVersionHelper.IsRelease() || (AppVersionHelper.IsNotRelease() && cryptoProcessor != null))
				{
					cryptoProcessor.DecryptXmlDocument(doc);
				}

				byte[] bytes = Encoding.UTF8.GetBytes(doc.OuterXml);

				XmlSerializer s = new XmlSerializer(typeof (LoaderRootWrapper));

				using (var includingReader = new MemoryStream(bytes))
				{
					using (var xmlReader = XmlReader.Create(includingReader, XmlUtils.GetXmlReaderSettings()))
					{
						return GetQueries(((LoaderRootWrapper) s.Deserialize(xmlReader)));
					}
				}
			}
			catch (Exception exception)
			{
				log.Error(exception);

				if (AppVersionHelper.IsDebug() || isExternal)
				{
					return LoadFromXml(fileName);
				}

				throw;
			}
		}

		/// <summary>
		/// Save queries to file
		/// </summary>
		/// <param name="fileName">Xml-file name</param>
		/// <param name="queries">Collection of queries</param>
		public static void SaveToXml(string fileName, List<QueryInfo> queries)
		{
			LoaderRootWrapper wrapper = new LoaderRootWrapper();

			wrapper.SqlInfos = new List<LoaderMSSQLQueryWrapper>
			{
				ReadLoaderQueryWrapper(queries, QuerySource.MSSQL),
				ReadLoaderQueryWrapper(queries, QuerySource.TDSQL),
				ReadLoaderQueryWrapper(queries, QuerySource.SQLite),
				ReadLoaderQueryWrapper(queries, QuerySource.SQLiteExternal),
				ReadLoaderQueryWrapper(queries, QuerySource.ActiveDirectory),
				ReadLoaderQueryWrapper(queries, QuerySource.EventLog),
				ReadLoaderQueryWrapper(queries, QuerySource.NetworkInformation)
			};

			XmlSerializer serializer = new XmlSerializer(typeof(LoaderRootWrapper));

			using (FileStream writer = new FileStream(fileName, FileMode.Create))
			{
				using (XmlWriter xmlWriter = XmlWriter.Create(writer, XmlUtils.GetXmlWriterSettings()))
				{
					serializer.Serialize(xmlWriter, wrapper);
				}
			}
		}

		private static LoaderMSSQLQueryWrapper ReadLoaderQueryWrapper(List<QueryInfo> queries, QuerySource querySource)
		{
			LoaderMSSQLQueryWrapper queryWrapper = new LoaderMSSQLQueryWrapper
			{
				Infos = queries.Where(query => query.Source == querySource).ToList(),
				Type  = querySource.ToString()
			};

			return queryWrapper;
		}

		/// <summary>
		/// Save queries to encoded file
		/// </summary>
		/// <param name="fileName">Xml-file name</param>
		/// <param name="queries">Collection of queries</param>
		/// <param name="cryptoProcessor">Encoder</param>
		public static void SaveToXml(string fileName, List<QueryInfo> queries, CryptoProcessor cryptoProcessor)
		{
			XmlDocument doc = new XmlDocument();

			SaveToXml(fileName, queries);

			doc.PreserveWhitespace = true;

			doc.Load(fileName);

			cryptoProcessor.EncryptXmlDocument(doc);

			doc.Save(fileName);
		}
	}
}
