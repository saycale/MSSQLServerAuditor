using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;
using MSSQLServerAuditor.Managers;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Gui
{
	public partial class RunningTaskInfoForm : Form
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private readonly MsSqlAuditorModel _model;
		private string                     _mainText;
		private string                     _errorText;
		private bool                       _isLoaded;
		private bool                       _isNeedClose;

		public TreeTaskProgressManager ProgressManager { get; private set; }

		private string MainText
		{
			get { return this._mainText; }

			set
			{
				if (this._mainText != value)
				{
					this._mainText = value;
					Refresh();
				}
			}
		}

		public RunningTaskInfoForm()
		{
			this._model       = null;
			this._mainText    = null;
			this._errorText   = null;
			this._isLoaded    = false;
			this._isNeedClose = false;
		}

		public RunningTaskInfoForm(TreeTaskProgressManager progressManager, MsSqlAuditorModel model) : this()
		{
			this._model = model;

			ProgressManager = progressManager;
			InitializeComponent();

			progressManager.Changed += ProgressManagerOnChanged;
			progressManager.EverythingIsDone += ProgressManagerOnEverythingIsDone;
		}

		public void Show(IWin32Window owner, Point pt)
		{
			StartPosition = FormStartPosition.Manual;

			Location = new Point(pt.X, pt.Y - Height - 5);

			// Show();
			Show(owner);
		}

		private void ComposeText()
		{
			var sb = new StringBuilder();

			this._errorText = null;

			if(ProgressManager.IsCancelRequested)
			{
				this._errorText = _model.LocaleManager.GetLocalizedText("common", "operationCancellationRequested");
			}

			string[] waiting = ProgressManager.Waiting.Values.ToArray();
			string[] running = ProgressManager.Running.Values.ToArray();

			sb.AppendLine(string.Format(_model.LocaleManager.GetLocalizedText("common", "queriesRunning"),
				running.Length, running.Length + waiting.Length));

			sb.AppendLine("----------------------------------------------------------");

			foreach (var p in running)
			{
				sb.AppendLine("" + p);
			}

			if (waiting.Any())
			{
				sb.AppendLine("");
				sb.AppendLine(string.Format(_model.LocaleManager.GetLocalizedText("common", "queuedQueries"), waiting.Length));
				sb.AppendLine("----------------------------------------------------------");

				foreach (var p in waiting)
				{
					sb.AppendLine("" + p);
				}
			}

			try
			{
				this.SafeInvoke(() => MainText = sb.ToString());
			}
			catch (Exception ex)
			{
				log.Error(ex);
			}
		}

		private void RunningTaskInfoForm_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawRectangle(Pens.Black, new Rectangle(0, 0, Width - 1, Height - 1));

			var rect = this.ClientRectangle;
			rect.Inflate(-SystemFonts.DefaultFont.Height, -SystemFonts.DefaultFont.Height);

			if (!string.IsNullOrEmpty(_errorText))
			{
				e.Graphics.DrawString(_errorText, SystemFonts.DefaultFont, Brushes.DarkRed, rect);
				rect.Offset(0, SystemFonts.DefaultFont.Height);
				rect.Inflate(0, -SystemFonts.DefaultFont.Height);
			}

			e.Graphics.DrawString(MainText, SystemFonts.DefaultFont, SystemBrushes.WindowText, rect);
		}

		private void RunningTaskInfoForm_VisibleChanged(object sender, EventArgs e)
		{
			if (Visible)
			{
				ProgressManager.Changed += ProgressManagerOnChanged;
				ProgressManager.EverythingIsDone += ProgressManagerOnEverythingIsDone;
				ComposeText();
			}
			else
			{
				ProgressManager.Changed -= ProgressManagerOnChanged;
				ProgressManager.EverythingIsDone -= ProgressManagerOnEverythingIsDone;
			}
		}

		private void RunningTaskInfoForm_Deactivate(object sender, EventArgs e)
		{
			// Visible = false;
			Close();
		}

		private void ProgressManagerOnEverythingIsDone(object s, EventArgs e)
		{
			// TryInvoke(() => Visible = false);
			if (this._isLoaded)
			{
				if (this.IsHandleCreated && this.InvokeRequired)
				{
					this.BeginInvoke(new Action(Close));
					this._isNeedClose = true;
				}
				else
				{
					this._isNeedClose = true;
				}
			}
			else
			{
				this._isNeedClose = true;
			}
		}

		private void ProgressManagerOnChanged(object sender, EventArgs e)
		{
			ComposeText();
		}

		private void TryInvoke(Action action)
		{
			try
			{
				if (!IsDisposed && IsHandleCreated)
				{
					Invoke(action);
				}
			}
			catch (Exception ex)
			{
				log.Error(ex);
			}
		}

		private void RunningTaskInfoForm_Load(object sender, EventArgs e)
		{
			if (this._isNeedClose)
			{
				Close();
				return;
			}

			this._isLoaded = true;
		}
	}
}
