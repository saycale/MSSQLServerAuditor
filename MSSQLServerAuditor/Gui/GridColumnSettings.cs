using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Gui
{
    /// <summary>
    /// Settings of column of the table.
    /// </summary>
    public class GridColumnSettings
    {
        private string _id;
        private int _width;
        private bool _visible;

        public GridColumnSettings()
        {
            this._id = string.Empty;
            this._width = 150;
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
}
