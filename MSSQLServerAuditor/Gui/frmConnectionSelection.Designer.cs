namespace MSSQLServerAuditor.Gui
{
    partial class frmConnectionSelection
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConnectionSelection));
			this.connectionGroupInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.dgvCurrentConnection = new System.Windows.Forms.DataGridView();
			this.isEnabledDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.instanceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.authenticationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.expiryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Buildexpirydate = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.connectionInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.panel1 = new System.Windows.Forms.Panel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.listConnections = new System.Windows.Forms.DataGridView();
			this.nameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.templateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.instancesCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.splitter1 = new System.Windows.Forms.Splitter();
			((System.ComponentModel.ISupportInitialize)(this.connectionGroupInfoBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvCurrentConnection)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.connectionInfoBindingSource)).BeginInit();
			this.panel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.listConnections)).BeginInit();
			this.SuspendLayout();
			//
			// connectionGroupInfoBindingSource
			//
			this.connectionGroupInfoBindingSource.DataSource = typeof(MSSQLServerAuditor.Model.ConnectionGroupInfo);
			//
			// dgvCurrentConnection
			//
			this.dgvCurrentConnection.AllowUserToAddRows = false;
			this.dgvCurrentConnection.AllowUserToDeleteRows = false;
			this.dgvCurrentConnection.AllowUserToResizeRows = false;
			this.dgvCurrentConnection.AutoGenerateColumns = false;
			this.dgvCurrentConnection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCurrentConnection.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.isEnabledDataGridViewCheckBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.instanceDataGridViewTextBoxColumn,
            this.authenticationDataGridViewTextBoxColumn,
            this.expiryDataGridViewTextBoxColumn,
            this.Buildexpirydate});
			this.dgvCurrentConnection.DataSource = this.connectionInfoBindingSource;
			this.dgvCurrentConnection.Dock = System.Windows.Forms.DockStyle.Left;
			this.dgvCurrentConnection.Location = new System.Drawing.Point(0, 0);
			this.dgvCurrentConnection.MultiSelect = false;
			this.dgvCurrentConnection.Name = "dgvCurrentConnection";
			this.dgvCurrentConnection.RowHeadersVisible = false;
			this.dgvCurrentConnection.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvCurrentConnection.Size = new System.Drawing.Size(463, 292);
			this.dgvCurrentConnection.TabIndex = 1;
			this.dgvCurrentConnection.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DgvCurrentConnectionCellFormatting);
			this.dgvCurrentConnection.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvCurrentConnection_ColumnWidthChanged);
			//
			// isEnabledDataGridViewCheckBoxColumn
			//
			this.isEnabledDataGridViewCheckBoxColumn.DataPropertyName = "IsEnabled";
			this.isEnabledDataGridViewCheckBoxColumn.HeaderText = "";
			this.isEnabledDataGridViewCheckBoxColumn.Name = "isEnabledDataGridViewCheckBoxColumn";
			this.isEnabledDataGridViewCheckBoxColumn.Width = 30;
			//
			// nameDataGridViewTextBoxColumn
			//
			this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
			this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
			this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
			this.nameDataGridViewTextBoxColumn.ReadOnly = true;
			this.nameDataGridViewTextBoxColumn.Visible = false;
			//
			// instanceDataGridViewTextBoxColumn
			//
			this.instanceDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.instanceDataGridViewTextBoxColumn.DataPropertyName = "Instance";
			this.instanceDataGridViewTextBoxColumn.HeaderText = "Instance";
			this.instanceDataGridViewTextBoxColumn.Name = "instanceDataGridViewTextBoxColumn";
			this.instanceDataGridViewTextBoxColumn.ReadOnly = true;
			//
			// authenticationDataGridViewTextBoxColumn
			//
			this.authenticationDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.authenticationDataGridViewTextBoxColumn.DataPropertyName = "Authentication";
			this.authenticationDataGridViewTextBoxColumn.HeaderText = "Authentication";
			this.authenticationDataGridViewTextBoxColumn.Name = "authenticationDataGridViewTextBoxColumn";
			this.authenticationDataGridViewTextBoxColumn.ReadOnly = true;
			//
			// expiryDataGridViewTextBoxColumn
			//
			this.expiryDataGridViewTextBoxColumn.DataPropertyName = "LicenseExpiryDate";
			this.expiryDataGridViewTextBoxColumn.HeaderText = "License expiry";
			this.expiryDataGridViewTextBoxColumn.Name = "expiryDataGridViewTextBoxColumn";
			this.expiryDataGridViewTextBoxColumn.ReadOnly = true;
			this.expiryDataGridViewTextBoxColumn.Width = 117;
			//
			// Buildexpirydate
			//
			this.Buildexpirydate.DataPropertyName = "Buildexpirydate";
			this.Buildexpirydate.HeaderText = "Build License expiry";
			this.Buildexpirydate.Name = "Buildexpirydate";
			this.Buildexpirydate.ReadOnly = true;
			this.Buildexpirydate.Width = 130;
			//
			// connectionInfoBindingSource
			//
			this.connectionInfoBindingSource.DataSource = typeof(MSSQLServerAuditor.Model.InstanceInfo);
			//
			// panel1
			//
			this.panel1.Controls.Add(this.flowLayoutPanel1);
			this.panel1.Controls.Add(this.listConnections);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(463, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(222, 292);
			this.panel1.TabIndex = 3;
			//
			// flowLayoutPanel1
			//
			this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.btnOk);
			this.flowLayoutPanel1.Controls.Add(this.btnCancel);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(50, 253);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
			this.flowLayoutPanel1.Size = new System.Drawing.Size(172, 39);
			this.flowLayoutPanel1.TabIndex = 4;
			//
			// btnOk
			//
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Enabled = false;
			this.btnOk.Location = new System.Drawing.Point(8, 8);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			//
			// btnCancel
			//
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(89, 8);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			//
			// listConnections
			//
			this.listConnections.AllowUserToAddRows = false;
			this.listConnections.AllowUserToDeleteRows = false;
			this.listConnections.AutoGenerateColumns = false;
			this.listConnections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.listConnections.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn1,
            this.templateDataGridViewTextBoxColumn,
            this.instancesCountDataGridViewTextBoxColumn});
			this.listConnections.DataSource = this.connectionGroupInfoBindingSource;
			this.listConnections.Dock = System.Windows.Forms.DockStyle.Top;
			this.listConnections.Location = new System.Drawing.Point(0, 0);
			this.listConnections.Name = "listConnections";
			this.listConnections.ReadOnly = true;
			this.listConnections.RowHeadersVisible = false;
			this.listConnections.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.listConnections.Size = new System.Drawing.Size(222, 230);
			this.listConnections.TabIndex = 3;
			this.listConnections.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.listConnections_CellDoubleClick);
			this.listConnections.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.listConnections_CellEnter);
			this.listConnections.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.listConnections_ColumnWidthChanged);
			this.listConnections.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listConnections_KeyDown);
			//
			// nameDataGridViewTextBoxColumn1
			//
			this.nameDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.nameDataGridViewTextBoxColumn1.DataPropertyName = "Name";
			this.nameDataGridViewTextBoxColumn1.HeaderText = "Name";
			this.nameDataGridViewTextBoxColumn1.Name = "nameDataGridViewTextBoxColumn1";
			this.nameDataGridViewTextBoxColumn1.ReadOnly = true;
			//
			// templateDataGridViewTextBoxColumn
			//
			this.templateDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.templateDataGridViewTextBoxColumn.DataPropertyName = "TemplateFileName";
			this.templateDataGridViewTextBoxColumn.HeaderText = "Template";
			this.templateDataGridViewTextBoxColumn.Name = "templateDataGridViewTextBoxColumn";
			this.templateDataGridViewTextBoxColumn.ReadOnly = true;
			//
			// instancesCountDataGridViewTextBoxColumn
			//
			this.instancesCountDataGridViewTextBoxColumn.DataPropertyName = "InstancesCount";
			this.instancesCountDataGridViewTextBoxColumn.FillWeight = 30F;
			this.instancesCountDataGridViewTextBoxColumn.HeaderText = "Count";
			this.instancesCountDataGridViewTextBoxColumn.Name = "instancesCountDataGridViewTextBoxColumn";
			this.instancesCountDataGridViewTextBoxColumn.ReadOnly = true;
			this.instancesCountDataGridViewTextBoxColumn.Width = 50;
			//
			// splitter1
			//
			this.splitter1.Location = new System.Drawing.Point(463, 0);
			this.splitter1.MinExtra = 185;
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 292);
			this.splitter1.TabIndex = 0;
			this.splitter1.TabStop = false;
			//
			// frmConnectionSelection
			//
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(685, 292);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.dgvCurrentConnection);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmConnectionSelection";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select connection";
			this.Load += new System.EventHandler(this.frmConnectionSelection_Load);
			this.Shown += new System.EventHandler(this.frmConnectionSelection_Shown);
			this.Resize += new System.EventHandler(this.frmConnectionSelection_Resize);
			((System.ComponentModel.ISupportInitialize)(this.connectionGroupInfoBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvCurrentConnection)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.connectionInfoBindingSource)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.listConnections)).EndInit();
			this.ResumeLayout(false);

        }
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;

        #endregion

        private System.Windows.Forms.DataGridView dgvCurrentConnection;
        private System.Windows.Forms.BindingSource connectionInfoBindingSource;
        private System.Windows.Forms.BindingSource connectionGroupInfoBindingSource;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.DataGridView listConnections;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn templateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn instancesCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isEnabledDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn instanceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn authenticationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn expiryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Buildexpirydate;
      //private System.Windows.Forms.DataGridViewTextBoxColumn nameWithInstancesCountDataGridViewTextBoxColumn;
    }
}