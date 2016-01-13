using MSSQLServerAuditor.SQLite.Common;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	public class ServerInstanceRow : AutoincrementTableRow
	{
		public ServerInstanceRow() 
			: base(ServerInstanceDirectory.CreateTableDefinition())
		{			
		}

		public static ServerInstanceRow Copy(ITableRow sourceRow)
		{
			ServerInstanceRow row = new ServerInstanceRow();
			sourceRow.CopyValues(row);
			return row;
		}

		internal long ConnectionGroupId
		{
			get
			{
				return this.GetValue<long>(ServerInstanceDirectory.ConnectionGroupIdFn);
			}
			set
			{
				this.SetValue(ServerInstanceDirectory.ConnectionGroupIdFn, value);
			}
		}

		internal long LoginId
		{
			get
			{
				return this.GetValue<long>(ServerInstanceDirectory.LoginIdFn);
			}
			set
			{
				this.SetValue(ServerInstanceDirectory.LoginIdFn, value);
			}
		}

		internal string ConnectionName
		{
			get
			{
				return this.GetValue<string>(ServerInstanceDirectory.ConnectionNameFn);
			}
			set
			{
				this.SetValue(ServerInstanceDirectory.ConnectionNameFn, value);
			}
		}

		internal string ServerInstanceName
		{
			get
			{
				return this.GetValue<string>(ServerInstanceDirectory.ServerInstanceNameFn);
			}
			set
			{
				this.SetValue(ServerInstanceDirectory.ServerInstanceNameFn, value);
			}
		}

		internal string ServerInstanceVersion
		{
			get
			{
				return this.GetValue<string>(ServerInstanceDirectory.ServerInstanceVersionFn);
			}
			set
			{
				this.SetValue(ServerInstanceDirectory.ServerInstanceVersionFn, value);
			}
		}

		internal bool IsDeleted
		{
			get
			{
				return this.GetValue<bool>(ServerInstanceDirectory.IsDeletedFn, false);
			}
			set
			{
				this.SetValue(ServerInstanceDirectory.IsDeletedFn, value);
			}
		}

		internal bool IsDynamicConnection
		{
			get
			{
				return this.GetValue(ServerInstanceDirectory.IsDynamicConnectionFn, false);
			}
			set
			{
				this.SetValue(ServerInstanceDirectory.IsDynamicConnectionFn, value);
			}
		}

		internal bool IsOdbc
		{
			get
			{
				return this.GetValue(ServerInstanceDirectory.IsOdbcFn, false);
			}
			set
			{
				this.SetValue(ServerInstanceDirectory.IsOdbcFn, value);
			}
		}

		internal string DbType
		{
			get
			{
				return this.GetValue<string>(ServerInstanceDirectory.DbTypeFn);
			}
			set
			{
				this.SetValue(ServerInstanceDirectory.DbTypeFn, value);
			}
		}
	}
}
