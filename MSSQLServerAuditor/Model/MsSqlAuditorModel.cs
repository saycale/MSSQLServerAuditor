#define KEEPUNSIGNED
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using log4net;
using MSSQLServerAuditor.BusinessLogic;
using MSSQLServerAuditor.Model.Internationalization;
using MSSQLServerAuditor.Model.Loaders;
using MSSQLServerAuditor.Model.Settings;
using MSSQLServerAuditor.Utils;
using MSSQLServerAuditor.BusinessLogic.LocalStorage;
using MSSQLServerAuditor.SQLite.Databases;
using MSSQLServerAuditor.Utils.Cryptography;
using LayoutSettings = MSSQLServerAuditor.Model.Settings.LayoutSettings;

namespace MSSQLServerAuditor.Model
{
	/// <summary>
	/// Event args for settings change event
	/// </summary>
	public class SettingsChangedEventArgs : EventArgs
	{
		public SettingsInfo OldSetting { get; private set; }
		public SettingsInfo NewSetting { get; private set; }

		public SettingsChangedEventArgs()
		{
			this.OldSetting = null;
			this.NewSetting = null;
		}

		public SettingsChangedEventArgs(SettingsInfo oldInfo, SettingsInfo newInfo) : this()
		{
			this.OldSetting = oldInfo;
			this.NewSetting = newInfo;
		}
	}

	/// <summary>
	/// Delegate for settings change event
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="e">Event args</param>
	public delegate void SettingsChangedDelegate(object sender, SettingsChangedEventArgs e);

	/// <summary>
	/// MS SQL Server Auditor data model
	/// </summary>
	public class MsSqlAuditorModel
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private readonly VisualizeProcessor                               _visualizeProcessor;
		private          IStorageManager                                  _storageManager;
		private readonly Dictionary<ConnectionGroupInfo, IStorageManager> _vaultProcessors;
		private          DbFS                                             _dbFs;
		private          SettingsInfo                                     _settings;
		private          LayoutSettings                                   _layoutSettings;
		private          LocaleManager                                    _localeManager;
		private          ActivityLayoutSetting                            _templateSettings;
		private readonly FilesProvider                                    _filesProvider;
		private readonly ICryptoService                                   _cryptoService;

		/// <summary>
		/// Constructor with mainform
		/// </summary>
		/// <param name="mainForm">Mainform </param>
		public MsSqlAuditorModel()
		{
			this._vaultProcessors  = new Dictionary<ConnectionGroupInfo, IStorageManager>();
			this._settings         = null;
			this._layoutSettings   = null;
			this._localeManager    = null;
			this._templateSettings = null;
			this._filesProvider    = new FilesProvider(this);

			LoadSettings();

			this._visualizeProcessor = new VisualizeProcessor(this);
			this._cryptoService      = new CachingCryptoService(Settings.EncryptionKey);

			DeleteTempCurrentDbFile();
		}

		public void Initialize()
		{
			StorageManager manager = new StorageManager(this, this._filesProvider, true);

			manager.InitializeDataBases();

			this._storageManager = manager;
			this._dbFs = new DbFS(this._filesProvider.GetDbFsFileName());
		}

		~MsSqlAuditorModel()
		{
			DeleteTempCurrentDbFile();
		}

		/// <summary>
		/// Settings change event
		/// </summary>
		public event SettingsChangedDelegate SettingsChanged;

		/// <summary>
		/// Occurs in case of change of settings
		/// </summary>
		/// <param name="e">SettingsChangedEventArgs</param>
		protected virtual void OnSettingsChanged(SettingsChangedEventArgs e)
		{
			SettingsChangedDelegate handler = SettingsChanged;

			if (handler != null)
			{
				handler(this, e);
			}
		}

		private void DeleteTempCurrentDbFile()
		{
			try
			{
				if (File.Exists(FilesProvider.GetTempCurrentDbFileName()))
				{
					GC.Collect();
					File.Delete(FilesProvider.GetTempCurrentDbFileName());
				}
			}
			catch (Exception ex)
			{
				log.Error("Unable to remove temporary db file", ex);
			}
		}

		//void ReloadConectionGroupInfos()
		//{
		//    var filenames = _filesProvider.GetConnectionsFilenames();
		//    _connections = new List<ConnectionGroupInfo>();
		//    foreach (var filename in filenames)
		//    {
		//        _connections.AddRange(ConnectionsLoader.LoadFromXml(filename));
		//    }
		//}

		/// <summary>
		/// Load Template
		/// </summary>
		/// <param name="filename">Xml-file name</param>
		/// <param name="connectionGroup"></param>
			/// <param name="isExternal">Is opened from user file template</param>
		/// <returns>Tree template</returns>
		public TemplateNodeInfo LoadTemplateNodes(
			string            filename,
			bool              isExternal,
			out string        startupTemplateId,
			out Stack<string> startupTemplateInfoIdStack
		)
		{
			if (!File.Exists(filename))
			{
				throw new FileNotFoundException("Template needed!");
			}

			var doc = new XmlDocument();

			doc.Load(filename);

			if (AppVersionHelper.IsNotDebug() && !isExternal)
			{
				var cryptoProcessor = new CryptoProcessor(
					this.Settings.SystemSettings.PublicKeyXmlSign,
					this.Settings.SystemSettings.PrivateKeyXmlDecrypt
				);

				cryptoProcessor.DecryptXmlDocument(doc);
			}

			return TemplateNodesLoader.LoadFromXml(this, doc, out startupTemplateId, out startupTemplateInfoIdStack);
		}

		/// <summary>
		/// Load queries with signature check
		/// </summary>
		/// <param name="filename">Xml-file name</param>
		/// <param name="isExternal">Is opened from user file template</param>
		/// <returns>Queries list</returns>
		public List<QueryInfo> LoadQueries(string filename, bool isExternal)
		{
			log.InfoFormat("filename:'{0}',isExternal:'{1}'",
				filename ?? "<null>",
				isExternal
			);

			List<string> wrongQueries = new List<string>();
			CryptoProcessor cryptoProcessor = null;

			try
			{
				if (AppVersionHelper.IsNotDebug())
				{
					cryptoProcessor = new CryptoProcessor(
						Settings.SystemSettings.PublicKeyXmlSign,
						Settings.SystemSettings.PrivateKeyXmlDecrypt
					);
				}
			}
			catch (System.Security.XmlSyntaxException ex)
			{
				log.Error(ex);
			}
			catch (Exception ex)
			{
				log.Error(ex);
				log.Error(ex.GetBaseException());
			}

			List<QueryInfo> queries = QueriesLoader.LoadFromXml(filename, cryptoProcessor, isExternal);

			for (int i = queries.Count - 1; i >= 0; i--)
			{
				QueryInfo query = queries[i];

				log.InfoFormat("query:'{0}'", query);

				if (AppVersionHelper.IsNotDebug() && !isExternal)
				{
					for (int j = query.DatabaseSelect.Count - 1; j >= 0; j--)
					{
						QueryItemInfo queryItem = query.DatabaseSelect[j];

						if (cryptoProcessor != null && !cryptoProcessor.Verify(queryItem.Text, queryItem.Signature))
						{
							if (!wrongQueries.Contains(query.Name))
							{
								wrongQueries.Add(query.Name);
							}

							query.DatabaseSelect.RemoveAt(j);
						}
					}

					for (int j = query.Items.Count - 1; j >= 0; j--)
					{
						QueryItemInfo queryItem = query.Items[j];

						log.InfoFormat("queryItem.Text:'{0}'", queryItem.Text);

						if (cryptoProcessor != null && !cryptoProcessor.Verify(queryItem.Text, queryItem.Signature))
						{
							if (!wrongQueries.Contains(query.Name))
							{
								wrongQueries.Add(query.Name);
							}

							query.Items.RemoveAt(j);
						}
					}
				}

				if (query.Items.Count == 0)
				{
					queries.RemoveAt(i);
				}
			}

			if ((Settings.WarnAboutUnsignedQuery) && (wrongQueries.Count > 0))
			{
				StringBuilder sb = new StringBuilder();

				sb.Append(filename + Environment.NewLine + Environment.NewLine);

				foreach (string wrongQuery in wrongQueries)
				{
					sb.Append(wrongQuery + Environment.NewLine);
				}

				MessageBox.Show(sb.ToString(), LocaleManager.GetLocalizedText(LocaleManager.Exceptions, "wrongQueriesSignatures"));
			}

			return queries;
		}

		/// <summary>
		/// Get available connection list
		/// </summary>
		public List<ConnectionGroupInfo> GetAvailableConnections()
		{
			var filenames   = this._filesProvider.GetConnectionsFilenames();
			var connections = new List<ConnectionGroupInfo>();

			foreach (var filename in filenames)
			{
				connections.AddRange(ConnectionsLoader.LoadFromXml(filename));
			}

			return connections;
		}

		/// <summary>
		/// Get current locale manager
		/// </summary>
		public LocaleManager LocaleManager
		{
			get
			{
				if (this._localeManager == null)
				{
					this._localeManager = new LocaleManager(
						() => Settings.InterfaceLanguage, FilesProvider.LocaleFileName
					);
				}

				return this._localeManager;
			}
		}

		/// <summary>
		/// Get new Sql Processor
		/// </summary>
		/// <returns>New Sql Processor</returns>
		public SqlProcessor GetNewSqlProcessor()
		{
			return GetNewSqlProcessor(CancellationToken.None);
		}

		/// <summary>
		/// Get new Sql Processor with cancellation
		/// </summary>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>New Sql Processor</returns>
		public SqlProcessor GetNewSqlProcessor(CancellationToken cancellationToken)
		{
			return new SqlProcessor(this, cancellationToken);
		}

		/// <summary>
		/// Get current Visualize processor
		/// </summary>
		internal VisualizeProcessor VisualizeProcessor
		{
			get { return this._visualizeProcessor; }
		}

		internal ICryptoService CryptoService
		{
			get { return this._cryptoService; }
		}

		/// <summary>
		/// Default (active) Vault processor
		/// </summary>
		public IStorageManager DefaultVaultProcessor
		{
			get { return this._storageManager; }
		}

		/// <summary>
		/// Get vault processor for Connection group)
		/// </summary>
		/// <param name="connectionGroup"></param>
		/// <returns></returns>
		public IStorageManager GetVaultProcessor(ConnectionGroupInfo connectionGroup)
		{
			if (this._vaultProcessors.ContainsKey(connectionGroup))
			{
				return this._vaultProcessors[connectionGroup];
			}

			return DefaultVaultProcessor;
		}

		/// <summary>
		/// SQLite DB file system for temporary data
		/// </summary>
		public DbFS DbFs
		{
			get { return this._dbFs; }
		}

		/// <summary>
		/// Get current settings
		/// </summary>
		public SettingsInfo Settings
		{
			get { return this._settings; }
		}

		/// <summary>
		/// Layout settings.
		/// </summary>
		public LayoutSettings LayoutSettings
		{
			get { return this._layoutSettings; }
		}

		/// <summary>
		/// Template settings.
		/// </summary>
		public ActivityLayoutSetting TemplateSettings
		{
			get { return this._templateSettings; }
		}

		private void LoadSettings()
		{
			if (!File.Exists(FilesProvider.SystemSettingsFileName))
			{
				MessageBox.Show("System settings file doesn't exists at path " + FilesProvider.SystemSettingsFileName);
				throw new FileLoadException("System settings file doesn't exists at path " + FilesProvider.SystemSettingsFileName);
			}

			if (File.Exists(FilesProvider.UserSettingsFileName))
			{
				this._settings = SettingsLoader.LoadFromXml(
					FilesProvider.UserSettingsFileName,
					FilesProvider.SystemSettingsFileName
				);
			}
			else
			{
				this._settings = SettingsLoader.LoadFromXml(
					FilesProvider.UserSettingDefaultFileName,
					FilesProvider.SystemSettingsFileName
				);

				SettingsLoader.SaveToXml(
					FilesProvider.UserSettingsFileName,
					this._settings
				);
			}

			if (File.Exists(FilesProvider.UserLayoutSettingsFileName))
			{
				this._layoutSettings = SettingsLoader.LoadAsBaseFromXml<LayoutSettings>(
					FilesProvider.UserLayoutSettingsFileName
				);
			}
			else
			{
				this._layoutSettings = new LayoutSettings();

				SettingsLoader.SaveToXml(
					FilesProvider.UserLayoutSettingsFileName,
					this._layoutSettings
				);
			}

			if (File.Exists(FilesProvider.UserTemplateSettingsFileName))
			{
				// this._templateSettings = SettingsLoader.LoadAsTemplateFromXml<ActivityLayoutSetting>(FilesProvider.UserTemplateSettingsFileName);
				// ActivityLayoutSetting loader = new ActivityLayoutSetting();
				this._templateSettings = new ActivityLayoutSetting();

				this._templateSettings.UserSettings = SettingsLoader.LoadAsTemplateFromXml<List<InstanceTemplate>>(
					FilesProvider.UserTemplateSettingsFileName
				);
			}
			else
			{
				this._templateSettings = new ActivityLayoutSetting();

				SettingsLoader.SaveTemplateToXml(
					FilesProvider.UserTemplateSettingsFileName,
					this._templateSettings
				);
			}
		}

		/// <summary>
		/// Set new settings
		/// </summary>
		/// <param name="settingsInfo">New settings</param>
		public void SetSettings(SettingsInfo settingsInfo)
		{
			var oldSetting = this._settings.GetCopy();

			this._settings = settingsInfo;

			SaveSettings();

			OnSettingsChanged(new SettingsChangedEventArgs(oldSetting, settingsInfo));
		}

		/// <summary>
		/// Saves current settings
		/// </summary>
		public void SaveSettings()
		{
			SettingsLoader.SaveToXml(FilesProvider.UserSettingsFileName, this._settings);
		}

		/// <summary>
		/// Get current files provider
		/// </summary>
		public FilesProvider FilesProvider
		{
			get { return this._filesProvider; }
		}

		readonly ConcurrentBag<Tuple<string, QueryInfo>> _queriesCache = new ConcurrentBag<Tuple<string, QueryInfo>>();

		/// <summary>
		/// Get query info
		/// </summary>
		/// <param name="templateQueryInfo">Template node query info</param>
		/// <returns>Query info</returns>
		public List<QueryInfo> GetQueryByTemplateNodeQueryInfo(TemplateNodeQueryInfo templateQueryInfo)
		{
			log.InfoFormat("templateQueryInfo:QueryFileName:'{0}';QueryName:'{1}'",
				templateQueryInfo.QueryFileName,
				templateQueryInfo.QueryName
			);

			string key = templateQueryInfo.QueryFileName + "|" + templateQueryInfo.QueryName;

			if (!this._queriesCache.Any(x=>x.Item1 == key))
			{
				log.DebugFormat("QueryFileName:'{0}'",
					FilesProvider.GetQueryFileName(
						templateQueryInfo.QueryFileName,
						templateQueryInfo.TemplateNode.ConnectionGroup.TemplateDir
					)
				);

				List<QueryInfo> queries =
					LoadQueries(FilesProvider.GetQueryFileName(
						templateQueryInfo.QueryFileName,
						templateQueryInfo.TemplateNode.ConnectionGroup.TemplateDir
					),
					templateQueryInfo.TemplateNode.ConnectionGroup.IsExternal
				);

				foreach (QueryInfo query in queries)
				{
					log.InfoFormat("query:name:'{0}';source:'{1}'",
						query.Name,
						query.Source
					);

					string curKey = templateQueryInfo.QueryFileName + "|" + query.Name;

					if (!this._queriesCache.Any(x => x.Item1 == curKey && x.Item2.Source == query.Source))
					{
						this._queriesCache.Add(new Tuple<string, QueryInfo>(curKey, query));
					}
				}
			}

			if (!this._queriesCache.Any(x => x.Item1 == key))
			{
				throw new ArgumentException(
					string.Format(
						LocaleManager.GetLocalizedText(
							LocaleManager.Exceptions,
							"querynotfound"
						),
						key
					)
				);
			}

			return this._queriesCache.Where(x => x.Item1 == key).Select(x => x.Item2).ToList();
		}

		/// <summary>
		/// Flush query cache
		/// </summary>
		public void FlushQueryCache()
		{
			if (this._queriesCache != null)
			{
				lock (this._queriesCache)
				{
					this._queriesCache.Clear();
				}
			}
		}

		/// <summary>
		/// Flush locale cache
		/// </summary>
		public void FlushLocaleCache()
		{
			this._localeManager = null;
		}

		public void FlushVaultProcessors()
		{
			if (DefaultVaultProcessor != null)
			{
				DefaultVaultProcessor.FlushDbStructureCache();
			}

			if (this._vaultProcessors != null)
			{
				foreach (KeyValuePair<ConnectionGroupInfo, IStorageManager> pair in this._vaultProcessors)
				{
					if (pair.Value != null)
					{
						pair.Value.FlushDbStructureCache();
					}
				}
			}
		}

		public List<BindingWrapper<ModuleType>> GetModuleTypes(ConnectionType connectionType)
		{
			List<BindingWrapper<ModuleType>> moduleTypes = new List<BindingWrapper<ModuleType>>();

			if (connectionType != null && connectionType.ModuleTypes != null && connectionType.ModuleTypes.Any())
			{
				foreach (ModuleType moduleType in connectionType.ModuleTypes)
				{
					BindingWrapper<ModuleType> wrapper = new BindingWrapper<ModuleType>(
						moduleType,
						m =>
						{
							i18n localeItem = m.Locales.FirstOrDefault(
								l => l.Language == Settings.InterfaceLanguage
							);

							string displayName = localeItem != null
								? localeItem.Text
								: m.Id;

							return displayName.RemoveWhitespaces();
						}
					);

					moduleTypes.Add(wrapper);
				}
			}

			return moduleTypes;
		}

		public BindingWrapper<ConnectionType> GetConnectionType(QuerySource dbType)
		{
			ConnectionType[] connectionTypes = Settings.SystemSettings.ConnectionTypes;

			if (connectionTypes != null)
			{
				foreach (ConnectionType connectionType in connectionTypes)
				{
					QuerySource querySource;

					if (Enum.TryParse(connectionType.Id, true, out querySource))
					{
						if (querySource == dbType)
						{
							return new BindingWrapper<ConnectionType>(
								connectionType,
								ct =>
								{
									i18n localeItem = ct.Locales.FirstOrDefault(
										l => l.Language == Settings.InterfaceLanguage
									);

									string displayName = localeItem != null
										? localeItem.Text
										: ct.Id;

									return displayName.RemoveWhitespaces();
								}
							);
						}
					}
				}
			}

			return null;
		}

		public List<BindingWrapper<ConnectionType>> ConnectionTypes
		{
			get
			{
				List<BindingWrapper<ConnectionType>> typeWrappers    = new List<BindingWrapper<ConnectionType>>();
				ConnectionType[]                     connectionTypes = Settings.SystemSettings.ConnectionTypes;

				if (connectionTypes == null)
				{
					return typeWrappers;
				}

				foreach (ConnectionType connectionType in connectionTypes)
				{
					BindingWrapper<ConnectionType> wrapper = new BindingWrapper<ConnectionType>(
						connectionType,
						ct =>
						{
							QuerySource querySource;
							string      displayName;

							if (Enum.TryParse(ct.Id, true, out querySource))
							{
								i18n localeItem = ct.Locales.FirstOrDefault(
									l => l.Language == Settings.InterfaceLanguage
								);

								displayName = localeItem != null
									? localeItem.Text
									: ct.Id;
							}
							else
							{
								displayName = ct.Id;
							}

							return displayName.RemoveWhitespaces();
						}
					);

					typeWrappers.Add(wrapper);
				}

				return typeWrappers;
			}
		}

		/// <summary>
		/// Register association connection group -> vault processor
		/// </summary>
		/// <param name="vaultProcessor">Vault processor</param>
		/// <param name="connectionGroup">Connection group</param>
		public void AssociateVaultProcessor(IStorageManager vaultProcessor, ConnectionGroupInfo connectionGroup)
		{
			if (vaultProcessor != DefaultVaultProcessor)
			{
				this._vaultProcessors.Add(connectionGroup, vaultProcessor);
			}
		}

		/// <summary>
		/// Unregister association connection group -> vault processor
		/// </summary>
		/// <param name="connectionGroup">Connection group</param>
		public void DeassociateVaultProcessor(ConnectionGroupInfo connectionGroup)
		{
			if (this._vaultProcessors.ContainsKey(connectionGroup))
			{
				this._vaultProcessors.Remove(connectionGroup);
			}
		}

		/// <summary>
		/// AppDataFolder
		/// </summary>
		public string AppDataFolder
		{
			get
			{
				string strAppDataFolder = string.Empty;

				if (this._filesProvider != null)
				{
					strAppDataFolder = this._filesProvider.AppDataFolder;
				}

				return strAppDataFolder;
			}
		}

		/// <summary>
		/// UserDocsAppFolder
		/// </summary>
		public string UserDocsAppFolder
		{
			get
			{
				string strUserDocsAppFolder = string.Empty;

				if (this._filesProvider != null)
				{
					strUserDocsAppFolder = this._filesProvider.UserDocsAppFolder;
				}

				return strUserDocsAppFolder;
			}
		}
	}
}
