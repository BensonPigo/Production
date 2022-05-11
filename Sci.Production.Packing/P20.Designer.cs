namespace Sci.Production.Packing
{
    partial class P20
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.btnFind = new Sci.Win.UI.Button();
            this.btnImportBarcode = new Sci.Win.UI.Button();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.txtPOID = new Sci.Win.UI.TextBox();
            this.txtPackID = new Sci.Win.UI.TextBox();
            this.txtScanBarcode = new Sci.Win.UI.TextBox();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.gridPackErrTransfer = new Sci.Win.UI.Grid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dateRangeTransfer = new Sci.Win.UI.DateRange();
            this.label5 = new Sci.Win.UI.Label();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridPackErrTransfer)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "SP#";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(263, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "PO#";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(521, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Pack ID";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(241, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "Scan Carton Barcode";
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(769, 5);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 30);
            this.btnFind.TabIndex = 5;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // btnImportBarcode
            // 
            this.btnImportBarcode.Location = new System.Drawing.Point(524, 3);
            this.btnImportBarcode.Name = "btnImportBarcode";
            this.btnImportBarcode.Size = new System.Drawing.Size(328, 30);
            this.btnImportBarcode.TabIndex = 6;
            this.btnImportBarcode.Text = "Import From Barcode";
            this.btnImportBarcode.UseVisualStyleBackColor = true;
            this.btnImportBarcode.Click += new System.EventHandler(this.BtnImportBarcode_Click);
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(87, 9);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(163, 23);
            this.txtSP.TabIndex = 7;
            // 
            // txtPOID
            // 
            this.txtPOID.BackColor = System.Drawing.Color.White;
            this.txtPOID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPOID.Location = new System.Drawing.Point(341, 9);
            this.txtPOID.Name = "txtPOID";
            this.txtPOID.Size = new System.Drawing.Size(163, 23);
            this.txtPOID.TabIndex = 8;
            // 
            // txtPackID
            // 
            this.txtPackID.BackColor = System.Drawing.Color.White;
            this.txtPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackID.Location = new System.Drawing.Point(599, 9);
            this.txtPackID.Name = "txtPackID";
            this.txtPackID.Size = new System.Drawing.Size(164, 23);
            this.txtPackID.TabIndex = 9;
            // 
            // txtScanBarcode
            // 
            this.txtScanBarcode.BackColor = System.Drawing.Color.White;
            this.txtScanBarcode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtScanBarcode.Location = new System.Drawing.Point(253, 7);
            this.txtScanBarcode.Name = "txtScanBarcode";
            this.txtScanBarcode.Size = new System.Drawing.Size(251, 23);
            this.txtScanBarcode.TabIndex = 10;
            this.txtScanBarcode.Validating += new System.ComponentModel.CancelEventHandler(this.TxtScanBarcode_Validating);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(537, 137);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(785, 137);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // gridPackErrTransfer
            // 
            this.gridPackErrTransfer.AllowUserToAddRows = false;
            this.gridPackErrTransfer.AllowUserToDeleteRows = false;
            this.gridPackErrTransfer.AllowUserToResizeRows = false;
            this.gridPackErrTransfer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridPackErrTransfer.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridPackErrTransfer.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridPackErrTransfer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPackErrTransfer.DataSource = this.listControlBindingSource1;
            this.gridPackErrTransfer.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridPackErrTransfer.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridPackErrTransfer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridPackErrTransfer.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridPackErrTransfer.Location = new System.Drawing.Point(12, 173);
            this.gridPackErrTransfer.Name = "gridPackErrTransfer";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridPackErrTransfer.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridPackErrTransfer.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridPackErrTransfer.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridPackErrTransfer.RowTemplate.Height = 24;
            this.gridPackErrTransfer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPackErrTransfer.ShowCellToolTips = false;
            this.gridPackErrTransfer.Size = new System.Drawing.Size(866, 298);
            this.gridPackErrTransfer.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridPackErrTransfer.TabIndex = 13;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnImportBarcode);
            this.panel1.Controls.Add(this.txtScanBarcode);
            this.panel1.Location = new System.Drawing.Point(12, 91);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(866, 40);
            this.panel1.TabIndex = 14;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.dateRangeTransfer);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.btnFind);
            this.panel2.Controls.Add(this.txtSP);
            this.panel2.Controls.Add(this.txtPOID);
            this.panel2.Controls.Add(this.txtPackID);
            this.panel2.Location = new System.Drawing.Point(12, 7);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(866, 78);
            this.panel2.TabIndex = 15;
            // 
            // dateRangeTransfer
            // 
            // 
            // 
            // 
            this.dateRangeTransfer.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeTransfer.DateBox1.Name = "";
            this.dateRangeTransfer.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeTransfer.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeTransfer.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeTransfer.DateBox2.Name = "";
            this.dateRangeTransfer.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeTransfer.DateBox2.TabIndex = 1;
            this.dateRangeTransfer.IsRequired = false;
            this.dateRangeTransfer.Location = new System.Drawing.Point(106, 43);
            this.dateRangeTransfer.Name = "dateRangeTransfer";
            this.dateRangeTransfer.Size = new System.Drawing.Size(280, 23);
            this.dateRangeTransfer.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 23);
            this.label5.TabIndex = 10;
            this.label5.Text = "Transfer Date";
            // 
            // btnToExcel
            // 
            this.btnToExcel.Location = new System.Drawing.Point(623, 137);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(153, 30);
            this.btnToExcel.TabIndex = 21;
            this.btnToExcel.Text = "To Excel(Detail)";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // P20
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 474);
            this.Controls.Add(this.btnToExcel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gridPackErrTransfer);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Name = "P20";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P20. Confirm Packing Error Revise";
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.gridPackErrTransfer, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.btnToExcel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridPackErrTransfer)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Button btnFind;
        private Win.UI.Button btnImportBarcode;
        private Win.UI.TextBox txtSP;
        private Win.UI.TextBox txtPOID;
        private Win.UI.TextBox txtPackID;
        private Win.UI.TextBox txtScanBarcode;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnClose;
        private Win.UI.Grid gridPackErrTransfer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Win.UI.DateRange dateRangeTransfer;
        private Win.UI.Label label5;
        private Win.UI.Button btnToExcel;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}