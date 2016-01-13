using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Xml;
using log4net;
using MSSQLServerAuditor.BusinessLogic.LocalStorage;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Groups;
using MSSQLServerAuditor.Preprocessor;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.BusinessLogic
{
	/// <summary>
	/// Processor for saving, converting and visualizing data from queries
	/// </summary>
	internal class VisualizeProcessor
	{
		private static readonly ILog              log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly        MsSqlAuditorModel _model;

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="model">Used model</param>
		public VisualizeProcessor(MsSqlAuditorModel model)
		{
			this._model = model;
		}

		public VisualizeData GetVisualizeData(
			NodeDataProvider dataProvider,
			GraphicsInfo     graphicsInfo
		)
		{
			XmlDocument          xmlData       = dataProvider.XmlDocument;
			MultyQueryResultInfo queriesResult = dataProvider.QueryResult;

			VisualizeData result = new VisualizeData
			{
				NodeLastUpdated        = queriesResult.NodeLastUpdated,
				NodeLastUpdateDuration = queriesResult.NodeLastUpdateDuration
			};

			if (xmlData != null && xmlData.DocumentElement != null)
			{
				result.SourceXml = xmlData.FormatXml();
			}

			ConcreteTemplateNodeDefinition nodeDefinition = dataProvider.NodeDefinition;

			string xslFileName = GetXslFileName(nodeDefinition);

			if (xslFileName != null && File.Exists(xslFileName))
			{
				XmlDocument xslDoc = new XmlDocument();

				xslDoc.Load(xslFileName);

				ConnectionGroupInfo connectionGroup = nodeDefinition.Connection;

				if (AppVersionHelper.IsNotDebug() && !connectionGroup.IsExternal)
				{
					CryptoProcessor cryptoProcessor = new CryptoProcessor(
						this._model.Settings.SystemSettings.PublicKeyXmlSign,
						this._model.Settings.SystemSettings.PrivateKeyXmlDecrypt
					);

					cryptoProcessor.DecryptXmlDocument(xslDoc);
				}

				try
				{
					XslPreprocessManager preprocessManager = GetManager(
						dataProvider,
						graphicsInfo
					);

					List<PreprocessorAreaData> datas;

					using (preprocessManager.ExecuteXslPreprocessing(xslDoc, out datas))
					{
					}

					foreach (PreprocessorAreaData preprocessorAreaData in datas)
					{
						preprocessorAreaData.CheckPreprocessors();
					}

					result.PreprocessorAreas = datas.ToList();
				}
				catch (Exception ex)
				{
					log.ErrorFormat(
						"nodeDefinition.TemplateNode.Queries(.Name)='{0}';xslFileName='{1}';Exception:'{2}'",
						nodeDefinition.TemplateNode.Queries.Select(q => q.QueryName).Join(", "),
						xslFileName,
						ex
					);
				}
			}

			return result;
		}

		public int? GetDataRowCount(ConcreteTemplateNodeDefinition nodeDefinition, ConnectionGroupInfo connectionGroup)
		{
			return this._model.GetVaultProcessor(connectionGroup)
				.GetDataRowCount(
					connectionGroup,
					nodeDefinition
				);
		}

		private XslPreprocessManager GetManager(
			NodeDataProvider dataProvider,
			GraphicsInfo     graphicsInfo
		)
		{
			XslPreprocessManager result = new XslPreprocessManager(this._model.DbFs);

			result.AvailablePreprocessors.Add(new GraphPreprocessor(dataProvider, graphicsInfo));
			result.AvailablePreprocessors.Add(new GraphPreprocessorDialog(dataProvider, graphicsInfo));
			result.AvailablePreprocessors.Add(new DataGridPreprocessorDialog(dataProvider, graphicsInfo));
			result.AvailablePreprocessors.Add(new HtmlPreprocessorDialog(dataProvider, graphicsInfo, result));
			// result.AvailablePreprocessors.Add(new RDLPreprocessorDialog(dataProvider, graphicsInfo));

			return result;
		}

		private TemplateNodeInfo GetTemplateToDisplay(
			ConcreteTemplateNodeDefinition nodeDefinition,
			out string                     dbName
		)
		{
			dbName = nodeDefinition.Group.Id;

			return nodeDefinition.TemplateNode;
		}

		public string GetXslFileName(ConcreteTemplateNodeDefinition nodeDefinition)
		{
			string           dbName            = string.Empty;
			TemplateNodeInfo templateToDisplay = GetTemplateToDisplay(nodeDefinition, out dbName);

			if (string.IsNullOrEmpty(templateToDisplay.XslTemplateFileName))
			{
				return null;
			}

			if (!string.IsNullOrWhiteSpace(templateToDisplay.XslTemplateFileName))
			{
				return this._model.FilesProvider.GetXslLocalizedTemplateFileName(
					templateToDisplay.XslTemplateFileName,
					nodeDefinition.Connection.TemplateDir
				);
			}

			TemplateNodeLocaleInfo localized = templateToDisplay.GetLocale(
				true,
				this._model.Settings.ReportLanguage
			);

			return this._model.FilesProvider.GetXslTemplateFileName(
				localized.TemplateFile,
				nodeDefinition.Connection.TemplateDir
			);
		}
	}
}
