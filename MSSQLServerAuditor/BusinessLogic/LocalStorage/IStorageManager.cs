using System.Collections.Generic;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Databases;

namespace MSSQLServerAuditor.BusinessLogic.LocalStorage
{
	/// <summary>
	/// Vault processor interface
	/// </summary>
	public interface IStorageManager
	{
		/// <summary>
		/// Collection (list) of historical SQLite Databases
		/// </summary>
		List<HistoryStorage> HistoryStorage { get; }

		/// <summary>
		/// Current data scope
		/// </summary>
		CurrentStorage CurrentStorage { get; }

		/// <summary>
		/// Reports data scope
		/// </summary>
		ReportStorage ReportStorage { get; }

		/// <summary>
		/// Save data to SQLite DB
		/// </summary>
		/// <param name="templateNodeInfo">Template node</param>
		/// <param name="results">Results to save</param>
		void SaveRequestedData(
			TemplateNodeInfo     templateNodeInfo,
			MultyQueryResultInfo results
		);

		/// <summary>
		/// Flush cache of SQLite DB tables structure
		/// </summary>
		void FlushDbStructureCache();

		/// <summary>
		/// Read current results
		/// </summary>
		/// <param name="connectionGroup">Connection group</param>
		/// <param name="concreteTemplateNode">Concrete template node</param>
		/// <returns></returns>
		MultyQueryResultInfo ReadCurrentResult(
			ConnectionGroupInfo            connectionGroup,
			ConcreteTemplateNodeDefinition concreteTemplateNode
		);

		int? GetDataRowCount(
			ConnectionGroupInfo            connectionGroup,
			ConcreteTemplateNodeDefinition concreteTemplateNode
		);
	}
}
