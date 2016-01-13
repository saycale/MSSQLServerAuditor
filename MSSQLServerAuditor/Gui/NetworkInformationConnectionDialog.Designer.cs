namespace MSSQLServerAuditor.Gui
{
	partial class NetworkInformationConnectionDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NetworkInformationConnectionDialog));
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblHost = new System.Windows.Forms.Label();
			this.txtHost = new System.Windows.Forms.TextBox();
			this.cmbProtocol = new System.Windows.Forms.ComboBox();
			this.lblProtocol = new System.Windows.Forms.Label();
			this.lblPort = new System.Windows.Forms.Label();
			this.nupPort = new System.Windows.Forms.NumericUpDown();
			this.nupTimeout = new System.Windows.Forms.NumericUpDown();
			this.lblTimeout = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.nupPort)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nupTimeout)).BeginInit();
			this.SuspendLayout();
			//
			// btnOk
			//
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(96, 195);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 6;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			//
			// btnCancel
			//
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(177, 195);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			//
			// lblHost
			//
			this.lblHost.AutoSize = true;
			this.lblHost.Location = new System.Drawing.Point(12, 9);
			this.lblHost.Name = "lblHost";
			this.lblHost.Size = new System.Drawing.Size(32, 13);
			this.lblHost.TabIndex = 8;
			this.lblHost.Text = "Host:";
			//
			// txtHost
			//
			this.txtHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtHost.Location = new System.Drawing.Point(12, 25);
			this.txtHost.Name = "txtHost";
			this.txtHost.Size = new System.Drawing.Size(240, 20);
			this.txtHost.TabIndex = 9;
			this.txtHost.TextChanged += new System.EventHandler(this.txtHost_TextChanged);
			//
			// cmbProtocol
			//
			this.cmbProtocol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbProtocol.FormattingEnabled = true;
			this.cmbProtocol.Location = new System.Drawing.Point(12, 68);
			this.cmbProtocol.Name = "cmbProtocol";
			this.cmbProtocol.Size = new System.Drawing.Size(240, 21);
			this.cmbProtocol.TabIndex = 11;
			//
			// lblProtocol
			//
			this.lblProtocol.AutoSize = true;
			this.lblProtocol.Location = new System.Drawing.Point(9, 52);
			this.lblProtocol.Name = "lblProtocol";
			this.lblProtocol.Size = new System.Drawing.Size(49, 13);
			this.lblProtocol.TabIndex = 10;
			this.lblProtocol.Text = "Protocol:";
			//
			// lblPort
			//
			this.lblPort.AutoSize = true;
			this.lblPort.Location = new System.Drawing.Point(9, 96);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new System.Drawing.Size(29, 13);
			this.lblPort.TabIndex = 12;
			this.lblPort.Text = "Port:";
			//
			// nupPort
			//
			this.nupPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.nupPort.Location = new System.Drawing.Point(12, 112);
			this.nupPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.nupPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nupPort.Name = "nupPort";
			this.nupPort.Size = new System.Drawing.Size(240, 20);
			this.nupPort.TabIndex = 13;
			this.nupPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nupPort.ValueChanged += new System.EventHandler(this.nupPort_ValueChanged);
			//
			// nupTimeout
			//
			this.nupTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.nupTimeout.Location = new System.Drawing.Point(12, 155);
			this.nupTimeout.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.nupTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nupTimeout.Name = "nupTimeout";
			this.nupTimeout.Size = new System.Drawing.Size(240, 20);
			this.nupTimeout.TabIndex = 15;
			this.nupTimeout.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nupTimeout.ValueChanged += new System.EventHandler(this.nupTimeout_ValueChanged);
			//
			// lblTimeout
			//
			this.lblTimeout.AutoSize = true;
			this.lblTimeout.Location = new System.Drawing.Point(9, 139);
			this.lblTimeout.Name = "lblTimeout";
			this.lblTimeout.Size = new System.Drawing.Size(48, 13);
			this.lblTimeout.TabIndex = 14;
			this.lblTimeout.Text = "Timeout:";
			//
			// NetworkInformationConnectionDialog
			//
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(264, 230);
			this.Controls.Add(this.nupTimeout);
			this.Controls.Add(this.lblTimeout);
			this.Controls.Add(this.nupPort);
			this.Controls.Add(this.lblPort);
			this.Controls.Add(this.cmbProtocol);
			this.Controls.Add(this.lblProtocol);
			this.Controls.Add(this.txtHost);
			this.Controls.Add(this.lblHost);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(272, 222);
			this.Name = "NetworkInformationConnectionDialog";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Network connection";
			((System.ComponentModel.ISupportInitialize)(this.nupPort)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nupTimeout)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblHost;
		private System.Windows.Forms.TextBox txtHost;
		private System.Windows.Forms.ComboBox cmbProtocol;
		private System.Windows.Forms.Label lblProtocol;
		private System.Windows.Forms.Label lblPort;
		private System.Windows.Forms.NumericUpDown nupPort;
		private System.Windows.Forms.NumericUpDown nupTimeout;
		private System.Windows.Forms.Label lblTimeout;
	}
}