namespace Sci.Production.Planning
{
    partial class P08
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
            this.panel3 = new Sci.Win.UI.Panel();
            this.btnExcelImport = new Sci.Win.UI.Button();
            this.btnDownloadTemplate = new Sci.Win.UI.Button();
            this.dateSewingOffline = new Sci.Win.UI.DateRange();
            this.label5 = new Sci.Win.UI.Label();
            this.dateSewingInline = new Sci.Win.UI.DateRange();
            this.label4 = new Sci.Win.UI.Label();
            this.dateSewing = new Sci.Win.UI.DateRange();
            this.txtfactory1 = new Sci.Production.Class.Txtfactory();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.txtMdivision1 = new Sci.Production.Class.TxtMdivision();
            this.label1 = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.grid1 = new Sci.Win.UI.Grid();
            this.btnEdit = new Sci.Win.UI.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnExcelImport);
            this.panel3.Controls.Add(this.btnDownloadTemplate);
            this.panel3.Controls.Add(this.dateSewingOffline);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.dateSewingInline);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.dateSewing);
            this.panel3.Controls.Add(this.txtfactory1);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.txtMdivision1);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1008, 121);
            this.panel3.TabIndex = 20;
            // 
            // btnExcelImport
            // 
            this.btnExcelImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcelImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnExcelImport.Location = new System.Drawing.Point(884, 84);
            this.btnExcelImport.Name = "btnExcelImport";
            this.btnExcelImport.Size = new System.Drawing.Size(112, 30);
            this.btnExcelImport.TabIndex = 7;
            this.btnExcelImport.Text = "Excel Import";
            this.btnExcelImport.UseVisualStyleBackColor = true;
            this.btnExcelImport.Click += new System.EventHandler(this.BtnExcelImport_Click);
            // 
            // btnDownloadTemplate
            // 
            this.btnDownloadTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownloadTemplate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnDownloadTemplate.Location = new System.Drawing.Point(835, 48);
            this.btnDownloadTemplate.Name = "btnDownloadTemplate";
            this.btnDownloadTemplate.Size = new System.Drawing.Size(161, 30);
            this.btnDownloadTemplate.TabIndex = 6;
            this.btnDownloadTemplate.Text = "Download Template";
            this.btnDownloadTemplate.UseVisualStyleBackColor = true;
            this.btnDownloadTemplate.Click += new System.EventHandler(this.BtnDownloadTemplate_Click);
            // 
            // dateSewingOffline
            // 
            // 
            // 
            // 
            this.dateSewingOffline.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSewingOffline.DateBox1.Name = "";
            this.dateSewingOffline.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSewingOffline.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSewingOffline.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSewingOffline.DateBox2.Name = "";
            this.dateSewingOffline.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSewingOffline.DateBox2.TabIndex = 1;
            this.dateSewingOffline.Location = new System.Drawing.Point(480, 41);
            this.dateSewingOffline.Name = "dateSewingOffline";
            this.dateSewingOffline.Size = new System.Drawing.Size(280, 23);
            this.dateSewingOffline.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(383, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 23);
            this.label5.TabIndex = 63;
            this.label5.Text = "Sewing Offline";
            // 
            // dateSewingInline
            // 
            // 
            // 
            // 
            this.dateSewingInline.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSewingInline.DateBox1.Name = "";
            this.dateSewingInline.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSewingInline.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSewingInline.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSewingInline.DateBox2.Name = "";
            this.dateSewingInline.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSewingInline.DateBox2.TabIndex = 1;
            this.dateSewingInline.Location = new System.Drawing.Point(480, 12);
            this.dateSewingInline.Name = "dateSewingInline";
            this.dateSewingInline.Size = new System.Drawing.Size(280, 23);
            this.dateSewingInline.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(383, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 23);
            this.label4.TabIndex = 61;
            this.label4.Text = "Sewing Inline";
            // 
            // dateSewing
            // 
            // 
            // 
            // 
            this.dateSewing.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSewing.DateBox1.Name = "";
            this.dateSewing.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSewing.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSewing.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSewing.DateBox2.Name = "";
            this.dateSewing.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSewing.DateBox2.TabIndex = 1;
            this.dateSewing.Location = new System.Drawing.Point(100, 41);
            this.dateSewing.Name = "dateSewing";
            this.dateSewing.Size = new System.Drawing.Size(280, 23);
            this.dateSewing.TabIndex = 2;
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.BoolFtyGroupList = true;
            this.txtfactory1.FilteMDivision = false;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.IsIE = false;
            this.txtfactory1.IsMultiselect = false;
            this.txtfactory1.IsProduceFty = false;
            this.txtfactory1.IssupportJunk = false;
            this.txtfactory1.Location = new System.Drawing.Point(251, 12);
            this.txtfactory1.MDivision = null;
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.NeedInitialFactory = true;
            this.txtfactory1.Size = new System.Drawing.Size(66, 23);
            this.txtfactory1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(182, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 23);
            this.label3.TabIndex = 58;
            this.label3.Text = "Factory";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 23);
            this.label2.TabIndex = 57;
            this.label2.Text = "Sewing Date";
            // 
            // txtMdivision1
            // 
            this.txtMdivision1.BackColor = System.Drawing.Color.White;
            this.txtMdivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision1.Location = new System.Drawing.Point(100, 12);
            this.txtMdivision1.Name = "txtMdivision1";
            this.txtMdivision1.NeedInitialMdivision = true;
            this.txtMdivision1.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 23);
            this.label1.TabIndex = 55;
            this.label1.Text = "MDivision";
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnQuery.Location = new System.Drawing.Point(899, 12);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(97, 30);
            this.btnQuery.TabIndex = 5;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSave.Location = new System.Drawing.Point(899, 619);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(97, 30);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
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
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(0, 121);
            this.grid1.Name = "grid1";
            this.grid1.RowHeadersVisible = false;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(1008, 492);
            this.grid1.TabIndex = 8;
            this.grid1.TabStop = false;
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnEdit.Location = new System.Drawing.Point(796, 619);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(97, 30);
            this.btnEdit.TabIndex = 21;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // P08
            // 
            this.ClientSize = new System.Drawing.Size(1008, 661);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.btnSave);
            this.EditMode = true;
            this.Name = "P08";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P08. Daily Output Status Record";
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.grid1, 0);
            this.Controls.SetChildIndex(this.btnEdit, 0);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel3;
        private Win.UI.Button btnQuery;
        private Win.UI.Button btnSave;
        private Win.UI.Grid grid1;
        private Win.UI.DateRange dateSewingOffline;
        private Win.UI.Label label5;
        private Win.UI.DateRange dateSewingInline;
        private Win.UI.Label label4;
        private Win.UI.DateRange dateSewing;
        private Class.Txtfactory txtfactory1;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Class.TxtMdivision txtMdivision1;
        private Win.UI.Label label1;
        private Win.UI.Button btnExcelImport;
        private Win.UI.Button btnDownloadTemplate;
        private Win.UI.Button btnEdit;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}
