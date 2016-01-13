using System;
using System.Collections.Generic;
using System.Data;
using MSSQLServerAuditor.SQLite.Common.Triggers;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.SQLite.Common.Definitions
{
	/// <summary>
	/// Definition for SQLite DB Table
	/// </summary>
	public class TableDefinition : QueryDefinition
	{
		public const string            DateCreated = "date_created";
		public const string            DateUpdated = "date_updated";
		private readonly List<Trigger> _triggers;
		private PrimaryKeyDefinition   _primaryKey;

		private TableDefinition()
		{
			this.Name        = null;
			this.Indexes     = new List<IndexDefinition>();
			this._triggers   = new List<Trigger>();
			this._primaryKey = PrimaryKeyDefinition.Empty;
		}

		/// <summary>
		/// Constructor for table
		/// </summary>
		/// <param name="name">Table name</param>
		public TableDefinition(string name) : this()
		{
			this.Name = name;
		}

		/// <summary>
		/// Table name
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Indexes of table
		/// </summary>
		public List<IndexDefinition> Indexes { get; private set; }

		/// <summary>
		/// Table primary key
		/// </summary>
		public PrimaryKeyDefinition PrimaryKey
		{
			get { return this._primaryKey; }
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("PrimaryKey");
				}

				this._primaryKey = value;
			}
		}

		/// <summary>
		/// Triggers for table
		/// </summary>
		public List<Trigger> Triggers
		{
			get
			{
				return this._triggers;
			}
		}

		/// <summary>
		/// Get default index name of table
		/// </summary>
		/// <returns></returns>
		public string GetIndexName()
		{
			return this.Name != null
				? this.Name.AsIndexName()
				: null;
		}

		public TableDefinition SetSimplePrimaryKey(
			string fieldName,
			bool   isAutoIncrement = false
		)
		{
			PrimaryKey = PrimaryKeyDefinition.CreateSimple(
				fieldName,
				isAutoIncrement
			);

			return this;
		}

		public TableDefinition SetCompoundPrimaryKey(
			IEnumerable<string> fields
		)
		{
			PrimaryKey = PrimaryKeyDefinition.CreateCompound(fields);

			return this;
		}

		/// <summary>
		/// Get default FK name of table
		/// </summary>
		/// <returns></returns>
		public string GetAsFk()
		{
			return this.Name != null
				? this.Name.AsFk()
				: null;
		}

		/// <summary>
		/// Adding a FK to table
		/// </summary>
		/// <param name="tableDefinition">Definition of connected table</param>
		public void AddFk(
			TableDefinition tableDefinition,
			SqlDbType       sqlType,
			bool            unique,
			bool            isNotNull,
			object          defaultValue,
			int?            forcedIndex
		)
		{
			this.Fields.Add(
				tableDefinition.GetAsFk(),
				new FieldDefinition(
					tableDefinition.GetAsFk(),
					sqlType,
					unique,
					isNotNull,
					defaultValue,
					forcedIndex
				)
			);
		}

		public TableDefinition AddField(
			string    name,
			SqlDbType sqlType,
			bool      unique,
			bool      isNotNull,
			object    defaultValue,
			int?      forcedIndex
		)
		{
			this.AddField(
				new FieldDefinition(
					name,
					sqlType,
					unique,
					isNotNull,
					defaultValue,
					forcedIndex
				)
			);

			return this;
		}

		public TableDefinition AddField(FieldDefinition fieldDefinition)
		{
			if (!this.Fields.ContainsKey(fieldDefinition.Name))
			{
				this.Fields.Add(fieldDefinition.Name, fieldDefinition);
			}

			return this;
		}

		public TableDefinition AddNVarCharField(string name, bool unique, bool isNotNull)
		{
			return this.AddField(
				name,
				SqlDbType.NVarChar,
				unique,
				isNotNull,
				null,
				null
			);
		}

		public TableDefinition AddBigIntField(string name, bool unique, bool isNotNull)
		{
			return this.AddField(
				name,
				SqlDbType.BigInt,
				unique,
				isNotNull,
				null,
				null
			);
		}

		public TableDefinition AddBitField(string name, bool unique, bool isNotNull, object defaultValue = null)
		{
			return this.AddField(
				name,
				SqlDbType.Bit,
				unique,
				isNotNull,
				defaultValue,
				null
			);
		}

		public TableDefinition AddDateTimeField(string name, bool unique, bool isNotNull)
		{
			return this.AddField(
				name,
				SqlDbType.DateTime,
				unique,
				isNotNull,
				null,
				null
			);
		}

		public TableDefinition AddIntField(string name, bool unique, bool isNotNull)
		{
			return this.AddField(
				name,
				SqlDbType.Int,
				unique,
				isNotNull,
				null,
				null
			);
		}

		public TableDefinition AddDateCreateField()
		{
			this.AddField(
				DateCreated,
				SqlDbType.DateTime,
				false,
				true,
				SQLiteHelper.CurrentTimestamp,
				null
			);

			// this.Triggers.Add(
			//    new PreserveTrigger(
			//       this,
			//       DateCreated
			//    )
			// );

			return this;
		}

		public TableDefinition AddDateUpdatedField(string triggerAssociatedColumn = "rowid")
		{
			this.AddField(
				DateUpdated,
				SqlDbType.DateTime,
				false,
				false,
				null,
				null
			);

			this.Triggers.Add(
				new SetNewValueAtUpdateTrigger(
					this,
					DateUpdated,
					triggerAssociatedColumn,
					SQLiteHelper.CurrentTimestamp
				)
			);

			return this;
		}
	}
}
