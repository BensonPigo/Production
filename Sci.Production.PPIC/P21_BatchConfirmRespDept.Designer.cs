namespace Sci.Production.PPIC
{
    partial class P21_BatchConfirmRespDept
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblConfirmDate = new Sci.Win.UI.Label();
            this.dateRangeConfirm = new Sci.Win.UI.DateRange();
            this.btnQuery = new Sci.Win.UI.Button();
            this.gridICR = new Sci.Win.UI.Grid();
            this.gridICR_ResponsibilityDept = new Sci.Win.UI.Grid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnReject = new Sci.Win.UI.Button();
            this.btnConfirm = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridICR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridICR_ResponsibilityDept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblConfirmDate
            // 
            this.lblConfirmDate.Location = new System.Drawing.Point(9, 9);
            this.lblConfirmDate.Name = "lblConfirmDate";
            this.lblConfirmDate.Size = new System.Drawing.Size(91, 23);
            this.lblConfirmDate.TabIndex = 0;
            this.lblConfirmDate.Text = "Confirm Date";
            // 
            // dateRangeConfirm
            // 
            // 
            // 
            // 
            this.dateRangeConfirm.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeConfirm.DateBox1.Name = "";
            this.dateRangeConfirm.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeConfirm.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeConfirm.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeConfirm.DateBox2.Name = "";
            this.dateRangeConfirm.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeConfirm.DateBox2.TabIndex = 1;
            this.dateRangeConfirm.IsRequired = false;
            this.dateRangeConfirm.Location = new System.Drawing.Point(103, 9);
            this.dateRangeConfirm.Name = "dateRangeConfirm";
            this.dateRangeConfirm.Size = new System.Drawing.Size(280, 23);
            this.dateRangeConfirm.TabIndex = 1;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(824, 5);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 2;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // gridICR
            // 
            this.gridICR.AllowUserToAddRows = false;
            this.gridICR.AllowUserToDeleteRows = false;
            this.gridICR.AllowUserToResizeRows = false;
            this.gridICR.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridICR.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridICR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridICR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridICR.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridICR.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridICR.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridICR.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridICR.Location = new System.Drawing.Point(0, 0);
            this.gridICR.MultiSelect = false;
            this.gridICR.Name = "gridICR";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridICR.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridICR.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridICR.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridICR.RowTemplate.Height = 24;
            this.gridICR.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridICR.ShowCellToolTips = false;
            this.gridICR.Size = new System.Drawing.Size(416, 410);
            this.gridICR.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridICR.TabIndex = 3;
            this.gridICR.SelectionChanged += new System.EventHandler(this.GridICR_SelectionChanged);
            // 
            // gridICR_ResponsibilityDept
            // 
            this.gridICR_ResponsibilityDept.AllowUserToAddRows = false;
            this.gridICR_ResponsibilityDept.AllowUserToDeleteRows = false;
            this.gridICR_ResponsibilityDept.AllowUserToResizeRows = false;
            this.gridICR_ResponsibilityDept.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridICR_ResponsibilityDept.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridICR_ResponsibilityDept.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridICR_ResponsibilityDept.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridICR_ResponsibilityDept.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridICR_ResponsibilityDept.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridICR_ResponsibilityDept.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridICR_ResponsibilityDept.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridICR_ResponsibilityDept.Location = new System.Drawing.Point(0, 0);
            this.gridICR_ResponsibilityDept.Name = "gridICR_ResponsibilityDept";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridICR_ResponsibilityDept.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridICR_ResponsibilityDept.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridICR_ResponsibilityDept.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridICR_ResponsibilityDept.RowTemplate.Height = 24;
            this.gridICR_ResponsibilityDept.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridICR_ResponsibilityDept.ShowCellToolTips = false;
            this.gridICR_ResponsibilityDept.Size = new System.Drawing.Size(477, 410);
            this.gridICR_ResponsibilityDept.TabIndex = 4;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(9, 38);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridICR);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridICR_ResponsibilityDept);
            this.splitContainer1.Size = new System.Drawing.Size(895, 410);
            this.splitContainer1.SplitterDistance = 416;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 5;
            // 
            // btnReject
            // 
            this.btnReject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReject.Location = new System.Drawing.Point(652, 454);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(80, 30);
            this.btnReject.TabIndex = 6;
            this.btnReject.Text = "Reject";
            this.btnReject.UseVisualStyleBackColor = true;
            this.btnReject.Click += new System.EventHandler(this.BtnReject_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Location = new System.Drawing.Point(738, 454);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(80, 30);
            this.btnConfirm.TabIndex = 7;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(824, 454);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // P21_BatchConfirmRespDept
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 494);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnReject);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.dateRangeConfirm);
            this.Controls.Add(this.lblConfirmDate);
            this.Name = "P21_BatchConfirmRespDept";
            this.OnLineHelpID = "Sci.Win.Forms.Base";
            this.Text = "P21. Batch Confirm Responsibility Dept.";
            ((System.ComponentModel.ISupportInitialize)(this.gridICR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridICR_ResponsibilityDept)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label lblConfirmDate;
        private Win.UI.DateRange dateRangeConfirm;
        private Win.UI.Button btnQuery;
        private Win.UI.Grid gridICR;
        private Win.UI.Grid gridICR_ResponsibilityDept;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Button btnReject;
        private Win.UI.Button btnConfirm;
        private Win.UI.Button btnClose;
    }
}