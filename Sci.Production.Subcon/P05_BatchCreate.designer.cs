namespace Sci.Production.Subcon
{
    partial class P05_BatchCreate
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
            this.gridBatchCreateFromSubProcessData = new Sci.Win.UI.Grid();
            this.dateInlineDate = new Sci.Win.UI.DateRange();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.labSewingInline = new Sci.Win.UI.Label();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.panel1 = new Sci.Win.UI.Panel();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.chkSize = new Sci.Win.UI.CheckBox();
            this.chkArticle = new Sci.Win.UI.CheckBox();
            this.label2 = new Sci.Win.UI.Label();
            this.lblFilter = new Sci.Win.UI.Label();
            this.checkBoxReqQtyHasValue = new Sci.Win.UI.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.labelArtworkType = new Sci.Win.UI.Label();
            this.txtartworktype_ftyArtworkType = new Sci.Production.Class.Txtartworktype_fty();
            this.btnFindNow = new Sci.Win.UI.Button();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelReqDate = new Sci.Win.UI.Label();
            this.dateReqDate = new Sci.Win.UI.DateBox();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchCreateFromSubProcessData)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridBatchCreateFromSubProcessData
            // 
            this.gridBatchCreateFromSubProcessData.AllowUserToAddRows = false;
            this.gridBatchCreateFromSubProcessData.AllowUserToDeleteRows = false;
            this.gridBatchCreateFromSubProcessData.AllowUserToResizeRows = false;
            this.gridBatchCreateFromSubProcessData.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBatchCreateFromSubProcessData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridBatchCreateFromSubProcessData.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBatchCreateFromSubProcessData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBatchCreateFromSubProcessData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridBatchCreateFromSubProcessData.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBatchCreateFromSubProcessData.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBatchCreateFromSubProcessData.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBatchCreateFromSubProcessData.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBatchCreateFromSubProcessData.Location = new System.Drawing.Point(0, 0);
            this.gridBatchCreateFromSubProcessData.Name = "gridBatchCreateFromSubProcessData";
            this.gridBatchCreateFromSubProcessData.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridBatchCreateFromSubProcessData.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridBatchCreateFromSubProcessData.RowTemplate.Height = 24;
            this.gridBatchCreateFromSubProcessData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBatchCreateFromSubProcessData.ShowCellToolTips = false;
            this.gridBatchCreateFromSubProcessData.Size = new System.Drawing.Size(1024, 375);
            this.gridBatchCreateFromSubProcessData.TabIndex = 0;
            this.gridBatchCreateFromSubProcessData.TabStop = false;
            // 
            // dateInlineDate
            // 
            // 
            // 
            // 
            this.dateInlineDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateInlineDate.DateBox1.Name = "";
            this.dateInlineDate.DateBox1.Size = new System.Drawing.Size(111, 23);
            this.dateInlineDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateInlineDate.DateBox2.Location = new System.Drawing.Point(133, 0);
            this.dateInlineDate.DateBox2.Name = "";
            this.dateInlineDate.DateBox2.Size = new System.Drawing.Size(111, 23);
            this.dateInlineDate.DateBox2.TabIndex = 1;
            this.dateInlineDate.IsRequired = false;
            this.dateInlineDate.Location = new System.Drawing.Point(88, 49);
            this.dateInlineDate.Name = "dateInlineDate";
            this.dateInlineDate.Size = new System.Drawing.Size(245, 23);
            this.dateInlineDate.TabIndex = 3;
            // 
            // dateSCIDelivery
            // 
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSCIDelivery.DateBox1.Name = "";
            this.dateSCIDelivery.DateBox1.Size = new System.Drawing.Size(110, 23);
            this.dateSCIDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox2.Location = new System.Drawing.Point(132, 0);
            this.dateSCIDelivery.DateBox2.Name = "";
            this.dateSCIDelivery.DateBox2.Size = new System.Drawing.Size(110, 23);
            this.dateSCIDelivery.DateBox2.TabIndex = 1;
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(435, 15);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(243, 23);
            this.dateSCIDelivery.TabIndex = 2;
            // 
            // labSewingInline
            // 
            this.labSewingInline.BackColor = System.Drawing.Color.SkyBlue;
            this.labSewingInline.Location = new System.Drawing.Point(9, 49);
            this.labSewingInline.Name = "labSewingInline";
            this.labSewingInline.Size = new System.Drawing.Size(77, 23);
            this.labSewingInline.TabIndex = 8;
            this.labSewingInline.Text = "Sew. Inline";
            this.labSewingInline.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.BackColor = System.Drawing.Color.SkyBlue;
            this.labelSCIDelivery.Location = new System.Drawing.Point(346, 15);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(86, 23);
            this.labelSCIDelivery.TabIndex = 7;
            this.labelSCIDelivery.Text = "SCI  Delivery";
            this.labelSCIDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(227, 15);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(107, 23);
            this.txtSPNoEnd.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridBatchCreateFromSubProcessData);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 113);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1024, 375);
            this.panel1.TabIndex = 23;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkSize);
            this.groupBox1.Controls.Add(this.chkArticle);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblFilter);
            this.groupBox1.Controls.Add(this.checkBoxReqQtyHasValue);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.labelArtworkType);
            this.groupBox1.Controls.Add(this.txtartworktype_ftyArtworkType);
            this.groupBox1.Controls.Add(this.dateInlineDate);
            this.groupBox1.Controls.Add(this.dateSCIDelivery);
            this.groupBox1.Controls.Add(this.labSewingInline);
            this.groupBox1.Controls.Add(this.labelSCIDelivery);
            this.groupBox1.Controls.Add(this.txtSPNoEnd);
            this.groupBox1.Controls.Add(this.btnFindNow);
            this.groupBox1.Controls.Add(this.txtSPNoStart);
            this.groupBox1.Controls.Add(this.labelSPNo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1024, 113);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // chkSize
            // 
            this.chkSize.AutoSize = true;
            this.chkSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkSize.Location = new System.Drawing.Point(161, 85);
            this.chkSize.Name = "chkSize";
            this.chkSize.Size = new System.Drawing.Size(54, 21);
            this.chkSize.TabIndex = 24;
            this.chkSize.Text = "Size";
            this.chkSize.UseVisualStyleBackColor = true;
            this.chkSize.CheckedChanged += new System.EventHandler(this.CheckBoxReqQtyHasValue_CheckedChanged);
            // 
            // chkArticle
            // 
            this.chkArticle.AutoSize = true;
            this.chkArticle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkArticle.Location = new System.Drawing.Point(89, 85);
            this.chkArticle.Name = "chkArticle";
            this.chkArticle.Size = new System.Drawing.Size(66, 21);
            this.chkArticle.TabIndex = 23;
            this.chkArticle.Text = "Article";
            this.chkArticle.UseVisualStyleBackColor = true;
            this.chkArticle.CheckedChanged += new System.EventHandler(this.CheckBoxReqQtyHasValue_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 23);
            this.label2.TabIndex = 21;
            this.label2.Text = "Expand By";
            // 
            // lblFilter
            // 
            this.lblFilter.Location = new System.Drawing.Point(346, 83);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(86, 23);
            this.lblFilter.TabIndex = 20;
            this.lblFilter.Text = "Filter";
            // 
            // checkBoxReqQtyHasValue
            // 
            this.checkBoxReqQtyHasValue.AutoSize = true;
            this.checkBoxReqQtyHasValue.Checked = true;
            this.checkBoxReqQtyHasValue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxReqQtyHasValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBoxReqQtyHasValue.Location = new System.Drawing.Point(435, 85);
            this.checkBoxReqQtyHasValue.Name = "checkBoxReqQtyHasValue";
            this.checkBoxReqQtyHasValue.Size = new System.Drawing.Size(99, 21);
            this.checkBoxReqQtyHasValue.TabIndex = 17;
            this.checkBoxReqQtyHasValue.Text = "Req. Qty>0";
            this.checkBoxReqQtyHasValue.UseVisualStyleBackColor = true;
            this.checkBoxReqQtyHasValue.CheckedChanged += new System.EventHandler(this.CheckBoxReqQtyHasValue_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(201, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(22, 17);
            this.label8.TabIndex = 15;
            this.label8.Text = "～";
            // 
            // labelArtworkType
            // 
            this.labelArtworkType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.labelArtworkType.Location = new System.Drawing.Point(346, 49);
            this.labelArtworkType.Name = "labelArtworkType";
            this.labelArtworkType.Size = new System.Drawing.Size(86, 23);
            this.labelArtworkType.TabIndex = 12;
            this.labelArtworkType.Text = "Artwork Type";
            this.labelArtworkType.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtartworktype_ftyArtworkType
            // 
            this.txtartworktype_ftyArtworkType.BackColor = System.Drawing.Color.White;
            this.txtartworktype_ftyArtworkType.CClassify = "";
            this.txtartworktype_ftyArtworkType.CSubprocess = "Y";
            this.txtartworktype_ftyArtworkType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtartworktype_ftyArtworkType.Location = new System.Drawing.Point(435, 49);
            this.txtartworktype_ftyArtworkType.Name = "txtartworktype_ftyArtworkType";
            this.txtartworktype_ftyArtworkType.Size = new System.Drawing.Size(132, 23);
            this.txtartworktype_ftyArtworkType.TabIndex = 4;
            this.txtartworktype_ftyArtworkType.Validating += new System.ComponentModel.CancelEventHandler(this.Txtartworktype_ftyArtworkType_Validating);
            // 
            // btnFindNow
            // 
            this.btnFindNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFindNow.Location = new System.Drawing.Point(919, 15);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(101, 30);
            this.btnFindNow.TabIndex = 5;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.BtnFindNow_Click);
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(88, 15);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(107, 23);
            this.txtSPNoStart.TabIndex = 0;
            // 
            // labelSPNo
            // 
            this.labelSPNo.BackColor = System.Drawing.Color.SkyBlue;
            this.labelSPNo.Location = new System.Drawing.Point(9, 15);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(77, 23);
            this.labelSPNo.TabIndex = 0;
            this.labelSPNo.Text = "SP#";
            this.labelSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelReqDate
            // 
            this.labelReqDate.Location = new System.Drawing.Point(548, 19);
            this.labelReqDate.Name = "labelReqDate";
            this.labelReqDate.Size = new System.Drawing.Size(95, 23);
            this.labelReqDate.TabIndex = 13;
            this.labelReqDate.Text = "Req. Date";
            // 
            // dateReqDate
            // 
            this.dateReqDate.Location = new System.Drawing.Point(649, 19);
            this.dateReqDate.Name = "dateReqDate";
            this.dateReqDate.Size = new System.Drawing.Size(109, 23);
            this.dateReqDate.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(946, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(74, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(864, 15);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(79, 30);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnToExcel);
            this.groupBox2.Controls.Add(this.labelReqDate);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.dateReqDate);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 488);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1024, 53);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            // 
            // btnToExcel
            // 
            this.btnToExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnToExcel.Location = new System.Drawing.Point(776, 15);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(84, 30);
            this.btnToExcel.TabIndex = 1;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // P05_BatchCreate
            // 
            this.ClientSize = new System.Drawing.Size(1024, 541);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.DefaultControl = "dateApproveDate";
            this.Name = "P05_BatchCreate";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Batch Create From Sub Process Data";
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchCreateFromSubProcessData)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid gridBatchCreateFromSubProcessData;
        private Win.UI.DateRange dateInlineDate;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.Label labSewingInline;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.Panel panel1;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Button btnFindNow;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.Label labelSPNo;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnImport;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.Button btnToExcel;
        private Win.UI.Label labelReqDate;
        private Win.UI.Label labelArtworkType;
        private Win.UI.DateBox dateReqDate;
        private Class.Txtartworktype_fty txtartworktype_ftyArtworkType;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private System.Windows.Forms.Label label8;
        private Win.UI.CheckBox checkBoxReqQtyHasValue;
        private Win.UI.Label lblFilter;
        private Win.UI.Label label2;
        private Win.UI.CheckBox chkSize;
        private Win.UI.CheckBox chkArticle;
    }
}
