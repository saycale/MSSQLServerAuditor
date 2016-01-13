using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using log4net;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Model.Loaders
{
	/// <summary>
	/// Loader for templates
	/// </summary>
	public class TemplateNodesLoader
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// Load template from Xml-file
		/// </summary>
		/// <param name="model">Model mssql auditor.</param>
		/// <param name="document">XML document.</param>
		/// <param name="connectionGroup"></param>
		/// <returns>List of templates</returns>
		public static TemplateNodeInfo LoadFromXml(MsSqlAuditorModel model, XmlDocument document, out string startupTemplateId, out Stack<string> startupTemplateInfoIdStack)
		{
			var    serializer = new XmlSerializer(typeof(Template));
			var    root       = new TemplateNodeInfo();
			string templateId = null;

			//
			// #248 - fix memory leaks during XML files processing
			//
			// var nodeReader = new XmlNodeReader(document);
			root.Locales = new List<TemplateNodeLocaleInfo>();

			using (var nodeReader = new XmlNodeReader(document))
			{
				using (var xmlReader = XmlReader.Create(nodeReader, XmlUtils.GetXmlReaderSettings()))
				{
					var wrapper = (Template)serializer.Deserialize(xmlReader);

					root.MainFormWindowTitle = wrapper.MainFormWindowTitle;
					root.TreeTitle           = wrapper.TreeTitle;
					root.Childs              = wrapper.Infos;
					templateId               = wrapper.Id;
					startupTemplateId        = wrapper.StartupTemplateId;

					foreach (var localeInfo in wrapper.Locales)
					{
						root.Locales.Add(new TemplateNodeLocaleInfo()
						{
							Language     = localeInfo.Language,
							Text         = localeInfo.Text,
							TemplateFile = localeInfo.TemplateFile
						});
					}

					root.TemplateId          = templateId;
					root.Queries             = new List<TemplateNodeQueryInfo>();
					root.GroupQueries        = wrapper.GroupQueries ?? new List<TemplateNodeQueryInfo>();
					root.ConnectionQueries   = new List<TemplateNodeQueryInfo>();
					root.SqlCodeGuardQueries = new List<TemplateNodeSqlGuardQueryInfo>();

					startupTemplateInfoIdStack = new Stack<string>();

					if (!String.IsNullOrEmpty(startupTemplateId))
					{
						SetupStartupTemplateNodeIdStack(wrapper.Infos, startupTemplateInfoIdStack, startupTemplateId);
					}
				}
			}

			NormalizeTemplateNode(root);
			root.Init(templateId, model);

			return root;
		}

		/// <summary>
		/// Set stack from TemplateNodeInfoId.
		/// </summary>
		private static bool SetupStartupTemplateNodeIdStack(
			List<TemplateNodeInfo> templateList,
			Stack<string>          startupTemplateInfoIdStack,
			string                 startupTemplateId
		)
		{
			bool result = false;

			foreach (TemplateNodeInfo currentInfo in templateList)
			{
				if (currentInfo.Id == startupTemplateId)
				{
					startupTemplateInfoIdStack.Push(currentInfo.Id);
					result = true;
					break;
				}
				else
				{
					// Not need update childrens.
					result = SetupStartupTemplateNodeIdStack(
						currentInfo.Childs,
						startupTemplateInfoIdStack,
						startupTemplateId
					);

					if (result)
					{
						startupTemplateInfoIdStack.Push(currentInfo.Id);
						break;
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Check template database type.
		/// </summary>
		/// <param name="model">Model mssql auditor.</param>
		/// <param name="template">Template.</param>
		/// <param name="selectedSource">Selected db source.</param>
		/// <returns>If template available.</returns>
		public static bool CheckFromXml(MsSqlAuditorModel model, Template template, QuerySource selectedSource)
		{
			return template.DBType == selectedSource;
		}

		private static void NormalizeTemplateNode(TemplateNodeInfo node)
		{
			foreach (TemplateNodeLocaleInfo locale in node.Locales)
			{
				locale.Text = locale.Text.RemoveWhitespaces();
			}

			foreach (TemplateNodeInfo subnode in node.Childs)
			{
				NormalizeTemplateNode(subnode);
			}
		}

		/// <summary>
		/// Save templates to file.
		/// </summary>
		/// <param name="fileName">Xml-file name.</param>
		/// <param name="nodes">Nodes.</param>
		/// <param name="id">Template node id.</param>
		public static void SaveToXml(string fileName, List<TemplateNodeInfo> nodes, string id)
		{
			Template wrapper = new Template();

			wrapper.Infos = nodes;
			wrapper.Id    = id;

			XmlSerializer s = new XmlSerializer(typeof(Template));

			using (FileStream writer = new FileStream(fileName, FileMode.Create))
			{
				using (XmlWriter xmlWriter = XmlWriter.Create(writer, XmlUtils.GetXmlWriterSettings()))
				{
					s.Serialize(xmlWriter, wrapper);
				}
			}
		}

		/// <summary>
		/// Get templates.
		/// </summary>
		/// <returns></returns>
		public static List<Template> GetTemplates()
		{
			List<Template> retList      = new List<Template>();
			DirectoryInfo directoryInfo = new DirectoryInfo(
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Program.Model.Settings.TemplateDirectory));

			if (directoryInfo.Exists)
			{
				try
				{
					var serializer = new XmlSerializer(typeof(Template));

					var files = directoryInfo.GetFiles("*.xml");

					foreach (FileInfo file in files)
					{
						try
						{
							var doc = new XmlDocument();
							doc.Load(file.FullName);

							if (AppVersionHelper.IsNotDebug())
							{
								var cryptoProcessor = new CryptoProcessor(
									Program.Model.Settings.SystemSettings.PublicKeyXmlSign,
									Program.Model.Settings.SystemSettings.PrivateKeyXmlDecrypt);

								cryptoProcessor.DecryptXmlDocument(doc);
							}

							//
							// #248 - fix memory leaks during XML files processing
							//
							// var nodeReader = new XmlNodeReader(doc);
							using (var nodeReader = new XmlNodeReader(doc))
							{
								using (var xmlReader = XmlReader.Create(nodeReader, XmlUtils.GetXmlReaderSettings()))
								{
									var template = (Template)serializer.Deserialize(xmlReader);
									retList.Add(template);
								}
							}
						}
						catch (Exception ex)
						{
							log.Error(ex);

							if (file != null)
							{
								log.ErrorFormat("File:'{0}'", file);
							}

							log.ErrorFormat("Folder:'{0}'", directoryInfo);
						}
					}
				}
				catch (Exception ex)
				{
					log.Error(ex);
					log.ErrorFormat("Folder:'{0}'", directoryInfo);
				}
			}
			else
			{
				log.Error("Folder with models is not exists");
				log.ErrorFormat("Folder:'{0}'", directoryInfo);
			}

			return retList;
		}

		/// <summary>
		/// Item1 = name. Item2 = directory.
		/// </summary>
		/// <param name="templateID"></param>
		/// <returns></returns>
		public static Tuple<string, string> GetTemplateById(string templateID)
		{
			string strFileName          = string.Empty;
			string strFolderName        = string.Empty;
			List<Template> retList      = new List<Template>();
			DirectoryInfo directoryInfo = new DirectoryInfo(
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Program.Model.Settings.TemplateDirectory));

			if (directoryInfo.Exists)
			{
				try
				{
					var serializer = new XmlSerializer(typeof(Template));

					var files = directoryInfo.GetFiles("*.xml");

					foreach (FileInfo file in files)
					{
						try
						{
							var doc = new XmlDocument();
							doc.Load(file.FullName);

							if (AppVersionHelper.IsNotDebug())
							{
								var cryptoProcessor = new CryptoProcessor(
									Program.Model.Settings.SystemSettings.PublicKeyXmlSign,
									Program.Model.Settings.SystemSettings.PrivateKeyXmlDecrypt);

								cryptoProcessor.DecryptXmlDocument(doc);
							}

							//
							// #248 - fix memory leaks during XML files processing
							//
							// var nodeReader = new XmlNodeReader(doc);
							using (var nodeReader = new XmlNodeReader(doc))
							{
								using (var xmlReader = XmlReader.Create(nodeReader, XmlUtils.GetXmlReaderSettings()))
								{
									var template = (Template)serializer.Deserialize(xmlReader);

									if (templateID == template.Id)
									{
										strFileName   = file.Name;
										strFolderName = file.DirectoryName;
									}
								}
							}
						}
						catch (Exception ex)
						{
							log.Error(ex);

							if (file != null)
							{
								log.ErrorFormat("File:'{0}'", file);
							}

							log.ErrorFormat("Folder:'{0}'", directoryInfo);
						}
					}
				}
				catch (Exception ex)
				{
					log.Error(ex);
					log.ErrorFormat("Folder:'{0}'", directoryInfo);
				}
			}
			else
			{
				log.Error("Folder with models is not exists");
				log.ErrorFormat("Folder:'{0}'", directoryInfo);
			}

			return new Tuple<string, string>(strFileName, strFolderName);
		}

		public static Template GetTemplateByFile(string strFileName)
		{
			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(strFileName);

				Template retTemplate;

				if (AppVersionHelper.IsNotDebug())
				{
					var cryptoProcessor = new CryptoProcessor(
						Program.Model.Settings.SystemSettings.PublicKeyXmlSign,
						Program.Model.Settings.SystemSettings.PrivateKeyXmlDecrypt);

					cryptoProcessor.DecryptXmlDocument(doc);
				}

				using (var nodeReader = new XmlNodeReader(doc))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(Template));

					using (var xmlReader = XmlReader.Create(nodeReader, XmlUtils.GetXmlReaderSettings()))
					{
						retTemplate = (Template)serializer.Deserialize(xmlReader);
					}
				}

				return retTemplate;
			}
			catch (Exception ex)
			{
				log.Error(ex);
			}

			return null;
		}
	}
}
