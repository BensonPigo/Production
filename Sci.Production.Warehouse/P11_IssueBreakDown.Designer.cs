﻿namespace Sci.Production.Warehouse
{
    partial class P11_IssueBreakDown
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridQtyBreakDown = new Sci.Win.UI.Grid();
            this.gridIssueBreakDown = new Sci.Win.UI.Grid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.gridIssueBreakDownBS = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.gridQtyBreakDownBS = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridQtyBreakDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridIssueBreakDown)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridIssueBreakDownBS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridQtyBreakDownBS)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridQtyBreakDown);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridIssueBreakDown);
            this.splitContainer1.Size = new System.Drawing.Size(784, 505);
            this.splitContainer1.SplitterDistance = 234;
            this.splitContainer1.TabIndex = 0;
            // 
            // gridQtyBreakDown
            // 
            this.gridQtyBreakDown.AllowUserToAddRows = false;
            this.gridQtyBreakDown.AllowUserToDeleteRows = false;
            this.gridQtyBreakDown.AllowUserToResizeRows = false;
            this.gridQtyBreakDown.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridQtyBreakDown.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridQtyBreakDown.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridQtyBreakDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridQtyBreakDown.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridQtyBreakDown.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridQtyBreakDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridQtyBreakDown.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridQtyBreakDown.Location = new System.Drawing.Point(0, 0);
            this.gridQtyBreakDown.Name = "gridQtyBreakDown";
            this.gridQtyBreakDown.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridQtyBreakDown.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridQtyBreakDown.RowTemplate.Height = 24;
            this.gridQtyBreakDown.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridQtyBreakDown.Size = new System.Drawing.Size(784, 234);
            this.gridQtyBreakDown.TabIndex = 0;
            this.gridQtyBreakDown.TabStop = false;
            // 
            // gridIssueBreakDown
            // 
            this.gridIssueBreakDown.AllowUserToAddRows = false;
            this.gridIssueBreakDown.AllowUserToDeleteRows = false;
            this.gridIssueBreakDown.AllowUserToResizeRows = false;
            this.gridIssueBreakDown.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridIssueBreakDown.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridIssueBreakDown.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridIssueBreakDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridIssueBreakDown.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridIssueBreakDown.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridIssueBreakDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridIssueBreakDown.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridIssueBreakDown.Location = new System.Drawing.Point(0, 0);
            this.gridIssueBreakDown.Name = "gridIssueBreakDown";
            this.gridIssueBreakDown.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridIssueBreakDown.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridIssueBreakDown.RowTemplate.Height = 24;
            this.gridIssueBreakDown.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridIssueBreakDown.Size = new System.Drawing.Size(784, 267);
            this.gridIssueBreakDown.TabIndex = 0;
            this.gridIssueBreakDown.TabStop = false;
            this.gridIssueBreakDown.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridIssueBreakDown_CellValidated);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 511);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 50);
            this.panel1.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(692, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(606, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // P11_IssueBreakDown
            // 
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitContainer1);
            this.Name = "P11_IssueBreakDown";
            this.Text = "P11 Issue Qty Break Down";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridQtyBreakDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridIssueBreakDown)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridIssueBreakDownBS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridQtyBreakDownBS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Grid gridQtyBreakDown;
        private Win.UI.Grid gridIssueBreakDown;
        private System.Windows.Forms.Panel panel1;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnSave;
        private Win.UI.ListControlBindingSource gridIssueBreakDownBS;
        private Win.UI.ListControlBindingSource gridQtyBreakDownBS;

    }
}
