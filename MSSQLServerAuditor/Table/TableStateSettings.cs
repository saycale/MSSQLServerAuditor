using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Table
{
	/// <summary>
	/// Settings of column of the table.
	/// </summary>
	public class TableColumnSettings
	{
		private string _id;
		private int    _width;
		private bool   _visible;

		public TableColumnSettings()
		{
			this._id      = null;
			this._width   = 150;
			this._visible = true;
		}

		/// <summary>
		/// ID column.
		/// </summary>
		[XmlAttribute]
		public string Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

		/// <summary>
		/// Width column.
		/// </summary>
		[XmlAttribute]
		public int Width
		{
			get { return this._width; }
			set { this._width = value; }
		}

		/// <summary>
		/// Visible column.
		/// </summary>
		[XmlAttribute]
		public bool Visible
		{
			get { return this._visible; }
			set { this._visible = value; }
		}
	}

	/// <summary>
	/// State settings table.
	/// </summary>
	[XmlRoot]
	public class TableStateSettings
	{
		private ListSortDirection         _sortDirection;
		private string                    _columnIdForSort;
		private List<TableColumnSettings> _columnSettings;

		public TableStateSettings()
		{
			this._sortDirection   = ListSortDirection.Ascending;
			this._columnIdForSort = null;
			this._columnSettings  = new List<TableColumnSettings>();
		}

		/// <summary>
		/// Sorting according a direction.
		/// </summary>
		[XmlElement]
		public ListSortDirection SortDirection
		{
			get { return this._sortDirection; }
			set { this._sortDirection = value; }
		}

		/// <summary>
		/// Column id for sorting.
		/// </summary>
		[XmlElement]
		public string ColumnIdForSort
		{
			get { return this._columnIdForSort; }
			set { this._columnIdForSort = value; }
		}

		/// <summary>
		/// Settings column in table.
		/// </summary>
		[XmlArray]
		public List<TableColumnSettings> ColumnSettings
		{
			get { return this._columnSettings; }
			set { this._columnSettings = value; }
		}

		/// <summary>
		/// Gets settings for column.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public TableColumnSettings GetColumnSettings(string id)
		{
			TableColumnSettings settings = (from c in this._columnSettings where c.Id == id select c).FirstOrDefault();

			if (settings == null)
			{
				settings = new TableColumnSettings();
				settings.Id = id;

				this._columnSettings.Add(settings);
			}

			return settings;
		}
	}
}
