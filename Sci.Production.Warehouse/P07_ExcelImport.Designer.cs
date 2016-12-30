﻿namespace Sci.Production.Warehouse
{
    partial class P07_ExcelImport
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnWriteIn = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.panel7 = new Sci.Win.UI.Panel();
            this.grid2 = new Sci.Win.UI.Grid();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel6 = new Sci.Win.UI.Panel();
            this.panel10 = new Sci.Win.UI.Panel();
            this.panel12 = new Sci.Win.UI.Panel();
            this.grid1 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel11 = new Sci.Win.UI.Panel();
            this.panel9 = new Sci.Win.UI.Panel();
            this.btnCheckImport = new Sci.Win.UI.Button();
            this.btnRemoveExcel = new Sci.Win.UI.Button();
            this.btnAddExcel = new Sci.Win.UI.Button();
            this.panel8 = new Sci.Win.UI.Panel();
            this.label1 = new Sci.Win.UI.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel9.SuspendLayout();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 528);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(824, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 528);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(814, 10);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Controls.Add(this.btnWriteIn);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(10, 486);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(814, 42);
            this.panel4.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(729, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnWriteIn
            // 
            this.btnWriteIn.Location = new System.Drawing.Point(639, 6);
            this.btnWriteIn.Name = "btnWriteIn";
            this.btnWriteIn.Size = new System.Drawing.Size(80, 30);
            this.btnWriteIn.TabIndex = 0;
            this.btnWriteIn.Text = "Write In";
            this.btnWriteIn.UseVisualStyleBackColor = true;
            this.btnWriteIn.Click += new System.EventHandler(this.button4_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 10);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(814, 476);
            this.panel5.TabIndex = 4;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.grid2);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 144);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(814, 332);
            this.panel7.TabIndex = 1;
            // 
            // grid2
            // 
            this.grid2.AllowUserToAddRows = false;
            this.grid2.AllowUserToDeleteRows = false;
            this.grid2.AllowUserToResizeRows = false;
            this.grid2.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid2.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid2.DataSource = this.listControlBindingSource2;
            this.grid2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid2.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid2.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid2.Location = new System.Drawing.Point(0, 0);
            this.grid2.Name = "grid2";
            this.grid2.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid2.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid2.RowTemplate.Height = 24;
            this.grid2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid2.Size = new System.Drawing.Size(814, 332);
            this.grid2.TabIndex = 0;
            this.grid2.TabStop = false;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.panel10);
            this.panel6.Controls.Add(this.panel9);
            this.panel6.Controls.Add(this.panel8);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(814, 144);
            this.panel6.TabIndex = 0;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.panel12);
            this.panel10.Controls.Add(this.panel11);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(68, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(574, 144);
            this.panel10.TabIndex = 2;
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.grid1);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel12.Location = new System.Drawing.Point(0, 0);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(574, 134);
            this.panel12.TabIndex = 1;
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(0, 0);
            this.grid1.Name = "grid1";
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(574, 134);
            this.grid1.TabIndex = 0;
            this.grid1.TabStop = false;
            // 
            // panel11
            // 
            this.panel11.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel11.Location = new System.Drawing.Point(0, 134);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(574, 10);
            this.panel11.TabIndex = 0;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.btnCheckImport);
            this.panel9.Controls.Add(this.btnRemoveExcel);
            this.panel9.Controls.Add(this.btnAddExcel);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel9.Location = new System.Drawing.Point(642, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(172, 144);
            this.panel9.TabIndex = 1;
            // 
            // btnCheckImport
            // 
            this.btnCheckImport.Location = new System.Drawing.Point(23, 106);
            this.btnCheckImport.Name = "btnCheckImport";
            this.btnCheckImport.Size = new System.Drawing.Size(146, 30);
            this.btnCheckImport.TabIndex = 2;
            this.btnCheckImport.Text = "Check && Import";
            this.btnCheckImport.UseVisualStyleBackColor = true;
            this.btnCheckImport.Click += new System.EventHandler(this.btnCheckImport_Click);
            // 
            // btnRemoveExcel
            // 
            this.btnRemoveExcel.Location = new System.Drawing.Point(23, 44);
            this.btnRemoveExcel.Name = "btnRemoveExcel";
            this.btnRemoveExcel.Size = new System.Drawing.Size(146, 30);
            this.btnRemoveExcel.TabIndex = 1;
            this.btnRemoveExcel.Text = "Remove Excel";
            this.btnRemoveExcel.UseVisualStyleBackColor = true;
            this.btnRemoveExcel.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnAddExcel
            // 
            this.btnAddExcel.Location = new System.Drawing.Point(23, 7);
            this.btnAddExcel.Name = "btnAddExcel";
            this.btnAddExcel.Size = new System.Drawing.Size(146, 30);
            this.btnAddExcel.TabIndex = 0;
            this.btnAddExcel.Text = "Add Excel";
            this.btnAddExcel.UseVisualStyleBackColor = true;
            this.btnAddExcel.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.label1);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(68, 144);
            this.panel8.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Attach file";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // P07_ExcelImport
            // 
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(834, 528);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Name = "P07_ExcelImport";
            this.Text = "P07. Excel Import";
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel9.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel5;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnWriteIn;
        private Win.UI.Panel panel7;
        private Win.UI.Grid grid2;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private Win.UI.Panel panel6;
        private Win.UI.Panel panel10;
        private Win.UI.Grid grid1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel9;
        private Win.UI.Button btnCheckImport;
        private Win.UI.Button btnRemoveExcel;
        private Win.UI.Button btnAddExcel;
        private Win.UI.Panel panel8;
        private Win.UI.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Win.UI.Panel panel12;
        private Win.UI.Panel panel11;
    }
}
