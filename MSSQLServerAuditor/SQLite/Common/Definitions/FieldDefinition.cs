using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using log4net;
using MSSQLServerAuditor.SQLite.Commands;

namespace MSSQLServerAuditor.SQLite.Common.Definitions
{
	/// <summary>
	/// Field definition of SQLite DB table row
	/// </summary>
	public struct FieldDefinition : IEquatable<FieldDefinition>
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private readonly string    _name;
		private readonly SqlDbType _sqlType;
		private readonly bool      _unique;
		private readonly bool      _isNotNull;
		private readonly object    _default;
		private readonly int?      _forcedIndex;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">Field name</param>
		/// <param name="sqlType">Type of field</param>
		/// <param name="unique">Is field unique?</param>
		/// <param name="isNotNull">Is column declared as NOT NULL?</param>
		/// <param name="defaultValue">Default value of field</param>
		public FieldDefinition(
			string    name,
			SqlDbType sqlType,
			bool      unique,
			bool      isNotNull,
			object    defaultValue = null,
			int?      forcedIndex  = null
		)
		{
			this._name         = name;
			this._sqlType      = sqlType;
			this._unique       = unique;
			this._isNotNull    = isNotNull;
			this._default      = defaultValue;
			this._forcedIndex  = forcedIndex;
		}

		public int? ForcedIndex
		{
			get
			{
				return this._forcedIndex;
			}
		}

		/// <summary>
		/// Column name
		/// </summary>
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		/// <summary>
		/// SQLite column type
		/// </summary>
		public SqlDbType SqlType
		{
			get
			{
				return this._sqlType;
			}
		}

		/// <summary>
		/// Is the column is declared as UNIQUE value
		/// </summary>
		public bool Unique
		{
			get
			{
				return this._unique;
			}
		}

		/// <summary>
		/// Is the column is declared as NOT NULL value
		/// </summary>
		public bool IsNotNull
		{
			get
			{
				return this._isNotNull;
			}
		}

		/// <summary>
		/// Default value
		/// </summary>
		public object Default
		{
			get
			{
				return this._default;
			}
		}

		public bool Equals(FieldDefinition other)
		{
			char[] trimChars =
			{
				' ',
				'(',
				')'
			};

			string def1 = (this._default  == null) ? String.Empty : this._default.ToString();
			string def2 = (other._default == null) ? String.Empty : other._default.ToString();

			def1 = def1.Trim(trimChars);
			def2 = def2.Trim(trimChars);

			// Log.DebugFormat(
			//    @"1) Column:Name:'{0}';Type:'{1}';IsPrimaryKey:'{2}';IsNotNull:'{3}';Unique:'{4}';Def:'{5}'",
			//    this._name,
			//    this._sqlType.ToSQLiteDbType(),
			//    this._isPrimaryKey,
			//    this._isNotNull,
			//    this._unique,
			//    def1
			// );

			// Log.DebugFormat(
			//    @"2) Column:Name:'{0}';Type:'{1}';IsPrimaryKey:'{2}';IsNotNull:'{3}';Unique:'{4}';Def:'{5}'",
			//    other._name,
			//    other._sqlType.ToSQLiteDbType(),
			//    other._isPrimaryKey,
			//    other._isNotNull,
			//    other._unique,
			//    def2
			// );

			return string.Equals(this._name, other._name) &&
				this._sqlType.ToSQLiteDbType() == other._sqlType.ToSQLiteDbType() &&
				this._isNotNull.Equals(other._isNotNull) &&
				def1.Equals(def2);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			return obj is FieldDefinition && this.Equals((FieldDefinition)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = (this._name != null ? this._name.GetHashCode() : 0);

				hashCode = (hashCode * 397) ^ (int)this._sqlType.ToSQLiteDbType();
				hashCode = (hashCode * 397) ^ this._unique.GetHashCode();
				hashCode = (hashCode * 397) ^ this._isNotNull.GetHashCode();
				hashCode = (hashCode * 397) ^ this._default.GetHashCode();

				return hashCode;
			}
		}

		public static bool operator ==(FieldDefinition left, FieldDefinition right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(FieldDefinition left, FieldDefinition right)
		{
			return !left.Equals(right);
		}

		public bool CanFill(FieldDefinition other)
		{
			return
				string.Equals(this._name, other._name) &&
				this._sqlType.ToSQLiteDbType().Equals(other._sqlType.ToSQLiteDbType());
		}

		public static void AddFields(List<FieldDefinition> dst, IEnumerable<FieldDefinition> src)
		{
			if (src != null)
			{
				foreach (var f in src)
				{
					if (f.ForcedIndex == null)
					{
						dst.Add(f);
					}
					else
					{
						dst.Insert(f.ForcedIndex.Value, f);
					}
				}
			}
		}
	}
}
