using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MSSQLServerAuditor.Gui.CustomControls
{
	public partial class LabelWithProgress : Label
	{
		private int   _mVal;
		private Color _mBarColor;
		private int   _mbarHeight;
		private bool  _showProgress;

		public LabelWithProgress()
		{
			this._mVal         = 0;
			this._mBarColor    = Color.Blue;
			this._mbarHeight   = 0;
			this._showProgress = false;

			InitializeComponent();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			int percent = this._mVal / 100;

			if (this._showProgress)
			{
				var g       = e.Graphics;
				var brush   = new SolidBrush(this.BarColor);
				var rect    = e.ClipRectangle;
				int height  = this._mbarHeight;

				rect.Width = rect.Width * percent;

				if (height == 0)
				{
					height = rect.Height;
				}

				rect.Y = (rect.Height - height) / 2;

				rect.Height = height;

				g.FillRectangle(brush, rect);
			}

			base.OnPaint(e);
		}

		[DefaultValue(0)]
		public int Value
		{
			get { return this._mVal; }

			set
			{
				if (value < 0)
				{
					this._mVal = value;
				}
				else if (value > 100)
				{
					this._mVal = 100;
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

		[DefaultValue(false)]
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
