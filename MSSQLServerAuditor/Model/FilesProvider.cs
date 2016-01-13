using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using log4net;
using MSSQLServerAuditor.Utils;
using MSSQLServerAuditor.Model.Loaders;

namespace MSSQLServerAuditor.Model
{
	/// <summary>
	/// Files locator
	/// </summary>
	public class FilesProvider
	{
		private static readonly ILog       log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly MsSqlAuditorModel _model;
		private Dictionary<string, string> _tempDocsFolderCache;
		private readonly string            _ConnectionFileNameFormatString;
		private readonly string            _OfflineDataTempFolder;

		/// <summary>
		/// File name pattern connections.
		/// </summary>
		public readonly string ConnectionsFileNamePattern;

		/// <summary>
		/// User documents application folder.
		/// </summary>
		public string UserDocsAppFolder
		{
			get;
			private set;
		}

		/// <summary>
		/// User documents application folder last directory only.
		/// </summary>
		public string UserDocsAppFolderLastOnly
		{
			get;
			private set;
		}

		/// <summary>
		/// Startup folder.
		/// </summary>
		public readonly string StartUpFolder;

		/// <summary>
		/// File name system settings.
		/// </summary>
		public readonly string SystemSettingsFileName;

		/// <summary>
		/// File name user default settings.
		/// </summary>
		public readonly string UserSettingDefaultFileName;

		/// <summary>
		/// Locale file name.
		/// </summary>
		public readonly string LocaleFileName;

		/// <summary>
		/// Default constructor
		/// </summary>
		public FilesProvider()
		{
			this._model                          = null;
			this._tempDocsFolderCache            = new Dictionary<string, string>();
			this.StartUpFolder                   = Application.StartupPath;
			this.SystemSettingsFileName          = null;
			this.UserSettingDefaultFileName      = null;
			this.LocaleFileName                  = null;
			this.UserDocsAppFolder               = null;
			this.UserDocsAppFolderLastOnly       = null;
			this._ConnectionFileNameFormatString = "MSSQLServerAuditor.Connections.{0}.xml";
			this.ConnectionsFileNamePattern      = string.Format(this._ConnectionFileNameFormatString, "*");
			this._OfflineDataTempFolder          = "Offline";
		}

		/// <summary>
		/// Constructor with model
		/// </summary>
		/// <param name="model">Model</param>
		public FilesProvider(MsSqlAuditorModel model) : this()
		{
			this._model = model;

			if (this._model != null)
			{
				this.SystemSettingsFileName = Path.Combine(
					this.StartUpFolder,
					CurrentAssembly.ProcessNameBase + ".SystemSettings.xml"
				);

				this.UserSettingDefaultFileName = Path.Combine(
					this.StartUpFolder,
					CurrentAssembly.ProcessNameBase + ".UserSettings.xml"
				);

				this.LocaleFileName = Path.Combine(
					this.StartUpFolder,
					CurrentAssembly.ProcessNameBase + ".i18n.xml"
				);
			}

			var systemSetting = SettingsLoader.GetSystemSetting(this.SystemSettingsFileName);

			if (systemSetting != null && !string.IsNullOrEmpty(systemSetting.UserDocsAppFolder))
			{
				this.UserDocsAppFolder = Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.Personal),
					systemSetting.UserDocsAppFolder
				);

				this.UserDocsAppFolderLastOnly = systemSetting.UserDocsAppFolder;
			}
			else
			{
				if (this._model != null)
				{
					this.UserDocsAppFolder = Path.Combine(
						Environment.GetFolderPath(Environment.SpecialFolder.Personal),
						CurrentAssembly.ProcessNameBase
					);

					this.UserDocsAppFolderLastOnly = CurrentAssembly.ProcessNameBase;
				}
			}

			if (this.UserDocsAppFolder != null)
			{
				if (!Directory.Exists(this.UserDocsAppFolder))
				{
					Directory.CreateDirectory(this.UserDocsAppFolder);
				}
			}

			var fileSystemWatcher = new FileSystemWatcher(this.StartUpFolder)
				{
					IncludeSubdirectories = true,
					EnableRaisingEvents = true
				};

			fileSystemWatcher.Changed +=FileSystemWatcherChanged;
			fileSystemWatcher.Created += FileSystemWatcherChanged;
			fileSystemWatcher.Deleted += FileSystemWatcherChanged;
			fileSystemWatcher.Renamed += FileSystemWatcherChanged;

			var fileUserWatcher = new FileSystemWatcher(this.UserDocsAppFolder)
				{
					IncludeSubdirectories = false,
					EnableRaisingEvents   = true
				};

			fileUserWatcher.Changed += FileUserWatcherChanged;
			fileUserWatcher.Created += FileUserWatcherChanged;
			fileUserWatcher.Deleted += FileUserWatcherChanged;
			fileUserWatcher.Renamed += FileUserWatcherChanged;

			var tempDocsWatcher = new FileSystemWatcher(GetTempDocsFolder())
				{
					IncludeSubdirectories = true,
					EnableRaisingEvents = true
				};

			tempDocsWatcher.Changed += FileUserWatcherChanged;
			tempDocsWatcher.Created += FileUserWatcherChanged;
			tempDocsWatcher.Deleted += FileUserWatcherChanged;
			tempDocsWatcher.Renamed += FileUserWatcherChanged;
		}

		/// <summary>
		/// Get user settings file name with path.
		/// </summary>
		public string UserSettingsFileName
		{
			get {
				string strFilePath = this.UserDocsAppFolder;

				if (this._model != null)
				{
					strFilePath = Path.Combine(
						strFilePath,
						CurrentAssembly.ProcessNameBase + "." + CurrentAssembly.Version + ".UserSettings.xml"
					);
				}

				return strFilePath;
			}
		}

		/// <summary>
		/// Get user layout settings file name with path.
		/// </summary>
		public string UserLayoutSettingsFileName
		{
			get {
				string strFilePath = this.UserDocsAppFolder;

				if (this._model != null)
				{
					strFilePath = Path.Combine(
						strFilePath,
						CurrentAssembly.ProcessNameBase + "." + CurrentAssembly.Version + ".UserLayoutSettings.xml"
					);
				}

				return strFilePath;
			}
		}

		/// <summary>
		/// Get user template settings file name with path.
		/// </summary>
		public string UserTemplateSettingsFileName
		{
			get {
				string strFilePath = this.UserDocsAppFolder;

				if (this._model != null)
				{
					strFilePath = Path.Combine(
						strFilePath,
						CurrentAssembly.ProcessNameBase + "." + CurrentAssembly.Version + ".UserTemplateSettings.xml"
					);
				}

				return strFilePath;
			}
		}

		public string AppDataFolder
		{
			get
			{
				log.InfoFormat(
					"LocalApplicationData:{0};UserDocsAppFolderLastOnly:{1}",
					Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
					this.UserDocsAppFolderLastOnly
				);

				return Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
					this.UserDocsAppFolderLastOnly,
					this.UserDocsAppFolderLastOnly
				);
			}
		}

		private void FileUserWatcherChanged(object sender, FileSystemEventArgs e)
		{
			// if (this._model != null)
			// {
			// 	this._model.FlushConnectionsCache();
			// }
		}

		void FileSystemWatcherChanged(object sender, FileSystemEventArgs e)
		{
			if (this._model != null)
			{
				this._model.FlushQueryCache();
				this._model.FlushLocaleCache();
				this._model.FlushVaultProcessors();
			}
		}

		/// <summary>
		/// Get path directory license.
		/// </summary>
		public string LicensesDirectory
		{
			get
			{
				log.InfoFormat(
					"UserDocsAppFolder:{0};LicenseDirectory:{1}",
					this.UserDocsAppFolder,
					this._model.Settings.SystemSettings.LicenseDirectory ?? ""
				);

				return Path.Combine(
					this.UserDocsAppFolder,
					this._model.Settings.SystemSettings.LicenseDirectory ?? ""
				);
			}
		}

		/// <summary>
		/// Get names of connections files
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetConnectionsFilenames()
		{
			if (!Directory.Exists(LicensesDirectory))
			{
				return Enumerable.Empty<string>();
			}

			return Directory.EnumerateFiles(
				LicensesDirectory,
				ConnectionsFileNamePattern
			);
		}

		/// <summary>
		/// Compose new file name for added connection.
		/// </summary>
		/// <returns></returns>
		public string ComposeNewFileNameForAddedConnection()
		{
			if (!Directory.Exists(LicensesDirectory))
			{
				Directory.CreateDirectory(LicensesDirectory);
			}

			string name = Path.Combine(
				LicensesDirectory,
				string.Format(
					this._ConnectionFileNameFormatString,
					DateTime.Now.ToString("yyyyMMdd")
				)
			);

			int i = 1;

			while (File.Exists(name))
			{
				name = Path.Combine(
					LicensesDirectory,
					string.Format(
						this._ConnectionFileNameFormatString,
						DateTime.Now.ToString("yyyyMMdd") + "(" + i + ")"
					)
				);

				i++;
			}

			return name;
		}

		/// <summary>
		/// Get Xsl full filename
		/// </summary>
		/// <param name="templateFile">Xsl-file name</param>
		/// <param name="baseDir">Base dir file. If nool or Empty - StartUpFolder</param>
		public string GetXslTemplateFileName(string templateFile, string baseDir = null)
		{
			if (string.IsNullOrEmpty(baseDir))
			{
				return Path.Combine(
					this.StartUpFolder + @"\" + Program.Model.Settings.TemplateDirectory,
					"Templates",
					templateFile
				);
			}

			return Path.Combine(
				baseDir,
				"Templates",
				templateFile
			);
		}

		public string GetXslLocalizedTemplateFileName(string templateFile, string baseDir = null)
		{
			if (string.IsNullOrEmpty(baseDir))
			{
				return Path.Combine(
					this.StartUpFolder + @"\" + Program.Model.Settings.TemplateDirectory,
					"Templates",
					this._model.Settings.ReportLanguage ?? "en",
					templateFile
				);
			}

			return Path.Combine(
				baseDir,
				"Templates",
				this._model.Settings.ReportLanguage ?? "en",
				templateFile
			);
		}

		/// <summary>
		/// Get template filename
		/// </summary>
		/// <param name="templateFile">Xml-file name</param>
		/// <param name="baseDir">Base dir file. If nool or Empty - StartUpFolder</param>
		/// <returns></returns>
		public string GetTreeTemplateFileName(string templateFile, string baseDir = null)
		{
			if (string.IsNullOrEmpty(baseDir))
			{
				return Path.Combine(
					this.StartUpFolder + @"\"+ Program.Model.Settings.TemplateDirectory,
					templateFile
				);
			}

			return Path.Combine(baseDir, templateFile);
		}

		/// <summary>
		/// Get query filename
		/// </summary>
		/// <param name="queryFileName">Xml-file name</param>
		/// <param name="baseDir">Base dir file. If nool or Empty - StartUpFolder</param>
		/// <returns></returns>
		public string GetQueryFileName(string queryFileName, string baseDir = null)
		{
			if (string.IsNullOrEmpty(baseDir))
			{
				return Path.Combine(
					this.StartUpFolder + @"\" + Program.Model.Settings.TemplateDirectory,
					queryFileName
				);
			}

			return Path.Combine(baseDir, queryFileName);
		}

		/// <summary>
		/// Get JS folder.
		/// </summary>
		/// <returns>Return JS folder</returns>
		public string GetJsFolder()
		{
			return Path.Combine(this.StartUpFolder, "Js");
		}

		/// <summary>
		/// Get temp documents folder.
		/// </summary>
		/// <param name="subFolder">Subfolder.</param>
		/// <returns>Return temp documents folder.</returns>
		public string GetTempDocsFolder(string subFolder = null)
		{
			string subFolderNotNull = subFolder ?? "....";

			if (this._tempDocsFolderCache.ContainsKey(subFolderNotNull))
			{
				return this._tempDocsFolderCache[subFolderNotNull];
			}

			string path = Path.Combine(this.AppDataFolder);

			if (subFolder != null)
			{
				path = Path.Combine(path, subFolder);
			}

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			this._tempDocsFolderCache.Add(subFolderNotNull, path);

			return path;
		}

		/// <summary>
		/// Get temp XLS file name.
		/// </summary>
		/// <returns>Return temp XLS file name.</returns>
		public string GetTempXslFileName()
		{
			return CombineFileName(GetTempDocsFolder(this._OfflineDataTempFolder), "temp.xsl");
		}

		/// <summary>
		/// Return temp data file name.
		/// </summary>
		/// <returns></returns>
		public string GetTempDataFileName()
		{
			return CombineFileName(GetTempDocsFolder(this._OfflineDataTempFolder), "temp.xml");
		}

		/// <summary>
		/// Return current SQLite DB file name.
		/// </summary>
		/// <returns></returns>
		public string GetCurrentDbFileName()
		{
			return CombineFileName(this.AppDataFolder, "current.sqlite");
		}

		/// <summary>
		/// Return temporary current SQLite DB file name.
		/// </summary>
		/// <returns></returns>
		public string GetTempCurrentDbFileName()
		{
			return CombineFileName(this.AppDataFolder, "current_tmp.sqlite");
		}

		/// <summary>
		/// Return DbFS SQLite DB file name.
		/// </summary>
		/// <returns></returns>
		public string GetDbFsFileName()
		{
			return CombineFileName(this.AppDataFolder, "dbFS.sqlite");
		}

		/// <summary>
		/// Return historic SQLite DB file pattern.
		/// </summary>
		/// <returns></returns>
		public string GetHistoricDbFileName()
		{
			return CombineFileName(this.AppDataFolder, "{0}.sqlite");
		}

		/// <summary>
		/// Return report SQLite DB file name.
		/// </summary>
		/// <returns></returns>
		public string GetReportDbFileName()
		{
			return CombineFileName(this.AppDataFolder, "report.sqlite");
		}

		private string CombineFileName(string path, string dbNameSuffix)
		{
			string strFileName = null;

			if (this._model != null)
			{
				strFileName = Path.Combine(
					path,
					CurrentAssembly.Version + "_" + dbNameSuffix
				);
			}

			return strFileName;
		}
	}
}
