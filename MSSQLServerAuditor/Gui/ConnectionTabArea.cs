using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.Model.Settings;
using MSSQLServerAuditor.Preprocessor;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Gui
{
	public partial class ConnectionTabArea : UserControl
	{
		private PreprocessorAreaData _area;
		private ConnectionTabControl _parent;
		private MsSqlAuditorModel    _model;
		private string               _templateId;
		private string               _nodeId;
		private string               _areaId;
		private bool                 _handlingSplitterMoved;
		private bool                 _splitterChanged;
		private string               _columns;
		private string               _rows;
		private string               _reportLanguage;
		private SplitterSettings     _splitterSettings;

		public ConnectionTabArea()
		{
			InitializeComponent();
		}

		internal void Init(ConnectionTabControl parent, PreprocessorAreaData area, ConcreteTemplateNodeDefinition definition, MsSqlAuditorModel model)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}

			if (area == null)
			{
				throw new ArgumentNullException("area");
			}

			if (definition == null)
			{
				throw new ArgumentNullException("definition");
			}

			if (model == null)
			{
				throw new ArgumentNullException("model");
			}

			if (!area.IsConfigured)
			{
				throw new ArgumentOutOfRangeException("area");
			}

			this._area   = area;
			this._parent = parent;
			this._model  = model;

			table.StartConfiguring();

			var rows    = this._area.Rows;
			var columns = this._area.Columns;

			// _templateId = !String.IsNullOrWhiteSpace(definition.Connection.TemplateId) ? definition.Connection.TemplateId : definition.Connection.TemplateFileName;
			if (!String.IsNullOrWhiteSpace(definition.Connection.TemplateId))
			{
				this._templateId = definition.Connection.TemplateId;
			}
			else
			{
				this._templateId = string.IsNullOrEmpty(definition.Connection.TemplateDir)
					? definition.Connection.TemplateFileName
					: Path.Combine(definition.Connection.TemplateDir, definition.Connection.TemplateFileName);
			}

			this._nodeId           = !String.IsNullOrWhiteSpace(definition.TemplateNode.Id) ? definition.TemplateNode.Id : definition.TemplateNode.Name;
			this._areaId           = !String.IsNullOrWhiteSpace(this._area.Id) ? this._area.Id : this._area.Name;
			this._reportLanguage   = this._model.Settings.ReportLanguage;
			this._splitterSettings = this._model.LayoutSettings.GetExtendedSettings<SplitterSettings>(this._templateId, String.Empty, this._reportLanguage);

			if (this._splitterSettings == null)
			{
				this._splitterSettings = new SplitterSettings
				{
					SplitterNodeSettingList = new List<SplitterSetting>()
				};
			}

			var splitterSetting =
				this._splitterSettings.SplitterNodeSettingList.FirstOrDefault(
					s =>
						string.Equals(s.NodeId, this._nodeId, StringComparison.InvariantCultureIgnoreCase)
						&&
						string.Equals(s.AreaId, this._areaId, StringComparison.InvariantCultureIgnoreCase)
				);

			if (splitterSetting != null)
			{
				var newRows    = PreprocessorAreaData.ParseGridDimension(splitterSetting.Rows,    "splitterSetting.Rows");
				var newColumns = PreprocessorAreaData.ParseGridDimension(splitterSetting.Columns, "splitterSetting.Columns");

				if (rows.Length == newRows.Length && columns.Length == newColumns.Length)
				{
					rows    = newRows;
					columns = newColumns;
				}
			}

			table.SetRows(rows);
			table.SetColumns(columns);

			List<PreprocessorData> preprocessors = _area.Preprocessors;

			for (int i = 0, iMax = preprocessors.Count; i < iMax; i++)
			{
				PreprocessorData preprocessor = preprocessors[i];
				Control control = preprocessor.ContentFactory.CreateControl();

				if (control != null)
				{
					if (control is WebBrowser)
					{
						// control.PreviewKeyDown += (s, e) =>
						control.PreviewKeyDown += ConnectionTabArea_PreviewKeyDown;

						this.Disposed += (s, e) =>
						{
							//
							// #248 - fix memory leaks during Web rendering
							//
							// if (e.KeyCode == Keys.F5)
							// {
							//     _parent.F5RefreshView();
							// }

							control.PreviewKeyDown -= ConnectionTabArea_PreviewKeyDown;
							IntPtr currentProcessHandle = (IntPtr)(-1);
							SafeNativeMethods.EmptyWorkingSet(currentProcessHandle);
						};
					}

					#region Wrap control to frame with title
					if (preprocessor.VerticalTextAlign != null)
					{
						var titleFrame = new TitleFrame(
							preprocessor.VerticalTextAlign,
							preprocessor.TextAlign,
							control
						);

						titleFrame.Title = preprocessor.Name;

						control = titleFrame;
					}
					#endregion

					table.AddControlToCell(
						preprocessor.Column - 1,
						preprocessor.Row - 1,
						preprocessor.ColSpan,
						preprocessor.RowSpan,
						control
					);
				}
			}

			table.StopConfiguring();

			this._splitterChanged = false;

			if (!this._handlingSplitterMoved)
			{
				this._handlingSplitterMoved = true;

				table.SplitterMoved += TableSplitterMoved;
			}
		}

		private void ConnectionTabArea_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.F5)
			{
				this._parent.F5RefreshView();
			}
		}

		/// <summary>
		/// Event of changing settings
		/// </summary>
		public event EventHandler SettingsChanged;

		protected virtual void OnSettingsChanged()
		{
			var handler = SettingsChanged;

			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		private void TableSplitterMoved(object sender, EventArgs e)
		{
			this._splitterChanged = true;

			var tbl = ((DynamicTableLayoutPanel)sender);

			this._columns = PreprocessorAreaData.FormatGridDimension(tbl.GetColumns());
			this._rows    = PreprocessorAreaData.FormatGridDimension(tbl.GetRows());

			UpdateSettings();

			OnSettingsChanged();
		}

		/// <summary>
		/// Updates settings if changed
		/// </summary>
		/// <returns>true, if settings have been changed</returns>
		public bool UpdateSettings()
		{
			if (!this._splitterChanged)
			{
				return false;
			}

			this._splitterChanged = false;

			var splitterSetting =
				this._splitterSettings.SplitterNodeSettingList.FirstOrDefault(
					s =>
						string.Equals(s.NodeId, _nodeId, StringComparison.InvariantCultureIgnoreCase) &&
						string.Equals(s.AreaId, _areaId, StringComparison.InvariantCultureIgnoreCase)
				);

			if (splitterSetting != null)
			{
				splitterSetting.Columns = this._columns;
				splitterSetting.Rows    = this._rows;
			}
			else
			{
				this._splitterSettings.SplitterNodeSettingList.Add(
					new SplitterSetting()
					{
						NodeId  = this._nodeId,
						AreaId  = this._areaId,
						Columns = this._columns,
						Rows    = this._rows
					}
				);
			}

			this._model.LayoutSettings.SetExtendedSettings(
				this._templateId,
				String.Empty,
				this._reportLanguage,
				this._splitterSettings
			);

			return true;
		}
	}
}
