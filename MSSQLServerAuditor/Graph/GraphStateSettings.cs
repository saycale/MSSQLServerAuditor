using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Graph
{
	/// <summary>
	/// Class for graph user settings
	/// </summary>
	public class GraphSeriesSettings
	{
		private string _id;
		private bool   _visible;

		public GraphSeriesSettings()
		{
			this._id      = null;
			this._visible = true;
		}

		/// <summary>
		/// Column Id
		/// </summary>
		[XmlAttribute]
		public string Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

		/// <summary>
		/// Column visible
		/// </summary>
		[XmlAttribute]
		public bool Visible
		{
			get { return this._visible; }
			set { this._visible = value; }
		}
	}

	/// <summary>
	/// Settings for Graph user state
	/// </summary>
	[XmlRoot]
	public class GraphStateSettings
	{
		private List<GraphSeriesSettings> _seriesSettings;
		private bool                      _legendEnabled;

		public GraphStateSettings()
		{
			this._seriesSettings = new List<GraphSeriesSettings>();
			this._legendEnabled  = true;
		}

		/// <summary>
		/// Settings for columns
		/// </summary>
		[XmlArray]
		public List<GraphSeriesSettings> ColumnSettings
		{
			get { return this._seriesSettings; }
			set { this._seriesSettings = value; }
		}

		/// <summary>
		/// Show Legend in Graph
		/// </summary>
		[XmlAttribute]
		public bool LegendEnabled
		{
			get { return this._legendEnabled; }
			set { this._legendEnabled = value; }
		}

		/// <summary>
		/// Get serries settings.
		/// </summary>
		/// <param name="id">ID</param>
		/// <returns>Settings.</returns>
		public GraphSeriesSettings GetSeriesSettings(string id)
		{
			var settings = (from c in this._seriesSettings where c.Id == id select c).FirstOrDefault();

			if (settings == null)
			{
				settings = new GraphSeriesSettings();
				settings.Id = id;

				this._seriesSettings.Add(settings);
			}

			return settings;
		}
	}
}
