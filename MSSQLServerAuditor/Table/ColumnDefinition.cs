using System.Data;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Table
{
    using System.Drawing;
    using System.Windows.Forms;

    using MSSQLServerAuditor.Table.Settings;

    /// <summary>
    /// Definition for the table
    /// </summary>
    public class ColumnDefinition
    {
        private string _columnName;
        private SqlDbType _type;
        private string _format;
        private string _tag;

        /// <summary>
        /// Text name of the column
        /// </summary>
        [XmlAttribute]
        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        /// <summary>
        /// Data type for column
        /// </summary>
        [XmlAttribute]
        public SqlDbType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// Text format
        /// </summary>
        [XmlAttribute]
        public string Format
        {
            get { return _format; }
            set { _format = value; }
        }

        /// <summary>
        /// Tag objects.
        /// </summary>
        [XmlAttribute]
        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        /// Gets or sets the align.
        /// </summary>
        [XmlAttribute]
        public ColumnAlign Align { get; set; }

        /// <summary>
        /// Gets or sets the align.
        /// </summary>
        [XmlAttribute]
        public ColumnAlign HeaderAlign { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether header bold.
        /// </summary>
        [XmlAttribute]
        public bool HeaderBold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether header italic.
        /// </summary>
        [XmlAttribute]
        public bool HeaderItalic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether header size.
        /// </summary>
        [XmlAttribute]
        public string HeaderFontSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether bold.
        /// </summary>
        [XmlAttribute]
        public bool Bold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether italic.
        /// </summary>
        [XmlAttribute]
        public bool Italic { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        [XmlAttribute]
        public string FontSize { get; set; }

        public FontStyle GetHeaderFontStyle()
        {
            return this.HeaderItalic
                       ? (this.HeaderBold ? FontStyle.Bold : FontStyle.Regular) | FontStyle.Italic
                       : (this.HeaderBold ? FontStyle.Bold : FontStyle.Regular) | FontStyle.Regular;
        }

        public FontStyle GetFontStyle()
        {
            return this.Italic
                       ? (this.Bold ? FontStyle.Bold : FontStyle.Regular) | FontStyle.Italic
                       : (this.Bold ? FontStyle.Bold : FontStyle.Regular) | FontStyle.Regular;
        }
    }
}