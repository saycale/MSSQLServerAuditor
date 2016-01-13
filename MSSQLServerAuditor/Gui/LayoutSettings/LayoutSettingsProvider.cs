using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MSSQLServerAuditor.Gui.LayoutSettings
{
	class LayoutSettingsProvider<TLayoutSettings> : ILayoutSettingsProvider<TLayoutSettings> where TLayoutSettings : class, IFormLayoutSettings, new()
	{
		private TLayoutSettings                   _settings;
		private readonly string                   _preprocessor;
		private readonly string                   _frmId;
		private Form                              _form;
		private IDictionary<DataGridView, string> _prefixes;

		public LayoutSettingsProvider()
		{
			this._settings     = null;
			this._preprocessor = null;
			this._frmId        = null;
			this._form         = null;
			this._prefixes     = new Dictionary<DataGridView, string>();
		}

		public LayoutSettingsProvider(Form form, string frmId = null, string preprocessor = null) : this()
		{
			if (form == null)
			{
				throw new ArgumentNullException("form");
			}

			this._form         = form;
			this._frmId        = frmId        ?? String.Empty;
			this._preprocessor = preprocessor ?? form.Name;
		}

		public TLayoutSettings Settings
		{
			get { return this._settings; }
		}

		public void AttachToChangeLayout()
		{
			if (this._form != null)
			{
				this._form.Resize += FormResize;
				this._form.Move   += FormMove;
			}

			var dgws = GetControls<DataGridView>(this._form);

			if (dgws != null)
			{
				foreach (var dgw in dgws)
				{
					dgw.ColumnWidthChanged += DgwOnColumnWidthChanged;
				}
			}

			var slitters = GetControls<SplitContainer>(this._form);

			if (slitters != null)
			{
				foreach (var slitter in slitters)
				{
					slitter.SplitterMoved += OnSplitterMoved;
				}
			}
		}

		public void DettachToChangeLayout()
		{
			if (this._form != null)
			{
				this._form.Resize -= FormResize;
				this._form.Move   -= FormMove;
			}

			var dgws = GetControls<DataGridView>(this._form);

			if (dgws != null)
			{
				foreach (var dgw in dgws)
				{
					dgw.ColumnWidthChanged -= DgwOnColumnWidthChanged;
				}
			}

			var slitters = GetControls<SplitContainer>(this._form);

			if (slitters != null)
			{
				foreach (var slitter in slitters)
				{
					slitter.SplitterMoved -= OnSplitterMoved;
				}
			}
		}

		public void SetPrefix(DataGridView dataGrid, string prefix)
		{
			if (this._prefixes != null)
			{
				if (this._prefixes.ContainsKey(dataGrid))
				{
					this._prefixes[dataGrid] = prefix;
				}
				else
				{
					this._prefixes.Add(dataGrid, prefix);
				}
			}
		}

		/// <summary>
		/// Load GUI settings from XML
		/// </summary>
		/// <param name="defValue"></param>
		/// <returns>true - settings loaded, false - no</returns>
		public bool LoadSettings(TLayoutSettings defValue = null)
		{
			bool result = false;

			this._settings = Program.Model.LayoutSettings.GetExtendedSettings<TLayoutSettings>(
				this._frmId,
				this._preprocessor,
				Program.Model.Settings.InterfaceLanguage
			);

			result = this._settings != null;

			if (this._settings == null && defValue != null)
			{
				this._settings = defValue;
			}

			if (this._settings != null)
			{
				return result;
			}

			return result;
		}

		/// <summary>
		/// Apply settings from object _settings to GUI
		/// </summary>
		public void ApplySettings()
		{
			if (this._settings != null)
			{
				if (this._form != null)
				{
					this._form.Location = this._settings.Location;
					this._form.Size     = this._settings.Size;
				}

				if (this._settings.ColumnSettings != null && this._settings.ColumnSettings.Count != 0)
				{
					var dgws = GetControls<DataGridView>(this._form);

					if (dgws != null)
					{
						foreach (var dgw in dgws)
						{
							ApplyColumnSettings(dgw);
						}
					}
				}
			}

			var slitters = GetControls<SplitContainer>(this._form);

			if (slitters != null)
			{
				foreach (var slitter in slitters)
				{
					ApplySplitterSettings(slitter);
				}
			}
		}

		private void ApplyColumnSettings(DataGridView dgw)
		{
			var prefix = dgw.Name;

			if (this._prefixes != null)
			{
				if (this._prefixes.ContainsKey(dgw))
				{
					prefix = this._prefixes[dgw];
				}
			}

			foreach (DataGridViewColumn column in dgw.Columns)
			{
				string id = null;

				if (column != null)
				{
					id = String.Format("{0}{1}",
						prefix      ?? String.Empty,
						column.Name ?? String.Empty
					);

					if (this._settings != null)
					{
						if (this._settings.ColumnSettings.All(el => el.Id != id))
						{
							continue;
						}

						column.Width   = this._settings.GetColumnSettings(id).Width;
						column.Visible = this._settings.GetColumnSettings(id).Visible;
					}
				}
			}
		}

		private void ApplySplitterSettings(SplitContainer splitter)
		{
			string              id               = null;
			var                 splitterDistance = 0;
			SplitterPosSettings splitterSettings = null;

			if (splitter != null)
			{
				id = splitter.Name;

				if (this._settings != null)
				{
					splitterSettings = this._settings.GetSplitterSettings(id);
				}

				if (splitterSettings != null)
				{
					if (splitter.Orientation == Orientation.Vertical)
					{
						splitterDistance = splitterSettings.X;
					}
					else
					{
						splitterDistance = splitterSettings.Y;
					}

					if (splitterDistance != 0)
					{
						splitter.SplitterDistance = splitterDistance;
					}
					else
					{
						if (splitter.Orientation == Orientation.Vertical)
						{
							splitterSettings.X = splitter.SplitterDistance;
						}
						else
						{
							splitterSettings.Y = splitter.SplitterDistance;
						}

						if (this._settings != null)
						{
							this._settings.SetSplitterSettings(id, splitterSettings.X, splitterSettings.Y);
						}
					}
				}
			}
		}

		/// <summary>
		/// Update _settings object by real GUI settings
		/// </summary>
		public void UpdateSettings()
		{
			if (this._settings == null)
			{
				this._settings = new TLayoutSettings();
			}

			if (this._form != null && this._settings != null)
			{
				this._settings.Size     = this._form.Size;
				this._settings.Location = this._form.Location;
			}

			var dgws = GetControls<DataGridView>(this._form);

			if (dgws != null)
			{
				foreach (var dgw in dgws)
				{
					UpdateColumnSettings(dgw);
				}
			}

			var slitters = GetControls<SplitContainer>(this._form);

			if (slitters != null)
			{
				foreach (var slitter in slitters)
				{
					UpdateSpliterSettings(slitter);
				}
			}
		}

		private void UpdateColumnSettings(DataGridView dgw)
		{
			string id     = null;
			var    prefix = dgw.Name;

			if (this._prefixes != null)
			{
				if (this._prefixes.ContainsKey(dgw))
				{
					prefix = this._prefixes[dgw];
				}
			}

			foreach (DataGridViewColumn column in dgw.Columns)
			{
				if (column != null)
				{
					id = String.Format("{0}{1}",
						prefix      ?? String.Empty,
						column.Name ?? String.Empty
					);

					if (this._settings != null)
					{
						this._settings.SetColumnSettings(
							id,
							column.Visible,
							column.Width
						);
					}
				}
			}
		}

		private void UpdateSpliterSettings(SplitContainer splitter)
		{
			string id = null;
			int    x  = 0;
			int    y  = 0;

			if (splitter != null)
			{
				id = splitter.Name;

				if (splitter.Orientation == Orientation.Vertical)
				{
					x = splitter.SplitterDistance;
				}
				else
				{
					y = splitter.SplitterDistance;
				}

				if (this._settings != null)
				{
					this._settings.SetSplitterSettings(id, x, y);
				}
			}
		}

		/// <summary>
		/// Save GUI settings from XML
		/// </summary>
		public void SaveSettings()
		{
			if (this._settings == null)
			{
				this._settings = new TLayoutSettings();
			}

			Program.Model.LayoutSettings.SetExtendedSettings<TLayoutSettings>(
				this._frmId,
				this._preprocessor,
				Program.Model.Settings.InterfaceLanguage,
				this._settings
			);
		}

		#region Layout change event handlers

		private void DgwOnColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
		{
			string       prefix = null;
			DataGridView dgw    = sender as DataGridView;

			if (dgw != null)
			{
				prefix = dgw.Name;

				if (this._prefixes != null)
				{
					if (this._prefixes.ContainsKey(dgw))
					{
						prefix = this._prefixes[dgw];
					}
				}

				if (e != null)
				{
					SaveNewColumnChange(prefix, e.Column);
				}

				SaveSettings();
			}
		}

		void OnSplitterMoved(object sender, SplitterEventArgs e)
		{
			string         id      = null;
			SplitContainer slitter = sender as SplitContainer;

			if (slitter != null)
			{
				id = slitter.Name;

				if (e != null)
				{
					SaveNewSplitterPosition(id, e.SplitX, e.SplitY);
				}

				SaveSettings();
			}
		}

		private void FormMove(object sender, EventArgs e)
		{
			if (this._settings == null)
			{
				this._settings = new TLayoutSettings();
			}

			if (this._settings != null && this._form != null)
			{
				if (this._settings.Location.X != this._form.Location.X || this._settings.Location.Y != this._form.Location.Y)
				{
					this._settings.Location = this._form.Location;
					SaveSettings();
				}
			}
		}

		private void FormResize(object sender, EventArgs e)
		{
			if (this._settings == null)
			{
				this._settings = new TLayoutSettings();
			}

			if (this._settings != null && this._form != null)
			{
				if(this._settings.Size.Height != this._form.Size.Height || this._settings.Size.Width != this._form.Size.Width)
				{
					this._settings.Size = this._form.Size;
					SaveSettings();
				}
			}
		}

		#endregion

		private void SaveNewColumnChange(string prefix, DataGridViewColumn column)
		{
			string id = null;

			if (column != null)
			{
				id = String.Format("{0}{1}",
					prefix      ?? String.Empty,
					column.Name ?? String.Empty
				);

				if (this._settings == null)
				{
					this._settings = new TLayoutSettings();
				}

				if (this._settings != null)
				{
					this._settings.SetColumnSettings(id, column.Visible, column.Width);
				}
			}
		}

		private void SaveNewSplitterPosition(string id, int xPosition, int yPosition)
		{
			if (this._settings == null)
			{
				this._settings = new TLayoutSettings();
			}

			if (this._settings != null)
			{
				this._settings.SetSplitterSettings(id, xPosition, yPosition);
			}
		}

		private IEnumerable<T> GetControls<T>(Control control)
		{
			List<T> res = null;

			if (control != null)
			{
				res = new List<T>();

				res.AddRange(control.Controls.OfType<T>());

				foreach (Control subtControl in control.Controls)
				{
					var subres = GetControls<T>(subtControl);

					if (subres != null)
					{
						res.AddRange(subres);
					}
				}
			}

			return res;
		}

		public void Dispose()
		{
			IDisposable dispSettings = null;

			DettachToChangeLayout();

			this._form     = null;
			this._prefixes = null;

			if (this._settings != null)
			{
				dispSettings = this._settings as IDisposable;
			}

			if (dispSettings != null)
			{
				dispSettings.Dispose();
			}

			this._settings = null;
		}
	}
}
