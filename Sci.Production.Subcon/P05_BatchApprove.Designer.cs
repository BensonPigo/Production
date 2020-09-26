namespace Sci.Production.Subcon
{
    partial class P05_BatchApprove
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
            this.gridArtworkReqDetail = new Sci.Win.UI.Grid();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.btnRefresh = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnApprove = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridArtworkReq = new Sci.Win.UI.Grid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkLabelDept = new System.Windows.Forms.LinkLabel();
            this.linkLabelMg = new System.Windows.Forms.LinkLabel();
            this.label1 = new Ict.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridArtworkReqDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridArtworkReq)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridArtworkReqDetail
            // 
            this.gridArtworkReqDetail.AllowUserToAddRows = false;
            this.gridArtworkReqDetail.AllowUserToDeleteRows = false;
            this.gridArtworkReqDetail.AllowUserToResizeRows = false;
            this.gridArtworkReqDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridArtworkReqDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridArtworkReqDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridArtworkReqDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridArtworkReqDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridArtworkReqDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridArtworkReqDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridArtworkReqDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridArtworkReqDetail.Location = new System.Drawing.Point(0, 0);
            this.gridArtworkReqDetail.Name = "gridArtworkReqDetail";
            this.gridArtworkReqDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridArtworkReqDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridArtworkReqDetail.RowTemplate.Height = 24;
            this.gridArtworkReqDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridArtworkReqDetail.ShowCellToolTips = false;
            this.gridArtworkReqDetail.Size = new System.Drawing.Size(932, 269);
            this.gridArtworkReqDetail.TabIndex = 1;
            this.gridArtworkReqDetail.TabStop = false;
            // 
            // btnToExcel
            // 
            this.btnToExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnToExcel.Location = new System.Drawing.Point(12, 577);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(155, 30);
            this.btnToExcel.TabIndex = 2;
            this.btnToExcel.Text = "Lock List To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Location = new System.Drawing.Point(173, 577);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 30);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(840, 577);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnApprove
            // 
            this.btnApprove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApprove.Location = new System.Drawing.Point(749, 576);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(85, 30);
            this.btnApprove.TabIndex = 5;
            this.btnApprove.Text = "Approve";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.BtnApprove_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 27);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridArtworkReq);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridArtworkReqDetail);
            this.splitContainer1.Size = new System.Drawing.Size(932, 543);
            this.splitContainer1.SplitterDistance = 270;
            this.splitContainer1.TabIndex = 6;
            // 
            // gridArtworkReq
            // 
            this.gridArtworkReq.AllowUserToAddRows = false;
            this.gridArtworkReq.AllowUserToDeleteRows = false;
            this.gridArtworkReq.AllowUserToResizeRows = false;
            this.gridArtworkReq.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridArtworkReq.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridArtworkReq.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridArtworkReq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridArtworkReq.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridArtworkReq.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridArtworkReq.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridArtworkReq.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridArtworkReq.Location = new System.Drawing.Point(0, 0);
            this.gridArtworkReq.Name = "gridArtworkReq";
            this.gridArtworkReq.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridArtworkReq.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridArtworkReq.RowTemplate.Height = 24;
            this.gridArtworkReq.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridArtworkReq.ShowCellToolTips = false;
            this.gridArtworkReq.Size = new System.Drawing.Size(932, 270);
            this.gridArtworkReq.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.linkLabelMg);
            this.panel1.Controls.Add(this.linkLabelDept);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(932, 29);
            this.panel1.TabIndex = 7;
            // 
            // linkLabelDept
            // 
            this.linkLabelDept.AutoSize = true;
            this.linkLabelDept.Enabled = false;
            this.linkLabelDept.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelDept.Location = new System.Drawing.Point(1, 5);
            this.linkLabelDept.Name = "linkLabelDept";
            this.linkLabelDept.Size = new System.Drawing.Size(110, 20);
            this.linkLabelDept.TabIndex = 0;
            this.linkLabelDept.TabStop = true;
            this.linkLabelDept.Text = "Dept. Mgr Apv";
            this.linkLabelDept.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelDept_LinkClicked);
            // 
            // linkLabelMg
            // 
            this.linkLabelMg.AutoSize = true;
            this.linkLabelMg.Enabled = false;
            this.linkLabelMg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelMg.Location = new System.Drawing.Point(120, 5);
            this.linkLabelMg.Name = "linkLabelMg";
            this.linkLabelMg.Size = new System.Drawing.Size(93, 20);
            this.linkLabelMg.TabIndex = 1;
            this.linkLabelMg.TabStop = true;
            this.linkLabelMg.Text = "Mg Mgr Apv";
            this.linkLabelMg.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelMg_LinkClicked);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(109, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "|";
            // 
            // P05_BatchApprove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(932, 611);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnApprove);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnToExcel);
            this.Controls.Add(this.splitContainer1);
            this.Name = "P05_BatchApprove";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Subcon P05 Batch Approve";
            ((System.ComponentModel.ISupportInitialize)(this.gridArtworkReqDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridArtworkReq)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Win.UI.Grid gridArtworkReqDetail;
        private Win.UI.Button btnToExcel;
        private Win.UI.Button btnRefresh;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnApprove;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Grid gridArtworkReq;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel linkLabelMg;
        private System.Windows.Forms.LinkLabel linkLabelDept;
        private Ict.Win.UI.Label label1;
    }
}