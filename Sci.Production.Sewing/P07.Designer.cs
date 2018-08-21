﻿namespace Sci.Production.Sewing
{
    partial class P07
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
            this.btnImportFromBarcode = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.txtPO = new Sci.Win.UI.TextBox();
            this.txtPackID = new Sci.Win.UI.TextBox();
            this.txtScanBarcode = new Sci.Win.UI.TextBox();
            this.dateRangeReceive = new Sci.Win.UI.DateRange();
            this.label5 = new Sci.Win.UI.Label();
            this.gridTransfer = new Sci.Win.UI.Grid();
            this.label6 = new Sci.Win.UI.Label();
            this.comboTransferTo = new Sci.Production.Class.comboDropDownList(this.components);
            this.btnUpdateAll = new Sci.Win.UI.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gridTransfer)).BeginInit();
            this.panel7.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "SP#";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(214, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "PO#";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(406, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Pack ID";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(4, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(152, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "Scan Cartons Barcode";
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(678, 8);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 30);
            this.btnFind.TabIndex = 4;
            this.btnFind.TabStop = false;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // btnImportFromBarcode
            // 
            this.btnImportFromBarcode.Location = new System.Drawing.Point(557, 5);
            this.btnImportFromBarcode.Name = "btnImportFromBarcode";
            this.btnImportFromBarcode.Size = new System.Drawing.Size(201, 30);
            this.btnImportFromBarcode.TabIndex = 6;
            this.btnImportFromBarcode.TabStop = false;
            this.btnImportFromBarcode.Text = "Import From Barcode";
            this.btnImportFromBarcode.UseVisualStyleBackColor = true;
            this.btnImportFromBarcode.Click += new System.EventHandler(this.BtnImportFromBarcode_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(562, 136);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 7;
            this.btnSave.TabStop = false;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(683, 136);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 8;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(100, 12);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(111, 23);
            this.txtSP.TabIndex = 0;
            // 
            // txtPO
            // 
            this.txtPO.BackColor = System.Drawing.Color.White;
            this.txtPO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPO.Location = new System.Drawing.Point(292, 12);
            this.txtPO.Name = "txtPO";
            this.txtPO.Size = new System.Drawing.Size(111, 23);
            this.txtPO.TabIndex = 1;
            // 
            // txtPackID
            // 
            this.txtPackID.BackColor = System.Drawing.Color.White;
            this.txtPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackID.Location = new System.Drawing.Point(484, 12);
            this.txtPackID.Name = "txtPackID";
            this.txtPackID.Size = new System.Drawing.Size(111, 23);
            this.txtPackID.TabIndex = 2;
            // 
            // txtScanBarcode
            // 
            this.txtScanBarcode.BackColor = System.Drawing.Color.White;
            this.txtScanBarcode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtScanBarcode.Location = new System.Drawing.Point(159, 9);
            this.txtScanBarcode.Name = "txtScanBarcode";
            this.txtScanBarcode.Size = new System.Drawing.Size(221, 23);
            this.txtScanBarcode.TabIndex = 4;
            this.txtScanBarcode.Validating += new System.ComponentModel.CancelEventHandler(this.TxtScanBarcode_Validating);
            // 
            // dateRangeReceive
            // 
            // 
            // 
            // 
            this.dateRangeReceive.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeReceive.DateBox1.Name = "";
            this.dateRangeReceive.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeReceive.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeReceive.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeReceive.DateBox2.Name = "";
            this.dateRangeReceive.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeReceive.DateBox2.TabIndex = 1;
            this.dateRangeReceive.Location = new System.Drawing.Point(100, 39);
            this.dateRangeReceive.Name = "dateRangeReceive";
            this.dateRangeReceive.Size = new System.Drawing.Size(280, 23);
            this.dateRangeReceive.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(4, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 23);
            this.label5.TabIndex = 6;
            this.label5.Text = "Receive Date";
            // 
            // gridTransfer
            // 
            this.gridTransfer.AllowUserToAddRows = false;
            this.gridTransfer.AllowUserToDeleteRows = false;
            this.gridTransfer.AllowUserToResizeRows = false;
            this.gridTransfer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridTransfer.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridTransfer.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridTransfer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTransfer.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridTransfer.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridTransfer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridTransfer.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridTransfer.Location = new System.Drawing.Point(0, 172);
            this.gridTransfer.Name = "gridTransfer";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridTransfer.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridTransfer.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridTransfer.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridTransfer.RowTemplate.Height = 24;
            this.gridTransfer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridTransfer.ShowCellToolTips = false;
            this.gridTransfer.Size = new System.Drawing.Size(777, 288);
            this.gridTransfer.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridTransfer.TabIndex = 14;
            this.gridTransfer.TabStop = false;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(9, 140);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 23);
            this.label6.TabIndex = 7;
            this.label6.Text = "Transfer To";
            // 
            // comboTransferTo
            // 
            this.comboTransferTo.BackColor = System.Drawing.Color.White;
            this.comboTransferTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboTransferTo.FormattingEnabled = true;
            this.comboTransferTo.IsSupportUnselect = true;
            this.comboTransferTo.Location = new System.Drawing.Point(105, 139);
            this.comboTransferTo.Name = "comboTransferTo";
            this.comboTransferTo.OldText = "";
            this.comboTransferTo.Size = new System.Drawing.Size(121, 24);
            this.comboTransferTo.TabIndex = 5;
            this.comboTransferTo.Type = "Pms_DRYTransferTo";
            // 
            // btnUpdateAll
            // 
            this.btnUpdateAll.Location = new System.Drawing.Point(232, 136);
            this.btnUpdateAll.Name = "btnUpdateAll";
            this.btnUpdateAll.Size = new System.Drawing.Size(176, 30);
            this.btnUpdateAll.TabIndex = 7;
            this.btnUpdateAll.TabStop = false;
            this.btnUpdateAll.Text = "Update All Transfer To";
            this.btnUpdateAll.UseVisualStyleBackColor = true;
            this.btnUpdateAll.Click += new System.EventHandler(this.BtnUpdateAll_Click);
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.label4);
            this.panel7.Controls.Add(this.btnImportFromBarcode);
            this.panel7.Controls.Add(this.txtScanBarcode);
            this.panel7.Location = new System.Drawing.Point(4, 86);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(767, 45);
            this.panel7.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtPackID);
            this.panel1.Controls.Add(this.dateRangeReceive);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtSP);
            this.panel1.Controls.Add(this.txtPO);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.btnFind);
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(767, 78);
            this.panel1.TabIndex = 1;
            // 
            // P07
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 463);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.btnUpdateAll);
            this.Controls.Add(this.comboTransferTo);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.gridTransfer);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.DefaultControl = "txtScanBarcode";
            this.DefaultControlForEdit = "txtScanBarcode";
            this.Name = "P07";
            this.Text = "P07. Dry Room Transfer Carton Input";
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.gridTransfer, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.comboTransferTo, 0);
            this.Controls.SetChildIndex(this.btnUpdateAll, 0);
            this.Controls.SetChildIndex(this.panel7, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridTransfer)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Button btnFind;
        private Win.UI.Button btnImportFromBarcode;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnClose;
        private Win.UI.TextBox txtSP;
        private Win.UI.TextBox txtPO;
        private Win.UI.TextBox txtPackID;
        private Win.UI.TextBox txtScanBarcode;
        private Win.UI.Grid gridTransfer;
        private Win.UI.DateRange dateRangeReceive;
        private Win.UI.Label label5;
        private Win.UI.Label label6;
        private Class.comboDropDownList comboTransferTo;
        private Win.UI.Button btnUpdateAll;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel1;
    }
}