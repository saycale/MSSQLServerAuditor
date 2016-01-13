using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Model
{
	/// <summary>
	/// Query scope
	/// </summary>
	public enum QueryScope
	{
		/// <summary>
		/// Query for instance
		/// </summary>
		[XmlEnum(Name = "instance")]
		Instance,

		/// <summary>
		/// Query for database
		/// </summary>
		[XmlEnum(Name = "database")]
		Database,

		/// <summary>
		/// Query for InstanceGroup
		/// </summary>
		[XmlEnum(Name = "InstanceGroup")]
		InstanceGroup
	}

	/// <summary>
	/// Query item for defined version of MS SQL Server
	/// </summary>
	public class QueryItemInfo
	{
		/// <summary>
		/// Min supported version (* - any)
		/// </summary>
		[XmlAttribute(AttributeName = "MinSupportedVersion")]
		public string MinVersion { get; set; }

		/// <summary>
		/// Max supported version (* - any)
		/// </summary>
		[XmlAttribute(AttributeName = "MaxSupportedVersion")]
		public string MaxVersion { get; set; }

		/// <summary>
		/// Signature
		/// </summary>
		[XmlAttribute(AttributeName = "signature")]
		public string Signature { get; set; }

		/// <summary>
		/// Query scope
		/// </summary>
		[XmlAttribute(AttributeName = "scope")]
		public QueryScope Scope { get; set; }

		/// <summary>
		/// Gets or sets the child groups.
		/// </summary>
		[XmlElement(ElementName = "group-select-text")]
		public List<QueryItemInfo> ChildGroups { get; set; }

		[XmlElement(ElementName = "executeIf-select")]
		public string ExecuteIfSqlText { get; set; }

		/// <summary>
		/// Sql query text
		/// </summary>
		[XmlElement(ElementName = "sql")]
		[XmlText]
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets the parameters.
		/// </summary>
		[XmlArray(ElementName = "sql-select-parameters")]
		[XmlArrayItem(ElementName = "sql-select-parameter")]
		public List<QueryParameterInfo> Parameters { get; set; }

		/// <summary>
		/// Parent query.
		/// </summary>
		[XmlIgnore]
		public QueryInfo ParentQuery { get; set; }

		/// <summary>
		/// Determines is query item applicable for server version
		/// </summary>
		/// <param name="version">Server version</param>
		/// <returns>Is it match</returns>
		public bool IsApplicableVersion(InstanceVersion version)
		{
			if (ParentQuery.Source == QuerySource.SQLite)
			{
				return true;
			}

			var minVersion = InstanceVersion.GetMinVersion(MinVersion);
			var maxVersion = InstanceVersion.GetMaxVersion(MaxVersion);

			return version.CompareTo(minVersion) >= 0 && version.CompareTo(maxVersion) <= 0;
		}

		/// <summary>
		/// Objects to string.
		/// </summary>
		/// <returns>String objects.</returns>
		public override string ToString()
		{
			return (ParentQuery != null ? ParentQuery + Environment.NewLine : "")
				   + "MinVersion=" + MinVersion + " MaxVersion=" + MaxVersion;
		}

		public QueryItemInfo Clone()
		{
			return (QueryItemInfo) MemberwiseClone();
		}
	}

	/// <summary>
	/// Definition for query parameter
	/// </summary>
	public class QueryParameterInfo
	{
		private string    _name;
		private SqlDbType _type;
		private bool      _isNull;
		private string    _defaultValue;

		/// <summary>
		/// Parameter name
		/// </summary>
		[XmlAttribute(AttributeName = "name")]
		public string Name
		{
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// Parameter type <see href="http://msdn.microsoft.com/en-us/library/system.data.sqldbtype.aspx"></see>
		/// </summary>
		[XmlAttribute(AttributeName = "type")]
		public SqlDbType Type
		{
			get { return this._type; }
			set { this._type = value; }
		}

		/// <summary>
		/// Is null by default
		/// </summary>
		[XmlAttribute(AttributeName = "isnull")]
		public bool IsNull
		{
			get { return this._isNull; }
			set { this._isNull = value; }
		}

		/// <summary>
		/// Default string representation
		/// </summary>
		[XmlAttribute(AttributeName = "default")]
		public string DefaultStringValue
		{
			get { return this._defaultValue; }
			set { this._defaultValue = value; }
		}

		/// <summary>
		/// Returns default value as object
		/// </summary>
		/// <returns>Default value</returns>
		public object GetDefaultValue()
		{
			return ParameterValue.GetValue(DefaultStringValue, IsNull, Type);
		}
	}

	/// <summary>
	/// Source DBMS enum for query
	/// </summary>
	public enum QuerySource
	{
		/// <summary>
		/// External MS SQL Instance
		/// </summary>
		MSSQL,
		/// <summary>
		/// Internal current, reports and historic SQLite databases
		/// </summary>
		SQLite,
		/// <summary>
		/// External SQLite database instance
		/// </summary>
		SQLiteExternal,
		/// <summary>
		/// External Teradata SQL Server
		/// </summary>
		TDSQL,
		/// <summary>
		/// ActiveDirectory Connector
		/// https://msdn.microsoft.com/en-us/library/system.directoryservices.activedirectory.aspx
		/// </summary>
		ActiveDirectory,
		/// <summary>
		/// EventLog Connector
		/// https://msdn.microsoft.com/en-us/library/System.Diagnostics.EventLog.aspx
		/// </summary>
		EventLog,
		/// <summary>
		/// NetworkInformation Connector
		/// https://msdn.microsoft.com/en-us/library/system.net.networkinformation.pingreply.status.aspx
		/// </summary>
		NetworkInformation
	}

	/// <summary>
	/// Query info for different versions of connected data provider
	/// </summary>
	public class QueryInfo
	{
		private string                   _Id;
		private string                   _name;
		private string                   _queryIndexFields;
		private QuerySource              _source;
		private QueryScope               _scope;
		private List<QueryItemInfo>      _items;
		private List<QueryItemInfo>      _databaseSelect;
		private List<QueryParameterInfo> _parameters;
		private List<NormalizeInfo>      _normalizeInfos;
		private HistoryFillStatementList _fillStatementList;

		internal void OnDesialized()
		{
			foreach (var i in Items)
			{
				i.ParentQuery = this;
			}

			foreach (var i in DatabaseSelect)
			{
				i.ParentQuery = this;
			}

			foreach (var i in GroupSelect)
			{
				i.ParentQuery = this;
			}
		}

		public NormalizeInfo GetDbStructureForRecordSet(Int64 recordSet)
		{
			this._normalizeInfos.Sort((info, normalizeInfo) => info.RecordSet.CompareTo(normalizeInfo.RecordSet));

			for (int i = this._normalizeInfos.Count-1; i >=0 ; i--)
			{
				if (this._normalizeInfos[i].RecordSet <= recordSet)
				{
					return NormalizeInfos[i];
				}
			}

			return null;
		}

		/// <summary>
		/// Query Id
		/// </summary>
		[XmlAttribute(AttributeName = "id")]
		public string Id
		{
			get { return this._Id; }
			set { this._Id = value; }
		}

		/// <summary>
		/// Query name
		/// </summary>
		[XmlAttribute(AttributeName = "name")]
		public string Name
		{
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// Query index fields
		/// </summary>
		[XmlAttribute(AttributeName = "primary_key_columns")]
		public string QueryIndexFileds
		{
			get { return this._queryIndexFields; }
			set { this._queryIndexFields = value; }
		}

		/// <summary>
		/// Source DBMS for query.
		/// </summary>
		[XmlIgnore]
		public QuerySource Source
		{
			get { return this._source; }
			set { this._source = value; }
		}

		/// <summary>
		/// Query scope
		/// </summary>
		[XmlAttribute(AttributeName = "scope")]
		public QueryScope Scope
		{
			get { return this._scope; }
			set { this._scope = value; }
		}

		/// <summary>
		/// Items for defined versions
		/// </summary>
		[XmlElement(ElementName = "sql-select-text")]
		public List<QueryItemInfo> Items
		{
			get { return this._items; }
			set { this._items = value; }
		}

		/// <summary>
		/// Queries for database selections
		/// </summary>
		[XmlElement(ElementName = "database-select-text")]
		public List<QueryItemInfo> DatabaseSelect
		{
			get { return this._databaseSelect; }
			set { this._databaseSelect = value; }
		}

		/// <summary>
		/// Queries for Instance group selections
		/// </summary>
		[XmlElement(ElementName = "group-select-text")]
		public List<QueryItemInfo> GroupSelect { get; set; }

		/// <summary>
		/// Parameters for query
		/// </summary>
		[XmlArray(ElementName = "sql-select-parameters")]
		[XmlArrayItem(ElementName = "sql-select-parameter")]
		public List<QueryParameterInfo> Parameters
		{
			get { return this._parameters; }
			set { this._parameters = value; }
		}

		/// <summary>
		/// Normalization rules
		/// </summary>
		[XmlElement(ElementName = "db-structure")]
		public List<NormalizeInfo> NormalizeInfos
		{
			get { return this._normalizeInfos; }
			set { this._normalizeInfos = value; }
		}

		/// <summary>
		/// Statements to fill historic SQLite DB
		/// </summary>
		[XmlElement(ElementName = "sqlite_statements")]
		public HistoryFillStatementList FillStatementList
		{
			get { return this._fillStatementList; }
			set { this._fillStatementList = value; }
		}

		/// <summary>
		/// Objects to string.
		/// </summary>
		/// <returns>String objects.</returns>
		public override string ToString()
		{
			return string.Format(
				"QueryInfo: Name={0} Scope={1}",
				Name,
				Scope
			);
		}
	}

	/// <summary>
	/// Statements to be executed for Historic SQLite DB
	/// </summary>
	public class HistoryFillStatementList
	{
		private List<HistoryFillStatement> _fillStatements;

		/// <summary>
		/// Statements
		/// </summary>
		[XmlElement("sqlite_statement")]
		public List<HistoryFillStatement> FillStatements
		{
			get { return this._fillStatements; }
			set { this._fillStatements = value; }
		}

		/// <summary>
		/// Get sorted statements
		/// </summary>
		/// <returns></returns>
		public List<HistoryFillStatement> GetSortedStatements()
		{
			this._fillStatements.Sort((statement, fillStatement) => statement.Id.CompareTo(fillStatement.Id));

			return this._fillStatements;
		}
	}

	/// <summary>
	/// Statement to be executed for Historic SQLite DB
	/// </summary>
	public class HistoryFillStatement
	{
		private int    _id;
		private string _text;

		/// <summary>
		/// Statement id (sorting field)
		/// </summary>
		[XmlAttribute("id")]
		public int Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

		/// <summary>
		/// SQLite command text
		/// </summary>
		[XmlText()]
		public string Text
		{
			get { return this._text; }
			set { this._text = value; }
		}
	}
}
