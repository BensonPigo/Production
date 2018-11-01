namespace Sci.Production.Quality
{
    partial class P09
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.tabControl1 = new Sci.Win.UI.TabControl();
            this.tabInspectionReport = new System.Windows.Forms.TabPage();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnDownloadFile = new Sci.Win.UI.Button();
            this.grid1 = new Sci.Win.UI.Grid();
            this.btnQuery = new Sci.Win.UI.Button();
            this.txtpo = new Sci.Win.UI.TextBox();
            this.txtsp = new Sci.Win.UI.TextBox();
            this.txtSeq = new Sci.Production.Class.txtSeq();
            this.dateRange1 = new Sci.Win.UI.DateRange();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.tab1stBulkDyelot = new System.Windows.Forms.TabPage();
            this.btnQuery2 = new Sci.Win.UI.Button();
            this.button1 = new Sci.Win.UI.Button();
            this.btnSave2 = new Sci.Win.UI.Button();
            this.grid2 = new Sci.Win.UI.Grid();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.txtColor = new Sci.Win.UI.TextBox();
            this.txtRefno = new Sci.Win.UI.TextBox();
            this.txtsupplier1 = new Sci.Production.Class.txtsupplier();
            this.label6 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.contextMenuStrip1 = new Sci.Win.UI.ContextMenuStrip();
            this.inspectionReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabInspectionReport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.tab1stBulkDyelot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabInspectionReport);
            this.tabControl1.Controls.Add(this.tab1stBulkDyelot);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1094, 524);
            this.tabControl1.TabIndex = 1;
            // 
            // tabInspectionReport
            // 
            this.tabInspectionReport.Controls.Add(this.btnClose);
            this.tabInspectionReport.Controls.Add(this.btnSave);
            this.tabInspectionReport.Controls.Add(this.btnDownloadFile);
            this.tabInspectionReport.Controls.Add(this.grid1);
            this.tabInspectionReport.Controls.Add(this.btnQuery);
            this.tabInspectionReport.Controls.Add(this.txtpo);
            this.tabInspectionReport.Controls.Add(this.txtsp);
            this.tabInspectionReport.Controls.Add(this.txtSeq);
            this.tabInspectionReport.Controls.Add(this.dateRange1);
            this.tabInspectionReport.Controls.Add(this.label3);
            this.tabInspectionReport.Controls.Add(this.label2);
            this.tabInspectionReport.Controls.Add(this.label1);
            this.tabInspectionReport.Location = new System.Drawing.Point(4, 25);
            this.tabInspectionReport.Name = "tabInspectionReport";
            this.tabInspectionReport.Padding = new System.Windows.Forms.Padding(3);
            this.tabInspectionReport.Size = new System.Drawing.Size(1086, 495);
            this.tabInspectionReport.TabIndex = 0;
            this.tabInspectionReport.Text = "Inspection Report";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(995, 459);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(909, 459);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDownloadFile
            // 
            this.btnDownloadFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownloadFile.Location = new System.Drawing.Point(773, 459);
            this.btnDownloadFile.Name = "btnDownloadFile";
            this.btnDownloadFile.Size = new System.Drawing.Size(130, 30);
            this.btnDownloadFile.TabIndex = 9;
            this.btnDownloadFile.Text = "Download File";
            this.btnDownloadFile.UseVisualStyleBackColor = true;
            this.btnDownloadFile.Click += new System.EventHandler(this.btnDownloadFile_Click);
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(0, 42);
            this.grid1.Name = "grid1";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid1.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(1075, 411);
            this.grid1.TabIndex = 8;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(995, 6);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 7;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // txtpo
            // 
            this.txtpo.BackColor = System.Drawing.Color.White;
            this.txtpo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtpo.Location = new System.Drawing.Point(611, 6);
            this.txtpo.Name = "txtpo";
            this.txtpo.Size = new System.Drawing.Size(100, 23);
            this.txtpo.TabIndex = 6;
            // 
            // txtsp
            // 
            this.txtsp.BackColor = System.Drawing.Color.White;
            this.txtsp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtsp.Location = new System.Drawing.Point(395, 6);
            this.txtsp.Name = "txtsp";
            this.txtsp.Size = new System.Drawing.Size(100, 23);
            this.txtsp.TabIndex = 5;
            // 
            // txtSeq
            // 
            this.txtSeq.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSeq.Location = new System.Drawing.Point(501, 6);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.seq1 = "";
            this.txtSeq.seq2 = "";
            this.txtSeq.Size = new System.Drawing.Size(61, 23);
            this.txtSeq.TabIndex = 4;
            // 
            // dateRange1
            // 
            // 
            // 
            // 
            this.dateRange1.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRange1.DateBox1.Name = "";
            this.dateRange1.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRange1.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRange1.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRange1.DateBox2.Name = "";
            this.dateRange1.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRange1.DateBox2.TabIndex = 1;
            this.dateRange1.Location = new System.Drawing.Point(72, 6);
            this.dateRange1.Name = "dateRange1";
            this.dateRange1.Size = new System.Drawing.Size(280, 23);
            this.dateRange1.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(575, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "PO";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(355, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "SP#";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "ETA";
            // 
            // tab1stBulkDyelot
            // 
            this.tab1stBulkDyelot.Controls.Add(this.btnQuery2);
            this.tab1stBulkDyelot.Controls.Add(this.button1);
            this.tab1stBulkDyelot.Controls.Add(this.btnSave2);
            this.tab1stBulkDyelot.Controls.Add(this.grid2);
            this.tab1stBulkDyelot.Controls.Add(this.txtColor);
            this.tab1stBulkDyelot.Controls.Add(this.txtRefno);
            this.tab1stBulkDyelot.Controls.Add(this.txtsupplier1);
            this.tab1stBulkDyelot.Controls.Add(this.label6);
            this.tab1stBulkDyelot.Controls.Add(this.label5);
            this.tab1stBulkDyelot.Controls.Add(this.label4);
            this.tab1stBulkDyelot.Location = new System.Drawing.Point(4, 25);
            this.tab1stBulkDyelot.Name = "tab1stBulkDyelot";
            this.tab1stBulkDyelot.Padding = new System.Windows.Forms.Padding(3);
            this.tab1stBulkDyelot.Size = new System.Drawing.Size(1086, 495);
            this.tab1stBulkDyelot.TabIndex = 1;
            this.tab1stBulkDyelot.Text = "1st Bulk Dyelot";
            // 
            // btnQuery2
            // 
            this.btnQuery2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery2.Location = new System.Drawing.Point(998, 6);
            this.btnQuery2.Name = "btnQuery2";
            this.btnQuery2.Size = new System.Drawing.Size(80, 30);
            this.btnQuery2.TabIndex = 13;
            this.btnQuery2.Text = "Query";
            this.btnQuery2.UseVisualStyleBackColor = true;
            this.btnQuery2.Click += new System.EventHandler(this.btnQuery2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(998, 459);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 30);
            this.button1.TabIndex = 12;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave2
            // 
            this.btnSave2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave2.Location = new System.Drawing.Point(912, 459);
            this.btnSave2.Name = "btnSave2";
            this.btnSave2.Size = new System.Drawing.Size(80, 30);
            this.btnSave2.TabIndex = 11;
            this.btnSave2.Text = "Save";
            this.btnSave2.UseVisualStyleBackColor = true;
            this.btnSave2.Click += new System.EventHandler(this.btnSave2_Click);
            // 
            // grid2
            // 
            this.grid2.AllowUserToAddRows = false;
            this.grid2.AllowUserToDeleteRows = false;
            this.grid2.AllowUserToResizeRows = false;
            this.grid2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid2.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid2.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid2.DataSource = this.listControlBindingSource2;
            this.grid2.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid2.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid2.Location = new System.Drawing.Point(6, 42);
            this.grid2.Name = "grid2";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid2.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grid2.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid2.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid2.RowTemplate.Height = 24;
            this.grid2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid2.ShowCellToolTips = false;
            this.grid2.Size = new System.Drawing.Size(1072, 411);
            this.grid2.TabIndex = 8;
            // 
            // txtColor
            // 
            this.txtColor.BackColor = System.Drawing.Color.White;
            this.txtColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtColor.Location = new System.Drawing.Point(459, 6);
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(100, 23);
            this.txtColor.TabIndex = 7;
            // 
            // txtRefno
            // 
            this.txtRefno.BackColor = System.Drawing.Color.White;
            this.txtRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefno.Location = new System.Drawing.Point(289, 6);
            this.txtRefno.Name = "txtRefno";
            this.txtRefno.Size = new System.Drawing.Size(100, 23);
            this.txtRefno.TabIndex = 6;
            // 
            // txtsupplier1
            // 
            this.txtsupplier1.DisplayBox1Binding = "";
            this.txtsupplier1.Location = new System.Drawing.Point(72, 6);
            this.txtsupplier1.Name = "txtsupplier1";
            this.txtsupplier1.Size = new System.Drawing.Size(147, 23);
            this.txtsupplier1.TabIndex = 4;
            this.txtsupplier1.TextBox1Binding = "";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(392, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 23);
            this.label6.TabIndex = 3;
            this.label6.Text = "Color";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(222, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 23);
            this.label5.TabIndex = 2;
            this.label5.Text = "Refno";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(5, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 23);
            this.label4.TabIndex = 1;
            this.label4.Text = "Supplier";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inspectionReportToolStripMenuItem,
            this.testReportToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(177, 48);
            // 
            // inspectionReportToolStripMenuItem
            // 
            this.inspectionReportToolStripMenuItem.Name = "inspectionReportToolStripMenuItem";
            this.inspectionReportToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.inspectionReportToolStripMenuItem.Text = "Inspection Report";
            this.inspectionReportToolStripMenuItem.Click += new System.EventHandler(this.inspectionReportToolStripMenuItem_Click);
            // 
            // testReportToolStripMenuItem
            // 
            this.testReportToolStripMenuItem.Name = "testReportToolStripMenuItem";
            this.testReportToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.testReportToolStripMenuItem.Text = "Test Report";
            this.testReportToolStripMenuItem.Click += new System.EventHandler(this.testReportToolStripMenuItem_Click);
            // 
            // P09
            // 
            this.ClientSize = new System.Drawing.Size(1094, 524);
            this.Controls.Add(this.tabControl1);
            this.DefaultControl = "txtSPNo";
            this.Name = "P09";
            this.Text = "P09.Fabric Inspection Document Record";
            this.Controls.SetChildIndex(this.tabControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabInspectionReport.ResumeLayout(false);
            this.tabInspectionReport.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.tab1stBulkDyelot.ResumeLayout(false);
            this.tab1stBulkDyelot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabInspectionReport;
        private Win.UI.DateRange dateRange1;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private System.Windows.Forms.TabPage tab1stBulkDyelot;
        private Win.UI.TextBox txtpo;
        private Win.UI.TextBox txtsp;
        private Class.txtSeq txtSeq;
        private Win.UI.Button btnQuery;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnDownloadFile;
        private Win.UI.Grid grid1;
        private Win.UI.TextBox txtColor;
        private Win.UI.TextBox txtRefno;
        private Class.txtsupplier txtsupplier1;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Grid grid2;
        private Win.UI.Button btnSave2;
        private Win.UI.Button button1;
        private Win.UI.Button btnQuery2;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private Win.UI.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem inspectionReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testReportToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}
