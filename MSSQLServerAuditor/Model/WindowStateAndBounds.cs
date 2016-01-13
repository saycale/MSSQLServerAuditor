using System.Drawing;
using System.Windows.Forms;

namespace MSSQLServerAuditor.Model
{
    /// <summary>
    /// Represents a window state (i.e. maximized, minimized) and desktop bounds of window
    /// </summary>
    public struct WindowStateAndBounds
    {
        /// <summary>
        /// Window location (left top corner)
        /// </summary>
        public Point Location { get; set; }
        /// <summary>
        /// Window size
        /// </summary>
        public Size Size { get; set; }
        /// <summary>
        /// Window state (i.e. maximized, minimized)
        /// </summary>
        public FormWindowState WindowState { get; set; }
        /// <summary>
        /// Value with new state and same bounds
        /// </summary>
        public WindowStateAndBounds NewWindowState(FormWindowState newValue)
        {
            return new WindowStateAndBounds { Location = this.Location, Size = this.Size, WindowState = newValue};
        }
        /// <summary>
        /// Value with new bounds and same state
        /// </summary>
        public WindowStateAndBounds NewBounds(Rectangle bounds)
        {
            return new WindowStateAndBounds { Location = bounds.Location, Size = bounds.Size, WindowState = this.WindowState };
        }

        /// <summary>
        /// New state.
        /// </summary>
        /// <param name="state">From window state.</param>
        /// <returns>Return window state and bound.</returns>
        public WindowStateAndBounds NewState(FormWindowState state)
        {
            return new WindowStateAndBounds() {Location = this.Location, Size = this.Size, WindowState = state};
        }

        /// <summary>
        /// Rectengular bounds with location and size specified with corresponding properties
        /// </summary>
        public Rectangle GetBounds()
        {
            return new Rectangle(Location, Size);
        }
        /// <summary>
        /// Is value empty (contains no information)
        /// </summary>
        public bool IsEmpty()
        {
            return Size.IsEmpty;
        }
    }
}