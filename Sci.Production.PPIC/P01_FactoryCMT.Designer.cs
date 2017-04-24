﻿namespace Sci.Production.PPIC
{
    partial class P01_FactoryCMT
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.numStdFtyCMP = new Sci.Win.UI.NumericBox();
            this.label10 = new Sci.Win.UI.Label();
            this.numLocalPurchase = new Sci.Win.UI.NumericBox();
            this.label9 = new Sci.Win.UI.Label();
            this.numSubProcess = new Sci.Win.UI.NumericBox();
            this.label8 = new Sci.Win.UI.Label();
            this.numCPUCost = new Sci.Win.UI.NumericBox();
            this.label7 = new Sci.Win.UI.Label();
            this.numCPU = new Sci.Win.UI.NumericBox();
            this.label6 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridFactoryCMT = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFactoryCMT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 363);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(762, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 363);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(752, 10);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Controls.Add(this.numStdFtyCMP);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.numLocalPurchase);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Controls.Add(this.numSubProcess);
            this.panel4.Controls.Add(this.label8);
            this.panel4.Controls.Add(this.numCPUCost);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.numCPU);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(10, 286);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(752, 77);
            this.panel4.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(671, 28);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // numStdFtyCMP
            // 
            this.numStdFtyCMP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numStdFtyCMP.DecimalPlaces = 2;
            this.numStdFtyCMP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numStdFtyCMP.IsSupportEditMode = false;
            this.numStdFtyCMP.Location = new System.Drawing.Point(516, 46);
            this.numStdFtyCMP.Name = "numStdFtyCMP";
            this.numStdFtyCMP.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numStdFtyCMP.ReadOnly = true;
            this.numStdFtyCMP.Size = new System.Drawing.Size(100, 23);
            this.numStdFtyCMP.TabIndex = 14;
            this.numStdFtyCMP.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(487, 46);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 23);
            this.label10.TabIndex = 13;
            this.label10.Text = "=";
            this.label10.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label10.TextStyle.Color = System.Drawing.Color.Black;
            this.label10.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label10.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // numLocalPurchase
            // 
            this.numLocalPurchase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numLocalPurchase.DecimalPlaces = 3;
            this.numLocalPurchase.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numLocalPurchase.IsSupportEditMode = false;
            this.numLocalPurchase.Location = new System.Drawing.Point(392, 46);
            this.numLocalPurchase.Name = "numLocalPurchase";
            this.numLocalPurchase.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numLocalPurchase.ReadOnly = true;
            this.numLocalPurchase.Size = new System.Drawing.Size(76, 23);
            this.numLocalPurchase.TabIndex = 12;
            this.numLocalPurchase.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(369, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 23);
            this.label9.TabIndex = 11;
            this.label9.Text = "+";
            this.label9.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            this.label9.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // numSubProcess
            // 
            this.numSubProcess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numSubProcess.DecimalPlaces = 3;
            this.numSubProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numSubProcess.IsSupportEditMode = false;
            this.numSubProcess.Location = new System.Drawing.Point(283, 46);
            this.numSubProcess.Name = "numSubProcess";
            this.numSubProcess.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSubProcess.ReadOnly = true;
            this.numSubProcess.Size = new System.Drawing.Size(76, 23);
            this.numSubProcess.TabIndex = 10;
            this.numSubProcess.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(261, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 23);
            this.label8.TabIndex = 9;
            this.label8.Text = "+";
            this.label8.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label8.TextStyle.Color = System.Drawing.Color.Black;
            this.label8.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label8.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // numCPUCost
            // 
            this.numCPUCost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numCPUCost.DecimalPlaces = 3;
            this.numCPUCost.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numCPUCost.IsSupportEditMode = false;
            this.numCPUCost.Location = new System.Drawing.Point(190, 46);
            this.numCPUCost.Name = "numCPUCost";
            this.numCPUCost.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCPUCost.ReadOnly = true;
            this.numCPUCost.Size = new System.Drawing.Size(60, 23);
            this.numCPUCost.TabIndex = 8;
            this.numCPUCost.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(171, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(10, 23);
            this.label7.TabIndex = 7;
            this.label7.Text = "*";
            this.label7.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label7.TextStyle.Color = System.Drawing.Color.Black;
            this.label7.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label7.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // numCPU
            // 
            this.numCPU.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numCPU.DecimalPlaces = 3;
            this.numCPU.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numCPU.IsSupportEditMode = false;
            this.numCPU.Location = new System.Drawing.Point(103, 46);
            this.numCPU.Name = "numCPU";
            this.numCPU.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCPU.ReadOnly = true;
            this.numCPU.Size = new System.Drawing.Size(60, 23);
            this.numCPU.TabIndex = 6;
            this.numCPU.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(3, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 23);
            this.label6.TabIndex = 5;
            this.label6.Text = "Factory CMT = ";
            this.label6.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label6.TextStyle.Color = System.Drawing.Color.Black;
            this.label6.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label6.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(516, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "Std. Fty CMP";
            this.label5.TextStyle.BorderColor = System.Drawing.Color.Blue;
            this.label5.TextStyle.Color = System.Drawing.Color.Blue;
            this.label5.TextStyle.ExtBorderColor = System.Drawing.Color.Blue;
            this.label5.TextStyle.GradientColor = System.Drawing.Color.Blue;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(379, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 34);
            this.label4.TabIndex = 3;
            this.label4.Text = "Local Purchase\\r\\nStd. Cost";
            this.label4.TextStyle.BorderColor = System.Drawing.Color.Blue;
            this.label4.TextStyle.Color = System.Drawing.Color.Blue;
            this.label4.TextStyle.ExtBorderColor = System.Drawing.Color.Blue;
            this.label4.TextStyle.GradientColor = System.Drawing.Color.Blue;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(283, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 34);
            this.label3.TabIndex = 2;
            this.label3.Text = "Sub Process\\r\\nStd. Cost";
            this.label3.TextStyle.BorderColor = System.Drawing.Color.Blue;
            this.label3.TextStyle.Color = System.Drawing.Color.Blue;
            this.label3.TextStyle.ExtBorderColor = System.Drawing.Color.Blue;
            this.label3.TextStyle.GradientColor = System.Drawing.Color.Blue;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(189, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "CPU Cost";
            this.label2.TextStyle.BorderColor = System.Drawing.Color.Blue;
            this.label2.TextStyle.Color = System.Drawing.Color.Blue;
            this.label2.TextStyle.ExtBorderColor = System.Drawing.Color.Blue;
            this.label2.TextStyle.GradientColor = System.Drawing.Color.Blue;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(116, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "CPU";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Blue;
            this.label1.TextStyle.Color = System.Drawing.Color.Blue;
            this.label1.TextStyle.ExtBorderColor = System.Drawing.Color.Blue;
            this.label1.TextStyle.GradientColor = System.Drawing.Color.Blue;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridFactoryCMT);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 10);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(752, 276);
            this.panel5.TabIndex = 4;
            // 
            // gridFactoryCMT
            // 
            this.gridFactoryCMT.AllowUserToAddRows = false;
            this.gridFactoryCMT.AllowUserToDeleteRows = false;
            this.gridFactoryCMT.AllowUserToResizeRows = false;
            this.gridFactoryCMT.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridFactoryCMT.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridFactoryCMT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFactoryCMT.DataSource = this.listControlBindingSource1;
            this.gridFactoryCMT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridFactoryCMT.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridFactoryCMT.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridFactoryCMT.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridFactoryCMT.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridFactoryCMT.Location = new System.Drawing.Point(0, 0);
            this.gridFactoryCMT.Name = "gridFactoryCMT";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridFactoryCMT.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridFactoryCMT.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridFactoryCMT.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridFactoryCMT.RowTemplate.Height = 24;
            this.gridFactoryCMT.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFactoryCMT.Size = new System.Drawing.Size(752, 276);
            this.gridFactoryCMT.TabIndex = 0;
            this.gridFactoryCMT.TabStop = false;
            // 
            // P01_FactoryCMT
            // 
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(772, 363);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Name = "P01_FactoryCMT";
            this.Text = "Factory CMT";
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridFactoryCMT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Win.UI.NumericBox numStdFtyCMP;
        private Win.UI.Label label10;
        private Win.UI.NumericBox numLocalPurchase;
        private Win.UI.Label label9;
        private Win.UI.NumericBox numSubProcess;
        private Win.UI.Label label8;
        private Win.UI.NumericBox numCPUCost;
        private Win.UI.Label label7;
        private Win.UI.NumericBox numCPU;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridFactoryCMT;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Button btnClose;
    }
}
