using System.IO;
using System.Net.Mail;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MSSQLServerAuditor.BusinessLogic;
using MSSQLServerAuditor.Graph;

namespace MSSQLServerAuditor.Preprocessor
{
	/// <summary>
	/// Xsl preprocessor for creation and configuration GraphControl (without injection)
	/// Duplicates behaviour of <see cref="GraphPreprocessorDialog"/>
	/// </summary>
	public class GraphPreprocessor : GraphPreprocessorDialog
	{
		/// <summary>
		/// Initializing object GraphPreprocessorDialog.
		/// </summary>
		public GraphPreprocessor(
			NodeDataProvider dataProvider,
			GraphicsInfo     graphicsInfo
		) : base(
				dataProvider,
				graphicsInfo
			)
		{
		}
	}

	/// <summary>
	/// Xsl preprocessor for creation and configuration GraphControl without injection
	/// </summary>
	public class GraphPreprocessorDialog : Preprocessor
	{
		private class GraphContentFactory : ContentFactory
		{
			public GraphContentFactory(
				IPreprocessor preprocessor,
				string        id,
				string        configuration
			) : base(
					preprocessor,
					id,
					configuration
				)
			{
			}

			public override Control CreateControl()
			{
				return CreateControlImpl();
			}

			public override MailContent CreateMailContent()
			{
				GraphControl graphControl = CreateControlImpl();
				MailContent  mailContent = new MailContent();
				MemoryStream ms          = new MemoryStream();

				graphControl.ChartInstance.SaveImage(
					ms,
					ChartImageFormat.Png
				);

				ms.Position = 0;

				LinkedResource imgLink = new LinkedResource(ms, "image/png")
				{
					ContentId = "chart1"
				};

				mailContent.Resource = imgLink;

				mailContent.Message = string.Format(
					@"Graph image:<br> <img src=""cid:chart1"" width=""{0}"" height=""{1}"" > ",
					Preprocessor.GraphicsInfo.Size.Width,
					Preprocessor.GraphicsInfo.Size.Height
				);

				return mailContent;
			}

			public override bool CanCreateMailContent
			{
				get { return true; }
			}

			private GraphControl CreateControlImpl()
			{
				GraphControl control = new GraphControl();

				control.SetId(this._id, PreprocessorTypeName);

				control.SetConfigurationFromText(
					this._configuration,
					this.Preprocessor.GraphicsInfo.Size,
					this.Preprocessor.DataProvider.XmlDocument
				);

				return control;
			}
		}

		/// <summary>
		/// Constant preprocessor type name.
		/// </summary>
		private const string PreprocessorTypeName = "GraphPreprocessorDialog";

		/// <summary>
		/// Initializing object GraphPreprocessorDialog.
		/// </summary>
		public GraphPreprocessorDialog(
			NodeDataProvider dataProvider,
			GraphicsInfo     graphicsInfo
		) : base(
				dataProvider,
				graphicsInfo
			)
		{
		}

		public override IContentFactory CreateContentFactory(string id, string configuration)
		{
			return new GraphContentFactory(
				this,
				id,
				configuration
			);
		}
	}
}
