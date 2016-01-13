using System.Collections.Generic;
using System.Data.SQLite;
using System.Reflection;
using log4net;
using MSSQLServerAuditor.SQLite.Commands;
using MSSQLServerAuditor.SQLite.Common.Definitions;

namespace MSSQLServerAuditor.SQLite.Common
{
	public abstract class TableBase
	{
		private static readonly Dictionary<string, object> LockDictionary     = new Dictionary<string, object>();
		private static readonly object                     LockDictionaryLock = new object();
		protected readonly object                          _globalLock;

		internal TableBase()
		{
			this._globalLock = new object();
		}

		protected TableBase(
			SQLiteConnection connection,
			TableDefinition  tableDefinition
		) : this()
		{
			this.TableDefinition = tableDefinition;
			this.Connection      = connection;
		}

		/// <summary>
		/// Table definition
		/// </summary>
		public TableDefinition TableDefinition
		{
			get;
			protected set;
		}

		protected SQLiteConnection Connection
		{
			get; set;
		}

		/// <summary>
		/// Set new name to table
		/// </summary>
		/// <param name="newName">New name</param>
		protected void Rename(string newName)
		{
			using (this.Connection.OpenWrapper())
			{
				TableRenameCommand command = new TableRenameCommand(
					this.Connection,
					this.TableDefinition,
					newName
				);

				command.Execute(100);
			}

			Dictionary<string, FieldDefinition> fields = this.TableDefinition.Fields;

			this.TableDefinition = new TableDefinition(newName);

			foreach (KeyValuePair<string, FieldDefinition> field in fields)
			{
				this.TableDefinition.Fields.Add(
					field.Key,
					field.Value
				);
			}
		}

		/// <summary>
		/// Copy table
		/// </summary>
		/// <param name="toTable">Destination table</param>
		protected void Copy(TableBase toTable)
		{
			using (this.Connection.OpenWrapper())
			{
				CopyTableCommand copyTableCommand = new CopyTableCommand(
					this.Connection,
					this.TableDefinition,
					toTable.TableDefinition
				);

				copyTableCommand.SetFieldDefinitions(
					QueryDefinition.GetMinimumDefinitions(
						(definition, fieldDefinition) => definition.CanFill(fieldDefinition),
						this.TableDefinition,
						toTable.TableDefinition
					)
				);

				copyTableCommand.Execute(100);
			}
		}

		/// <summary>
		/// Drop table
		/// </summary>
		protected long Drop()
		{
			long iRows = 0L;

			using (this.Connection.OpenWrapper())
			{
				DropTableCommand dropCommand = new DropTableCommand(this.Connection, this.TableDefinition);

				iRows = dropCommand.Execute(100);
			}

			return iRows;
		}

		/// <summary>
		/// Get new row
		/// </summary>
		protected ITableRow NewRow()
		{
			return new TableRow(this.TableDefinition);
		}

		protected void CreateTable()
		{
			lock (this._globalLock)
			{
				using (this.Connection.OpenWrapper())
				{
					CreateTableCommand command = new CreateTableCommand(
						this.Connection,
						this.TableDefinition
					);

					command.Execute(100);
				}
			}
		}

		/// <summary>
		/// Table lock
		/// </summary>
		/// <returns></returns>
		protected object GetLockObject()
		{
			lock (LockDictionaryLock)
			{
				object lockObject;

				if (LockDictionary.TryGetValue(this.TableDefinition.Name, out lockObject))
				{
					return lockObject;
				}

				object result = new object();

				LockDictionary.Add(this.TableDefinition.Name, result);

				return result;
			}
		}
	}
}
