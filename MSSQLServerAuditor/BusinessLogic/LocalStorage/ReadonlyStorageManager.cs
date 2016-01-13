using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Databases;
using System.Collections.Generic;

namespace MSSQLServerAuditor.BusinessLogic.LocalStorage
{
	internal class ReadonlyStorageManager : IStorageManager
	{
		private readonly StorageManager _storageManager;

		public ReadonlyStorageManager(
			MsSqlAuditorModel model,
			string            currentDbFile,
			string            historicDbFile,
			string            reportDbFile
		)
		{
			this._storageManager = new StorageManager(
				model,
				false,
				currentDbFile,
				historicDbFile,
				reportDbFile
			);
		}

		public List<HistoryStorage> HistoryStorage
		{
			get
			{
				return this._storageManager.HistoryStorage;
			}
		}

		public CurrentStorage CurrentStorage
		{
			get
			{
				return this._storageManager.CurrentStorage;
			}
		}

		public ReportStorage ReportStorage
		{
			get
			{
				return this._storageManager.ReportStorage;
			}
		}

		public void SaveRequestedData(
			TemplateNodeInfo     templateNodeInfo,
			MultyQueryResultInfo results
		)
		{
			this._storageManager.SaveRequestedData(templateNodeInfo, results);
		}

		public void FlushDbStructureCache()
		{
			this._storageManager.FlushDbStructureCache();
		}

		public int? GetDataRowCount(
			ConnectionGroupInfo            connectionGroup,
			ConcreteTemplateNodeDefinition concreteTemplateNode
		)
		{
			int? result =
				this._storageManager.GetDataRowCount(connectionGroup, concreteTemplateNode) ??
				this._storageManager.CurrentStorage.GetRowCount(concreteTemplateNode.TemplateNode);

			return result;
		}

		public MultyQueryResultInfo ReadCurrentResult(
			ConnectionGroupInfo            connectionGroup,
			ConcreteTemplateNodeDefinition concreteTemplateNode
		)
		{
			return this._storageManager.ReadCurrentResult(
				connectionGroup,
				concreteTemplateNode
			);
		}
	}
}
