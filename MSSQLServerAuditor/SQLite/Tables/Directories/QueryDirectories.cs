using MSSQLServerAuditor.SQLite.Databases;

namespace MSSQLServerAuditor.SQLite.Tables.Directories
{
	public class QueryDirectories
	{
		private readonly QueryDirectoryBase          _queryDirectory;
		private readonly QueryParameterDirectoryBase _queryParameterDirectory;

		private QueryDirectories(
			QueryDirectoryBase          queryDirectory,
			QueryParameterDirectoryBase queryParameterDirectory
		)
		{
			this._queryDirectory          = queryDirectory;
			this._queryParameterDirectory = queryParameterDirectory;
		}

		public static QueryDirectories GetInstance(CurrentStorage storage, bool isGroupQuery)
		{
			if (isGroupQuery)
			{
				return new QueryDirectories(
					storage.QueryGroupDirectory,
					storage.QueryGroupParameterDirectory
				);
			}

			return new QueryDirectories(
				storage.QueryDirectory,
				storage.QueryParameterDirectory
			);
		}

		public QueryDirectoryBase QueryDirectory
		{
			get { return this._queryDirectory; }
		}

		public QueryParameterDirectoryBase QueryParameterDirectory
		{
			get { return this._queryParameterDirectory; }
		}
	}
}
