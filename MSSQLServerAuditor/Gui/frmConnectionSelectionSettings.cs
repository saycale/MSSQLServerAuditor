using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;
using MSSQLServerAuditor.Gui.LayoutSettings;

namespace MSSQLServerAuditor.Gui
{
	/// <summary>
	/// State settings table.
	/// </summary>
	[XmlRoot]
	public class frmConnectionSelectionSettings
	{
		private List<GridColumnSettings>  _columnSettings;
		private List<SplitterPosSettings> _splitterSettings;
		private Size                      _size;
		private Point                     _location;

		public frmConnectionSelectionSettings()
		{
			this._columnSettings   = new List<GridColumnSettings>();
			this._splitterSettings = new List<SplitterPosSettings>();
			this._size             = new Size();
			this._location         = new Point();
		}

		/// <summary>
		/// Window location (left top corner)
		/// </summary>
		[XmlElement]
		public Point Location
		{
			get { return this._location; }
			set { this._location = value; }
		}

		/// <summary>
		/// Window size
		/// </summary>
		[XmlElement]
		public Size Size
		{
			get { return this._size; }
			set { this._size = value; }
		}

		/// <summary>
		/// Settings column in table.
		/// </summary>
		[XmlArray]
		public List<GridColumnSettings> ColumnSettings
		{
			get { return this._columnSettings; }
			set { this._columnSettings = value; }
		}

		/// <summary>
		/// Settings position splitters
		/// </summary>
		[XmlArray]
		public List<SplitterPosSettings> SplitterSettings
		{
			get { return this._splitterSettings; }
			set { this._splitterSettings = value; }
		}

		/// <summary>
		/// Gets settings for column.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public GridColumnSettings GetColumnSettings(string id)
		{
			var settings = (from c in this._columnSettings where c.Id == id select c).FirstOrDefault();

			if (settings == null)
			{
				settings    = new GridColumnSettings();
				settings.Id = id;

				this._columnSettings.Add(settings);
			}

			return settings;
		}

		public void SetColumnSettings(string id, bool visible, int width)
		{
			var columnSettings = ColumnSettings.FirstOrDefault(el => el.Id == id);

			if (columnSettings == null)
			{
				columnSettings = new GridColumnSettings()
				{
					Id      = id,
					Visible = visible,
					Width   = width
				};

				ColumnSettings.Add(columnSettings);
			}
			else
			{
				columnSettings.Visible = visible;
				columnSettings.Width   = width;
			}
		}

		/// <summary>
		/// Gets settings for splitter.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public SplitterPosSettings GetSplitterSettings(string id)
		{
			var settings = (from c in this._splitterSettings where c.Id == id select c).FirstOrDefault();

			if (settings == null)
			{
				settings = new SplitterPosSettings(id);
				this._splitterSettings.Add(settings);
			}

			return settings;
		}

		public void SetSplitterSettings(string id, int xPosition, int yPosition)
		{
			var splitterSetting = SplitterSettings.FirstOrDefault(el => el.Id == id);

			if (splitterSetting == null)
			{
				splitterSetting = new SplitterPosSettings(id, xPosition, yPosition);
				SplitterSettings.Add(splitterSetting);
			}
			else
			{
				splitterSetting.X = xPosition;
				splitterSetting.Y = yPosition;
			}
		}
	}
}
