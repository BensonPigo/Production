namespace Sci.Production.Planning
{
    partial class P01_BatchApprove
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
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridBatchApprove = new Sci.Win.UI.Grid();
            this.labelFactory = new Sci.Win.UI.Label();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.labelArtworkType = new Sci.Win.UI.Label();
            this.txtartworktype_ftyArtworkType = new Sci.Production.Class.Txtartworktype_fty();
            this.checkOnlyAprrovedData = new Sci.Win.UI.CheckBox();
            this.labelApproveDate = new Sci.Win.UI.Label();
            this.dateApproveDate = new Sci.Win.UI.DateRange();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.labelSewingInlineDate = new Sci.Win.UI.Label();
            this.labelSubprocessInline = new Sci.Win.UI.Label();
            this.dateRangeSewInLine = new Sci.Win.UI.DateRange();
            this.dateSubprocessInline = new Sci.Win.UI.DateRange();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.btnQuery = new Sci.Win.UI.Button();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnApprove = new Sci.Win.UI.Button();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.btnUnApprove = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchApprove)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridBatchApprove);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 147);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 361);
            this.panel1.TabIndex = 23;
            // 
            // gridBatchApprove
            // 
            this.gridBatchApprove.AllowUserToAddRows = false;
            this.gridBatchApprove.AllowUserToDeleteRows = false;
            this.gridBatchApprove.AllowUserToResizeRows = false;
            this.gridBatchApprove.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBatchApprove.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridBatchApprove.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBatchApprove.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBatchApprove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridBatchApprove.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBatchApprove.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBatchApprove.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBatchApprove.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBatchApprove.Location = new System.Drawing.Point(0, 0);
            this.gridBatchApprove.Name = "gridBatchApprove";
            this.gridBatchApprove.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridBatchApprove.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridBatchApprove.RowTemplate.Height = 24;
            this.gridBatchApprove.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBatchApprove.Size = new System.Drawing.Size(1008, 361);
            this.gridBatchApprove.TabIndex = 0;
            this.gridBatchApprove.TabStop = false;
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(462, 82);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(125, 23);
            this.labelFactory.TabIndex = 19;
            this.labelFactory.Text = "Factory";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtfactory);
            this.groupBox1.Controls.Add(this.labelArtworkType);
            this.groupBox1.Controls.Add(this.txtartworktype_ftyArtworkType);
            this.groupBox1.Controls.Add(this.checkOnlyAprrovedData);
            this.groupBox1.Controls.Add(this.labelFactory);
            this.groupBox1.Controls.Add(this.labelApproveDate);
            this.groupBox1.Controls.Add(this.dateApproveDate);
            this.groupBox1.Controls.Add(this.labelSCIDelivery);
            this.groupBox1.Controls.Add(this.dateSCIDelivery);
            this.groupBox1.Controls.Add(this.labelSewingInlineDate);
            this.groupBox1.Controls.Add(this.labelSubprocessInline);
            this.groupBox1.Controls.Add(this.dateRangeSewInLine);
            this.groupBox1.Controls.Add(this.dateSubprocessInline);
            this.groupBox1.Controls.Add(this.txtSPNoEnd);
            this.groupBox1.Controls.Add(this.btnQuery);
            this.groupBox1.Controls.Add(this.txtSPNoStart);
            this.groupBox1.Controls.Add(this.labelSPNo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1008, 147);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.Location = new System.Drawing.Point(590, 82);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(107, 23);
            this.txtfactory.TabIndex = 6;
            // 
            // labelArtworkType
            // 
            this.labelArtworkType.Lines = 0;
            this.labelArtworkType.Location = new System.Drawing.Point(9, 115);
            this.labelArtworkType.Name = "labelArtworkType";
            this.labelArtworkType.Size = new System.Drawing.Size(128, 23);
            this.labelArtworkType.TabIndex = 22;
            this.labelArtworkType.Text = "Artwork Type";
            // 
            // txtartworktype_ftyArtworkType
            // 
            this.txtartworktype_ftyArtworkType.BackColor = System.Drawing.Color.White;
            this.txtartworktype_ftyArtworkType.CClassify = "";
            this.txtartworktype_ftyArtworkType.CSubprocess = "";
            this.txtartworktype_ftyArtworkType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtartworktype_ftyArtworkType.Location = new System.Drawing.Point(140, 115);
            this.txtartworktype_ftyArtworkType.Name = "txtartworktype_ftyArtworkType";
            this.txtartworktype_ftyArtworkType.Size = new System.Drawing.Size(140, 23);
            this.txtartworktype_ftyArtworkType.TabIndex = 7;
            // 
            // checkOnlyAprrovedData
            // 
            this.checkOnlyAprrovedData.AutoSize = true;
            this.checkOnlyAprrovedData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkOnlyAprrovedData.Location = new System.Drawing.Point(462, 115);
            this.checkOnlyAprrovedData.Name = "checkOnlyAprrovedData";
            this.checkOnlyAprrovedData.Size = new System.Drawing.Size(156, 21);
            this.checkOnlyAprrovedData.TabIndex = 8;
            this.checkOnlyAprrovedData.Text = "Only Aprroved  Data";
            this.checkOnlyAprrovedData.UseVisualStyleBackColor = true;
            this.checkOnlyAprrovedData.CheckedChanged += new System.EventHandler(this.CheckOnlyAprrovedData_CheckedChanged);
            // 
            // labelApproveDate
            // 
            this.labelApproveDate.Lines = 0;
            this.labelApproveDate.Location = new System.Drawing.Point(462, 48);
            this.labelApproveDate.Name = "labelApproveDate";
            this.labelApproveDate.Size = new System.Drawing.Size(125, 23);
            this.labelApproveDate.TabIndex = 18;
            this.labelApproveDate.Text = "Approve Date";
            // 
            // dateApproveDate
            // 
            this.dateApproveDate.IsRequired = false;
            this.dateApproveDate.Location = new System.Drawing.Point(590, 48);
            this.dateApproveDate.Name = "dateApproveDate";
            this.dateApproveDate.Size = new System.Drawing.Size(280, 23);
            this.dateApproveDate.TabIndex = 4;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Lines = 0;
            this.labelSCIDelivery.Location = new System.Drawing.Point(462, 15);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(125, 23);
            this.labelSCIDelivery.TabIndex = 16;
            this.labelSCIDelivery.Text = "SCI Delivery";
            // 
            // dateSCIDelivery
            // 
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(590, 15);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 3;
            // 
            // labelSewingInlineDate
            // 
            this.labelSewingInlineDate.Lines = 0;
            this.labelSewingInlineDate.Location = new System.Drawing.Point(9, 82);
            this.labelSewingInlineDate.Name = "labelSewingInlineDate";
            this.labelSewingInlineDate.Size = new System.Drawing.Size(128, 23);
            this.labelSewingInlineDate.TabIndex = 12;
            this.labelSewingInlineDate.Text = "Sewing Inline Date";
            // 
            // labelSubprocessInline
            // 
            this.labelSubprocessInline.Lines = 0;
            this.labelSubprocessInline.Location = new System.Drawing.Point(9, 48);
            this.labelSubprocessInline.Name = "labelSubprocessInline";
            this.labelSubprocessInline.Size = new System.Drawing.Size(128, 23);
            this.labelSubprocessInline.TabIndex = 11;
            this.labelSubprocessInline.Text = "Subprocess  Inline ";
            // 
            // dateRangeSewInLine
            // 
            this.dateRangeSewInLine.IsRequired = false;
            this.dateRangeSewInLine.Location = new System.Drawing.Point(140, 82);
            this.dateRangeSewInLine.Name = "dateRangeSewInLine";
            this.dateRangeSewInLine.Size = new System.Drawing.Size(280, 23);
            this.dateRangeSewInLine.TabIndex = 5;
            // 
            // dateSubprocessInline
            // 
            this.dateSubprocessInline.IsRequired = false;
            this.dateSubprocessInline.Location = new System.Drawing.Point(140, 48);
            this.dateSubprocessInline.Name = "dateSubprocessInline";
            this.dateSubprocessInline.Size = new System.Drawing.Size(280, 23);
            this.dateSubprocessInline.TabIndex = 0;
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(268, 15);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(122, 23);
            this.txtSPNoEnd.TabIndex = 2;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnQuery.Location = new System.Drawing.Point(901, 15);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(101, 30);
            this.btnQuery.TabIndex = 9;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(140, 15);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(122, 23);
            this.txtSPNoStart.TabIndex = 1;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Lines = 0;
            this.labelSPNo.Location = new System.Drawing.Point(9, 15);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(128, 23);
            this.labelSPNo.TabIndex = 0;
            this.labelSPNo.Text = "SP#";
            // 
            // btnToExcel
            // 
            this.btnToExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnToExcel.Location = new System.Drawing.Point(810, 15);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(96, 30);
            this.btnToExcel.TabIndex = 2;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(912, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnApprove
            // 
            this.btnApprove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApprove.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnApprove.Location = new System.Drawing.Point(590, 15);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(97, 30);
            this.btnApprove.TabIndex = 0;
            this.btnApprove.Text = "Approve";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.BtnApprove_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnUnApprove);
            this.groupBox2.Controls.Add(this.btnToExcel);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnApprove);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 508);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1008, 53);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            // 
            // btnUnApprove
            // 
            this.btnUnApprove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUnApprove.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnUnApprove.Location = new System.Drawing.Point(693, 15);
            this.btnUnApprove.Name = "btnUnApprove";
            this.btnUnApprove.Size = new System.Drawing.Size(112, 30);
            this.btnUnApprove.TabIndex = 1;
            this.btnUnApprove.Text = "UnApprove";
            this.btnUnApprove.UseVisualStyleBackColor = true;
            this.btnUnApprove.Click += new System.EventHandler(this.BtnUnApprove_Click);
            // 
            // P01_BatchApprove
            // 
            this.ClientSize = new System.Drawing.Size(1008, 561);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "P01_BatchApprove";
            this.Text = "Sub Process Batch Approve";
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchApprove)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel1;
        private Win.UI.Grid gridBatchApprove;
        private Win.UI.Label labelFactory;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Label labelArtworkType;
        private Class.Txtartworktype_fty txtartworktype_ftyArtworkType;
        private Win.UI.CheckBox checkOnlyAprrovedData;
        private Win.UI.Label labelApproveDate;
        private Win.UI.DateRange dateApproveDate;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.Label labelSewingInlineDate;
        private Win.UI.Label labelSubprocessInline;
        private Win.UI.DateRange dateRangeSewInLine;
        private Win.UI.DateRange dateSubprocessInline;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.Button btnQuery;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.Label labelSPNo;
        private Win.UI.Button btnToExcel;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnApprove;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.Button btnUnApprove;
        private Class.Txtfactory txtfactory;
    }
}
