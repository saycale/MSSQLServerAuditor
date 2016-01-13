namespace MSSQLServerAuditor.Gui
{
	partial class frmUserSettingsNode
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserSettingsNode));
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.panel3 = new System.Windows.Forms.Panel();
			this.picIconNode = new System.Windows.Forms.PictureBox();
			this.cmbNodeIcon = new System.Windows.Forms.ComboBox();
			this.chkDeactivateNode = new System.Windows.Forms.CheckBox();
			this.lblNodeName = new System.Windows.Forms.Label();
			this.lblNodeIcon = new System.Windows.Forms.Label();
			this.lblNodeColor = new System.Windows.Forms.Label();
			this.txtNodeName = new System.Windows.Forms.TextBox();
			this.txtNodeColorName = new System.Windows.Forms.TextBox();
			this.panelColor = new System.Windows.Forms.Panel();
			this.btnSelectFontColor = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btnDefaultName = new System.Windows.Forms.Button();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picIconNode)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			//
			// panel3
			//
			this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel3.Controls.Add(this.picIconNode);
			this.panel3.Location = new System.Drawing.Point(13, 107);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(21, 21);
			this.panel3.TabIndex = 15;
			//
			// picIconNode
			//
			this.picIconNode.Dock = System.Windows.Forms.DockStyle.Fill;
			this.picIconNode.Location = new System.Drawing.Point(0, 0);
			this.picIconNode.Name = "picIconNode";
			this.picIconNode.Size = new System.Drawing.Size(19, 19);
			this.picIconNode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picIconNode.TabIndex = 12;
			this.picIconNode.TabStop = false;
			//
			// cmbNodeIcon
			//
			this.cmbNodeIcon.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cmbNodeIcon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbNodeIcon.FormattingEnabled = true;
			this.cmbNodeIcon.Location = new System.Drawing.Point(40, 107);
			this.cmbNodeIcon.Name = "cmbNodeIcon";
			this.cmbNodeIcon.Size = new System.Drawing.Size(228, 21);
			this.cmbNodeIcon.TabIndex = 18;
			this.cmbNodeIcon.SelectedIndexChanged += new System.EventHandler(this.cmbNodeIcon_SelectedIndexChanged);
			//
			// chkDeactivateNode
			//
			this.chkDeactivateNode.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.chkDeactivateNode, 3);
			this.chkDeactivateNode.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chkDeactivateNode.Location = new System.Drawing.Point(274, 107);
			this.chkDeactivateNode.Name = "chkDeactivateNode";
			this.chkDeactivateNode.Size = new System.Drawing.Size(148, 21);
			this.chkDeactivateNode.TabIndex = 14;
			this.chkDeactivateNode.Text = "Deactivity node";
			this.chkDeactivateNode.UseVisualStyleBackColor = true;
			//
			// lblNodeName
			//
			this.lblNodeName.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.lblNodeName, 4);
			this.lblNodeName.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lblNodeName.Location = new System.Drawing.Point(13, 9);
			this.lblNodeName.Name = "lblNodeName";
			this.lblNodeName.Size = new System.Drawing.Size(369, 13);
			this.lblNodeName.TabIndex = 1;
			this.lblNodeName.Text = "Node name";
			//
			// lblNodeIcon
			//
			this.lblNodeIcon.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.lblNodeIcon, 4);
			this.lblNodeIcon.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lblNodeIcon.Location = new System.Drawing.Point(13, 91);
			this.lblNodeIcon.Name = "lblNodeIcon";
			this.lblNodeIcon.Size = new System.Drawing.Size(369, 13);
			this.lblNodeIcon.TabIndex = 13;
			this.lblNodeIcon.Text = "Icon node";
			//
			// lblNodeColor
			//
			this.lblNodeColor.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.lblNodeColor, 4);
			this.lblNodeColor.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lblNodeColor.Location = new System.Drawing.Point(13, 51);
			this.lblNodeColor.Name = "lblNodeColor";
			this.lblNodeColor.Size = new System.Drawing.Size(369, 13);
			this.lblNodeColor.TabIndex = 17;
			this.lblNodeColor.Text = "Font color";
			//
			// txtNodeName
			//
			this.tableLayoutPanel1.SetColumnSpan(this.txtNodeName, 4);
			this.txtNodeName.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.txtNodeName.Location = new System.Drawing.Point(13, 28);
			this.txtNodeName.Name = "txtNodeName";
			this.txtNodeName.Size = new System.Drawing.Size(369, 20);
			this.txtNodeName.TabIndex = 0;
			//
			// txtNodeColorName
			//
			this.txtNodeColorName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtNodeColorName.Enabled = false;
			this.txtNodeColorName.Location = new System.Drawing.Point(40, 67);
			this.txtNodeColorName.Name = "txtNodeColorName";
			this.txtNodeColorName.Size = new System.Drawing.Size(228, 20);
			this.txtNodeColorName.TabIndex = 16;
			this.txtNodeColorName.Text = "Black";
			//
			// panelColor
			//
			this.panelColor.BackColor = System.Drawing.SystemColors.ControlText;
			this.panelColor.Location = new System.Drawing.Point(13, 67);
			this.panelColor.Name = "panelColor";
			this.panelColor.Size = new System.Drawing.Size(21, 21);
			this.panelColor.TabIndex = 15;
			//
			// btnSelectFontColor
			//
			this.tableLayoutPanel1.SetColumnSpan(this.btnSelectFontColor, 3);
			this.btnSelectFontColor.Location = new System.Drawing.Point(274, 67);
			this.btnSelectFontColor.Name = "btnSelectFontColor";
			this.btnSelectFontColor.Size = new System.Drawing.Size(26, 21);
			this.btnSelectFontColor.TabIndex = 10;
			this.btnSelectFontColor.Text = "...";
			this.btnSelectFontColor.UseVisualStyleBackColor = true;
			this.btnSelectFontColor.Click += new System.EventHandler(this.btnSelectFontColor_Click);
			//
			// btnCancel
			//
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnCancel.Location = new System.Drawing.Point(334, 177);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(94, 26);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			//
			// btnOk
			//
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnOk.Location = new System.Drawing.Point(234, 177);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(94, 26);
			this.btnOk.TabIndex = 0;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			//
			// tableLayoutPanel1
			//
			this.tableLayoutPanel1.ColumnCount = 6;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.Controls.Add(this.chkDeactivateNode, 3, 6);
			this.tableLayoutPanel1.Controls.Add(this.cmbNodeIcon, 2, 6);
			this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 6);
			this.tableLayoutPanel1.Controls.Add(this.lblNodeName, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblNodeIcon, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.btnSelectFontColor, 3, 4);
			this.tableLayoutPanel1.Controls.Add(this.txtNodeColorName, 2, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblNodeColor, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.panelColor, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.txtNodeName, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnDefaultName, 5, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 8;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(425, 168);
			this.tableLayoutPanel1.TabIndex = 6;
			//
			// btnDefaultName
			//
			this.btnDefaultName.Image = global::MSSQLServerAuditor.Properties.Resources.arrow_redo;
			this.btnDefaultName.Location = new System.Drawing.Point(388, 25);
			this.btnDefaultName.Name = "btnDefaultName";
			this.btnDefaultName.Size = new System.Drawing.Size(25, 23);
			this.btnDefaultName.TabIndex = 19;
			this.btnDefaultName.UseVisualStyleBackColor = true;
			this.btnDefaultName.Click += new System.EventHandler(this.btnDefaultName_Click);
			//
			// tableLayoutPanel2
			//
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel2.Controls.Add(this.btnCancel, 2, 1);
			this.tableLayoutPanel2.Controls.Add(this.btnOk, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(431, 206);
			this.tableLayoutPanel2.TabIndex = 7;
			//
			// panel1
			//
			this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.tableLayoutPanel2.SetColumnSpan(this.panel1, 3);
			this.panel1.Controls.Add(this.tableLayoutPanel1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(425, 168);
			this.panel1.TabIndex = 0;
			//
			// frmUserSettingsNode
			//
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(431, 206);
			this.Controls.Add(this.tableLayoutPanel2);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(400, 230);
			this.Name = "frmUserSettingsNode";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "frmUserSettingsNode";
			this.Load += new System.EventHandler(this.frmUserSettingsNode_Load);
			this.panel3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picIconNode)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblNodeName;
		private System.Windows.Forms.TextBox txtNodeName;
		private System.Windows.Forms.CheckBox chkDeactivateNode;
		private System.Windows.Forms.Label lblNodeIcon;
		private System.Windows.Forms.Button btnSelectFontColor;
		private System.Windows.Forms.ColorDialog colorDialog;
		private System.Windows.Forms.ComboBox cmbNodeIcon;
		private System.Windows.Forms.Label lblNodeColor;
		private System.Windows.Forms.TextBox txtNodeColorName;
		private System.Windows.Forms.Panel panelColor;
		private System.Windows.Forms.PictureBox picIconNode;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnDefaultName;
	}
}
