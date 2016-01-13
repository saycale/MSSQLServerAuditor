using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Gui.LayoutSettings
{
	public interface IFormLayoutSettings
	{
		/// <summary>
		/// Window location (left top corner)
		/// </summary>
		[XmlElement]
		Point Location { get; set; }

		/// <summary>
		/// Window size
		/// </summary>
		[XmlElement]
		Size Size { get; set; }

		/// <summary>
		/// Settings column in table.
		/// </summary>
		[XmlArray]
		List<GridColumnSettings> ColumnSettings { get; set; }

		/// <summary>
		/// Gets settings for column.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		GridColumnSettings GetColumnSettings(string id);

		void SetColumnSettings(string id, bool visible, int width);

		/// <summary>
		/// Gets settings for splitter.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		SplitterPosSettings GetSplitterSettings(string id);

		void SetSplitterSettings(string id, int xPosition, int yPosition);
	}
}
