using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace MSSQLServerAuditor.Gui.CustomControls
{
	[System.ComponentModel.DesignerCategory("code")]
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.StatusStrip)]
	public partial class ToolStripStatusLabelWithProgress : ToolStripStatusLabel
	{
		private decimal _mVal;
		private Color   _mBarColor;
		private int     _mbarHeight;
		private bool    _showProgress;

		public ToolStripStatusLabelWithProgress()
		{
			this._mVal         = 0.0M;
			this._mBarColor    = Color.Blue;
			this._mbarHeight   = 0;
			this._showProgress = true;

			InitializeComponent();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			decimal percent = this._mVal / 100.0M;

			if (this._showProgress)
			{
				var g      = e.Graphics;
				var brush  = new SolidBrush(BarColor);
				var rect   = e.ClipRectangle;
				int height = this._mbarHeight;

				rect.Width = (int) (rect.Width * percent);

				if (height == 0)
				{
					height = rect.Height;
				}

				rect.Y = (int)((decimal)(rect.Height - height) / 2.0M);

				rect.Height = height;

				g.FillRectangle(brush, rect);

				this.BackColor = Color.Transparent;
			}

			base.OnPaint(e);
		}

		[DefaultValue(0)]
		public decimal Value
		{
			get { return this._mVal; }

			set
			{
				if (value < 0.0M)
				{
					this._mVal = value;
				}
				else if (value > 100.0M)
				{
					this._mVal = 100.0M;
				}
				else
				{
					this._mVal = value;
				}

				this.Invalidate();
			}
		}

		[DefaultValue(typeof(Color), "Blue")]
		public Color BarColor
		{
			get { return this._mBarColor; }

			set
			{
				this._mBarColor = value;
				this.Invalidate();
			}
		}

		[DefaultValue(0)]
		public int BarHeight
		{
			get { return this._mbarHeight; }

			set
			{
				if (value < 0 || value > this.Size.Height)
				{
					this._mVal = this.Size.Height;
				}
				else
				{
					this._mVal = value;
				}

				this.Invalidate();
			}
		}

		[DefaultValue(true)]
		public bool ShowProgerss
		{
			get { return this._showProgress; }

			set
			{
				this._showProgress = value;

				this.Invalidate();
			}
		}
	}
}
