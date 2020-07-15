namespace Sci.Production.Subcon
{
    partial class P01_BatchCreate
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
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.dateApproveDate = new Sci.Win.UI.DateRange();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.labelApproveDate = new Sci.Win.UI.Label();
            this.panel1 = new Sci.Win.UI.Panel();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.lblIrregularQtyReason = new Sci.Win.UI.Label();
            this.txtIrregularQtyReason = new Sci.Production.Class.TxtSubconReason();
            this.labelNoQuoteHintColor = new Sci.Win.UI.Label();
            this.labelNoQuoteHint = new Sci.Win.UI.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelArtworkType = new Sci.Win.UI.Label();
            this.txtartworktype_ftyArtworkType = new Sci.Production.Class.Txtartworktype_fty();
            this.btnFindNow = new Sci.Win.UI.Button();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelDelivery = new Sci.Win.UI.Label();
            this.labelIssueDate = new Sci.Win.UI.Label();
            this.dateDelivery = new Sci.Win.UI.DateBox();
            this.dateIssueDate = new Sci.Win.UI.DateBox();
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
            this.gridBatchCreateFromSubProcessData.Size = new System.Drawing.Size(1024, 356);
            this.gridBatchCreateFromSubProcessData.TabIndex = 0;
            this.gridBatchCreateFromSubProcessData.TabStop = false;
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
            this.dateSCIDelivery.Location = new System.Drawing.Point(520, 15);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(243, 23);
            this.dateSCIDelivery.TabIndex = 1;
            // 
            // dateApproveDate
            // 
            // 
            // 
            // 
            this.dateApproveDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateApproveDate.DateBox1.Name = "";
            this.dateApproveDate.DateBox1.Size = new System.Drawing.Size(110, 23);
            this.dateApproveDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateApproveDate.DateBox2.Location = new System.Drawing.Point(132, 0);
            this.dateApproveDate.DateBox2.Name = "";
            this.dateApproveDate.DateBox2.Size = new System.Drawing.Size(110, 23);
            this.dateApproveDate.DateBox2.TabIndex = 1;
            this.dateApproveDate.IsRequired = false;
            this.dateApproveDate.Location = new System.Drawing.Point(107, 15);
            this.dateApproveDate.Name = "dateApproveDate";
            this.dateApproveDate.Size = new System.Drawing.Size(243, 23);
            this.dateApproveDate.TabIndex = 0;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(381, 15);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSCIDelivery.Size = new System.Drawing.Size(136, 23);
            this.labelSCIDelivery.TabIndex = 7;
            this.labelSCIDelivery.Text = "SCI  Delivery";
            this.labelSCIDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(241, 43);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(107, 23);
            this.txtSPNoEnd.TabIndex = 3;
            // 
            // labelApproveDate
            // 
            this.labelApproveDate.Location = new System.Drawing.Point(9, 15);
            this.labelApproveDate.Name = "labelApproveDate";
            this.labelApproveDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelApproveDate.Size = new System.Drawing.Size(95, 23);
            this.labelApproveDate.TabIndex = 5;
            this.labelApproveDate.Text = "Req. Apv Date";
            this.labelApproveDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridBatchCreateFromSubProcessData);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 132);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1024, 356);
            this.panel1.TabIndex = 23;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblIrregularQtyReason);
            this.groupBox1.Controls.Add(this.txtIrregularQtyReason);
            this.groupBox1.Controls.Add(this.labelNoQuoteHintColor);
            this.groupBox1.Controls.Add(this.labelNoQuoteHint);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.labelArtworkType);
            this.groupBox1.Controls.Add(this.txtartworktype_ftyArtworkType);
            this.groupBox1.Controls.Add(this.dateSCIDelivery);
            this.groupBox1.Controls.Add(this.dateApproveDate);
            this.groupBox1.Controls.Add(this.labelSCIDelivery);
            this.groupBox1.Controls.Add(this.txtSPNoEnd);
            this.groupBox1.Controls.Add(this.labelApproveDate);
            this.groupBox1.Controls.Add(this.btnFindNow);
            this.groupBox1.Controls.Add(this.txtSPNoStart);
            this.groupBox1.Controls.Add(this.labelSPNo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1024, 132);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // lblIrregularQtyReason
            // 
            this.lblIrregularQtyReason.Location = new System.Drawing.Point(381, 46);
            this.lblIrregularQtyReason.Name = "lblIrregularQtyReason";
            this.lblIrregularQtyReason.Size = new System.Drawing.Size(136, 23);
            this.lblIrregularQtyReason.TabIndex = 18;
            this.lblIrregularQtyReason.Text = "Irregular Qty Reason";
            // 
            // txtIrregularQtyReason
            // 
            this.txtIrregularQtyReason.DisplayBox1Binding = "";
            this.txtIrregularQtyReason.Location = new System.Drawing.Point(520, 44);
            this.txtIrregularQtyReason.MutiSelect = true;
            this.txtIrregularQtyReason.Name = "txtIrregularQtyReason";
            this.txtIrregularQtyReason.Size = new System.Drawing.Size(393, 27);
            this.txtIrregularQtyReason.TabIndex = 4;
            this.txtIrregularQtyReason.TextBox1Binding = "";
            this.txtIrregularQtyReason.Type = "SQ";
            // 
            // labelNoQuoteHintColor
            // 
            this.labelNoQuoteHintColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(108)))), ((int)(((byte)(126)))));
            this.labelNoQuoteHintColor.Location = new System.Drawing.Point(9, 107);
            this.labelNoQuoteHintColor.Name = "labelNoQuoteHintColor";
            this.labelNoQuoteHintColor.Size = new System.Drawing.Size(19, 20);
            this.labelNoQuoteHintColor.TabIndex = 16;
            // 
            // labelNoQuoteHint
            // 
            this.labelNoQuoteHint.BackColor = System.Drawing.Color.Transparent;
            this.labelNoQuoteHint.Location = new System.Drawing.Point(31, 106);
            this.labelNoQuoteHint.Name = "labelNoQuoteHint";
            this.labelNoQuoteHint.Size = new System.Drawing.Size(395, 23);
            this.labelNoQuoteHint.TabIndex = 15;
            this.labelNoQuoteHint.Text = "No quote record, please check with Planning Team";
            this.labelNoQuoteHint.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(216, 46);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(22, 17);
            this.label8.TabIndex = 15;
            this.label8.Text = "～";
            // 
            // labelArtworkType
            // 
            this.labelArtworkType.Location = new System.Drawing.Point(9, 73);
            this.labelArtworkType.Name = "labelArtworkType";
            this.labelArtworkType.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.labelArtworkType.Size = new System.Drawing.Size(95, 23);
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
            this.txtartworktype_ftyArtworkType.Location = new System.Drawing.Point(107, 73);
            this.txtartworktype_ftyArtworkType.Name = "txtartworktype_ftyArtworkType";
            this.txtartworktype_ftyArtworkType.Size = new System.Drawing.Size(140, 23);
            this.txtartworktype_ftyArtworkType.TabIndex = 5;
            this.txtartworktype_ftyArtworkType.Validating += new System.ComponentModel.CancelEventHandler(this.Txtartworktype_ftyArtworkType_Validating);
            // 
            // btnFindNow
            // 
            this.btnFindNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFindNow.Location = new System.Drawing.Point(919, 15);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(101, 30);
            this.btnFindNow.TabIndex = 9;
            this.btnFindNow.TabStop = false;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.BtnFindNow_Click);
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(107, 44);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(107, 23);
            this.txtSPNoStart.TabIndex = 2;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(9, 44);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSPNo.Size = new System.Drawing.Size(95, 23);
            this.labelSPNo.TabIndex = 0;
            this.labelSPNo.Text = "SP#";
            this.labelSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelDelivery
            // 
            this.labelDelivery.Location = new System.Drawing.Point(565, 19);
            this.labelDelivery.Name = "labelDelivery";
            this.labelDelivery.Size = new System.Drawing.Size(95, 23);
            this.labelDelivery.TabIndex = 14;
            this.labelDelivery.Text = "Delivery";
            // 
            // labelIssueDate
            // 
            this.labelIssueDate.Location = new System.Drawing.Point(351, 19);
            this.labelIssueDate.Name = "labelIssueDate";
            this.labelIssueDate.Size = new System.Drawing.Size(95, 23);
            this.labelIssueDate.TabIndex = 13;
            this.labelIssueDate.Text = "Issue Date";
            // 
            // dateDelivery
            // 
            this.dateDelivery.Location = new System.Drawing.Point(664, 19);
            this.dateDelivery.Name = "dateDelivery";
            this.dateDelivery.Size = new System.Drawing.Size(106, 23);
            this.dateDelivery.TabIndex = 1;
            // 
            // dateIssueDate
            // 
            this.dateIssueDate.Location = new System.Drawing.Point(452, 19);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(109, 23);
            this.dateIssueDate.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(946, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(74, 30);
            this.btnCancel.TabIndex = 4;
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
            this.btnImport.TabIndex = 3;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelDelivery);
            this.groupBox2.Controls.Add(this.btnToExcel);
            this.groupBox2.Controls.Add(this.labelIssueDate);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.dateDelivery);
            this.groupBox2.Controls.Add(this.dateIssueDate);
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
            this.btnToExcel.TabIndex = 2;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // P01_BatchCreate
            // 
            this.ClientSize = new System.Drawing.Size(1024, 541);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.DefaultControl = "dateApproveDate";
            this.Name = "P01_BatchCreate";
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
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.DateRange dateApproveDate;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.Label labelApproveDate;
        private Win.UI.Panel panel1;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Button btnFindNow;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.Label labelSPNo;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnImport;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.Button btnToExcel;
        private Win.UI.Label labelDelivery;
        private Win.UI.Label labelIssueDate;
        private Win.UI.Label labelArtworkType;
        private Win.UI.DateBox dateDelivery;
        private Win.UI.DateBox dateIssueDate;
        private Class.Txtartworktype_fty txtartworktype_ftyArtworkType;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private System.Windows.Forms.Label label8;
        private Win.UI.Label labelNoQuoteHintColor;
        private Win.UI.Label labelNoQuoteHint;
        private Class.TxtSubconReason txtIrregularQtyReason;
        private Win.UI.Label lblIrregularQtyReason;
    }
}
