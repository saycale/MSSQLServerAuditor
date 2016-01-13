using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using log4net;

namespace MSSQLServerAuditor.SQLite.Commands
{
	internal enum SQLiteFieldType
	{
		Integer,
		Text,
		Numeric,
		Real,
		Blob,
		DateTime,
		Boolean,
		Unknown
	}

	/// <summary>
	/// Helper for SQLite
	/// </summary>
	public static class SQLiteTypeHelper
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static SQLiteFieldType ToSQLiteDbType(this SqlDbType dbType)
		{
			switch (dbType)
			{
				case SqlDbType.Char:
				case SqlDbType.NChar:
				case SqlDbType.NText:
				case SqlDbType.NVarChar:
				case SqlDbType.Xml:
				case SqlDbType.Text:
				case SqlDbType.VarChar:
					return SQLiteFieldType.Text;

				case SqlDbType.Bit:
					return SQLiteFieldType.Boolean;

				case SqlDbType.Decimal:
				case SqlDbType.Money:
				case SqlDbType.SmallMoney:
					return SQLiteFieldType.Numeric;

				case SqlDbType.Float:
				case SqlDbType.Real:
					return SQLiteFieldType.Real;

				case SqlDbType.Date:
				case SqlDbType.DateTime:
				case SqlDbType.DateTime2:
				case SqlDbType.SmallDateTime:
					return SQLiteFieldType.DateTime;

				case SqlDbType.TinyInt:
				case SqlDbType.SmallInt:
				case SqlDbType.Int:
				case SqlDbType.BigInt:
					return SQLiteFieldType.Integer;

				case SqlDbType.VarBinary:
				case SqlDbType.Binary:
					return SQLiteFieldType.Blob;

				default:
					return SQLiteFieldType.Unknown;
			}
		}

		internal static SqlDbType ToSqlDbType(this SQLiteFieldType sqLiteType)
		{
			switch (sqLiteType)
			{
				case SQLiteFieldType.Blob:
					return SqlDbType.VarBinary;
				case SQLiteFieldType.Boolean:
					return SqlDbType.Bit;
				case SQLiteFieldType.DateTime:
					return SqlDbType.DateTime;
				case SQLiteFieldType.Integer:
					return SqlDbType.BigInt;
				case SQLiteFieldType.Numeric:
					return SqlDbType.Decimal;
				case SQLiteFieldType.Real:
					return SqlDbType.Float;
				case SQLiteFieldType.Text:
					return SqlDbType.NVarChar;
				default:
					return SqlDbType.Variant;
			}
		}

		internal static SQLiteFieldType Parse(string strTypeValue)
		{
			string strSQLiteTypeValue = strTypeValue;

			// Log.InfoFormat("value:'{0}'", strTypeValue);

			if (strSQLiteTypeValue.CompareTo("DATETIME") == 0)
			{
				strSQLiteTypeValue = "DateTime";
			}

			// Log.InfoFormat("value:'{0}'", strSQLiteTypeValue);

			return (SQLiteFieldType)Enum.Parse(typeof(SQLiteFieldType), strSQLiteTypeValue);
		}

		internal static DbType ToDbType(this SqlDbType dbType)
		{
			var conversionParameter = new SqlParameter();

			try
			{
				conversionParameter.SqlDbType = dbType;
			}
			catch (Exception ex)
			{
				Log.Error("Can't map DbType:", ex);
			}

			return conversionParameter.DbType;
		}

		internal static SqlDbType ToSqlDbType(this DbType dbType)
		{
			var conversionParameter = new SqlParameter();

			try
			{
				conversionParameter.DbType = dbType;
			}
			catch (Exception ex)
			{
				Log.Error("Can't map DbType:", ex);
			}

			return conversionParameter.SqlDbType;
		}

		private static object GetEmptyInstance(Type type)
		{
			object instance = null;

			if (type == typeof(String))
			{
				instance = String.Empty;
			}

			if ((instance == null) && type.IsArray)
			{
				instance = Activator.CreateInstance(type, 0); //an empty array
			}

			if (instance == null)
			{
				instance = Activator.CreateInstance(type, new object[] { });
			}

			return instance;
		}

		public static SqlDbType ToSqlDbType(this Type type)
		{
			var srcType = type;

			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				srcType = Nullable.GetUnderlyingType(type);
			}

			var conversionParameter = new SqlParameter();
			var instance = GetEmptyInstance(srcType);
			conversionParameter.Value = instance;
			return conversionParameter.SqlDbType;
		}
	}
}
