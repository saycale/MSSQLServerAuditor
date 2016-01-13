namespace MSSQLServerAuditor.Gui
{
	partial class frmUserSettingsParameters
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserSettingsParameters));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.iQueryParametersBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.btnRemove = new System.Windows.Forms.Button();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.lblQueries = new System.Windows.Forms.Label();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.queriesBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.dgvParameters = new System.Windows.Forms.DataGridView();
			this.ParameterColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.KeyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DefaultColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.parametersBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.lblParameters = new System.Windows.Forms.Label();
			this.btnApply = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.iQueryParametersBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.queriesBindingSource)).BeginInit();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvParameters)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.parametersBindingSource)).BeginInit();
			this.SuspendLayout();
			//
			// tableLayoutPanel1
			//
			this.tableLayoutPanel1.ColumnCount = 6;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.btnCancel, 5, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnOk, 4, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnAdd, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnRemove, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnApply, 3, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(548, 275);
			this.tableLayoutPanel1.TabIndex = 0;
			//
			// btnCancel
			//
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(470, 249);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			//
			// btnOk
			//
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(389, 249);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "Ok";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			//
			// btnAdd
			//
			this.btnAdd.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.iQueryParametersBindingSource, "AvailbleEdit", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.btnAdd.Location = new System.Drawing.Point(3, 249);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(75, 23);
			this.btnAdd.TabIndex = 6;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			//
			// iQueryParametersBindingSource
			//
			this.iQueryParametersBindingSource.DataSource = typeof(MSSQLServerAuditor.Model.UserSettingsParameters.IQueryParameters);
			//
			// btnRemove
			//
			this.btnRemove.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.iQueryParametersBindingSource, "AvailableRemove", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.btnRemove.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.iQueryParametersBindingSource, "AvailbleEdit", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.btnRemove.Location = new System.Drawing.Point(84, 249);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(75, 23);
			this.btnRemove.TabIndex = 7;
			this.btnRemove.Text = "Remove";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			//
			// splitContainer1
			//
			this.tableLayoutPanel1.SetColumnSpan(this.splitContainer1, 6);
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(3, 3);
			this.splitContainer1.Name = "splitContainer1";
			//
			// splitContainer1.Panel1
			//
			this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel3);
			this.splitContainer1.Panel1Collapsed = true;
			this.splitContainer1.Panel1MinSize = 100;
			//
			// splitContainer1.Panel2
			//
			this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel2);
			this.splitContainer1.Panel2MinSize = 300;
			this.tableLayoutPanel1.SetRowSpan(this.splitContainer1, 2);
			this.splitContainer1.Size = new System.Drawing.Size(542, 240);
			this.splitContainer1.SplitterDistance = 100;
			this.splitContainer1.TabIndex = 8;
			//
			// tableLayoutPanel3
			//
			this.tableLayoutPanel3.ColumnCount = 1;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.lblQueries, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.listBox1, 0, 1);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 2;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(100, 100);
			this.tableLayoutPanel3.TabIndex = 0;
			//
			// lblQueries
			//
			this.lblQueries.AutoSize = true;
			this.lblQueries.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblQueries.Location = new System.Drawing.Point(3, 0);
			this.lblQueries.Name = "lblQueries";
			this.lblQueries.Size = new System.Drawing.Size(94, 20);
			this.lblQueries.TabIndex = 4;
			this.lblQueries.Text = "Queries:";
			this.lblQueries.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// listBox1
			//
			this.listBox1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.iQueryParametersBindingSource, "SelectedQuery", true));
			this.listBox1.DataSource = this.queriesBindingSource;
			this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new System.Drawing.Point(3, 23);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(94, 74);
			this.listBox1.TabIndex = 3;
			this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			//
			// queriesBindingSource
			//
			this.queriesBindingSource.DataMember = "Queries";
			this.queriesBindingSource.DataSource = this.iQueryParametersBindingSource;
			//
			// tableLayoutPanel2
			//
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.dgvParameters, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.lblParameters, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(542, 240);
			this.tableLayoutPanel2.TabIndex = 0;
			//
			// dgvParameters
			//
			this.dgvParameters.AllowUserToAddRows = false;
			this.dgvParameters.AllowUserToDeleteRows = false;
			this.dgvParameters.AutoGenerateColumns = false;
			this.dgvParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvParameters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ParameterColumn,
            this.KeyColumn,
            this.TypeColumn,
            this.DefaultColumn,
            this.ValueColumn});
			this.tableLayoutPanel2.SetColumnSpan(this.dgvParameters, 4);
			this.dgvParameters.DataSource = this.parametersBindingSource;
			this.dgvParameters.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvParameters.Location = new System.Drawing.Point(3, 23);
			this.dgvParameters.Name = "dgvParameters";
			this.dgvParameters.RowHeadersVisible = false;
			this.dgvParameters.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvParameters.Size = new System.Drawing.Size(536, 214);
			this.dgvParameters.TabIndex = 2;
			this.dgvParameters.SelectionChanged += new System.EventHandler(this.dgvParameters_SelectionChanged);
			//
			// ParameterColumn
			//
			this.ParameterColumn.DataPropertyName = "LocParameter";
			this.ParameterColumn.HeaderText = "Parameter";
			this.ParameterColumn.Name = "ParameterColumn";
			this.ParameterColumn.ReadOnly = true;
			//
			// KeyColumn
			//
			this.KeyColumn.DataPropertyName = "Key";
			this.KeyColumn.HeaderText = "Key";
			this.KeyColumn.Name = "KeyColumn";
			this.KeyColumn.Visible = false;
			//
			// TypeColumn
			//
			this.TypeColumn.DataPropertyName = "Type";
			this.TypeColumn.HeaderText = "Type";
			this.TypeColumn.Name = "TypeColumn";
			this.TypeColumn.Visible = false;
			//
			// DefaultColumn
			//
			this.DefaultColumn.DataPropertyName = "Default";
			this.DefaultColumn.HeaderText = "Default";
			this.DefaultColumn.Name = "DefaultColumn";
			//
			// ValueColumn
			//
			this.ValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.ValueColumn.DataPropertyName = "Value";
			this.ValueColumn.HeaderText = "Value";
			this.ValueColumn.Name = "ValueColumn";
			//
			// parametersBindingSource
			//
			this.parametersBindingSource.DataMember = "Parameters";
			this.parametersBindingSource.DataSource = this.iQueryParametersBindingSource;
			//
			// lblParameters
			//
			this.lblParameters.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.lblParameters, 2);
			this.lblParameters.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblParameters.Location = new System.Drawing.Point(3, 0);
			this.lblParameters.Name = "lblParameters";
			this.lblParameters.Size = new System.Drawing.Size(536, 20);
			this.lblParameters.TabIndex = 5;
			this.lblParameters.Text = "Parameters:";
			this.lblParameters.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// btnApply
			//
			this.btnApply.Location = new System.Drawing.Point(308, 249);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(75, 23);
			this.btnApply.TabIndex = 9;
			this.btnApply.Text = "Apply";
			this.btnApply.UseVisualStyleBackColor = true;
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			//
			// frmUserSettingsParameters
			//
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(548, 275);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmUserSettingsParameters";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "frmUserSettingsParameters";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUserSettingsParameters_FormClosing);
			this.Load += new System.EventHandler(this.frmUserSettingsParameters_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.iQueryParametersBindingSource)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.queriesBindingSource)).EndInit();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvParameters)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.parametersBindingSource)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.DataGridView dgvParameters;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Label lblQueries;
		private System.Windows.Forms.Label lblParameters;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.BindingSource parametersBindingSource;
		private System.Windows.Forms.BindingSource iQueryParametersBindingSource;
		private System.Windows.Forms.BindingSource queriesBindingSource;
		private System.Windows.Forms.DataGridViewTextBoxColumn ParameterColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn KeyColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn TypeColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn DefaultColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn ValueColumn;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button btnApply;
	}
}
