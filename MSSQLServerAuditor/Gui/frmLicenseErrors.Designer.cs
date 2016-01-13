namespace MSSQLServerAuditor.Gui
{
    partial class frmLicenseErrors
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLicenseErrors));
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.instanceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.problemsTextDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.licenseProblemBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.oKbutton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.licenseProblemBindingSource)).BeginInit();
			this.SuspendLayout();
			//
			// dataGridView1
			//
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView1.AutoGenerateColumns = false;
			this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.instanceDataGridViewTextBoxColumn,
            this.problemsTextDataGridViewTextBoxColumn});
			this.dataGridView1.DataSource = this.licenseProblemBindingSource;
			this.dataGridView1.Location = new System.Drawing.Point(12, 12);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView1.Size = new System.Drawing.Size(766, 378);
			this.dataGridView1.TabIndex = 0;
			this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
			//
			// instanceDataGridViewTextBoxColumn
			//
			this.instanceDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.instanceDataGridViewTextBoxColumn.DataPropertyName = "Instance";
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.instanceDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
			this.instanceDataGridViewTextBoxColumn.FillWeight = 50F;
			this.instanceDataGridViewTextBoxColumn.HeaderText = "Instance";
			this.instanceDataGridViewTextBoxColumn.Name = "instanceDataGridViewTextBoxColumn";
			this.instanceDataGridViewTextBoxColumn.ReadOnly = true;
			//
			// problemsTextDataGridViewTextBoxColumn
			//
			this.problemsTextDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.problemsTextDataGridViewTextBoxColumn.DataPropertyName = "ProblemsText";
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.problemsTextDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
			this.problemsTextDataGridViewTextBoxColumn.HeaderText = "Problem description";
			this.problemsTextDataGridViewTextBoxColumn.Name = "problemsTextDataGridViewTextBoxColumn";
			this.problemsTextDataGridViewTextBoxColumn.ReadOnly = true;
			//
			// licenseProblemBindingSource
			//
			this.licenseProblemBindingSource.DataSource = typeof(MSSQLServerAuditor.Model.LicenseState);
			//
			// oKbutton
			//
			this.oKbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.oKbutton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.oKbutton.Location = new System.Drawing.Point(703, 396);
			this.oKbutton.Name = "oKbutton";
			this.oKbutton.Size = new System.Drawing.Size(75, 23);
			this.oKbutton.TabIndex = 1;
			this.oKbutton.Text = "OK";
			this.oKbutton.UseVisualStyleBackColor = true;
			//
			// frmLicenseErrors
			//
			this.AcceptButton = this.oKbutton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.oKbutton;
			this.ClientSize = new System.Drawing.Size(790, 431);
			this.Controls.Add(this.oKbutton);
			this.Controls.Add(this.dataGridView1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmLicenseErrors";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "License errors";
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.licenseProblemBindingSource)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource licenseProblemBindingSource;
        private System.Windows.Forms.Button oKbutton;
        private System.Windows.Forms.DataGridViewTextBoxColumn instanceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn problemsTextDataGridViewTextBoxColumn;
    }
}