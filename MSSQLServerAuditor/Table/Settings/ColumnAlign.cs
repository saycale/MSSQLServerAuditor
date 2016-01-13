// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnAlign.cs" company="">
//
// </copyright>
// <summary>
//   The column align.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MSSQLServerAuditor.Table.Settings
{
    using System;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    /// <summary>
    /// The column align.
    /// </summary>
    public enum ColumnAlign
    {
        /// <summary>
        /// The left.
        /// </summary>
        [XmlEnum("Left")]
        Left,

        /// <summary>
        /// The center.
        /// </summary>
        [XmlEnum("Center")]
        Center,

        /// <summary>
        /// The right.
        /// </summary>
        [XmlEnum("Right")]
        Right
    }

    /// <summary>
    /// The column align extensions.
    /// </summary>
    public static class ColumnAlignExtensions
    {
        /// <summary>
        /// The get column align.
        /// </summary>
        /// <param name="align">The align.</param>
        /// <returns>
        /// The <see cref="DataGridViewContentAlignment" />.
        /// </returns>
        public static DataGridViewContentAlignment GetColumnAlign(this ColumnAlign align)
        {
            switch (align)
            {
                case ColumnAlign.Left:
                    return DataGridViewContentAlignment.MiddleLeft;
                case ColumnAlign.Center:
                    return DataGridViewContentAlignment.MiddleCenter;
                case ColumnAlign.Right:
                    return DataGridViewContentAlignment.BottomRight;
                default:
                    throw new ArgumentOutOfRangeException("align");
            }
        }
    }
}