using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using MSSQLServerAuditor.Model.Settings;
using MSSQLServerAuditor.Gui;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Connections.Factories;
using MSSQLServerAuditor.SQLite.Common;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.Utils.Cryptography;

namespace MSSQLServerAuditor.BusinessLogic.LocalStorage
{
	internal interface ISerializationCancellationToken
	{
		bool CancellationPending { get; }
	}

	internal class FormSerializationCancellationToken : ISerializationCancellationToken
	{
		private readonly ProgressForm _progressForm;

		public FormSerializationCancellationToken()
		{
			this._progressForm = null;
		}

		public FormSerializationCancellationToken(ProgressForm progressForm) : this()
		{
			this._progressForm = progressForm;
		}

		public bool CancellationPending
		{
			get
			{
				bool isCancellationPending = false;

				if (this._progressForm != null)
				{
					isCancellationPending = this._progressForm.CancellationPending;
				}
				else
				{
					isCancellationPending = false;
				}

				return isCancellationPending;
			}
		}
	}

	internal class StorageSerializer
	{
		private static readonly ILog          Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly ConnectionTabControl _connectionTabControl;
		private readonly string               _dateSuffix;
		private readonly MsSqlAuditorModel    _model;

		public StorageSerializer(MsSqlAuditorModel model, ConnectionTabControl connectionTabControl)
		{
			this._model                = model;
			this._connectionTabControl = connectionTabControl;
			this._dateSuffix           = DateTime.Now.ToString("s").Replace(":", "-");
		}

		public void SaveData(
			string                          outputFolder,
			bool                            saveCurrentDb,
			bool                            saveHistoricDb,
			ISerializationCancellationToken canceller
		)
		{
			string currentDb  = this.GetCurrentDbPath(outputFolder);
			string historicDb = this.GetHistoricDbPath(outputFolder);
			string reportDb   = this.GetReportDbPath(outputFolder);

			if (File.Exists(currentDb))
			{
				File.Delete(currentDb);
			}

			foreach (PostBuildSqliteDb postBuildDb in this._model.Settings.SystemSettings.PostBuildSQLiteDbs)
			{
				string historicDbFileName = string.Format(historicDb, postBuildDb.FileName);

				if (File.Exists(historicDbFileName))
				{
					File.Delete(historicDbFileName);
				}
			}

			if (saveCurrentDb)
			{
				if (this._connectionTabControl != null)
				{
					this.SaveSrcData(currentDb, reportDb, canceller);
				}
				else
				{
					CheckCancellation(canceller);
					this.BackupCurrent(currentDb);

					CheckCancellation(canceller);
					this.BackupReport(reportDb);
				}
			}

			CheckCancellation(canceller);

			if (saveHistoricDb)
			{
				foreach (HistoryStorage historyStorage in this._model.DefaultVaultProcessor.HistoryStorage)
				{
					string histStorageName = Path.GetFileName(historyStorage.FileName);
					this.BackupHistoric(
						historyStorage,
						string.Format(historicDb, histStorageName)
					);
				}
			}
		}

		private string GetHistoricDbPath(string outPath)
		{
			return Path.Combine(outPath, "{0}" + this._dateSuffix + ".msh");
		}

		private string GetCurrentDbPath(string outPath)
		{
			return Path.Combine(outPath, "current" + this._dateSuffix + ".msd");
		}

		private string GetReportDbPath(string outPath)
		{
			return Path.Combine(outPath, "report" + this._dateSuffix + ".msr");
		}

		private void BackupCurrent(string currentDb)
		{
			this.BackupDatabase(this._model.DefaultVaultProcessor.CurrentStorage, currentDb);
		}

		private void BackupHistoric(HistoryStorage historyStorage, string historicDb)
		{
			this.BackupDatabase(historyStorage, historicDb);
		}

		private void BackupReport(string reportDb)
		{
			this.BackupDatabase(this._model.DefaultVaultProcessor.ReportStorage, reportDb);
		}

		private void BackupDatabase(Database srcDatabase, string dstFileName)
		{
			var destination = Database.GetOrCreate(dstFileName);

			using (SQLiteConnection srcConnection = ConnectionFactory.CreateSQLiteConnection(srcDatabase))
			{
				srcConnection.Open();

				using (SQLiteConnection dstConnection = ConnectionFactory.CreateSQLiteConnection(destination, false))
				{
					dstConnection.Open();

					srcConnection.BackupDatabase(
						dstConnection,
						"main",
						"main",
						-1,
						null,
						-1
					);
				}
			}

			GC.Collect();
		}

		private void SaveSrcData(
			string                          currentDb,
			string                          reportDb,
			ISerializationCancellationToken canceller
		)
		{
			ConnectionData connectionData;
			StorageManager backupStorageManager = new StorageManager(
				this._model, false, currentDb, null, reportDb);

			backupStorageManager.InitializeDataBases();

			CheckCancellation(canceller);

			IEnumerable<ConcreteTemplateNodeDefinition> nodes =
				this._connectionTabControl.GetCurrentTree(out connectionData);

			if (nodes != null)
			{
				IList<ConcreteTemplateNodeDefinition> nodeList = nodes as IList<ConcreteTemplateNodeDefinition> ?? nodes.ToList();

				if (nodeList.Any())
				{
					ConnectionGroupInfo sourceConnection = connectionData.ConnectionGroup;
					ConnectionGroupInfo destConnection   = sourceConnection.CopyXmlContent();

					destConnection.Identity = backupStorageManager.CurrentStorage.ConnectionGroupDirectory.GetId(destConnection);

					Dictionary<TemplateNodeInfo, TemplateNodeInfo> clonesDict = new Dictionary<TemplateNodeInfo, TemplateNodeInfo>();

					ConnectionData destConnectionData = new ConnectionData(this._model, destConnection);
					TemplateNodeInfo nodesRoot        = nodeList.First().TemplateNode.GetRootPatent();
					TemplateNodeInfo clonedNodesRoot  = nodesRoot.CloneNotSavedTree(clonesDict, destConnectionData);

					CheckCancellation(canceller);

					backupStorageManager.CurrentStorage.NodeInstances.SaveTree(clonedNodesRoot);

					CheckCancellation(canceller);

					foreach (ConcreteTemplateNodeDefinition concreteTemplateNode in nodeList)
					{
						CheckCancellation(canceller);

						MultyQueryResultInfo queriesResult = this._model.DefaultVaultProcessor.ReadCurrentResult(
							sourceConnection,
							concreteTemplateNode
						);

						CheckCancellation(canceller);

						TemplateNodeInfo templateNode = concreteTemplateNode.TemplateNode;

						if (queriesResult != null)
						{
							backupStorageManager.SerializeData(
								clonesDict[templateNode],
								queriesResult
							);
						}
					}
				}
			}
		}

		private void CheckCancellation(ISerializationCancellationToken canceller)
		{
			if (canceller == null)
			{
				throw new ArgumentNullException("canceller");
			}

			if (canceller.CancellationPending)
			{
				throw new OperationCanceledException();
			}
		}

		internal static List<ConnectionGroupInfo> GetStoredConnections(
			string         inFile,
			ICryptoService cryptoService
		)
		{
			if (!IsMsdFile(inFile))
			{
				return null;
			}

			Database          currentDb         = Database.GetOrCreate(inFile);
			DbEntriesProvider dbEntriesProvider = new DbEntriesProvider(currentDb, cryptoService);

			return dbEntriesProvider.CloneStoredConnections();
		}

		internal static IStorageManager GetReadonlyVaultProcessor(MsSqlAuditorModel model, string inMsd, string inMsh, string inMsr)
		{
			if (inMsd == null)
			{
				return new StorageManager(
					model,
					true,
					model.FilesProvider.GetTempCurrentDbFileName(),
					inMsh,
					inMsr
				);
			}

			return new ReadonlyStorageManager(
				model,
				inMsd,
				inMsh,
				inMsr
			);
		}

		public static string GetMshFromMsd(string inMsd)
		{
			return GetFileFromMsd(inMsd, "{0}", "msh");
		}

		public static string GetMsrFromMsd(string inMsd)
		{
			return GetFileFromMsd(inMsd, "report", "msr");
		}

		private static string GetFileFromMsd(string inMsd, string fileNamePrefix, string fileType)
		{
			string file = inMsd
				.Replace("current", fileNamePrefix)
				.Replace(".msd", "." + fileType);

			if (File.Exists(file))
			{
				return file;
			}

			return null;
		}

		public static bool IsMsdFile(string inMsd)
		{
			return inMsd.ToLower().EndsWith(".msd");
		}
	}
}