using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Tables;

namespace MSSQLServerAuditor.SQLite.Databases
{
	/// <summary>
	/// Simple file system inside SQLite DB
	/// </summary>
	public class DbFS : Database
	{
		private readonly object                          _syncLock;
		private readonly Dictionary<long, DbFileStorage> _tables;

		public DbFS(string fileName, bool readOnly = false)
			: base(fileName, readOnly)
		{
			this._syncLock = new object();
			this._tables   = new Dictionary<long, DbFileStorage>();
		}

		private class DbFileStorage
		{
			public FileTable FileTable     { get; private set; }
			public FolderTable FolderTable { get; private set; }

			public static DbFileStorage CreateTables(DbFS dbfs, long connectionId)
			{
				string folderTableName = string.Format("tFolder_{0}", connectionId);
				string fileTableName   = string.Format("tFile_{0}",   connectionId);

				return new DbFileStorage(dbfs.Connection, fileTableName, folderTableName);
			}

			private DbFileStorage(SQLiteConnection connection, string fileTableName, string folderTableName)
			{
				FolderTable folderTable = new FolderTable(connection, folderTableName);
				FileTable   fileTable   = new FileTable(connection, fileTableName, folderTableName);

				folderTable.UpdateScheme();
				fileTable.UpdateScheme();

				FolderTable = folderTable;
				FileTable   = fileTable;
			}
		}

		private class FileLocation
		{
			public long ConnectionId { get; private set; }
			public string Path       { get; private set; }

			public FileLocation(string filePath)
			{
				string[] pathParts = filePath.Split(System.IO.Path.DirectorySeparatorChar);

				ConnectionId = long.Parse(pathParts[0]);

				Path = string.Join(
					System.IO.Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture),
					pathParts,
					1,
					pathParts.Length - 1
				);
			}
		}

		private Int64? GetFolderId(FolderTable folderTable, string path)
		{
			Int64? int64FolderId = null;

			if (folderTable != null)
			{
				int64FolderId = folderTable.GetFolderId(path);
			}
			else
			{
				int64FolderId = null;
			}

			return int64FolderId;
		}

		public void WriteFile(string fileName, byte[] raw)
		{
			FileLocation  location = GetLocation(fileName);
			DbFileStorage storage  = GetStorage(location.ConnectionId);
			string        path     = location.Path;
			string        folder   = Path.GetDirectoryName(path);
			string        file     = Path.GetFileName(path);
			long?         folderId = this.GetFolderId(storage.FolderTable, folder);

			storage.FileTable.WriteFile(raw, folderId, file);
		}

		/// <summary>
		/// Read file from SQLite DB
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns></returns>
		public byte[] ReadFile(string fileName)
		{
			FileLocation  location = GetLocation(fileName);
			DbFileStorage storage  = GetStorage(location.ConnectionId);
			string        path     = location.Path;
			string        folder   = Path.GetDirectoryName(path);
			string        file     = Path.GetFileName(path);
			long?         folderId = this.GetFolderId(storage.FolderTable, folder);

			return storage.FileTable.ReadFile(folderId, file);
		}

		/// <summary>
		/// Write stream to SQLite DB
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <param name="stream">Stream</param>
		public void WriteStream(string fileName, Stream stream)
		{
			var buffer = new byte[stream.Length];

			stream.Read(buffer, 0, buffer.Length);
			this.WriteFile(fileName, buffer);
		}

		private FileLocation GetLocation(string filePath)
		{
			return new FileLocation(filePath);
		}

		private DbFileStorage GetStorage(long connectionGroupId)
		{
			DbFileStorage dbFileStorage;

			lock (this._syncLock)
			{
				this._tables.TryGetValue(connectionGroupId, out dbFileStorage);

				if (dbFileStorage == null)
				{
					dbFileStorage = DbFileStorage.CreateTables(this, connectionGroupId);
					this._tables.Add(connectionGroupId, dbFileStorage);
				}
			}

			return dbFileStorage;
		}
	}
}
