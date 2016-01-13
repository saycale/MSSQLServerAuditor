using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using Microsoft.Win32;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Internationalization;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Gui
{
	/// <summary>
	/// Default about box
	/// </summary>
	[Obfuscation(Exclude = true, ApplyToMembers = true)]
	partial class frmAboutBox : LocalizableForm
	{
		private MsSqlAuditorModel _model;

		public frmAboutBox(MsSqlAuditorModel model)
		{
			this._model = model;

			InitializeComponent();

			Text = string.Format(GetLocalizedText("captionText"), CurrentAssembly.ProcessName);

			tbProductName.Text       = CurrentAssembly.Product;
			tbRelease.Text           = AppVersionHelper.CurrentAppVersionType.ToString();
			tbVersion.Text           = CurrentAssembly.Version;
			tbCopyright.Text         = CurrentAssembly.Copyright;
			tbCompanyName.Text       = CurrentAssembly.Company;
			tbEmail.Text             = Program.Model.Settings.SystemSettings.SupportEmail;
			tbURL.Text               = Program.Model.Settings.SystemSettings.SupportUrl;
			tbSQLiteFolder.Text      = this.SQLiteFolder;
			tbUserDocsAppFolder.Text = this.UserDocsAppFolder;
			tbProcessId.Text         = Process.GetCurrentProcess().Id.ToString();
		}

		#region Assembly Attribute Accessors

		private string SQLiteFolder
		{
			get
			{
				string strSQLiteFolder = string.Empty;

				if (this._model != null)
				{
					strSQLiteFolder = this._model.AppDataFolder;
				}

				return strSQLiteFolder;
			}
		}

		private string UserDocsAppFolder
		{
			get
			{
				string strUserDocsAppFolder = string.Empty;

				if (this._model != null)
				{
					strUserDocsAppFolder = this._model.UserDocsAppFolder;
				}

				return strUserDocsAppFolder;
			}
		}
		#endregion
	}
}
