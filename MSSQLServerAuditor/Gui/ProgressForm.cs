using System;
using System.ComponentModel;
using System.Windows.Forms;
using MSSQLServerAuditor.Model.Internationalization;

namespace MSSQLServerAuditor.Gui
{
	/// <summary>
	/// Simple progress form.
	/// </summary>
	public partial class ProgressForm : LocalizableForm
	{
		private readonly BackgroundWorker _worker;
		private int                       _lastPercent;
		private string                    _lastStatus;

		/// <summary>
		/// Gets the progress bar so it is possible to customize it
		/// before displaying the form.
		/// </summary>
		public ProgressBar ProgressBar { get { return progressBar; } }

		/// <summary>
		/// Will be passed to the background worker.
		/// </summary>
		public object Argument { get; set; }

		/// <summary>
		/// Background worker's result.
		/// You may also check ShowDialog return value
		/// to know how the background worker finished.
		/// </summary>
		public RunWorkerCompletedEventArgs Result { get; private set; }

		/// <summary>
		/// True if the user clicked the Cancel button
		/// and the background worker is still running.
		/// </summary>
		public bool CancellationPending
		{
			get { return this._worker.CancellationPending; }
		}

		/// <summary>
		/// Text displayed once the Cancel button is clicked.
		/// </summary>
		public string CancellingText { get; set; }

		/// <summary>
		/// Default status text.
		/// </summary>
		public string DefaultStatusText { get; set; }

		/// <summary>
		/// Delegate for the DoWork event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">Contains the event data.</param>
		public delegate void DoWorkEventHandler(ProgressForm sender, DoWorkEventArgs e);

		/// <summary>
		/// Occurs when the background worker starts.
		/// </summary>
		public event DoWorkEventHandler DoWork;

		/// <summary>
		/// Constructor.
		/// </summary>
		public ProgressForm()
		{
			InitializeComponent();

			DefaultStatusText = GetLocalizedText("lbWait");
			CancellingText    = GetLocalizedText("lbCancellingOperation");;
			buttonCancel.Text = GetLocalizedText("btnCancel");

			this._worker = new BackgroundWorker
			{
				WorkerReportsProgress = true,
				WorkerSupportsCancellation = true
			};

			this._worker.DoWork             += new System.ComponentModel.DoWorkEventHandler(WorkerDoWork);
			this._worker.ProgressChanged    += new ProgressChangedEventHandler(WorkerProgressChanged);
			this._worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RunWorkerCompleted);
		}

		/// <summary>
		/// Changes the status text only.
		/// </summary>
		/// <param name="status">New status text.</param>
		public void SetProgress(string status)
		{
			if (status != _lastStatus && !this._worker.CancellationPending)
			{
				_lastStatus = status;

				this._worker.ReportProgress(
					progressBar.Minimum - 1,
					status
				);
			}
		}

		/// <summary>
		/// Changes the progress bar value only.
		/// </summary>
		/// <param name="percent">New value for the progress bar.</param>
		public void SetProgress(int percent)
		{
			//do not update the progress bar if the value didn't change
			if (percent != this._lastPercent)
			{
				this._lastPercent = percent;

				this._worker.ReportProgress(percent);
			}
		}

		/// <summary>
		/// Changes both progress bar value and status text.
		/// </summary>
		/// <param name="percent">New value for the progress bar.</param>
		/// <param name="status">New status text.</param>
		public void SetProgress(int percent, string status)
		{
			//update the form is at least one of the values need to be updated
			if (percent != this._lastPercent || (status != _lastStatus && !this._worker.CancellationPending))
			{
				this._lastPercent = percent;
				this._lastStatus  = status;

				this._worker.ReportProgress(
					percent,
					status
				);
			}
		}

		private void ProgressFormLoad(object sender, EventArgs e)
		{
			Result = null;

			buttonCancel.Enabled = true;

			progressBar.Value = progressBar.Minimum;
			labelStatus.Text  = DefaultStatusText;

			this._lastStatus       = DefaultStatusText;
			this._lastPercent      = progressBar.Minimum;

			this._worker.RunWorkerAsync(Argument);
		}

		private void ButtonCancelClick(object sender, EventArgs e)
		{
			this._worker.CancelAsync();

			buttonCancel.Enabled = false;
			labelStatus.Text = CancellingText;
		}

		private void WorkerDoWork(object sender, DoWorkEventArgs e)
		{
			if (DoWork != null)
			{
				DoWork(this, e);
			}
		}

		private void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (e.ProgressPercentage >= progressBar.Minimum && e.ProgressPercentage <= progressBar.Maximum)
			{
				progressBar.Value = e.ProgressPercentage;
			}

			if (e.UserState != null && !this._worker.CancellationPending)
			{
				labelStatus.Text = e.UserState.ToString();
			}
		}

		private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Result = e;

			if (e.Error != null)
			{
				DialogResult = DialogResult.Abort;
			}
			else if (e.Cancelled)
			{
				DialogResult = DialogResult.Cancel;
			}
			else
			{
				DialogResult = DialogResult.OK;
			}

			Close();
		}
	}
}
