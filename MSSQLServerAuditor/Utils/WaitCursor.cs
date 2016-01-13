using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MSSQLServerAuditor.Utils
{
    /// <summary>
    /// Wait cursor.
    /// </summary>
    public class WaitCursor : IDisposable
    {
        private readonly Cursor _wasCursor;
        private bool _disposed;

        /// <summary>
        /// Initializes the object WaitCursor.
        /// </summary>
        public WaitCursor()
        {
            _wasCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
            Cursor.Current = _wasCursor;
        }

        #endregion

        /// <summary>
        ///
        /// </summary>
        ~WaitCursor()
        {
            Debug.Assert(_disposed, this + ": Dispose has not been called");
        }
    }
}