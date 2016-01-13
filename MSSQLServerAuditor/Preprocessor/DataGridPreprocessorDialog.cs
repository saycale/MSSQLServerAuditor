using System;
using System.Windows.Forms;
using MSSQLServerAuditor.BusinessLogic;
using MSSQLServerAuditor.Table;

namespace MSSQLServerAuditor.Preprocessor
{
	/// <summary>
	/// Xsl preprocessor for creation and configuration GraphControl without injection
	/// </summary>
	public class DataGridPreprocessorDialog : Preprocessor
	{
		private class DataGridContentFactory : ContentFactory
		{
			public DataGridContentFactory(
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
				TableControl control = new TableControl();

				control.SetId(this._id, "DataGridPreprocessorDialog");

				control.SetConfigurationFromText(
					this._configuration,
					this.Preprocessor.GraphicsInfo.Size,
					this.Preprocessor.DataProvider.XmlDocument
				);

				return control;
			}
		}

		/// <summary>
		/// Data grid preprocessor dialog.
		/// </summary>
		public DataGridPreprocessorDialog(
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
			return new DataGridContentFactory(
				this,
				id,
				configuration
			);
		}
	}
}
