using System;
using System.Data.SQLite;
using System.Diagnostics.Contracts;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Common.Definitions;
using MSSQLServerAuditor.SQLite.Databases;

namespace MSSQLServerAuditor.SQLite.Tables
{
	public struct DatabaseVersion
	{
		public static readonly DatabaseVersion Current = new DatabaseVersion(1, 0);
		public readonly int                    Major;
		public readonly int                    Minor;

		public DatabaseVersion(int major, int minor)
			: this()
		{
			this.Major = major;
			this.Minor = minor;
		}

		public bool IsOlderOrSameAs(DatabaseVersion another)
		{
			return this.Major < another.Major ||
				   this.Major == another.Major && this.Minor <= another.Minor;
		}

		[Pure]
		public bool MajorChangedOver(DatabaseVersion another)
		{
			return this.Major != another.Major;
		}

		public bool MinorChangedOver(DatabaseVersion another)
		{
			return this.MajorChangedOver(another) || this.Minor != another.Minor;
		}
	}

	public class ServiceInfoTable : CurrentStorageTable
	{
		public const string TableName              = "d_ServiceInfo";
		public const string TableIdentityField     = "d_ServiceInfo_id";
		public const string DbSchemaVersionMajorFn = "DbSchemaVersionMajor";
		public const string DbSchemaVersionMinorFn = "DbSchemaVersionMinor";

		public ServiceInfoTable(CurrentStorage storage)
			: base(storage, CreateTableDefinition())
		{
		}

		public static TableDefinition CreateTableDefinition()
		{
			return TableDefinitionFactory.CreateWithAutoincrementKey(TableName, TableIdentityField)
				.AddIntField(DbSchemaVersionMajorFn, false, false)
				.AddIntField(DbSchemaVersionMinorFn, false, false);
		}

		public static DatabaseVersion ReadDbVesion(SQLiteConnection connection)
		{
			const string sql = "SELECT * FROM " + TableName;

			if (!TableExists(connection, TableName))
			{
				return new DatabaseVersion();
			}

			using (var cmd = new SQLiteCommand(sql, connection))
			{
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						return new DatabaseVersion(
							(int)(Int64)reader[DbSchemaVersionMajorFn],
							(int)(Int64)reader[DbSchemaVersionMinorFn]
						);
					}
				}
			}

			return new DatabaseVersion();
		}

		protected override string IdentityField
		{
			get { return TableIdentityField; }
		}

		public DatabaseVersion ReadDbVesion()
		{
			return ReadDbVesion(this.Connection);
		}

		public void WriteVersion(DatabaseVersion value)
		{
			using (this.Connection.OpenWrapper())
			{
				using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM " + TableName, this.Connection))
				{
					if ((Int64)cmd.ExecuteScalar() == 0)
					{
						using (SQLiteCommand cmd2 = new SQLiteCommand(
								"INSERT INTO " + TableName + " DEFAULT VALUES",
								this.Connection
							)
						)
						{
							cmd2.ExecuteNonQuery();
						}
					}
				}
			}

			string sql = string.Format(
				  "UPDATE {0} SET"
				+ "     {1} = {2}"
				+ "    ,{3} = {4}"
				+ " WHERE ("
				+ "    ({1} IS NULL OR {1} != {2})"
				+ "    OR"
				+ "    ({3} IS NULL OR {3} != {4})"
				+ " );",
				TableName,
				DbSchemaVersionMajorFn,
				value.Major,
				DbSchemaVersionMinorFn,
				value.Minor
			);

			using (this.Connection.OpenWrapper())
			{
				using (SQLiteCommand cmd = new SQLiteCommand(sql, this.Connection))
				{
					cmd.ExecuteNonQuery();
				}
			}
		}
	}
}
