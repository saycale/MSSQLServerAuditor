using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using log4net;
using MSSQLServerAuditor.SQLite.Commands;
using MSSQLServerAuditor.Model.Connections.Factories;
using MSSQLServerAuditor.Model.Connections.Parameters;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.SQLite.Tables.Directories;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Model.Connections.Query
{
	public class SqliteConnectionParameters
	{
		public class Values
		{
			public const string FieldGroupId    = "groupId";
			public const string FieldServerId   = "serverId";
			public const string FieldTemplateId = "templateId";
			public const string FieldLoginId    = "loginId";

			private Values(
				long? groupId,
				long? serverIntanceId,
				long? templateId,
				long? loginId
			)
			{
				this.GroupId          = groupId;
				this.ServerInstanceId = serverIntanceId;
				this.TemplateId       = templateId;
				this.LoginId          = loginId;
			}

			public long? GroupId          { get; private set; }
			public long? ServerInstanceId { get; private set; }
			public long? TemplateId       { get; private set; }
			public long? LoginId          { get; private set; }

			public static Values FromReader(SQLiteDataReader reader)
			{
				return new Values(
					ResolveId(reader[FieldGroupId]),
					ResolveId(reader[FieldServerId]),
					ResolveId(reader[FieldTemplateId]),
					ResolveId(reader[FieldLoginId])
				);
			}

			private static long? ResolveId(object value)
			{
				return value != null 
					? long.Parse(value.ToString()) 
					: (long?) null;
			}
		}

		static readonly string[] AliasesGroup      = { "group", "grp", "server group" };
		static readonly string[] AliasesConnection = { "connection", "cnn", "server" };
		static readonly string[] AliasesLogin      = { "login", "uid", "username", "user name", "user" };
		static readonly string[] AliasesTemplate   = { "template" };

		private const string FormattingString = @"group={0};connection={1};template={2};login={3}";

		public static SqliteConnectionParameters Parse(string connectionString)
		{
			ParametersParser parser = new ParametersParser(connectionString);

			SqliteConnectionParameters connParams = new SqliteConnectionParameters
			{
				Group      = parser.GetValue(AliasesGroup),
				Connection = parser.GetValue(AliasesConnection),
				Template   = parser.GetValue(AliasesTemplate),
				Login      = parser.GetValue(AliasesLogin)
			};

			return connParams;
		}

		public string Group      { get; set; }
		public string Connection { get; set; }
		public string Template   { get; set; }
		public string Login      { get; set; }

		public bool IsValid
		{
			get
			{
				return
					!string.IsNullOrEmpty(Group) &&
					!string.IsNullOrEmpty(Connection) &&
					!string.IsNullOrEmpty(Template) &&
					!string.IsNullOrEmpty(Login);
			}
		}

		public Values Resolve(CurrentStorage storage)
		{
			string sqlQuery = string.Format(
				@"SELECT DISTINCT {0}.[{26}] AS {22}, {1}.[{27}] AS {23}, {5}.[{28}] AS {24}, {6}.[{29}] AS {25} FROM {0}
					JOIN {1} ON {0}.[{26}] = {1}.{7}
					JOIN {2} ON {1}.[{27}] = {2}.{8}
					JOIN {3} ON {3}.[{30}] = {2}.{9}
					JOIN {4} ON {4}.[{31}] = {3}.{10}
					JOIN {5} ON {5}.[{28}] = {4}.{11}
					JOIN {6} ON {6}.[{29}] = {1}.{12}
					WHERE {0}.{13}={14} AND {1}.{15}={16} AND {1}.{17} NOT NULL AND {5}.{18}={19} AND {6}.{20}={21}",
				ConnectionGroupDirectory.TableName,
				ServerInstanceDirectory.TableName,
				QueryDirectory.TableName,
				TemplateNodeQueryDirectory.TableName,
				TemplateNodeDirectory.TableName,
				TemplateDirectory.TableName,
				LoginDirectory.TableName,

				ConnectionGroupDirectory.TableName.AsFk(),
				ServerInstanceDirectory.TableName.AsFk(),
				TemplateNodeQueryDirectory.TableName.AsFk(),
				TemplateNodeDirectory.TableName.AsFk(),
				TemplateDirectory.TableName.AsFk(),
				LoginDirectory.TableName.AsFk(),

				ConnectionGroupDirectory.NameFn,
				Values.FieldGroupId.AsParamName(),

				ServerInstanceDirectory.ConnectionNameFn,
				Values.FieldServerId.AsParamName(),

				ServerInstanceDirectory.DbTypeFn,

				TemplateDirectory.NameFieldName,
				Values.FieldTemplateId.AsParamName(),

				LoginDirectory.LoginFn,
				Values.FieldLoginId.AsParamName(),

				Values.FieldGroupId,
				Values.FieldServerId,
				Values.FieldTemplateId,
				Values.FieldLoginId,

				ConnectionGroupDirectory.TableIdentityField,
				ServerInstanceDirectory.TableIdentityField,
				TemplateDirectory.TableIdentityField,
				LoginDirectory.TableIdentityField,
				TemplateNodeQueryDirectory.TableIdentityField,
				TemplateNodeDirectory.TableIdentityField
			);

			SQLiteParameter groupParameter    = CreateStringParameter(Values.FieldGroupId.AsParamName(),    Group);
			SQLiteParameter serverParameter   = CreateStringParameter(Values.FieldServerId.AsParamName(),   Connection);
			SQLiteParameter templateParameter = CreateStringParameter(Values.FieldTemplateId.AsParamName(), Template);
			SQLiteParameter loginParameter    = CreateStringParameter(Values.FieldLoginId.AsParamName(),    Login);

			using (SQLiteConnection connection = ConnectionFactory.CreateSQLiteConnection(storage.FileName, true))
			{
				connection.Open();

				List<Values> result = new List<Values>();

				new SqlSelectCommand(
					connection,
					sqlQuery,
					reader => result.Add(Values.FromReader(reader)),
					groupParameter,
					serverParameter,
					templateParameter,
					loginParameter
				)
				.Execute(100);

				return result.FirstOrDefault();
			}
		}

		private static SQLiteParameter CreateStringParameter(string paramName, string paramValue)
		{
			return new SQLiteParameter(paramName, DbType.String)
			{
				Value = paramValue
			};
		}

		public string ToConnectionString()
		{
			return string.Format(
				FormattingString,
				Group,
				Connection,
				Template,
				Login
			);
		}
	}

	internal class SqliteInnerQueryConnection : AttachingSqliteQueryConnection
	{
		private static readonly ILog                              Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly        SqliteConnectionParameters.Values _paramValues;

		public SqliteInnerQueryConnection(
			CurrentStorage             currentStorage,
			SqliteConnectionParameters connectionParameters
		) : base(
				ConnectionFactory.CreateSQLiteConnection(currentStorage.FileName, true)
			)
		{
			SqliteConnectionParameters.Values paramValues =
				connectionParameters.Resolve(currentStorage);

			if (paramValues == null)
			{
				const string errorMessage = "Can not resolve SQLite internal connection parameters";

				Log.Error(errorMessage);

				throw new ArgumentException(errorMessage, "connectionParameters");
			}

			this._paramValues = paramValues;
		}

		public override SqliteQueryCommand GetSqliteCommand(
			string sqlText,
			int    commandTimeout
		)
		{
			SQLiteCommand sqliteCommand  = Connection.CreateCommand();
			sqliteCommand.CommandText    = sqlText;
			sqliteCommand.CommandTimeout = commandTimeout;

			return new SqliteInnerQueryCommand(
				sqliteCommand,
				this._paramValues
			);
		}

		private class SqliteInnerQueryCommand : SqliteQueryCommand
		{
			private readonly SqliteConnectionParameters.Values _paramValues;

			public SqliteInnerQueryCommand(
				SQLiteCommand                     command,
				SqliteConnectionParameters.Values paramValues
			) : base(command)
			{
				this._paramValues = paramValues;
			}

			protected override void AssignDefaultParameters()
			{
				this._sqliteCommand.Parameters.Add(
					SQLiteHelper.GetParameter(ConnectionGroupDirectory.TableName.AsFk(), this._paramValues.GroupId)
				);

				this._sqliteCommand.Parameters.Add(
					SQLiteHelper.GetParameter(ServerInstanceDirectory.TableName.AsFk(), this._paramValues.ServerInstanceId)
				);

				this._sqliteCommand.Parameters.Add(
					SQLiteHelper.GetParameter(LoginDirectory.TableName.AsFk(), this._paramValues.LoginId)
				);

				this._sqliteCommand.Parameters.Add(
					SQLiteHelper.GetParameter(TemplateDirectory.TableName.AsFk(), this._paramValues.TemplateId)
				);
			}
		}
	}
}
