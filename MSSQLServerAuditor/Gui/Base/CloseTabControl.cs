using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MSSQLServerAuditor.Gui.Base
{
	/// <summary>
	/// Fix Margin in tab controls
	/// </summary>
	public class FixedTabControl : TabControl
	{
		public const int TCM_FIRST      = 0x1300;
		public const int TCM_ADJUSTRECT = TCM_FIRST + 40;

		/// <summary>
		/// Provision of a rectangle.
		/// </summary>
		public struct RECT
		{
			/// <summary>
			/// Provision.
			/// </summary>
			public int Left, Top, Right, Bottom;
		}

		/// <summary>
		/// Windows process of message.
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == TCM_ADJUSTRECT)
			{
				RECT rc = (RECT) m.GetLParam(typeof(RECT));

				rc.Left   -= 3;
				rc.Right  += 3;
				rc.Top    -= 2;
				rc.Bottom += 3;

				Marshal.StructureToPtr(rc, m.LParam, true);
			}

			base.WndProc(ref m);
		}
	}

	public class TabControlEx : FixedTabControl
	{
		private bool _showSingleTab;

		public TabControlEx()
		{
			this.ShowSingleTab = true;
		}

		public bool ShowSingleTab
		{
			get
			{
				return this._showSingleTab;
			}

			set
			{
				this._showSingleTab = value;
				this.UpdateStyles();
			}
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			TabPage page = TabPages[e.Index];

			e.Graphics.FillRectangle(new SolidBrush(page.BackColor), e.Bounds);

			Rectangle paddedBounds = e.Bounds;

			paddedBounds.Inflate(-2, -2);
			TextRenderer.DrawText(e.Graphics, page.Text, Font, paddedBounds, page.ForeColor);
		}

		// hides tab panel when only one tab is available
		protected override void WndProc(ref Message m)
		{
			// Hide tabs by trapping the TCM_ADJUSTRECT message
			if (m.Msg == TCM_ADJUSTRECT && !DesignMode)
			{
				if (!ShowSingleTab && this.TabCount <= 1)
				{
					m.Result = (IntPtr)1;

					return;
				}

			}

			base.WndProc(ref m);
		}
	}

	/// <summary>
	/// Tab control with closable tabs
	/// </summary>
	internal class CloseableTabControl : FixedTabControl
	{
		private readonly List<CloseButton> _btnCloseList;
		private bool                       _recreate;
		private Rectangle                  _btnClose;
		private bool                       _showSingleTab;
		private bool                       _isClosableOn;

		private class CloseButton : IEquatable<CloseButton>
		{
			public Rectangle Bounds { get; set; }
			public string Text { get; set; }

			public bool Equals(CloseButton other)
			{
				return Bounds == other.Bounds && Text == other.Text;
			}
		}

		public CloseableTabControl()
		{
			this._recreate      = true;
			this._btnClose      = Rectangle.Empty;
			this.ShowSingleTab  = true;
			this.ClosableOn     = true;
			this.DrawMode       = TabDrawMode.OwnerDrawFixed;
			this._btnCloseList  = new List<CloseButton>();
			this.ControlAdded  += CloseableTabControl_ControlAdded;
		}

		public bool ShowSingleTab
		{
			get { return this._showSingleTab; }
			set
			{
				this._showSingleTab = value;

				this.UpdateStyles();
			}
		}

		public bool ClosableOn
		{
			get { return this._isClosableOn; }
			set
			{
				this._isClosableOn = value;

				this.UpdateStyles();
			}
		}

		protected override void WndProc(ref Message m)
		{
			// Hide tabs by trapping the TCM_ADJUSTRECT message
			if (m.Msg == TCM_ADJUSTRECT && !DesignMode && !ShowSingleTab && this.TabCount <= 1)
			{
				m.Result = (IntPtr) 1;
			}
			else
			{
				base.WndProc(ref m);
			}
		}

		private void CloseableTabControl_ControlAdded(object sender, ControlEventArgs e)
		{
			this._btnCloseList.Clear();
			this._recreate = true;
		}

		public void CloseTab(TabPage tp, bool askConfirmation = false)
		{
			int index = TabPages.IndexOf(tp);

			if (index < 0)
			{
				return;
			}

			if (!askConfirmation ||
				!Program.Model.Settings.ShowCloseTabConfirmation ||
				MessageBox.Show(
					Program.Model.LocaleManager.GetLocalizedText(GetType().Name, "shouldClose"),
					Program.Model.LocaleManager.GetLocalizedText(GetType().Name, "tabClose"),
					MessageBoxButtons.YesNo, MessageBoxIcon.Question
				) == DialogResult.Yes
			)
			{
				TabPages.Remove(tp);

				SelectedIndex = index != 0 ? index - 1 : 0;

				this._btnCloseList.Clear();
				this._recreate = true;
			}
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);

			if (!ClosableOn)
			{
				return;
			}

			foreach (CloseButton closeButton in this._btnCloseList)
			{
				if (closeButton.Bounds.Contains(e.X, e.Y))
				{
					TabPage tp = FindByText(closeButton.Text);

					CloseTab(tp, true);
				}
			}
		}

		private TabPage FindByText(string text)
		{
			return TabPages.Cast<TabPage>().FirstOrDefault(tp => tp.Text == text);
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			TabPage currentPage = this.TabPages[e.Index];
			int width = e.Graphics.MeasureString(currentPage.Text, this.Font).ToSize().Width + 8;

			if (e.Bounds.Width < width)
			{
				e = new DrawItemEventArgs(
					e.Graphics,
					Font,
					new Rectangle(
						e.Bounds.X,
						e.Bounds.Y,
						width,
						e.Bounds.Height
					),
					e.Index,
					e.State
				);
			}

			base.OnDrawItem(e);

			Rectangle paddedBounds = e.Bounds;
			paddedBounds.Inflate(-2, -2);

			e.Graphics.DrawString(currentPage.Text, this.Font, SystemBrushes.WindowText, paddedBounds);

			if (!ClosableOn)
			{
				return;
			}

			if (e.Bounds.Width < 50)
			{
				return;
			}

			this._btnClose = new Rectangle(e.Bounds.Right - 15, e.Bounds.Top + 4, 12, 12);

			CloseButton btn = new CloseButton
			{
				Bounds = this._btnClose,
				Text   = currentPage.Text
			};

			if (!this._btnCloseList.Contains(btn) && (this._btnCloseList.Count != TabPages.Count) && this._recreate)
			{
				this._btnCloseList.Add(btn);
			}

			Pen p = new Pen(Brushes.Brown, 2);

			e.Graphics.DrawLine(
				p,
				this._btnClose.Left   + 2,
				this._btnClose.Top    + 2,
				this._btnClose.Right  - 2,
				this._btnClose.Bottom - 2
			);

			e.Graphics.DrawLine(
				p,
				this._btnClose.Left   + 2,
				this._btnClose.Bottom - 2,
				this._btnClose.Right  - 2,
				this._btnClose.Top    + 2
			);
		}
	}
}
