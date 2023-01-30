﻿namespace Sci.Production.Warehouse
{
    partial class P55_Import
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
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.dateTransferOutDate = new Sci.Win.UI.DateBox();
            this.lblTransferOutDate = new Sci.Win.UI.Label();
            this.txtRefno = new Sci.Win.UI.TextBox();
            this.lblSP = new Sci.Win.UI.Label();
            this.txtSeq1 = new Sci.Production.Class.TxtSeq();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.lblTransfertoSubconID = new Sci.Win.UI.Label();
            this.btnFindNow = new Sci.Win.UI.Button();
            this.txtTransfertoSubconID = new Sci.Win.UI.TextBox();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.displayTotal = new Sci.Win.UI.DisplayBox();
            this.labelTotal = new Sci.Win.UI.Label();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.grid1 = new Sci.Win.UI.Grid();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dateTransferOutDate);
            this.groupBox1.Controls.Add(this.lblTransferOutDate);
            this.groupBox1.Controls.Add(this.txtRefno);
            this.groupBox1.Controls.Add(this.lblSP);
            this.groupBox1.Controls.Add(this.txtSeq1);
            this.groupBox1.Controls.Add(this.txtSP);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblTransfertoSubconID);
            this.groupBox1.Controls.Add(this.btnFindNow);
            this.groupBox1.Controls.Add(this.txtTransfertoSubconID);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(854, 92);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            // 
            // dateTransferOutDate
            // 
            this.dateTransferOutDate.Location = new System.Drawing.Point(437, 22);
            this.dateTransferOutDate.Name = "dateTransferOutDate";
            this.dateTransferOutDate.Size = new System.Drawing.Size(130, 23);
            this.dateTransferOutDate.TabIndex = 140;
            // 
            // lblTransferOutDate
            // 
            this.lblTransferOutDate.BackColor = System.Drawing.Color.SkyBlue;
            this.lblTransferOutDate.Location = new System.Drawing.Point(317, 22);
            this.lblTransferOutDate.Name = "lblTransferOutDate";
            this.lblTransferOutDate.Size = new System.Drawing.Size(117, 23);
            this.lblTransferOutDate.TabIndex = 139;
            this.lblTransferOutDate.Text = "Transfer Out Date";
            this.lblTransferOutDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtRefno
            // 
            this.txtRefno.BackColor = System.Drawing.Color.White;
            this.txtRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefno.Location = new System.Drawing.Point(420, 55);
            this.txtRefno.Name = "txtRefno";
            this.txtRefno.Size = new System.Drawing.Size(147, 23);
            this.txtRefno.TabIndex = 138;
            // 
            // lblSP
            // 
            this.lblSP.BackColor = System.Drawing.Color.SkyBlue;
            this.lblSP.Location = new System.Drawing.Point(9, 55);
            this.lblSP.Name = "lblSP";
            this.lblSP.Size = new System.Drawing.Size(98, 23);
            this.lblSP.TabIndex = 137;
            this.lblSP.Text = "SP#";
            this.lblSP.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSeq1
            // 
            this.txtSeq1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSeq1.Location = new System.Drawing.Point(238, 55);
            this.txtSeq1.Name = "txtSeq1";
            this.txtSeq1.Seq1 = "";
            this.txtSeq1.Seq2 = "";
            this.txtSeq1.Size = new System.Drawing.Size(61, 23);
            this.txtSeq1.TabIndex = 136;
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(110, 55);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(122, 23);
            this.txtSP.TabIndex = 135;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(317, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 134;
            this.label2.Text = "Refno";
            // 
            // lblTransfertoSubconID
            // 
            this.lblTransfertoSubconID.BackColor = System.Drawing.Color.SkyBlue;
            this.lblTransfertoSubconID.Location = new System.Drawing.Point(9, 19);
            this.lblTransfertoSubconID.Name = "lblTransfertoSubconID";
            this.lblTransfertoSubconID.Size = new System.Drawing.Size(147, 23);
            this.lblTransfertoSubconID.TabIndex = 133;
            this.lblTransfertoSubconID.Text = "Transfer to Sub con ID";
            this.lblTransfertoSubconID.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // btnFindNow
            // 
            this.btnFindNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFindNow.Location = new System.Drawing.Point(741, 22);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(101, 30);
            this.btnFindNow.TabIndex = 3;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.BtnFindNow_Click);
            // 
            // txtTransfertoSubconID
            // 
            this.txtTransfertoSubconID.BackColor = System.Drawing.Color.White;
            this.txtTransfertoSubconID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTransfertoSubconID.Location = new System.Drawing.Point(159, 19);
            this.txtTransfertoSubconID.Name = "txtTransfertoSubconID";
            this.txtTransfertoSubconID.Size = new System.Drawing.Size(140, 23);
            this.txtTransfertoSubconID.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(758, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(662, 16);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 30);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.displayTotal);
            this.groupBox2.Controls.Add(this.labelTotal);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 397);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(854, 53);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            // 
            // displayTotal
            // 
            this.displayTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.displayTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayTotal.Location = new System.Drawing.Point(542, 20);
            this.displayTotal.Name = "displayTotal";
            this.displayTotal.Size = new System.Drawing.Size(100, 23);
            this.displayTotal.TabIndex = 4;
            // 
            // labelTotal
            // 
            this.labelTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTotal.Location = new System.Drawing.Point(394, 20);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(145, 23);
            this.labelTotal.TabIndex = 3;
            this.labelTotal.Text = " Total Transfer Out Qty";
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(0, 92);
            this.grid1.Name = "grid1";
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(854, 305);
            this.grid1.TabIndex = 23;
            this.grid1.TabStop = false;
            // 
            // P55_Import
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 450);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "P55_Import";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P55 Import Detail";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.GroupBox groupBox1;
        private Win.UI.Button btnFindNow;
        private Win.UI.TextBox txtTransfertoSubconID;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnImport;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.DisplayBox displayTotal;
        private Win.UI.Label labelTotal;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Grid grid1;
        private Win.UI.Label lblSP;
        private Class.TxtSeq txtSeq1;
        private Win.UI.TextBox txtSP;
        private Win.UI.Label label2;
        private Win.UI.Label lblTransfertoSubconID;
        private Win.UI.TextBox txtRefno;
        private Win.UI.Label lblTransferOutDate;
        private Win.UI.DateBox dateTransferOutDate;
    }
}