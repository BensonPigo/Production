﻿namespace Sci.Production.Shipping
{
    partial class P04_ImportTransferIn
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
            this.txtScifactoryToFactory = new Sci.Production.Class.txtscifactory();
            this.labelToFactory = new Sci.Win.UI.Label();
            this.txtScifactoryFromFactory = new Sci.Production.Class.txtscifactory();
            this.btnQuery = new Sci.Win.UI.Button();
            this.labelFromFactory = new Sci.Win.UI.Label();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridImport = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 506);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(725, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 506);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtScifactoryToFactory);
            this.panel3.Controls.Add(this.labelToFactory);
            this.panel3.Controls.Add(this.txtScifactoryFromFactory);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.labelFromFactory);
            this.panel3.Controls.Add(this.txtSPNo);
            this.panel3.Controls.Add(this.labelSPNo);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(720, 45);
            this.panel3.TabIndex = 2;
            // 
            // txtScifactoryToFactory
            // 
            this.txtScifactoryToFactory.BackColor = System.Drawing.Color.White;
            this.txtScifactoryToFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtScifactoryToFactory.Location = new System.Drawing.Point(468, 11);
            this.txtScifactoryToFactory.Name = "txtScifactoryToFactory";
            this.txtScifactoryToFactory.Size = new System.Drawing.Size(66, 23);
            this.txtScifactoryToFactory.TabIndex = 2;
            // 
            // labelToFactory
            // 
            this.labelToFactory.Lines = 0;
            this.labelToFactory.Location = new System.Drawing.Point(394, 11);
            this.labelToFactory.Name = "labelToFactory";
            this.labelToFactory.Size = new System.Drawing.Size(70, 23);
            this.labelToFactory.TabIndex = 6;
            this.labelToFactory.Text = "To Factory";
            // 
            // txtScifactoryFromFactory
            // 
            this.txtScifactoryFromFactory.BackColor = System.Drawing.Color.White;
            this.txtScifactoryFromFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtScifactoryFromFactory.Location = new System.Drawing.Point(292, 11);
            this.txtScifactoryFromFactory.Name = "txtScifactoryFromFactory";
            this.txtScifactoryFromFactory.Size = new System.Drawing.Size(66, 23);
            this.txtScifactoryFromFactory.TabIndex = 1;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(620, 7);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 3;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // labelFromFactory
            // 
            this.labelFromFactory.Lines = 0;
            this.labelFromFactory.Location = new System.Drawing.Point(201, 11);
            this.labelFromFactory.Name = "labelFromFactory";
            this.labelFromFactory.Size = new System.Drawing.Size(87, 23);
            this.labelFromFactory.TabIndex = 2;
            this.labelFromFactory.Text = "From Factory";
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(45, 11);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(120, 23);
            this.txtSPNo.TabIndex = 0;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Lines = 0;
            this.labelSPNo.Location = new System.Drawing.Point(4, 11);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(37, 23);
            this.labelSPNo.TabIndex = 0;
            this.labelSPNo.Text = "SP#";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Controls.Add(this.btnImport);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 462);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(720, 44);
            this.panel4.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(620, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(534, 7);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(80, 30);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridImport);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 45);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(720, 417);
            this.panel5.TabIndex = 4;
            // 
            // gridImport
            // 
            this.gridImport.AllowUserToAddRows = false;
            this.gridImport.AllowUserToDeleteRows = false;
            this.gridImport.AllowUserToResizeRows = false;
            this.gridImport.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridImport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.gridImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridImport.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridImport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridImport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridImport.Location = new System.Drawing.Point(0, 0);
            this.gridImport.Name = "gridImport";
            this.gridImport.RowHeadersVisible = false;
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridImport.RowTemplate.Height = 24;
            this.gridImport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridImport.Size = new System.Drawing.Size(720, 417);
            this.gridImport.TabIndex = 0;
            this.gridImport.TabStop = false;
            // 
            // P04_ImportTransferIn
            // 
            this.AcceptButton = this.btnImport;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(730, 506);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Name = "P04_ImportTransferIn";
            this.Text = "Import Data - Transfer In";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Button btnQuery;
        private Win.UI.Label labelFromFactory;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.Label labelSPNo;
        private Win.UI.Panel panel4;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnImport;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridImport;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Class.txtscifactory txtScifactoryToFactory;
        private Win.UI.Label labelToFactory;
        private Class.txtscifactory txtScifactoryFromFactory;
    }
}
