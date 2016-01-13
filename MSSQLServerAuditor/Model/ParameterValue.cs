using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Model
{
	/// <summary>
	/// Sql query parameter value
	/// </summary>
	[Serializable]
	public class ParameterValue
	{
		private string                       _name;
		private string                       _value;
		private string                       _userValue;
		private bool                         _isNull;
		private List<TemplateNodeLocaleInfo> _locales;

		/// <summary>
		/// Localization of the node
		/// </summary>
		[XmlElement(ElementName = "i18n")]
		public List<TemplateNodeLocaleInfo> Locales
		{
			get
			{
				return this._locales;
			}
			set
			{
				this._locales = value;
			}
		}

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
		/// Is parameter null
		/// </summary>
		[XmlAttribute(AttributeName = "isnull")]
		public bool IsNull
		{
			get { return this._isNull; }
			set { this._isNull = value; }
		}

		/// <summary>
		/// String value of parameter
		/// </summary>
		[XmlAttribute(AttributeName = "value")]
		public string StringValue
		{
			get { return this._value; }
			set { this._value = value; }
		}

		public string UserValue
		{
			get { return this._userValue; }
			set { this._userValue = value; }
		}

		/// <summary>
		/// Returns parameter accoring type
		/// </summary>
		/// <param name="type">Parameter type <see href="http://msdn.microsoft.com/en-us/library/system.data.sqldbtype.aspx"></see></param>
		/// <returns>Parameter value</returns>
		public object GetValue(SqlDbType type)
		{
			return GetValue(UserValue ?? StringValue, IsNull, type);
		}

		/// <summary>
		/// Returns parameter accoring type
		/// </summary>
		/// <param name="type">Parameter type <see href="http://msdn.microsoft.com/en-us/library/system.data.sqldbtype.aspx"></see></param>
		/// <returns>Parameter value</returns>
		public object GetValue(DbType type)
		{
			return GetValue(UserValue ?? StringValue, IsNull, type);
		}

		/// <summary>
		/// Returns parameter accoring type
		/// </summary>
		/// <param name="stringValue">String parameter representation</param>
		/// <param name="isNull">Is parameter null</param>
		/// <param name="type">Type of parameter <see href="http://msdn.microsoft.com/en-us/library/system.data.sqldbtype.aspx"></see></param>
		/// <returns>Parameter value</returns>
		public static object GetValue(string stringValue, bool isNull, SqlDbType type)
		{
				if ((isNull) || (String.IsNullOrEmpty(stringValue)))
				{
					return DBNull.Value;
				}

				switch (type)
				{
					case SqlDbType.Char:
					case SqlDbType.NChar:
					case SqlDbType.NText:
					case SqlDbType.NVarChar:
					case SqlDbType.Xml:
					case SqlDbType.Text:
					case SqlDbType.VarBinary:
					case SqlDbType.VarChar:
						return stringValue;
					case SqlDbType.Bit:
						return Boolean.Parse(stringValue);
					case SqlDbType.TinyInt:
						return Byte.Parse(stringValue);
					case SqlDbType.Decimal:
					case SqlDbType.Money:
					case SqlDbType.SmallMoney:
						return Decimal.Parse(stringValue);
					case SqlDbType.Date:
					case SqlDbType.DateTime:
					case SqlDbType.DateTime2:
					case SqlDbType.SmallDateTime:
						return DateTime.Parse(stringValue);
					case SqlDbType.DateTimeOffset:
						return DateTimeOffset.Parse(stringValue);
					case SqlDbType.Float:
						return Double.Parse(stringValue, CultureInfo.InvariantCulture.NumberFormat);
					case SqlDbType.UniqueIdentifier:
						return Guid.Parse(stringValue);
					case SqlDbType.SmallInt:
						return Int16.Parse(stringValue);
					case SqlDbType.Int:
						return Int32.Parse(stringValue);
					case SqlDbType.BigInt:
						return Int64.Parse(stringValue);
					case SqlDbType.Real:
						return Single.Parse(stringValue);
					default:
						return stringValue;
				}
		}

		/// <summary>
		/// Returns parameter accoring type
		/// </summary>
		/// <param name="stringValue">String parameter representation</param>
		/// <param name="isNull">Is parameter null</param>
		/// <param name="type">Type of parameter <see href="http://msdn.microsoft.com/en-us/library/system.data.sqldbtype.aspx"></see></param>
		/// <returns>Parameter value</returns>
		public static object GetValue(string stringValue, bool isNull, DbType type)
		{
			if ((isNull) || (String.IsNullOrEmpty(stringValue)))
			{
				return DBNull.Value;
			}

			switch (type)
			{
				case DbType.AnsiString:
				case DbType.AnsiStringFixedLength:
				case DbType.StringFixedLength:
				case DbType.String:
				case DbType.Xml:
				case DbType.VarNumeric:
					return stringValue;
				case DbType.Boolean:
					return Boolean.Parse(stringValue);
				case DbType.Byte:
					return Byte.Parse(stringValue);
				case DbType.SByte:
					return SByte.Parse(stringValue);
				case DbType.Decimal:
				case DbType.Currency:
					return Decimal.Parse(stringValue);
				case DbType.Date:
				case DbType.DateTime:
				case DbType.DateTime2:
					return DateTime.Parse(stringValue);
				case DbType.DateTimeOffset:
					return DateTimeOffset.Parse(stringValue);
				case DbType.Double:
					return Double.Parse(stringValue);
				case DbType.Guid:
					return Guid.Parse(stringValue);
				case DbType.Int16:
					return Int16.Parse(stringValue);
				case DbType.Int32:
					return Int32.Parse(stringValue);
				case DbType.Int64:
					return Int64.Parse(stringValue);
				case DbType.UInt16:
					return UInt16.Parse(stringValue);
				case DbType.UInt32:
					return UInt16.Parse(stringValue);
				case DbType.UInt64:
					return UInt16.Parse(stringValue);
				case DbType.Single:
					return Single.Parse(stringValue);
				default:
					return stringValue;
			}
		}

		/// <summary>
		/// Objects to string.
		/// </summary>
		/// <returns>String objects.</returns>
		public override string ToString()
		{
			return "param:" + Name + "=" + StringValue;
		}

		public ParameterValue Clone()
		{
			return (ParameterValue) MemberwiseClone();
		}
	}

	public static class ParameterValueExtention
	{
		public static void TakeValuesFrom(this List<ParameterValue> dst, Dictionary<string, string> src)
		{
			foreach (var r in src)
			{
				var existing = dst.FirstOrDefault(pv => pv.Name.TrimStart('@') == r.Key);

				if (existing != null)
				{
					existing.StringValue = r.Value;
				}
			}
		}
	}
}
