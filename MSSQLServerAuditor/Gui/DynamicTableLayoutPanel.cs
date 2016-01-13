using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Gui
{
	public partial class DynamicTableLayoutPanel : TableLayoutPanel
	{
		private const int SplitterWidth = 6;
		private const int SplitterHeight = 6;
		private int       _splitterWidth;
		private int       _splitterHeight;
		private int[]     _columnWidths;
		private int[]     _rowHeights;
		private int[]     _columnAccWidths;
		private int[]     _rowAccHeights;
		private bool      _changingCapture;
		private bool      _drawGrid;

		public DynamicTableLayoutPanel()
		{
			this._currentSplitter            = EmptyPoint;
			this._currentHSplitterRect       = EmptyRect;
			this._lastHSplitterRect          = EmptyRect;
			this._currentVSplitterRect       = EmptyRect;
			this._lastVSplitterRect          = EmptyRect;
			this._currentSplitterMinLocation = EmptyPoint;
			this._currentSplitterMaxLimits   = Size.Empty;
			this._savedClip                  = EmptyRect;
			this._mouse                      = EmptyPoint;
			this._splitterWidth              = 6;
			this._splitterHeight             = 6;
			this._changingCapture            = false;
			this._drawGrid                   = false;

			InitializeComponent();
		}

		#region Public API

		/// <summary>
		/// Starts control configuring suspending layout changes
		/// </summary>
		public void StartConfiguring()
		{
			SuspendLayout();

			//
			// #248 - fix memory leaks during XML files processing
			//
			// Controls.Clear();
			while (Controls.Count > 0)
			{
				Controls[0].Dispose();
			}
		}

		/// <summary>
		/// Sets columns configuration
		/// </summary>
		/// <param name="widths">Column widths in percents</param>
		public void SetColumns(float[] widths)
		{
			ColumnCount = widths.Length;
			while (ColumnStyles.Count < ColumnCount) ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3f));
			while (ColumnStyles.Count > ColumnCount) ColumnStyles.RemoveAt(ColumnStyles.Count - 1);

			for (int i = 0, iMax = widths.Length; i < iMax; i++)
			{
				var columnStyle = ColumnStyles[i];
				columnStyle.SizeType = SizeType.Percent;
				columnStyle.Width = widths[i];
			}
		}

		/// <summary>
		/// Returns columns configuration
		/// </summary>
		/// <returns>Column widths in percents</returns>
		public float[] GetColumns()
		{
			return ColumnStyles.Cast<ColumnStyle>().Take(ColumnCount).Select(cs => cs.Width).ToArray();
		}

		/// <summary>
		/// Sets rows configuration
		/// </summary>
		/// <param name="heights">Row heights in percents</param>
		public void SetRows(float[] heights)
		{
			RowCount = heights.Length;
			while (RowStyles.Count < RowCount) RowStyles.Add(new RowStyle(SizeType.Percent, 33.3f));
			while (RowStyles.Count > RowCount) RowStyles.RemoveAt(RowStyles.Count - 1);

			for (int i = 0, iMax = heights.Length; i < iMax; i++)
			{
				var rowStyle = RowStyles[i];
				rowStyle.SizeType = SizeType.Percent;
				rowStyle.Height = heights[i];
			}
		}

		/// <summary>
		/// Returns rows configuration
		/// </summary>
		/// <returns>Row heights in percents</returns>
		public float[] GetRows()
		{
			return RowStyles.Cast<RowStyle>().Take(RowCount).Select(rs => rs.Height).ToArray();
		}

		/// <summary>
		/// Adds <paramref name="ctrl"/> to this table layout panel in cell <paramref name="column"/>:<paramref name="row"/> with the size of <paramref name="colSpan"/>:<paramref name="rowSpan"/>
		/// </summary>
		/// <param name="column">X coordinate of the cell, starts from 0</param>
		/// <param name="row">Y coordinate of the cell, starts from 0</param>
		/// <param name="colSpan">Width of control in cells</param>
		/// <param name="rowSpan">Height of control in cells</param>
		/// <param name="ctrl">Control to be added</param>
		public void AddControlToCell(int column, int row, int colSpan, int rowSpan, Control ctrl)
		{
			if (column < 0 || column >= ColumnCount)
			{
				throw new ArgumentOutOfRangeException("column");
			}

			if (row < 0 || row >= RowCount)
			{
				throw new ArgumentOutOfRangeException("row");
			}

			if (column + colSpan > ColumnCount)
			{
				throw new ArgumentOutOfRangeException("colSpan");
			}

			if (row + rowSpan > RowCount)
			{
				throw new ArgumentOutOfRangeException("rowSpan");
			}

			if (ctrl == null)
			{
				throw new ArgumentNullException("ctrl");
			}

			ctrl.Dock = DockStyle.Fill;

			ctrl.Margin =
				new Padding(
					column != 0 ? _splitterWidth / 2 : 0,
					row != 0 ? _splitterHeight / 2 : 0,
					column + colSpan < ColumnCount ? _splitterWidth - _splitterWidth / 2 : 0,
					row + rowSpan < RowCount ? _splitterHeight - _splitterHeight / 2 : 0);

			Controls.Add(ctrl);
			SetColumn(ctrl, column);
			SetRow(ctrl, row);

			if (colSpan > 1)
			{
				SetColumnSpan(ctrl, colSpan);
			}

			if (rowSpan > 1)
			{
				SetRowSpan(ctrl, rowSpan);
			}
		}

		/// <summary>
		/// Resumes control configuring performing all layout changes collected
		/// </summary>
		public void StopConfiguring()
		{
			ResumeLayout(true);
		}

		#endregion

		#region Event Handlers
#if DEBUG
		protected void ToggleGrid()
		{
			this._drawGrid = !this._drawGrid;

			Invalidate();
			CancelSplitter();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (this._drawGrid)
			{
				foreach (var x in this._columnAccWidths)
				{
					System.Windows.Forms.ControlPaint.DrawReversibleLine(PointToScreen(new Point(x, 0)), PointToScreen(new Point(x, Height)), Color.Green);
				}

				foreach (var y in this._rowAccHeights)
				{
					System.Windows.Forms.ControlPaint.DrawReversibleLine(PointToScreen(new Point(0, y)), PointToScreen(new Point(Width, y)), Color.DarkRed);
				}
			}
		}
#endif

		protected override void OnMouseDown(MouseEventArgs e)
		{
			SetMouseLocation(e.X, e.Y);
			InitSplitter();

			if (this._currentSplitter != EmptyPoint)
			{
				Trace.WriteLine("Window: " + ClientRectangle.ToString() + ", " + DisplayRectangle.ToString());
				Trace.WriteLine("Mouse Down: " + e.Button + ", " + e.X + ":" + e.Y + ", " + e.Location.ToString() + ", " + this._currentSplitter.ToString());

				SetMouseCaptureAndLimits();
				SetKeyboardFilter();
				CalcSplitterRect();
				DrawSplitterRect(SplitBarDrawMode.Start);
			}

			base.OnMouseDown(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.None || this._currentSplitter == EmptyPoint)
			{
				if (e.Button == MouseButtons.None && this._currentSplitter == EmptyPoint)
				{
					SetMouseLocation(e.X, e.Y);
					UpdateMouseCursor();
				}
			}
			else
			{
				Trace.WriteLine("Mouse Move: " + e.Button + ", " + e.X + ":" + e.Y + ", " + e.Location);

				SetMouseLocation(e.X, e.Y);
				CalcSplitterRect();
				DrawSplitterRect(SplitBarDrawMode.Move);
			}

			base.OnMouseMove(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			UpdateMouseCursor(true);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (this._currentSplitter != EmptyPoint)
			{
				Trace.WriteLine("Mouse Up  : " + e.Button + ", " + e.X + ":" + e.Y + ", " + e.Location);

				SetMouseLocation(e.X, e.Y);
				CalcSplitterRect();
				DrawSplitterRect(SplitBarDrawMode.End);
				ReleaseKeyboardFilter();
				ReleaseMouseCaptureAndLimits();
				UpdateTable();
				ResetSplitter();
			}

			base.OnMouseUp(e);
		}

		protected void CancelSplitter()
		{
			Trace.WriteLine("Spltr Cncl");

			DrawSplitterRect(SplitBarDrawMode.End);
			ReleaseKeyboardFilter();
			ReleaseMouseCaptureAndLimits();
			ResetSplitter();
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			base.OnLayout(levent);

			UpdateLayoutCache();
		}

		protected override void OnMouseCaptureChanged(EventArgs e)
		{
			Trace.WriteLine("Mouse Cptr: changed");

			base.OnMouseCaptureChanged(e);

			if (!this._changingCapture)
			{
				CancelSplitter();
			}
		}

		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			base.ScaleControl(factor, specified);

			_splitterHeight = (int)Math.Round(factor.Height * SplitterHeight);
			_splitterWidth = (int)Math.Round(factor.Height * SplitterWidth);
		}

		#endregion

		#region Splitter logic
		private static readonly Point EmptyPoint = new Point(int.MinValue, int.MinValue);
		private static readonly Rectangle EmptyRect = new Rectangle(int.MinValue, int.MinValue, int.MinValue, int.MinValue);
		private bool _currentSplitterHaveBeenDrawn;
		private Point _currentSplitter;
		private Rectangle _currentHSplitterRect;
		private Rectangle _lastHSplitterRect;
		private Rectangle _currentVSplitterRect;
		private Rectangle _lastVSplitterRect;
		private Point _currentSplitterMinLocation;
		private Size _currentSplitterMaxLimits;

		void InitSplitter()
		{
			ResetSplitter();

			var sX = GetSplitterIdx(_mouse.X, _splitterWidth, this._columnAccWidths);
			var sY = GetSplitterIdx(_mouse.Y, _splitterHeight, this._rowAccHeights);
			if (sX < 0 && sY < 0) return;

			this._currentSplitter = new Point(sX, sY);

			this._currentSplitterMinLocation =
				new Point(
					_splitterWidth + (this._currentSplitter.X <= 0 ? 0 : this._columnAccWidths[this._currentSplitter.X - 1]),
					_splitterHeight + (this._currentSplitter.Y <= 0 ? 0 : this._rowAccHeights[this._currentSplitter.Y - 1]));

			this._currentSplitterMaxLimits =
				new Size(
					(this._currentSplitter.X < 0
						? this._columnAccWidths[this._columnAccWidths.Length - 1]
						: this._currentSplitter.X == this._columnAccWidths.Length - 1
							? this._columnAccWidths[this._currentSplitter.X]
							: this._columnAccWidths[this._currentSplitter.X + 1]) - this._currentSplitterMinLocation.X - _splitterWidth,
					(this._currentSplitter.Y < 0
						? this._rowAccHeights[this._rowAccHeights.Length - 1]
						: this._currentSplitter.Y == this._rowAccHeights.Length - 1
							? this._rowAccHeights[this._currentSplitter.Y]
							: this._rowAccHeights[this._currentSplitter.Y + 1]) - this._currentSplitterMinLocation.Y - _splitterHeight);

			Trace.WriteLine("Mouse Splt: " + this._currentSplitter + ", " + this._currentSplitterMinLocation + ", " + this._currentSplitterMaxLimits);
		}

		void CalcSplitterRect()
		{
			if (this._currentSplitter.X >= 0)
			{
				this._currentHSplitterRect = new Rectangle(_mouse.X - _splitterWidth / 2, 0, _splitterWidth, Height);
			}

			if (this._currentSplitter.Y >= 0)
			{
				this._currentVSplitterRect = new Rectangle(0, _mouse.Y - _splitterHeight / 2, Width, _splitterHeight);
			}
		}

		enum SplitBarDrawMode
		{
			Start, Move, End
		}

		/// <summary>
		/// Draws the splitter bar at the current location. Will automatically cleanup anyplace the splitter was drawn previously.
		/// </summary>
		void DrawSplitterRect(SplitBarDrawMode mode)
		{
			Trace.WriteLine("Split Draw: " + _currentHSplitterRect + ", " + _lastHSplitterRect + ", " + _currentVSplitterRect + ", " + _lastVSplitterRect);

			if (mode != SplitBarDrawMode.Start && this._currentSplitterHaveBeenDrawn)
			{
				DrawSplitterRectIntl(_lastHSplitterRect, _lastVSplitterRect);
				this._currentSplitterHaveBeenDrawn = false;
			}
			else if (mode != SplitBarDrawMode.Start && !this._currentSplitterHaveBeenDrawn)
			{
				return;
			}

			if (mode != SplitBarDrawMode.End)
			{
				DrawSplitterRectIntl(_currentHSplitterRect, _currentVSplitterRect);

				this._lastHSplitterRect            = _currentHSplitterRect;
				this._lastVSplitterRect            = _currentVSplitterRect;
				this._currentSplitterHaveBeenDrawn = true;
			}
			else
			{
				if (this._currentSplitterHaveBeenDrawn)
				{
					DrawSplitterRectIntl(this._lastHSplitterRect, this._lastVSplitterRect);
				}

				this._currentSplitterHaveBeenDrawn = false;
			}
		}

		/// <summary>
		/// Draws the splitter line at the requested location. Should only be called by drawSpltBar.
		/// </summary>
		void DrawSplitterRectIntl(Rectangle hSplitterRect, Rectangle vSplitterRect)
		{
			Trace.WriteLine("Split Draw: internal, " + hSplitterRect + ", " + vSplitterRect);

			//*
			// First variant (the same as in the SplitterContainer)
			var parentHandle = Handle;

			var dc = IntPtr.Zero;
			var halftone = IntPtr.Zero;

			try
			{
				dc = UnsafeNativeMethods.GetDCEx(new HandleRef(this, parentHandle), NativeMethods.NullHandleRef, NativeMethods.DCX_CACHE | NativeMethods.DCX_LOCKWINDOWUPDATE);
				halftone = Utils.ControlPaint.CreateHalftoneHBRUSH();
				var saveBrush = SafeNativeMethods.SelectObject(new HandleRef(this, dc), new HandleRef(null, halftone));
				if (hSplitterRect == EmptyRect || vSplitterRect == EmptyRect)
				{
					if (hSplitterRect != EmptyRect) SafeNativeMethods.PatBlt(new HandleRef(this, dc), hSplitterRect.X, hSplitterRect.Y, hSplitterRect.Width, hSplitterRect.Height, NativeMethods.PATINVERT);
					if (vSplitterRect != EmptyRect) SafeNativeMethods.PatBlt(new HandleRef(this, dc), vSplitterRect.X, vSplitterRect.Y, vSplitterRect.Width, vSplitterRect.Height, NativeMethods.PATINVERT);
				}
				else
				{
					SafeNativeMethods.PatBlt(new HandleRef(this, dc), hSplitterRect.X, hSplitterRect.Y, hSplitterRect.Width, vSplitterRect.Y, NativeMethods.PATINVERT);
					SafeNativeMethods.PatBlt(new HandleRef(this, dc), hSplitterRect.X, vSplitterRect.Y + vSplitterRect.Height, hSplitterRect.Width, hSplitterRect.Height, NativeMethods.PATINVERT);
					SafeNativeMethods.PatBlt(new HandleRef(this, dc), vSplitterRect.X, vSplitterRect.Y, vSplitterRect.Width, vSplitterRect.Height, NativeMethods.PATINVERT);
				}
				SafeNativeMethods.SelectObject(new HandleRef(this, dc), new HandleRef(null, saveBrush));
			}
			finally
			{
				if (halftone != IntPtr.Zero) SafeNativeMethods.DeleteObject(new HandleRef(null, halftone));
				if (dc != IntPtr.Zero) UnsafeNativeMethods.ReleaseDC(new HandleRef(this, parentHandle), new HandleRef(null, dc));
			}
			/*/
			// Second variant (simpler)
			System.Windows.Forms.ControlPaint.FillReversibleRectangle(RectangleToScreen(splitterRect), SystemColors.ControlDark);
			//*/
		}

		/// <summary>
		/// Event occures after layout after splitter changed position
		/// </summary>
		public event EventHandler SplitterMoved;

		/// <summary>
		/// Fires SplitterMoved event
		/// </summary>
		protected virtual void OnSplitterMoved()
		{
			var handler = SplitterMoved;
			if (handler != null) handler(this, EventArgs.Empty);
		}

		void UpdateTable()
		{
			var changed = false;

			SuspendLayout();

			if (this._currentSplitter.X >= 0)
			{
				var sX = GetSplitterIdx(_mouse.X, _splitterWidth, this._columnAccWidths);
				if (sX != this._currentSplitter.X)
				{
					ColumnStyle left = ColumnStyles[this._currentSplitter.X], right = ColumnStyles[this._currentSplitter.X + 1];

					var newLeftWidth = (left.Width + right.Width) * (_mouse.X - (this._currentSplitter.X == 0 ? 0 : this._columnAccWidths[this._currentSplitter.X - 1]))
						/
						(_columnWidths[_currentSplitter.X] + _columnWidths[_currentSplitter.X + 1]);

					var newRightWidth = left.Width + right.Width - newLeftWidth;

					left.Width = newLeftWidth;
					right.Width = newRightWidth;

					changed = true;
				}
			}

			if (this._currentSplitter.Y >= 0)
			{
				var sY = GetSplitterIdx(_mouse.Y, _splitterHeight, this._rowAccHeights);

				if (sY != this._currentSplitter.Y)
				{
					RowStyle up = RowStyles[this._currentSplitter.Y], down = RowStyles[this._currentSplitter.Y + 1];

					var newLeftHeight = (up.Height + down.Height) * (_mouse.Y - (this._currentSplitter.Y == 0 ? 0 : this._rowAccHeights[this._currentSplitter.Y - 1])) / (_rowHeights[this._currentSplitter.Y] + _rowHeights[this._currentSplitter.Y + 1]);
					var newRightHeight = up.Height + down.Height - newLeftHeight;

					up.Height = newLeftHeight;
					down.Height = newRightHeight;

					changed = true;
				}
			}

			ResumeLayout(changed);

			if (changed)
			{
				OnSplitterMoved();
			}
		}

		void ResetSplitter()
		{
			this._currentSplitterHaveBeenDrawn = false;
			this._currentSplitter              = EmptyPoint;
			this._currentHSplitterRect         = EmptyRect;
			this._lastHSplitterRect            = EmptyRect;
			this._currentVSplitterRect         = EmptyRect;
			this._lastVSplitterRect            = EmptyRect;
			this._currentSplitterMinLocation   = EmptyPoint;
			this._currentSplitterMaxLimits     = Size.Empty;
		}
		#endregion

		#region Mouse Capturing
		private Rectangle _savedClip;
		private Point _mouse;

		void SetMouseLocation(int x, int y)
		{
			_mouse = new Point(x + 1, y + 1);
		}

		void SetMouseCaptureAndLimits()
		{
			this._changingCapture = true;
			Capture = true;
			_savedClip = Cursor.Clip;
			Cursor.Clip = RectangleToScreen(new Rectangle(this._currentSplitterMinLocation, this._currentSplitterMaxLimits));
			Trace.WriteLine("Mouse Clip: set, " + Cursor.Clip.ToString());
			this._changingCapture = false;
		}

		void ReleaseMouseCaptureAndLimits()
		{
			this._changingCapture = true;
			Capture = false;
			Cursor.Clip = _savedClip;
			Trace.WriteLine("Mouse Clip: released, " + Cursor.Clip.ToString());
			this._changingCapture = false;
		}

		private void UpdateMouseCursor(bool forceDefault = false)
		{
			var newCursor = Cursors.Default;

			if (!forceDefault)
			{
				var sX = GetSplitterIdx(_mouse.X, _splitterWidth, this._columnAccWidths);
				var sY = GetSplitterIdx(_mouse.Y, _splitterHeight, this._rowAccHeights);

				if (sX >= 0 && sY >= 0)
				{
					newCursor = Cursors.SizeAll;
				}
				else if (sY >= 0)
				{
					newCursor = Cursors.HSplit;
				}
				else if (sX >= 0)
				{
					newCursor = Cursors.VSplit;
				}
			}

			if (Cursor != newCursor)
			{
				Trace.WriteLine("Mouse Curs: Set Cursor");
				Cursor = newCursor;
			}
		}
		#endregion

		#region Keyboard Handling
		private static CodeAccessPermission unmanagedCode;

		public static CodeAccessPermission UnmanagedCode
		{
			get { return unmanagedCode ?? (unmanagedCode = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode)); }
		}

		private MessageFilter _filter;

		void SetKeyboardFilter()
		{
			Trace.WriteLine("Keybd Fltr: set");

			UnmanagedCode.Assert();
			try
			{
				if (_filter == null)
				{
					_filter = new MessageFilter(this);
				}
				Application.AddMessageFilter(_filter);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		void ReleaseKeyboardFilter()
		{
			Trace.WriteLine("Keybd Fltr: released");

			if (_filter != null)
			{
				Application.RemoveMessageFilter(_filter);
				_filter = null;
			}
		}

		class MessageFilter : IMessageFilter
		{
			private readonly DynamicTableLayoutPanel _owner;

			public MessageFilter(DynamicTableLayoutPanel owner)
			{
				this._owner = owner;
			}

#if DEBUG
			private readonly Keys[] _konamiCode = { Keys.Up, Keys.Up, Keys.Down, Keys.Down, Keys.Left, Keys.Right, Keys.Left, Keys.Right, Keys.B, Keys.A };
			private int _konamiCodePosition = 0;
#endif

			[SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			bool IMessageFilter.PreFilterMessage(ref Message m)
			{
				if (m.Msg >= NativeMethods.WM_KEYFIRST && m.Msg <= NativeMethods.WM_KEYLAST)
				{
					if ((m.Msg == NativeMethods.WM_KEYDOWN && (int)m.WParam == (int)Keys.Escape) || (m.Msg == NativeMethods.WM_SYSKEYDOWN))
					{
						this._owner.CancelSplitter();
						return true;
					}

#if DEBUG
					if (m.Msg == NativeMethods.WM_KEYDOWN)
					{
						if (this._konamiCodePosition >= 0 && this._konamiCodePosition < _konamiCode.Length)
						{
							if ((int)m.WParam == (int)_konamiCode[this._konamiCodePosition])
							{
								this._konamiCodePosition++;

								if (this._konamiCodePosition >= _konamiCode.Length)
								{
									this._owner.ToggleGrid();
								}
								return true;
							}
							else
							{
								this._konamiCodePosition = 0;
							}
						}
						else
						{
							this._konamiCodePosition = 0;
						}
					}
#endif
				}
				return false;
			}
		}
		#endregion

		#region Layout Cache
		void UpdateLayoutCache()
		{
			this._columnAccWidths = CalcAccSizes(_columnWidths = GetColumnWidths());
			this._rowAccHeights   = CalcAccSizes(_rowHeights   = GetRowHeights());
		}

		static int[] CalcAccSizes(int[] accSizes)
		{
			var acc = 0;
			var res = new int[accSizes.Length];

			for (int i = 0, iMax = accSizes.Length; i < iMax; i++)
			{
				res[i] = (acc += accSizes[i]);
			}

			return res;
		}

		static int GetSplitterIdx(int coordinate, int splitterSize, int[] accSizes)
		{
			int c1 = coordinate - splitterSize / 2, c2 = c1 + splitterSize;

			for (int i = 0, iMax = accSizes.Length - 1; (i < iMax) && (accSizes[i] < c2); i++)
			{
				if (c1 <= accSizes[i]) return i;
			}

			return -1;
		}
		#endregion
	}
}
