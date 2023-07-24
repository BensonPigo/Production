﻿namespace Sci.Production.Warehouse
{
    partial class P73_Import
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.btnQuery = new Sci.Win.UI.Button();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.BalanceQty = new Sci.Win.UI.CheckBox();
            this.btnUpdateAllLocation = new Sci.Win.UI.Button();
            this.txtLocation2 = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.txtRoll = new Sci.Win.UI.TextBox();
            this.label6 = new Sci.Win.UI.Label();
            this.txtColor = new Sci.Win.UI.TextBox();
            this.label5 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.txtSeq = new Sci.Production.Class.TxtSeq();
            this.txtDyelot = new Sci.Win.UI.TextBox();
            this.labelDyelot = new Sci.Win.UI.Label();
            this.txtLocation = new Sci.Win.UI.TextBox();
            this.labelLocation = new Sci.Win.UI.Label();
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridImport = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(999, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(903, 16);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 30);
            this.btnImport.TabIndex = 4;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnQuery.Location = new System.Drawing.Point(952, 22);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(84, 30);
            this.btnQuery.TabIndex = 12;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.IsSupportEditMode = false;
            this.txtSPNo.Location = new System.Drawing.Point(93, 22);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(122, 23);
            this.txtSPNo.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.BalanceQty);
            this.groupBox2.Controls.Add(this.btnUpdateAllLocation);
            this.groupBox2.Controls.Add(this.txtLocation2);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 477);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1095, 53);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            // 
            // BalanceQty
            // 
            this.BalanceQty.AutoSize = true;
            this.BalanceQty.Checked = true;
            this.BalanceQty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BalanceQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BalanceQty.Location = new System.Drawing.Point(12, 21);
            this.BalanceQty.Name = "BalanceQty";
            this.BalanceQty.Size = new System.Drawing.Size(128, 21);
            this.BalanceQty.TabIndex = 6;
            this.BalanceQty.Text = "Balance Qty > 0";
            this.BalanceQty.UseVisualStyleBackColor = true;
            this.BalanceQty.CheckedChanged += new System.EventHandler(this.BalanceQty_CheckedChanged);
            // 
            // btnUpdateAllLocation
            // 
            this.btnUpdateAllLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateAllLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnUpdateAllLocation.Location = new System.Drawing.Point(722, 16);
            this.btnUpdateAllLocation.Name = "btnUpdateAllLocation";
            this.btnUpdateAllLocation.Size = new System.Drawing.Size(156, 30);
            this.btnUpdateAllLocation.TabIndex = 3;
            this.btnUpdateAllLocation.Text = "Update All Location";
            this.btnUpdateAllLocation.UseVisualStyleBackColor = true;
            this.btnUpdateAllLocation.Click += new System.EventHandler(this.BtnUpdateAllLocation_Click);
            // 
            // txtLocation2
            // 
            this.txtLocation2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtLocation2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtLocation2.IsSupportEditMode = false;
            this.txtLocation2.Location = new System.Drawing.Point(220, 20);
            this.txtLocation2.Name = "txtLocation2";
            this.txtLocation2.ReadOnly = true;
            this.txtLocation2.Size = new System.Drawing.Size(400, 23);
            this.txtLocation2.TabIndex = 2;
            this.txtLocation2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TxtLocation2_MouseDown);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(146, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Location";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtRoll);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtColor);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtSeq);
            this.groupBox1.Controls.Add(this.txtDyelot);
            this.groupBox1.Controls.Add(this.btnQuery);
            this.groupBox1.Controls.Add(this.labelDyelot);
            this.groupBox1.Controls.Add(this.txtSPNo);
            this.groupBox1.Controls.Add(this.txtLocation);
            this.groupBox1.Controls.Add(this.labelLocation);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1095, 86);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // txtRoll
            // 
            this.txtRoll.BackColor = System.Drawing.Color.White;
            this.txtRoll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRoll.IsSupportEditMode = false;
            this.txtRoll.Location = new System.Drawing.Point(523, 22);
            this.txtRoll.Name = "txtRoll";
            this.txtRoll.Size = new System.Drawing.Size(77, 23);
            this.txtRoll.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(469, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 23);
            this.label6.TabIndex = 18;
            this.label6.Text = "Roll";
            // 
            // txtColor
            // 
            this.txtColor.BackColor = System.Drawing.Color.White;
            this.txtColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtColor.IsSupportEditMode = false;
            this.txtColor.Location = new System.Drawing.Point(374, 22);
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(80, 23);
            this.txtColor.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(296, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 16;
            this.label5.Text = "Color";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(14, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 23);
            this.label3.TabIndex = 14;
            this.label3.Text = "SP#";
            // 
            // txtSeq
            // 
            this.txtSeq.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSeq.Location = new System.Drawing.Point(221, 22);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.Seq1 = "";
            this.txtSeq.Seq2 = "";
            this.txtSeq.Size = new System.Drawing.Size(72, 23);
            this.txtSeq.TabIndex = 2;
            // 
            // txtDyelot
            // 
            this.txtDyelot.BackColor = System.Drawing.Color.White;
            this.txtDyelot.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDyelot.IsSupportEditMode = false;
            this.txtDyelot.Location = new System.Drawing.Point(657, 22);
            this.txtDyelot.Name = "txtDyelot";
            this.txtDyelot.Size = new System.Drawing.Size(73, 23);
            this.txtDyelot.TabIndex = 6;
            // 
            // labelDyelot
            // 
            this.labelDyelot.Location = new System.Drawing.Point(603, 22);
            this.labelDyelot.Name = "labelDyelot";
            this.labelDyelot.Size = new System.Drawing.Size(51, 23);
            this.labelDyelot.TabIndex = 9;
            this.labelDyelot.Text = "Dyelot";
            // 
            // txtLocation
            // 
            this.txtLocation.BackColor = System.Drawing.Color.White;
            this.txtLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocation.IsSupportEditMode = false;
            this.txtLocation.Location = new System.Drawing.Point(93, 52);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(122, 23);
            this.txtLocation.TabIndex = 9;
            // 
            // labelLocation
            // 
            this.labelLocation.Location = new System.Drawing.Point(14, 52);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(77, 23);
            this.labelLocation.TabIndex = 7;
            this.labelLocation.Text = "Location";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridImport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 86);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1095, 391);
            this.panel1.TabIndex = 20;
            // 
            // gridImport
            // 
            this.gridImport.AllowUserToAddRows = false;
            this.gridImport.AllowUserToDeleteRows = false;
            this.gridImport.AllowUserToResizeRows = false;
            this.gridImport.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridImport.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridImport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridImport.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridImport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridImport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridImport.Location = new System.Drawing.Point(0, 0);
            this.gridImport.Name = "gridImport";
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridImport.RowTemplate.Height = 24;
            this.gridImport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridImport.ShowCellToolTips = false;
            this.gridImport.Size = new System.Drawing.Size(1095, 391);
            this.gridImport.TabIndex = 0;
            this.gridImport.TabStop = false;
            // 
            // P73_Import
            // 
            this.ClientSize = new System.Drawing.Size(1095, 530);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.DefaultControl = "txtSPNo";
            this.Name = "P73_Import";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P73. Import Detail";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnCancel;
        private Win.UI.Button btnImport;
        private Win.UI.Button btnQuery;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Panel panel1;
        private Win.UI.Grid gridImport;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Label label2;
        private Win.UI.Button btnUpdateAllLocation;
        private Win.UI.TextBox txtLocation2;
        private Win.UI.TextBox txtDyelot;
        private Win.UI.Label labelDyelot;
        private Win.UI.TextBox txtLocation;
        private Win.UI.Label labelLocation;
        private Class.TxtSeq txtSeq;
        private Win.UI.CheckBox BalanceQty;
        private Win.UI.TextBox txtRoll;
        private Win.UI.Label label6;
        private Win.UI.TextBox txtColor;
        private Win.UI.Label label5;
        private Win.UI.Label label3;
    }
}
