using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using MSSQLServerAuditor.Preprocessor;
using ContentAlignment = System.Drawing.ContentAlignment;

namespace MSSQLServerAuditor.Gui
{
	/// <summary>
	/// Title frame for control
	/// </summary>
	public partial class TitleFrame : UserControl
	{
		#region internal members
		private VerticalTextAlign? _verticalTextAlign;
		private TextAlign          _textAlign;
		private readonly Label     _titleLabel;
		private readonly Control   _targetControl;
		#endregion

		#region Init
		public TitleFrame()
		{
			this._verticalTextAlign = null;
			this._textAlign         = TextAlign.Right;
			this._titleLabel        = new Label();
			this._targetControl     = null;
		}

		/// <summary>
		/// Title frame for control
		/// </summary>
		/// <param name="verticalTextAlign"></param>
		/// <param name="textAlign"></param>
		/// <param name="targetControl">Target control</param>
		public TitleFrame(VerticalTextAlign? verticalTextAlign, TextAlign textAlign, Control targetControl) : this()
		{
			this._titleLabel.AutoSize = true;
			this._titleLabel.Font     = new Font(this._titleLabel.Font, FontStyle.Bold | FontStyle.Underline);
			this._verticalTextAlign   = verticalTextAlign;
			this._textAlign           = textAlign;
			this._targetControl       = targetControl;

			InitializeComponent();
		}

		private void TitleFrame_Load(object sender, EventArgs e)
		{
			tlpBase.Controls.Add(this._titleLabel);
			tlpBase.SetRow(this._titleLabel, 0);

			if (this._targetControl != null)
			{
				tlpBase.Controls.Add(this._targetControl);
				tlpBase.SetRow(this._targetControl, 1);
				tlpBase.SetColumn(this._targetControl, 0);
				tlpBase.SetColumnSpan(this._targetControl, 3);

				this._targetControl.Dock = DockStyle.Fill;
			}

			SetAlignTitle(this._verticalTextAlign, this._textAlign);
		}
		#endregion

		#region Public properties
		/// <summary>
		/// Title.
		/// </summary>
		public string Title
		{
			set
			{
				if (this._titleLabel == null)
				{
					return;
				}

				this._titleLabel.Text = value ?? String.Empty;
			}
		}

		/// <summary>
		/// Vertical align of title. If null - dont show title.
		/// </summary>
		public VerticalTextAlign? VerticalTextAlign
		{
			set
			{
				if(this._verticalTextAlign == value)
				{
					return;
				}

				this._verticalTextAlign = value;
				SetAlignTitle(this._verticalTextAlign, this._textAlign);
			}

			get { return this._verticalTextAlign; }
		}

		/// <summary>
		/// Horizontal align of title.
		/// </summary>
		public TextAlign TextAlign
		{
			set
			{
				if (this._textAlign == value)
				{
					return;
				}

				this._textAlign = value;
				SetAlignTitle(this._verticalTextAlign, this._textAlign);
			}

			get { return this._textAlign; }
		}
		#endregion

		#region Service
		/// <summary>
		/// Set new place of title
		/// </summary>
		/// <param name="verticalTextAlign">Vertical align of title. If null - dont show title.</param>
		/// <param name="textAlign">Horizontal align of title.</param>
		private void SetAlignTitle(VerticalTextAlign? verticalTextAlign, TextAlign textAlign)
		{
			if (this._titleLabel == null || !tlpBase.Contains(this._titleLabel))
			{
				return;
			}

			if (verticalTextAlign == null)
			{
				tlpBase.RowStyles[0].SizeType = SizeType.Percent;
				tlpBase.RowStyles[0].Height = 0.0f;
				tlpBase.RowStyles[2].SizeType = SizeType.Percent;
				tlpBase.RowStyles[2].Height = 0.0f;
				return;
			}

			tlpBase.RowStyles[0].SizeType = SizeType.AutoSize;
			tlpBase.RowStyles[2].SizeType = SizeType.AutoSize;

			if (verticalTextAlign == Preprocessor.VerticalTextAlign.Top)
			{
				tlpBase.SetRow(this._titleLabel, 0);
			}
			else if (verticalTextAlign == Preprocessor.VerticalTextAlign.Bottom)
			{
				tlpBase.SetRow(this._titleLabel, 2);
			}

			if (textAlign == TextAlign.Left)
			{
				tlpBase.SetColumn(this._titleLabel, 0);
				this._titleLabel.TextAlign = ContentAlignment.MiddleLeft;
			}
			else if (textAlign == TextAlign.Center)
			{
				tlpBase.SetColumn(this._titleLabel, 1);
				this._titleLabel.TextAlign = ContentAlignment.MiddleCenter;
			}
			else if (textAlign == TextAlign.Right)
			{
				tlpBase.SetColumn(this._titleLabel, 2);
				this._titleLabel.TextAlign = ContentAlignment.MiddleRight;
			}
		}
		#endregion
	}
}
