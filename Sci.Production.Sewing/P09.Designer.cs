namespace Sci.Production.Sewing
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
            this.lbScanDate = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.txtPackID = new Sci.Win.UI.TextBox();
            this.txtsp = new Sci.Win.UI.TextBox();
            this.btnQuery = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.dateTransfer = new Sci.Win.UI.DateRange();
            this.lbPackIDmsg = new System.Windows.Forms.Label();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel1 = new Sci.Win.UI.Panel();
            this.labelmetal = new Sci.Win.UI.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridMain = new Sci.Win.UI.Grid();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label1 = new Sci.Win.UI.Label();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.btnExcel = new Sci.Win.UI.Button();
            this.label4 = new Sci.Win.UI.Label();
            this.txtMdivision1 = new Sci.Production.Class.TxtMdivision();
            this.label5 = new Sci.Win.UI.Label();
            this.txtfactory1 = new Sci.Production.Class.Txtfactory();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // lbScanDate
            // 
            this.lbScanDate.Location = new System.Drawing.Point(9, 14);
            this.lbScanDate.Name = "lbScanDate";
            this.lbScanDate.Size = new System.Drawing.Size(93, 23);
            this.lbScanDate.TabIndex = 7;
            this.lbScanDate.Text = "Scan Date";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 23);
            this.label2.TabIndex = 10;
            this.label2.Text = "Pack ID";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(390, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 11;
            this.label3.Text = "SP#";
            // 
            // txtPackID
            // 
            this.txtPackID.BackColor = System.Drawing.Color.White;
            this.txtPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackID.IsSupportEditMode = false;
            this.txtPackID.Location = new System.Drawing.Point(105, 51);
            this.txtPackID.Name = "txtPackID";
            this.txtPackID.Size = new System.Drawing.Size(163, 23);
            this.txtPackID.TabIndex = 3;
            // 
            // txtsp
            // 
            this.txtsp.BackColor = System.Drawing.Color.White;
            this.txtsp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtsp.IsSupportEditMode = false;
            this.txtsp.Location = new System.Drawing.Point(468, 51);
            this.txtsp.Name = "txtsp";
            this.txtsp.Size = new System.Drawing.Size(166, 23);
            this.txtsp.TabIndex = 4;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(656, 14);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 5;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // dateTransfer
            // 
            // 
            // 
            // 
            this.dateTransfer.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateTransfer.DateBox1.Name = "";
            this.dateTransfer.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateTransfer.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateTransfer.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateTransfer.DateBox2.Name = "";
            this.dateTransfer.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateTransfer.DateBox2.TabIndex = 1;
            this.dateTransfer.IsSupportEditMode = false;
            this.dateTransfer.Location = new System.Drawing.Point(107, 14);
            this.dateTransfer.Name = "dateTransfer";
            this.dateTransfer.Size = new System.Drawing.Size(280, 23);
            this.dateTransfer.TabIndex = 0;
            // 
            // lbPackIDmsg
            // 
            this.lbPackIDmsg.AutoSize = true;
            this.lbPackIDmsg.ForeColor = System.Drawing.Color.Red;
            this.lbPackIDmsg.Location = new System.Drawing.Point(12, 79);
            this.lbPackIDmsg.Name = "lbPackIDmsg";
            this.lbPackIDmsg.Size = new System.Drawing.Size(438, 17);
            this.lbPackIDmsg.TabIndex = 12;
            this.lbPackIDmsg.Text = "The Qty here is by pieces instead of complete set on below function.";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtfactory1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtMdivision1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnExcel);
            this.panel1.Controls.Add(this.labelmetal);
            this.panel1.Controls.Add(this.lbScanDate);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lbPackIDmsg);
            this.panel1.Controls.Add(this.txtPackID);
            this.panel1.Controls.Add(this.dateTransfer);
            this.panel1.Controls.Add(this.txtsp);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(834, 135);
            this.panel1.TabIndex = 12;
            // 
            // labelmetal
            // 
            this.labelmetal.Location = new System.Drawing.Point(9, 99);
            this.labelmetal.Name = "labelmetal";
            this.labelmetal.Size = new System.Drawing.Size(185, 23);
            this.labelmetal.TabIndex = 13;
            this.labelmetal.Text = "Metal Detection Record";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 135);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridMain);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(834, 393);
            this.splitContainer1.SplitterDistance = 206;
            this.splitContainer1.TabIndex = 13;
            // 
            // gridMain
            // 
            this.gridMain.AllowUserToAddRows = false;
            this.gridMain.AllowUserToDeleteRows = false;
            this.gridMain.AllowUserToResizeRows = false;
            this.gridMain.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridMain.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMain.DataSource = this.listControlBindingSource1;
            this.gridMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMain.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridMain.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridMain.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridMain.Location = new System.Drawing.Point(0, 0);
            this.gridMain.Name = "gridMain";
            this.gridMain.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridMain.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridMain.RowTemplate.Height = 24;
            this.gridMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridMain.ShowCellToolTips = false;
            this.gridMain.Size = new System.Drawing.Size(834, 206);
            this.gridMain.TabIndex = 0;
            this.gridMain.TabStop = false;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.gridDetail);
            this.splitContainer2.Size = new System.Drawing.Size(834, 183);
            this.splitContainer2.SplitterDistance = 33;
            this.splitContainer2.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "Discrepancy Record";
            // 
            // gridDetail
            // 
            this.gridDetail.AllowUserToAddRows = false;
            this.gridDetail.AllowUserToDeleteRows = false;
            this.gridDetail.AllowUserToResizeRows = false;
            this.gridDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetail.DataSource = this.listControlBindingSource2;
            this.gridDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDetail.Location = new System.Drawing.Point(0, 0);
            this.gridDetail.Name = "gridDetail";
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.ShowCellToolTips = false;
            this.gridDetail.Size = new System.Drawing.Size(834, 146);
            this.gridDetail.TabIndex = 0;
            this.gridDetail.TabStop = false;
            // 
            // btnExcel
            // 
            this.btnExcel.Location = new System.Drawing.Point(740, 14);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(80, 30);
            this.btnExcel.TabIndex = 6;
            this.btnExcel.Text = "Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(390, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 23);
            this.label4.TabIndex = 8;
            this.label4.Text = "M";
            // 
            // txtMdivision1
            // 
            this.txtMdivision1.BackColor = System.Drawing.Color.White;
            this.txtMdivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision1.Location = new System.Drawing.Point(439, 14);
            this.txtMdivision1.Name = "txtMdivision1";
            this.txtMdivision1.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision1.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(508, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 23);
            this.label5.TabIndex = 9;
            this.label5.Text = "Factory";
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.BoolFtyGroupList = true;
            this.txtfactory1.FilteMDivision = false;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.IsProduceFty = false;
            this.txtfactory1.IssupportJunk = false;
            this.txtfactory1.Location = new System.Drawing.Point(568, 14);
            this.txtfactory1.MDivision = null;
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.Size = new System.Drawing.Size(66, 23);
            this.txtfactory1.TabIndex = 2;
            // 
            // P09
            // 
            this.ClientSize = new System.Drawing.Size(834, 528);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Name = "P09";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P09.Query for MD Room Scan";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.splitContainer1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridMain)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label lbScanDate;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.TextBox txtPackID;
        private Win.UI.TextBox txtsp;
        private Win.UI.Button btnQuery;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.DateRange dateTransfer;
        private System.Windows.Forms.Label lbPackIDmsg;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private Win.UI.Panel panel1;
        private Win.UI.Label labelmetal;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Grid gridMain;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Win.UI.Label label1;
        private Win.UI.Grid gridDetail;
        private Class.Txtfactory txtfactory1;
        private Win.UI.Label label5;
        private Class.TxtMdivision txtMdivision1;
        private Win.UI.Label label4;
        private Win.UI.Button btnExcel;
    }
}
