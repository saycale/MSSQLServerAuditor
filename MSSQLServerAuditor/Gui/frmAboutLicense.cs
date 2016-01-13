using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using MSSQLServerAuditor.Model.Internationalization;

namespace MSSQLServerAuditor.Gui
{
	/// <summary>
	/// Dialog about license
	/// </summary>
	public partial class frmAboutLicense : LocalizableForm
	{
		/// <summary>
		/// Initializing object frmAboutLicense.
		/// </summary>
		public frmAboutLicense()
		{
			InitializeComponent();

			this.Text = GetLocalizedText("captionText");

			this.txtBHosts.Text = String.Format(GetLocalizedText("hostText"), AssemblyMachine)
				+ Environment.NewLine
				+String.Format(GetLocalizedText("userText"), AssemblyUserName);

			this.linkLblNewLicense.Text = GetLocalizedText("buyNewLicense");

			this.txtBHosts.GotFocus += textBox_GotFocus;

			this.txtBHosts.SelectionStart = 0;
		}

		[DllImport("user32.dll", EntryPoint = "HideCaret")]
		private static extern bool HideCaret(IntPtr hWnd);

		private void textBox_GotFocus(object sender, EventArgs e)
		{
			HideCaret((sender as TextBox).Handle);
		}

		private string AssemblyMachine
		{
			get { return Environment.MachineName; }
		}

		private string AssemblyUserName
		{
			get { return Environment.UserDomainName + @"\" + Environment.UserName; }
		}

		private void linkLblNewLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string supportMail          = Program.Model.Settings.SystemSettings.SupportEmail;
			string supportSubject       = Program.Model.Settings.SystemSettings.SupportSubject;
			string supportTemplateEmail = String.Format(Program.Model.Settings.SystemSettings.SupportTemplateMail,
				String.Format(GetLocalizedText("hostText"), AssemblyMachine)+ "; "
				, "\u2029"
				, " "+ String.Format(GetLocalizedText("userText"), AssemblyUserName));

			string mailto = String.Format("mailto:{0}?subject={1}&body={2}",
				supportMail,
				supportSubject,
				supportTemplateEmail
			);

			System.Diagnostics.Process.Start(mailto);
		}

		private void pbCopyHost_Click(object sender, EventArgs e)
		{
			Clipboard.Clear();

			Clipboard.SetText(String.Format(GetLocalizedText("hostText"), AssemblyMachine)
				+ Environment.NewLine
				+ String.Format(GetLocalizedText("userText"), AssemblyUserName));
		}
	}
}
