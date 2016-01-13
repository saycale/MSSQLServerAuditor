using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using ILMerging;
using MSSQLServerAuditor.Licenser.Model.SignPreparators;
using MSSQLServerAuditor.Licenser.Model.Loaders;
using MSSQLServerAuditor.Licenser.Utils;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Internationalization;
using MSSQLServerAuditor.Model.Loaders;
using MSSQLServerAuditor.Model.Settings;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Licenser.Model
{
	internal class LicenserModel
	{
		/// <summary>
		/// The referenced assemblies.
		/// </summary>
		private readonly string[] referencedDlls =
			{
				@"lib\log4net.dll",
				@"lib\log4net.xml",
				@"lib\Microsoft.Data.ConnectionUI.dll",
				@"lib\Microsoft.Data.ConnectionUI.Dialog.dll",
				@"lib\x64\SQLite.Interop.dll",
				@"lib\x86\SQLite.Interop.dll",
				@"lib\System.Data.SQLite.dll",
				@"lib\System.Data.SQLite.xml",
				@"lib\ParallelExtensionsExtras.dll",
				@"lib\CommandLine.dll",
				@"lib\CommandLine.xml",
				@"lib\SqlCodeGuard.API.dll",
				@"lib\Teradata.Client.Entity.dll",
				@"lib\Teradata.Client.Provider.dll",
				@"lib\Teradata.Client.Provider.xml",
				@"lib\Teradata.Client.Provider.Install.dll",
				@"lib\Teradata.Client.Provider.resources.dll",
				@"lib\Teradata.Net.Security.Tdgss.dll"
			};

		private readonly LocaleManager   _lM;
		private          LicSettingsInfo _settings;
		private          ILogger         _logger;
		private          string          _settingsFileName;

		public LicenserModel(LocaleManager localeManager)
		{
			this._lM               = localeManager;
			this._settings         = null;
			this._logger           = null;
			this._settingsFileName = null;
		}

		internal void SetLogger(ILogger logger)
		{
			this._logger = logger;
		}

		internal void LoadSettingsFromFile(string fileName)
		{
			this._settings         = LicSettingsLoader.LoadFromXml(fileName);
			this._settingsFileName = fileName;
		}

		internal void CreateSettingsAndSave(string fileName)
		{
			RSACryptoServiceProvider rsa = null;

			this._settings = new LicSettingsInfo();

			// sign
			rsa = new RSACryptoServiceProvider();
			this._settings.PrivateKeySign = rsa.ToXmlString(true);
			this._settings.PublicKeySign  = rsa.ToXmlString(false);

			// decrypt
			rsa = new RSACryptoServiceProvider();
			this._settings.PrivateKeyDecrypt = rsa.ToXmlString(true);
			this._settings.PublicKeyDecrypt  = rsa.ToXmlString(false);

			LicSettingsLoader.SaveToXml(fileName, this._settings);

			this._settingsFileName = fileName;
		}

		public LicSettingsInfo Settings
		{
			get { return this._settings; }
		}

		public void SignLicense(string inFile, string outFile)
		{
			var connections     = ConnectionsLoader.LoadFromXml(inFile);
			var cryptoProcessor = new CryptoProcessor(this._settings.PrivateKeySign, this._settings.PrivateKeyDecrypt);

			SignLicenses(connections, cryptoProcessor);

			ConnectionsLoader.SaveToXml(outFile, connections);
		}

		private string GetText(string key)
		{
			return this._lM.GetLocalizedText("LicenserModel", key);
		}

		private void SignLicenses(IEnumerable<ConnectionGroupInfo> connections, CryptoProcessor cp)
		{
			foreach (ConnectionGroupInfo connection in connections)
			{
				foreach (InstanceInfo instance in connection.Connections)
				{
					if (instance.LicenseInfo == null)
					{
						this._logger.WriteToLog(GetText("NoLicenseDataForInstance") + instance.Instance);
					}
					else
					{
						instance.LicenseInfo.Signature = cp.Sign(instance.GetHash());
					}
				}
			}
		}

		public void ExecuteSign()
		{
			var srcFolder = this._settings.SrcFolder;

			var systemSettings = SystemSettingsInfo.LoadFrom(Settings.SystemSettingsFile);

			if (!Directory.Exists(this._settings.DstFolderFull))
			{
				Directory.CreateDirectory(this._settings.DstFolderFull);
			}

			//var connections = ConnectionsLoader.LoadFromXml(_settings.ConnectionsFileName);
			var cryptoProcessor = new CryptoProcessor(this._settings.PrivateKeySign, this._settings.PrivateKeyDecrypt);

			//SignLicenses(connections, cryptoProcessor);

			//string dstConnectionsFileName = GetDstFileName(_settings.ConnectionsFileName);
			//ConnectionsLoader.SaveToXml(dstConnectionsFileName, connections);

			// process ini files for SQLite databases
			ProcessPostBuildDbTemplateFiles(srcFolder, cryptoProcessor);

			List<string>                     xslTemplates;
			Dictionary<String, List<String>> queriesInQueryFiles;

			//var treeNodeTemplateFileNames = connections.Select(cnn => cnn.TemplateFileName).Distinct().ToList();

			ProcessTreeNodeTemplateFiles(this._settings.TemplateFiles, out xslTemplates, out queriesInQueryFiles, cryptoProcessor);

			ProcessXslTemplateFiles(xslTemplates, srcFolder+@"\"+systemSettings.TemplateDirectory+@"\", cryptoProcessor);

			ProcessQueryFiles(queriesInQueryFiles, srcFolder + @"\" + systemSettings.TemplateDirectory + @"\", cryptoProcessor);
			//ProcessQueryFilesRootAnnotation(queriesInRootAnnotationQueryFiles, srcFolder, cryptoProcessor);
		}

		private void ProcessTreeNodeTemplateFiles(
			IEnumerable<string>                  treeNodeTemplateFileNames,
			out List<string>                     xslTemplateFiles,
			out Dictionary<string, List<string>> queriesInQueryFiles,
			CryptoProcessor                      cryptoProcessor
		)
		{
			xslTemplateFiles = new List<string>();
			queriesInQueryFiles = new Dictionary<string, List<string>>();
			var systemSettings = SystemSettingsInfo.LoadFrom(Settings.SystemSettingsFile);

			foreach (string templateFileName in treeNodeTemplateFileNames)
			{
				string fullTemplateFileName = Path.GetFullPath(templateFileName);

				if (File.Exists(fullTemplateFileName))
				{
					string dstTemplateFileName = Path.Combine(
						this._settings.DstFolderFull
						+ @"\"
						+ systemSettings.TemplateDirectory
						+ @"\",
						Path.GetFileName(templateFileName)
					);

					this.SureCopy(fullTemplateFileName, dstTemplateFileName);

					XmlDocument doc = new XmlDocument();

					doc.Load(dstTemplateFileName);

					string        startupTemplateId          = null;
					Stack<string> startupTemplateInfoIdStack = null;
					var           rootNode                   = TemplateNodesLoader.LoadFromXml(null, doc, out startupTemplateId, out startupTemplateInfoIdStack);
					var           allNodesInTemplate         = ToPlainList(rootNode);

					foreach (TemplateNodeInfo node in allNodesInTemplate)
					{
						//foreach (TemplateNodeLocaleInfo locale in
						//        node.Locales.Where(l => !string.IsNullOrEmpty(l.TemplateFile)))
						//{
						//    if (!xslTemplates.ContainsKey(locale.TemplateFile))
						//    {
						//        xslTemplates.Add(locale.TemplateFile, locale.Language);
						//    }
						//}

						if (!string.IsNullOrWhiteSpace(node.XslTemplateFileName) && !xslTemplateFiles.Contains(node.XslTemplateFileName))
						{
							xslTemplateFiles.Add(node.XslTemplateFileName);
						}

						var allQueries = node.Queries.Union(node.GroupQueries);

						foreach (TemplateNodeQueryInfo query in allQueries)
						{
							if (!queriesInQueryFiles.ContainsKey(query.QueryFileName))
							{
								queriesInQueryFiles.Add(query.QueryFileName, new List<string>());
							}

							if (!queriesInQueryFiles[query.QueryFileName].Contains(query.QueryName))
							{
								queriesInQueryFiles[query.QueryFileName].Add(query.QueryName);
							}
						}

						foreach (TemplateNodeQueryInfo query in node.ConnectionQueries)
						{
							if (!queriesInQueryFiles.ContainsKey(query.QueryFileName))
							{
								queriesInQueryFiles.Add(query.QueryFileName, new List<string>());
							}

							if (!queriesInQueryFiles[query.QueryFileName].Contains(query.QueryName))
							{
								queriesInQueryFiles[query.QueryFileName].Add(query.QueryName);
							}
						}
					}

					if (AppVersionHelper.IsNotDebug())
					{
						cryptoProcessor.EncryptXmlDocument(doc);

						doc.Save(dstTemplateFileName);
					}
				}
				else
				{
					this._logger.WriteToLog(this.GetText("TemplateTreeFileNotFound") + templateFileName);
				}
			}
		}

		private void SureDirForFileExists(string dst)
		{
			string dir = Path.GetDirectoryName(dst);

			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}
		}

		private void SureCopy(string src, string dst)
		{
			SureDirForFileExists(dst);
			File.Copy(src, dst, true);
		}

		private IEnumerable<string> GetReportLanguages()
		{
			var systemSettings = SystemSettingsInfo.LoadFrom(Settings.SystemSettingsFile);
			return systemSettings.AvailableReportLanguages;
		}

		/// <summary>
		/// Processes the XSL template files.
		/// </summary>

		/// <summary>
		/// Processes the XSL template files.
		/// </summary>
		/// <param name="srcFolder">The SRC folder.</param>
		/// <param name="cryptoProcessor">The crypto processor.</param>
		private void ProcessPostBuildDbTemplateFiles(string srcFolder, CryptoProcessor cryptoProcessor)
		{
			string srcFileName    = String.Empty;
			string dstFileName    = String.Empty;
			var    systemSettings = SystemSettingsInfo.LoadFrom(Settings.SystemSettingsFile);

			this._logger.WriteToLog(
				"{Begin: processing system SQL scripts...."
			);

			//
			// PostBuildCurrentDb
			// processing SQL scripts for current SQLite database
			//
			this._logger.WriteToLog(
				"systemSettings.PostBuildCurrentDb:'{0}'",
				systemSettings.PostBuildCurrentDb
			);

			srcFileName = Path.Combine(
				srcFolder,
				String.Empty,
				String.Empty,
				systemSettings.PostBuildCurrentDb
			);

			this._logger.WriteToLog(
				"srcFileName:'{0}'",
				srcFileName
			);

			dstFileName = Path.Combine(
				this._settings.DstFolderFull,
				String.Empty,
				String.Empty,
				systemSettings.PostBuildCurrentDb
			);

			this._logger.WriteToLog(
				"dstFileName:'{0}'",
				dstFileName
			);

			processQueryFile(
				srcFileName,
				dstFileName,
				cryptoProcessor
			);

			//
			// PostBuildHistoricDbs
			// processing SQL scripts for all SQLite historical databases
			//
			foreach (var postBuildHistoricDb in systemSettings.PostBuildSQLiteDbs)
			{
				this._logger.WriteToLog(
					"systemSettings.PostBuildHistoricDb:Script'{0}';Alias:{1};Filename:{2}",
					postBuildHistoricDb.PostBuildScript,
					postBuildHistoricDb.Alias,
					postBuildHistoricDb.FileName
				);

				srcFileName = Path.Combine(
					srcFolder,
					String.Empty,
					String.Empty,
					postBuildHistoricDb.PostBuildScript
				);

				this._logger.WriteToLog(
					"srcFileName:'{0}'",
					srcFileName
				);

				dstFileName = Path.Combine(
					this._settings.DstFolderFull,
					String.Empty,
					String.Empty,
					postBuildHistoricDb.PostBuildScript
				);

				this._logger.WriteToLog(
					"dstFileName:'{0}'",
					dstFileName
				);

				processQueryFile(
					srcFileName,
					dstFileName,
					cryptoProcessor
				);
			}

			this._logger.WriteToLog("}End: processing system SQL scripts");
		}

		/// <param name="xslTemplateFileNames">The XSL templates.</param>
		/// <param name="srcFolder">The SRC folder.</param>
		/// <param name="cryptoProcessor">The crypto processor.</param>
		private void ProcessXslTemplateFiles(IEnumerable<string> xslTemplateFileNames, string srcFolder, CryptoProcessor cryptoProcessor)
		{
			var languageFolders = GetReportLanguages().ToArray();
			var systemSettings = SystemSettingsInfo.LoadFrom(Settings.SystemSettingsFile);

			this._logger.WriteToLog(
				"start modules processing...."
			);

			foreach (var fileName in xslTemplateFileNames)
			{
				try
				{
					this._logger.WriteToLog(
						"Start module file name processing. File:'{0}'",
						fileName
					);

					foreach (var localeFolder in languageFolders)
					{
						var srcFileName = Path.Combine(
							srcFolder,
							"Templates",
							localeFolder,
							fileName
						);

						if (!File.Exists(srcFileName))
						{
							this._logger.WriteToLog(
								"ERROR: Module not found. File name: '{0}'",
								srcFileName
							);

							continue;
						}

						string dstFileName = Path.Combine(
							this._settings.DstFolderFull
							+ @"\"
							+ systemSettings.TemplateDirectory
							+ @"\",
							"Templates",
							localeFolder,
							fileName
						);

						this.SureCopy(srcFileName, dstFileName);

						new XSLPreparator(new KeyValuePair<string, string>(dstFileName, localeFolder), this._logger)
							.Prepare(this._settings.AdditionalTemplates);

						if (AppVersionHelper.IsNotDebug())
						{
							var doc = new XmlDocument();

							doc.Load(dstFileName);

							this._logger.WriteToLog(
								"EncryptXmlDocument: file name: '{0}'",
								dstFileName
							);

							cryptoProcessor.EncryptXmlDocument(doc);

							doc.Save(dstFileName);
						}
					}
				}
				catch (Exception e)
				{
					this._logger.WriteToLog(
						"ERROR: Module has not been processed. File:'{0}';Error:'{1}'",
						fileName,
						e.Message
					);
				}
			}

			this._logger.WriteToLog(
				"Modules processing is completed."
			);
		}

		private void ProcessQueryFiles(
			Dictionary<String, List<String>> queriesInQueryFiles,
			string                           srcFolder,
			CryptoProcessor                  cryptoProcessor
		)
		{
			this._logger.WriteToLog(
				"Start queries processing..."
			);

			processQueryFilesInner(queriesInQueryFiles, srcFolder, cryptoProcessor);

			this._logger.WriteToLog(
				"Queries processing is completed."
			);
		}

		//private void ProcessQueryFilesRootAnnotation(Dictionary<String, List<String>> queriesInQueryFiles, string srcFolder,
		//                               CryptoProcessor cryptoProcessor)
		//{
		//    this._logger.WriteToLog("Обработка запросов Root Annotation");
		//    processQueryFilesInner(queriesInQueryFiles, srcFolder, cryptoProcessor);
		//    this._logger.WriteToLog("Обработка запросов Root Annotation");
		//}

		/// <summary>
		/// Processes query file
		/// </summary>
		/// <param name="srcFileName">The source query file</param>
		/// <param name="dstFileName">The destination query file</param>
		/// <param name="cryptoProcessor">The crypto processor</param>
		private void processQueryFile(string srcFileName, string dstFileName, CryptoProcessor cryptoProcessor)
		{
			this._logger.WriteToLog(
				"Start file processing: '{0}' -> '{1}'",
				srcFileName,
				dstFileName
			);

			if (File.Exists(srcFileName))
			{
				try
				{
					List<QueryInfo> queries = QueriesLoader.LoadFromXml(srcFileName);

					foreach (QueryInfo query in queries)
					{
						this._logger.WriteToLog(
							"Query processing:{0}-{1}-{2}",
							query.Id,
							query.Source,
							query.Name
						);

						foreach (var itemInfo in query.DatabaseSelect)
						{
							this._logger.WriteToLog("Обработка запроса на выборку БД({0}-{1})", itemInfo.MinVersion, itemInfo.MaxVersion);
							this.SignQueryItem(cryptoProcessor, itemInfo, query);
						}

						foreach (var itemInfo in query.GroupSelect)
						{
							this._logger.WriteToLog("Обработка запроса на выборку групп({0}-{1})", itemInfo.MinVersion, itemInfo.MaxVersion);
							this.SignQueryItem(cryptoProcessor, itemInfo, query);
						}

						foreach (var itemInfo in query.Items)
						{
							this._logger.WriteToLog("Обработка запроса на выборку данных ({0}-{1})", itemInfo.MinVersion, itemInfo.MaxVersion);
							this.SignQueryItem(cryptoProcessor, itemInfo, query);
						}

						this._logger.WriteToLog(
							"Query has been processed:{0}-{1}-{2}",
							query.Id,
							query.Source,
							query.Name
						);
					}

					SureDirForFileExists(dstFileName);

					QueriesLoader.SaveToXml(dstFileName, queries, cryptoProcessor);

					this._logger.WriteToLog(
						"File has been SUCESSFULLY processed: {0} -> {1}",
						srcFileName,
						dstFileName
					);
				}
				catch (Exception e)
				{
					this._logger.WriteToLog("ERROR: File has not been processed: {0}", srcFileName);
					this._logger.WriteToLog("ERROR: Query processing error: {0}", e.Message);
					this._logger.WriteToLog("ERROR: File has NOT been processed: {0} -> {1}", srcFileName, dstFileName);
				}
			}
			else
			{
				this._logger.WriteToLog("QueryFile is not found: {0}", srcFileName);
			}
		}

		private void processQueryFilesInner(
			Dictionary<String, List<String>> queriesInQueryFiles,
			string                           srcFolder,
			CryptoProcessor                  cryptoProcessor
		)
		{
			var systemSettings = SystemSettingsInfo.LoadFrom(Settings.SystemSettingsFile);

			foreach (var pair in queriesInQueryFiles)
			{
				try
				{
					var srcFileName = Path.Combine(srcFolder, pair.Key);

					this._logger.WriteToLog("Start file processing: {0}", srcFileName);

					if (File.Exists(srcFileName))
					{
						List<QueryInfo> queries = QueriesLoader.LoadFromXml(srcFileName);

						foreach (string qName in pair.Value)
						{
							if (queries.All(c => c.Name != qName))
							{
								this._logger.WriteToLog(string.Format(GetText("QueriyNotFoundInFile"), qName, pair.Key));
							}
						}

						string dstQueriesFileName = GetDstFileName(pair.Key, true, subDir: systemSettings.TemplateDirectory);

						processQueryFile(srcFileName, dstQueriesFileName, cryptoProcessor);
					}
					else
					{
						this._logger.WriteToLog("QueryFile was not found: {0}", srcFileName);
					}

					this._logger.WriteToLog("File has been processed: {0}", srcFileName);
				}
				catch (Exception e)
				{
					this._logger.WriteToLog("ERROR: Some files has not been processed: {0}", srcFolder);
					this._logger.WriteToLog("ERROR: Query processing error: {0}", e.Message);
				}
			}

			this._logger.WriteToLog("Query processing has been completed.");
		}

		/// <summary>
		/// The sign query item.
		/// </summary>
		/// <param name="cryptoProcessor">The crypto processor.</param>
		/// <param name="itemInfo">The item info.</param>
		/// <param name="query">The query.</param>
		private void SignQueryItem(CryptoProcessor cryptoProcessor, QueryItemInfo itemInfo, QueryInfo query)
		{
			this._logger.WriteToLog("prepare to sign the query....");

			itemInfo.ParentQuery = query;
			new QueryPreparator(this.Settings, this._logger).PrepareIfNeeds(itemInfo);

			this._logger.WriteToLog("Sign query....");

			itemInfo.Signature = cryptoProcessor.Sign(itemInfo.Text);
		}

		private static IEnumerable<TemplateNodeInfo> ToPlainList(TemplateNodeInfo node)
		{
			var result = new List<TemplateNodeInfo> {node};

			foreach (var child in node.Childs)
			{
				result.AddRange(ToPlainList(child));
			}

			return result;
		}

		//private static IEnumerable<TemplateNodeInfo> GetTemplateNodeInfoList(List<TemplateNodeInfo> src)
		//{
		//    List<TemplateNodeInfo> result = new List<TemplateNodeInfo>();
		//    result.AddRange(src);
		//    foreach (var sub in src)
		//    {
		//        result.AddRange(GetTemplateNodeInfoList(sub.Childs));
		//    }
		//    return result;
		//}

		private string CreatePredefinedPropertiesAssembly()
		{
			string libSubfolder = Path.Combine(this._settings.DstFolderFull, "lib");

			var compilerParameters = new CompilerParameters
			{
				GenerateExecutable = false,
				OutputAssembly     = Path.Combine(libSubfolder, Consts.DataFileName),
				CompilerOptions    = "/platform:anycpu"
			};

			if (!Directory.Exists(libSubfolder))
			{
				Directory.CreateDirectory(libSubfolder);
			}

			CompilerResults r = CodeDomProvider.CreateProvider("C#").CompileAssemblyFromSource(
				compilerParameters,
				@"
	using System.Runtime.CompilerServices;

	[assembly: InternalsVisibleTo(""MSSQLServerAuditor"")]
	namespace MSSQLServerAuditor.Data
	{
	internal class PredefinedProperties
	{
		public static string PublicKeySign = """ + this._settings.PublicKeySign + @""";
		public static string PrivateKeyDecrypt = """ + this._settings.PrivateKeyDecrypt + @""";
	}
	}
	");
			return compilerParameters.OutputAssembly;
		}

		/// <summary>
		/// To get the full exe file name and extension.
		/// </summary>
		/// <returns>
		/// The <see cref="string" />.
		/// </returns>
		private string GetExeFileName()
		{
			return Path.GetFileName(this.GetExecutableFilePath());
		}

		private string GetExecutableFilePath()
		{
			if (string.IsNullOrEmpty(this._settings.AppPath))
			{
				return typeof(QueriesLoader).Assembly.GetName().CodeBase;
			}

			return this._settings.AppPath;
		}

		/// <summary>
		/// The copy executable file.
		/// </summary>
		private string CopyExecutableFile()
		{
			string strSourceFolder                         = Path.GetDirectoryName(this.GetExecutableFilePath()) ?? string.Empty;
			string strExeConfigFileNameSource              = string.Format("{0}.config", this.GetExeFileName());
			string strVshostExeManifestFileNameSource      = string.Format("{0}.vshost.exe.manifest", Path.GetFileNameWithoutExtension(this.GetExecutableFilePath()));
			string strExeFileNameDestination               = Settings.ExeFileName;
			string strExeConfigFileNameDestination         = string.Format("{0}.config", Settings.ExeFileName);
			string strVshostExeManifestFileNameDestination = string.Format("{0}.vshost.exe.manifest", Path.GetFileNameWithoutExtension(Settings.ExeFileName));
			string strExeFilePathDestination               = Path.Combine(this._settings.DstFolder, strExeFileNameDestination);

			// copy "*.exe" file
			this._logger.WriteToLog("copy:" + this.GetExecutableFilePath());

			File.Copy(
				this.GetExecutableFilePath(),
				Path.Combine(this._settings.DstFolder, strExeFileNameDestination),
				true
			);

			// copy "*.exe.config" file
			this._logger.WriteToLog("copy:" + strExeConfigFileNameSource);

			File.Copy(
				Path.Combine(strSourceFolder, strExeConfigFileNameSource),
				Path.Combine(this._settings.DstFolder, strExeConfigFileNameDestination),
				true
			);

			// copy "*.vshost.exe.manifest" file
			this._logger.WriteToLog("copy:" + strVshostExeManifestFileNameSource);

			File.Copy(
				Path.Combine(strSourceFolder, strVshostExeManifestFileNameSource),
				Path.Combine(this._settings.DstFolder, strVshostExeManifestFileNameDestination),
				true
			);

			return strExeFilePathDestination;
		}

		/// <summary>
		/// The copy DLL.
		/// </summary>
		/// <param name="destinationFolder">The destination folder.</param>
		private void CopyReferencedDLL(string destinationFolder)
		{
			foreach (var dllFileName in this.referencedDlls)
			{
				var dllDestinationFilePath = Path.Combine(destinationFolder, dllFileName);
				var dllDestinationFolder   = Path.GetDirectoryName(dllDestinationFilePath) ?? string.Empty;

				if (!Directory.Exists(dllDestinationFolder))
				{
					Directory.CreateDirectory(dllDestinationFolder);
				}

				string sourceFolder = Path.GetDirectoryName(this.GetExecutableFilePath()) ?? string.Empty;

				File.Copy(Path.Combine(sourceFolder, dllFileName), dllDestinationFilePath, true);
			}
		}

		public string ExecuteExeCombine()
		{
			string predefinedAssemblyName = CreatePredefinedPropertiesAssembly();

			var destination = this.CopyExecutableFile();
			this.CopyReferencedDLL(this._settings.DstFolderFull);

			//var merge = new ILMerge
			//                {
			//                    DebugInfo = false,
			//                    Closed = true,
			//                    TargetKind = ILMerge.Kind.WinExe,
			//                    UnionMerge = true
			//                };
			//merge.SetInputAssemblies(new[] { destination, predefinedAssemblyName });
			//merge.SetSearchDirectories(new[] {this._settings.DstFolderFull});
			//merge.OutputFile = destination;
			//merge.SetTargetPlatform(this._settings.NetPlatform, this._settings.NetFolder);
			//merge.Merge();

			//File.Delete(predefinedAssemblyName);

			if (this._settings.UseDnGuard)
			{
				string prtDir = Path.Combine(this._settings.DstFolderFull, "prt");

				// delete directory and all files
				// the directory has been left after the last deployment
				if (Directory.Exists(prtDir))
				{
					this._logger.WriteToLog("DeleteDirectory:" + prtDir);
					Directory.Delete(prtDir);
				}

				// create directory again (the directory was deleted on the prev step)
				if (!Directory.Exists(prtDir))
				{
					this._logger.WriteToLog("CreateDirectory:" + prtDir);
					Directory.CreateDirectory(prtDir);
				}

				var options = this._settings.DnGuardOptions;// " /encrypt_code /encrypt_cctor /encrypt_ctor /encrypt_blob /encrypt_res /encrypt_string /obmode=0";

				if (this._settings.DnGuardX64Opt)
				{
					options = options + " /x64opt";
				}

				var commandPath = Path.Combine(this._settings.DnGuardFolder, this._settings.DnGuardExeName);
				var commandLine = EnsureIsQuoted(GetDstFileName(this._settings.ExeFileName)) + " " + EnsureIsQuoted(prtDir) + options;

				this._logger.WriteToLog("CommandPath:" + commandPath);
				this._logger.WriteToLog("CommandLine:" + commandLine);

				ExecuteCommandLineProcess(commandPath, commandLine);

				commandLine = EnsureIsQuoted(predefinedAssemblyName) + " " + EnsureIsQuoted(prtDir) + options;
				this.ExecuteCommandLineProcess(commandPath, commandLine);

				if (File.Exists(destination))
				{
					this._logger.WriteToLog("DeleteFile:" + destination);
					File.Delete(destination);
				}

				string dataAssembly = Path.GetFileName(predefinedAssemblyName);
				if (dataAssembly != null)
				{
					File.Delete(predefinedAssemblyName);
					File.Move(Path.Combine(prtDir, dataAssembly), Path.Combine(this.Settings.DstFolderFull, dataAssembly));
				}

				File.Move(Path.Combine(prtDir, this._settings.ExeFileName), destination);

				foreach (var fileName in Directory.GetFiles(prtDir))
				{
					var dstName = Path.Combine(this._settings.DstFolderFull, Path.GetFileName(fileName));

					if (File.Exists(dstName))
					{
						this._logger.WriteToLog("DeleteFile:" + dstName);
						File.Delete(dstName);
					}

					File.Move(fileName, dstName);
				}

				Directory.Delete(prtDir, true);
			}

			return destination;
		}

		private static string EnsureIsQuoted(string path)
		{
			return @"""" + path.Replace("\"", "") + @"""";
		}

		public string GetDstFileName(
			string srcFileName,
			bool   asRelative  = false,
			string subDir      = null,
			string newFileName = null
		)
		{
			string fName = asRelative ? srcFileName : Path.GetFileName(srcFileName);

			if (!string.IsNullOrEmpty(newFileName))
			{
				fName = newFileName;
			}

			if (subDir == null)
			{
				return Path.Combine(this._settings.DstFolderFull, fName);
			}

			return Path.Combine(this._settings.DstFolderFull, subDir, fName);
		}

		public void CopyAdditionalFiles(params string[] files)
		{
			foreach (string file in files)
			{
				string dstFile = GetDstFileName(file);
				File.Copy(file, dstFile, true);
			}
		}

		public void CopyLicenseFiles(string folder, string fileName)
		{
			DirectoryInfo dirInfo     = new DirectoryInfo(folder);
			var           directories = dirInfo.GetDirectories();
			DirectoryInfo newDirInfo  = new DirectoryInfo(this._settings.DstFolderFull + "/License");

			if (!newDirInfo.Exists)
			{
				newDirInfo.Create();
			}
			foreach (var directory in directories)
			{
				var directoryLang = new DirectoryInfo(newDirInfo.FullName + "/"+directory);

				if (!directoryLang.Exists)
				{
					directoryLang.Create();
				}

				var file = directory.GetFiles().Where((x => x.Name == fileName)).FirstOrDefault();

				if (file != null)
				{
					file.CopyTo(directoryLang.FullName + "/" + file.Name, true);
				}
			}
		}

		public void CopyAdditionalFilesSubDir(string subDir, params string[] files)
		{
			foreach (string file in files)
			{
				string dstFile = GetDstFileName(file, false, subDir);
				string dirName = Path.GetDirectoryName(dstFile);

				if (!Directory.Exists(dirName))
				{
					Directory.CreateDirectory(dirName);
				}

				File.Copy(file, dstFile, true);
			}
		}

		public void SaveSettings()
		{
			LicSettingsLoader.SaveToXml(_settingsFileName, this._settings);
		}

		private static XmlElement FindChild(XmlElement e, Func<XmlElement, bool> condition)
		{
			foreach (var child in e.ChildNodes.OfType<XmlElement>())
			{
				if (condition(child))
				{
					return child;
				}

				var subChild = FindChild(child, condition);

				if (subChild != null)
				{
					return subChild;
				}
			}

			return null;
		}

		private void UpdateComboLangs(XmlDocument wixDoc)
		{
			var systemSettingsFile = GetDstFileName(this._settings.SystemSettingsFile);
			var systemSettings = File.Exists(systemSettingsFile)
				? SystemSettingsInfo.LoadFrom(systemSettingsFile)
				: new SystemSettingsInfo();

			// var uiLangs = systemSettings.AvailableUiLanguages.Any()
			// 	? systemSettings.AvailableUiLanguages
			// 	: (new LocaleManager(null, Path.Combine(this._settings.SrcFolder, FilesProvider.LocaleFileNameNoPath))).AvailableLocales;
			var uiLangs = systemSettings.AvailableUiLanguages;

			var uiS = string.Join(" ", uiLangs.Select(l => string.Format("<ListItem Text=\"{0}\" Value=\"{0}\"/>", l)).ToArray());

			var uiLangCombo = FindChild(wixDoc.DocumentElement,
				n => n.Name == "ComboBox" && n.HasAttribute("Property") && n.Attributes["Property"].Value == "UI_LANG");

			uiLangCombo.InnerXml = uiS;

			// var reportLangs = systemSettings.AvailableReportLanguages.Any()
			// 	? systemSettings.AvailableReportLanguages
			// 	: (new LocaleManager(null, Path.Combine(this._settings.SrcFolder, FilesProvider.LocaleFileNameNoPath))).AvailableLocales;
			var reportLangs = systemSettings.AvailableReportLanguages;

			var reportS = string.Join(" ", reportLangs.Select(l => string.Format("<ListItem Text=\"{0}\" Value=\"{0}\"/>", l)).ToArray());

			var reportLangCombo = FindChild(wixDoc.DocumentElement,
				n => n.Name == "ComboBox" && n.HasAttribute("Property") && n.Attributes["Property"].Value == "REPORT_LANG");

			reportLangCombo.InnerXml = reportS;
		}

		public string ProcessWixFile()
		{
			try
			{
				using (var stream = new FileStream(GetDstFileName("appIcon.ico"), FileMode.Create, FileAccess.Write))
				{
					Icon.ExtractAssociatedIcon(GetDstFileName(this._settings.ExeFileName)).Save(stream);
				}

				CopyAdditionalFiles(this._settings.WixBannerFileName);

				//string path = Path.GetDirectoryName(GetExeFileName());
				//CopyAdditionalFilesSubDir(Path.Combine(path, "System.Data.SQLite.dll"));
				//CopyAdditionalFilesSubDir("x64", Path.Combine(path, "x64", "SQLite.Interop.dll"));
				//CopyAdditionalFilesSubDir("x86", Path.Combine(path, "x86", "SQLite.Interop.dll"));

				var globalId = 0;

				XmlDocument wixXml = new XmlDocument();
				wixXml.Load(this._settings.WixFileName);

				var ai = new AssemblyInfo(MSSQLAuditorAssembly);
				wixXml["Wix"]["Product"].SetAttribute("Name", this._settings.ProgramName);
				wixXml["Wix"]["Product"].SetAttribute("Version", MSSQLAuditorVersion);
				wixXml["Wix"]["Product"].SetAttribute("Manufacturer", ai.Trademark);
				wixXml["Wix"]["Product"].SetAttribute("Id", this._settings.ProgramId);
				FileInfo bannerInfo = new FileInfo(this._settings.WixBannerFileName);
				wixXml["Wix"]["Product"]["Binary"].SetAttribute("SourceFile", bannerInfo.Name);

				foreach (var packageNode in
						wixXml["Wix"]["Product"].ChildNodes.OfType<XmlNode>().Where(n => n.Name == "Package"))
					packageNode.Attributes["Manufacturer"].Value = ai.Trademark;

				wixXml["Wix"]["Product"].ChildNodes.OfType<XmlNode>()
					.FirstOrDefault(n => n.Name == "Property" && n.Attributes != null && n.Attributes["Id"] != null && n.Attributes["Id"].Value == "ARPCONTACT")
					.Attributes["Value"].Value = ai.Trademark;

				var directories = wixXml["Wix"]["Product"]["Directory"];
				var features = wixXml["Wix"]["Product"]["Feature"];

				var sysSettings = FindComponentById(directories, "MSSQLSERVERAUDITOR.SYSTEMSETTINGS");
				sysSettings["File"].Attributes["Source"].Value = GetDstFileName(this._settings.SystemSettingsFile);

				var userSettings = FindComponentById(directories, "USER_SETTINGS_COMPONENT");
				userSettings["File"].Attributes["Source"].Value = GetDstFileName(this._settings.UserSettingsFile);

				var language = FindComponentById(directories, "MSSQLSERVERAUDITOR.LANGUAGE");
				language["File"].Attributes["Source"].Value = GetDstFileName(this._settings.LanguageFile);

				var exeFile = FindComponentById(directories, "MSSQLSERVERAUDITOR.EXE");
				exeFile["File"].Attributes["Source"].Value = GetDstFileName(this._settings.ExeFileName);

				exeFile["File"].Attributes["Name"].Value = this._settings.ExeFileName;

				var desctopIcon = exeFile.ChildNodes.Cast<XmlNode>()
					.FirstOrDefault(x => x.Attributes["Id"].Value == "_CEC423EFBFFA47E091C23D3E50161584");
				if (desctopIcon != null)
				{
					desctopIcon.Attributes["Name"].Value = this._settings.ShorcName;
				}

				var programMenuIcon = exeFile.ChildNodes.Cast<XmlNode>()
					   .FirstOrDefault(x => x.Attributes["Id"].Value == "_DB4BF0B7B28446C982BF9922D6DC0C9C");
				if (programMenuIcon != null)
				{
					programMenuIcon.Attributes["Name"].Value = this._settings.ShorcName;
				}

				exeFile.Attributes["Win64"].Value = this._settings.DnGuardX64Opt ? "yes" : "no";

				//#61
				//var connectionsFile = FindComponentById(directories, "CONNECTIONS_COMPONENT");
				//connectionsFile["File"].Attributes["Source"].Value = GetDstFileName(_settings.ConnectionsFileName);
				//connectionsFile["File"].Attributes["Name"].Value = Path.GetFileName(_settings.ConnectionsFileName);

				wixXml["Wix"]["Product"]["Package"].SetAttribute("Platform", this._settings.DnGuardX64Opt ? "x64" : "x86");
				wixXml["Wix"]["Product"].ChildNodes.OfType<XmlElement>()
					.First(e => e.GetAttribute("Id") == "DIRCA_TARGETDIR")
					.SetAttribute("Value", (this._settings.DnGuardX64Opt
						? @"[ProgramFiles64Folder]\"
						: @"[ProgramFilesFolder]\") + this._settings.DirectoryName);

				var files = EnumerateFilesDeep("");

				foreach (string f in files)
				{
					XmlNode directory = GetDirectoryNodeForFile(directories, f, ref globalId);
					var component = wixXml.CreateElement("Component", wixXml.DocumentElement.NamespaceURI);
					string id = "Component" + globalId++.ToString();
					component.SetAttribute("Id", id);
					component.SetAttribute("DiskId", "1");
					component.SetAttribute("Guid", Guid.NewGuid().ToString());
					directory.AppendChild(component);

					var file = wixXml.CreateElement("File", wixXml.DocumentElement.NamespaceURI);
					file.SetAttribute("Id", id);
					string fName = f.Split('\\').Last();
					file.SetAttribute("Name", fName);
					file.SetAttribute("Source", Path.Combine(this._settings.DstFolderFull, f));
					component.AppendChild(file);

					var componentRef = wixXml.CreateElement("ComponentRef", wixXml.DocumentElement.NamespaceURI);
					componentRef.SetAttribute("Id", id);
					features.AppendChild(componentRef);
				}

				DirectoryInfo licDirInfo = new DirectoryInfo(this._settings.LicenseFolder);
				var licDirs = licDirInfo.GetDirectories();
				var licForm = wixXml.GetElementsByTagName("Dialog").Cast<XmlNode>().Where(x => x.Attributes["Id"] != null && x.Attributes["Id"].Value == "LicenseForm").FirstOrDefault();

				if (licForm != null)
				{
					int i = 0;

					foreach (var directory in licDirs)
					{
						var file = directory.GetFiles().Where((x => x.Name == this._settings.LicenseFileName)).FirstOrDefault();

						if (file != null)
						{
							var currentControl = wixXml.CreateElement("Control", wixXml.DocumentElement.NamespaceURI);
							currentControl.SetAttribute("Type","ScrollableText");
							currentControl.SetAttribute("Id", "scLicense"+i.ToString());
							currentControl.SetAttribute("Width", "358");
							currentControl.SetAttribute("Height", "180");
							currentControl.SetAttribute("X", "5");
							currentControl.SetAttribute("Y", "52");
							currentControl.SetAttribute("Sunken", "yes");
							currentControl.SetAttribute("TabSkip", "no");
							currentControl.SetAttribute("LeftScroll", "no");

							var textElement = wixXml.CreateElement("Text", wixXml.DocumentElement.NamespaceURI);
							textElement.SetAttribute("SourceFile", "$(sys.CURRENTDIR)\\License\\" + directory +"\\"+ file.Name);
							currentControl.AppendChild(textElement);

							var conditionElementShow = wixXml.CreateElement("Condition", wixXml.DocumentElement.NamespaceURI);
							conditionElementShow.SetAttribute("Action", "show");
							conditionElementShow.InnerXml = string.Format("UI_LANG=\"{0}\"", directory.Name.ToLower());
							currentControl.AppendChild(conditionElementShow);

							var conditionElementHide = wixXml.CreateElement("Condition", wixXml.DocumentElement.NamespaceURI);
							conditionElementHide.SetAttribute("Action", "hide");
							conditionElementHide.InnerXml = string.Format("UI_LANG&lt;&gt;\"{0}\"", directory.Name.ToLower());
							currentControl.AppendChild(conditionElementHide);

							licForm.AppendChild(currentControl);
						}

						i++;
					}
				}

				UpdateComboLangs(wixXml);

				wixXml.Save(GetDstFileName("setup.wxs"));

				if (File.Exists(this._settings.OutputMsi))
				{
					File.Delete(this._settings.OutputMsi);
				}

				ExecuteCommandLineProcess(Path.Combine(this._settings.WixFolder, "candle"), "setup.wxs -ext WixUtilExtension.dll");
				ExecuteCommandLineProcess(Path.Combine(this._settings.WixFolder, "light"),
					"setup.wixobj -ext WixUtilExtension.dll -spdb -out " + Path.GetFullPath(this._settings.OutputMsi));

			}
			finally
			{
				string fl = Path.Combine(this._settings.DstFolderFull, "setup.wixobj");

				if (File.Exists(fl))
				{
					File.Delete(fl);
				}

				fl = Path.Combine(this._settings.DstFolderFull, "setup.wixpdb");

				if (File.Exists(fl))
				{
					File.Delete(fl);
				}

				fl = Path.Combine(this._settings.DstFolderFull, "appicon.ico");

				if (File.Exists(fl))
				{
					File.Delete(fl);
				}

				fl = Path.Combine(this._settings.DstFolderFull, "DefBannerBitmap.bmp");

				if (File.Exists(fl))
				{
					File.Delete(fl);
				}

				fl = Path.Combine(this._settings.DstFolderFull, "setup.wxs");

				if (File.Exists(fl))
				{
					File.Copy(fl, Path.Combine(Path.GetDirectoryName(this._settings.OutputMsi), "setup.wxs"), true);
					File.Delete(fl);
				}
			}

			return Path.Combine(this._settings.OutputMsi);
		}

		private void ExecuteCommandLineProcess(string fileName, string args)
		{
			var process = new Process();

			process.StartInfo.FileName = fileName;
			process.StartInfo.WorkingDirectory = this._settings.DstFolderFull;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.Arguments = args;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

			process.Start();

			this._logger.WriteToLog(
				process.StandardOutput.ReadToEnd()
			);

			process.WaitForExit();
		}

		private XmlNode GetDirectoryNodeForFile(XmlElement directories, string file, ref int globalId)
		{
			string[] parsed  = file.Split('\\');
			var      dirNode = directories;

			for (int i = 0; i < parsed.Length - 1; i++)
			{
				XmlElement foundSubNode = null;

				foreach (XmlElement subNode in dirNode.ChildNodes)
				{
					if ((subNode.Name == "Directory") && (subNode.Attributes["Name"] != null) &&
						(subNode.Attributes["Name"].Value == parsed[i]))
					{
						foundSubNode = subNode;
						break;
					}
				}

				if (foundSubNode == null)
				{
					foundSubNode = directories.OwnerDocument.CreateElement(
						"Directory",
						directories.OwnerDocument.DocumentElement.NamespaceURI
					);

					foundSubNode.SetAttribute("Name", parsed[i]);
					foundSubNode.SetAttribute("Id", "DIR" + globalId++.ToString());
					dirNode.AppendChild(foundSubNode);
				}

				dirNode = foundSubNode;
			}

			return dirNode;
		}

		private bool ShouldIgnoreFile(string fileName)
		{
			string f = fileName.ToLower();

			if ((f == Path.GetFileName(this._settings.SystemSettingsFile.ToLower())) ||
				(f == Path.GetFileName(this._settings.UserSettingsFile.ToLower())) ||
				(f == Path.GetFileName(this._settings.WixBannerFileName.ToLower())) ||
				(f == GetExeFileName().ToLower()) ||
				(f == this._settings.ExeFileName.ToLower()) ||
				(f == "setup.wxs") ||
				(f == "appicon.ico") ||
				(f.StartsWith("setup.")) ||
				//f == Path.GetFileName(_settings.ConnectionsFileName.ToLower())) ||
				(f == Path.GetFileName(this._settings.LanguageFile.ToLower())))
			{
				return true;
			}

			if (f == "about folder.txt")
			{
				return true;
			}

			return false;
		}

		private bool ShoudlIgnoreDirectory(string directory)
		{
			if (directory == ".svn")
			{
				return true;
			}

			return false;
		}

		private IEnumerable<string> EnumerateFilesDeep(string directory)
		{
			string path = Path.Combine(this._settings.DstFolderFull, directory);
			var files   = Directory.GetFiles(path);

			List<string> result =
				files.Select(file => Path.Combine(directory, Path.GetFileName(file)))
					 .Where(file => !ShouldIgnoreFile(file))
					 .ToList();

			var dirs = Directory.GetDirectories(path).Select(dir => Path.Combine(directory, Path.GetFileName(dir)));

			foreach (string dir in dirs)
			{
				if (ShoudlIgnoreDirectory(dir))
				{
					continue;
				}

				result.AddRange(EnumerateFilesDeep(dir));
			}

			return result;
		}

		private XmlNode FindComponentById(XmlNode directories, string id)
		{
			foreach (XmlNode directory in directories.ChildNodes)
			{
				if (directory.Attributes != null && directory.Attributes["Id"] != null &&
					directory.Attributes["Id"].Value == id)
				{
					return directory;
				}

				var res = FindComponentById(directory, id);

				if (res != null)
				{
					return res;
				}
			}

			return null;
		}

		public string MSSQLAuditorVersion
		{
			get
			{
				return Assembly.LoadFrom(this.GetExecutableFilePath()).GetName().Version.ToString();
			}
		}

		public Assembly MSSQLAuditorAssembly
		{
			get
			{
				return Assembly.LoadFrom(this.GetExecutableFilePath());
			}
		}

		public void CopyDirectory(string sourcePath, string destinationPath)
		{
			if (Directory.Exists(destinationPath))
			{
				Directory.Delete(destinationPath, true);
			}

			Directory.CreateDirectory(destinationPath);

			// First create all directories
			foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
			{
				Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));
			}

			// Copy all files
			foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
			{
				File.Copy(newPath, newPath.Replace(sourcePath, destinationPath));
			}
		}

		public string GetDefaultDnGuardOptions()
		{
			return " /encrypt_code /encrypt_cctor /encrypt_ctor /encrypt_blob /encrypt_res /encrypt_string /obmode=0";
		}
	}
}
