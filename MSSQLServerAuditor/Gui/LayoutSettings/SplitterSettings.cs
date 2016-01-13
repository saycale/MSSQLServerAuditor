using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Gui.LayoutSettings
{
	public class SplitterPosSettings
	{
		private readonly string _id;
		private          int    _x;
		private          int    _y;

		public SplitterPosSettings()
		{
			this._id = null;
			this._x  = 0;
			this._y  = 0;
		}

		public SplitterPosSettings(string id, int x = 0, int y = 0) : this()
		{
			this._id = id;
			this.X   = x;
			this.Y   = y;
		}

		[XmlAttribute]
		public string Id
		{
			get { return this._id; }
		}

		[XmlAttribute]
		public int X
		{
			get { return this._x; }
			set { this._x = value; }
		}

		[XmlAttribute]
		public int Y
		{
			get { return this._y; }
			set { this._y = value; }
		}
	}
}
