using MSSQLServerAuditor.Gui;
using MSSQLServerAuditor.Model;

namespace MSSQLServerAuditor.BusinessLogic.LocalStorage
{
	internal class StorageSerializationInfo
	{
		public StorageSerializationInfo(
			MsSqlAuditorModel    model,
			ConnectionTabControl tabControl,
			string               outputPath,
			bool                 saveCurrentDatabase,
			bool                 saveHistoricDatabase
		)
		{
			this.Model                = model;
			this.TabControl           = tabControl;
			this.OutputFolder         = outputPath;
			this.SaveCurrentDatabase  = saveCurrentDatabase;
			this.SaveHistoricDatabase = saveHistoricDatabase;
		}

		public MsSqlAuditorModel Model         { get; private set; }
		public ConnectionTabControl TabControl { get; private set; }
		public string OutputFolder             { get; private set; }
		public bool SaveCurrentDatabase        { get; private set; }
		public bool SaveHistoricDatabase       { get; private set; }
	}
}
