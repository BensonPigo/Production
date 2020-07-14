namespace Sci.Production.Subcon
{
    partial class P05_Import
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
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.btnFindNow = new Sci.Win.UI.Button();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.labelNoQuoteHintColor = new Sci.Win.UI.Label();
            this.labelNoSuppHint = new Sci.Win.UI.Label();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.chkSize = new Sci.Win.UI.CheckBox();
            this.chkArticle = new Sci.Win.UI.CheckBox();
            this.label2 = new Sci.Win.UI.Label();
            this.lblFilter = new Sci.Win.UI.Label();
            this.txtMasterSP = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.checkBoxAssignedSupp = new Sci.Win.UI.CheckBox();
            this.checkBoxReqQtyHasValue = new Sci.Win.UI.CheckBox();
            this.labSewingInline = new Sci.Win.UI.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.dateInlineDate = new Sci.Win.UI.DateRange();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridBatchImport = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(906, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(810, 16);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 30);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnFindNow
            // 
            this.btnFindNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFindNow.Location = new System.Drawing.Point(895, 15);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(101, 30);
            this.btnFindNow.TabIndex = 7;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.btnFindNow_Click);
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(107, 15);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(122, 23);
            this.txtSPNoStart.TabIndex = 0;
            // 
            // labelSPNo
            // 
            this.labelSPNo.BackColor = System.Drawing.Color.SkyBlue;
            this.labelSPNo.Location = new System.Drawing.Point(9, 15);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(95, 23);
            this.labelSPNo.TabIndex = 5;
            this.labelSPNo.Text = "SP#";
            this.labelSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelNoQuoteHintColor);
            this.groupBox2.Controls.Add(this.labelNoSuppHint);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 477);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1002, 53);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // labelNoQuoteHintColor
            // 
            this.labelNoQuoteHintColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(108)))), ((int)(((byte)(126)))));
            this.labelNoQuoteHintColor.Location = new System.Drawing.Point(9, 19);
            this.labelNoQuoteHintColor.Name = "labelNoQuoteHintColor";
            this.labelNoQuoteHintColor.Size = new System.Drawing.Size(19, 20);
            this.labelNoQuoteHintColor.TabIndex = 6;
            // 
            // labelNoSuppHint
            // 
            this.labelNoSuppHint.BackColor = System.Drawing.Color.Transparent;
            this.labelNoSuppHint.Location = new System.Drawing.Point(31, 18);
            this.labelNoSuppHint.Name = "labelNoSuppHint";
            this.labelNoSuppHint.Size = new System.Drawing.Size(395, 23);
            this.labelNoSuppHint.TabIndex = 5;
            this.labelNoSuppHint.Text = "No assign supplier({#Supplier}). Please check with planning team";
            this.labelNoSuppHint.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkSize);
            this.groupBox1.Controls.Add(this.chkArticle);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblFilter);
            this.groupBox1.Controls.Add(this.txtMasterSP);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.checkBoxAssignedSupp);
            this.groupBox1.Controls.Add(this.checkBoxReqQtyHasValue);
            this.groupBox1.Controls.Add(this.labSewingInline);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.dateInlineDate);
            this.groupBox1.Controls.Add(this.dateSCIDelivery);
            this.groupBox1.Controls.Add(this.labelSCIDelivery);
            this.groupBox1.Controls.Add(this.txtSPNoEnd);
            this.groupBox1.Controls.Add(this.btnFindNow);
            this.groupBox1.Controls.Add(this.txtSPNoStart);
            this.groupBox1.Controls.Add(this.labelSPNo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1002, 110);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // chkSize
            // 
            this.chkSize.AutoSize = true;
            this.chkSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkSize.Location = new System.Drawing.Point(179, 79);
            this.chkSize.Name = "chkSize";
            this.chkSize.Size = new System.Drawing.Size(54, 21);
            this.chkSize.TabIndex = 22;
            this.chkSize.Text = "Size";
            this.chkSize.UseVisualStyleBackColor = true;
            this.chkSize.CheckedChanged += new System.EventHandler(this.checkBoxReqQtyHasValue_CheckedChanged);
            // 
            // chkArticle
            // 
            this.chkArticle.AutoSize = true;
            this.chkArticle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkArticle.Location = new System.Drawing.Point(107, 79);
            this.chkArticle.Name = "chkArticle";
            this.chkArticle.Size = new System.Drawing.Size(66, 21);
            this.chkArticle.TabIndex = 21;
            this.chkArticle.Text = "Article";
            this.chkArticle.UseVisualStyleBackColor = true;
            this.chkArticle.CheckedChanged += new System.EventHandler(this.checkBoxReqQtyHasValue_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 23);
            this.label2.TabIndex = 20;
            this.label2.Text = "Expand By";
            // 
            // lblFilter
            // 
            this.lblFilter.Location = new System.Drawing.Point(418, 77);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(95, 23);
            this.lblFilter.TabIndex = 19;
            this.lblFilter.Text = "Filter";
            // 
            // txtMasterSP
            // 
            this.txtMasterSP.BackColor = System.Drawing.Color.White;
            this.txtMasterSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMasterSP.Location = new System.Drawing.Point(516, 15);
            this.txtMasterSP.Name = "txtMasterSP";
            this.txtMasterSP.Size = new System.Drawing.Size(122, 23);
            this.txtMasterSP.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.SkyBlue;
            this.label1.Location = new System.Drawing.Point(418, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 23);
            this.label1.TabIndex = 18;
            this.label1.Text = "Master SP# ";
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // checkBoxAssignedSupp
            // 
            this.checkBoxAssignedSupp.AutoSize = true;
            this.checkBoxAssignedSupp.Checked = true;
            this.checkBoxAssignedSupp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAssignedSupp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBoxAssignedSupp.Location = new System.Drawing.Point(616, 79);
            this.checkBoxAssignedSupp.Name = "checkBoxAssignedSupp";
            this.checkBoxAssignedSupp.Size = new System.Drawing.Size(380, 21);
            this.checkBoxAssignedSupp.TabIndex = 6;
            this.checkBoxAssignedSupp.Text = "Already assigned supplier({#supplier}) by planning team";
            this.checkBoxAssignedSupp.UseVisualStyleBackColor = true;
            this.checkBoxAssignedSupp.CheckedChanged += new System.EventHandler(this.checkBoxReqQtyHasValue_CheckedChanged);
            // 
            // checkBoxReqQtyHasValue
            // 
            this.checkBoxReqQtyHasValue.AutoSize = true;
            this.checkBoxReqQtyHasValue.Checked = true;
            this.checkBoxReqQtyHasValue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxReqQtyHasValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBoxReqQtyHasValue.Location = new System.Drawing.Point(516, 79);
            this.checkBoxReqQtyHasValue.Name = "checkBoxReqQtyHasValue";
            this.checkBoxReqQtyHasValue.Size = new System.Drawing.Size(99, 21);
            this.checkBoxReqQtyHasValue.TabIndex = 5;
            this.checkBoxReqQtyHasValue.Text = "Req. Qty>0";
            this.checkBoxReqQtyHasValue.UseVisualStyleBackColor = true;
            this.checkBoxReqQtyHasValue.CheckedChanged += new System.EventHandler(this.checkBoxReqQtyHasValue_CheckedChanged);
            // 
            // labSewingInline
            // 
            this.labSewingInline.BackColor = System.Drawing.Color.SkyBlue;
            this.labSewingInline.Location = new System.Drawing.Point(9, 46);
            this.labSewingInline.Name = "labSewingInline";
            this.labSewingInline.Size = new System.Drawing.Size(95, 23);
            this.labSewingInline.TabIndex = 15;
            this.labSewingInline.Text = "Sew. Inline";
            this.labSewingInline.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(237, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(22, 17);
            this.label8.TabIndex = 14;
            this.label8.Text = "～";
            // 
            // dateInlineDate
            // 
            // 
            // 
            // 
            this.dateInlineDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateInlineDate.DateBox1.Name = "";
            this.dateInlineDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateInlineDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateInlineDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateInlineDate.DateBox2.Name = "";
            this.dateInlineDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateInlineDate.DateBox2.TabIndex = 1;
            this.dateInlineDate.IsRequired = false;
            this.dateInlineDate.Location = new System.Drawing.Point(107, 46);
            this.dateInlineDate.Name = "dateInlineDate";
            this.dateInlineDate.Size = new System.Drawing.Size(280, 23);
            this.dateInlineDate.TabIndex = 3;
            // 
            // dateSCIDelivery
            // 
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSCIDelivery.DateBox1.Name = "";
            this.dateSCIDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSCIDelivery.DateBox2.Name = "";
            this.dateSCIDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDelivery.DateBox2.TabIndex = 1;
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(516, 46);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 4;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.BackColor = System.Drawing.Color.SkyBlue;
            this.labelSCIDelivery.Location = new System.Drawing.Point(418, 46);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(95, 23);
            this.labelSCIDelivery.TabIndex = 7;
            this.labelSCIDelivery.Text = "SCI  Delivery";
            this.labelSCIDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(265, 15);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(122, 23);
            this.txtSPNoEnd.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridBatchImport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 110);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1002, 367);
            this.panel1.TabIndex = 20;
            // 
            // gridBatchImport
            // 
            this.gridBatchImport.AllowUserToAddRows = false;
            this.gridBatchImport.AllowUserToDeleteRows = false;
            this.gridBatchImport.AllowUserToResizeRows = false;
            this.gridBatchImport.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBatchImport.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridBatchImport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBatchImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBatchImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridBatchImport.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBatchImport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBatchImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBatchImport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBatchImport.Location = new System.Drawing.Point(0, 0);
            this.gridBatchImport.Name = "gridBatchImport";
            this.gridBatchImport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridBatchImport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridBatchImport.RowTemplate.Height = 24;
            this.gridBatchImport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBatchImport.ShowCellToolTips = false;
            this.gridBatchImport.Size = new System.Drawing.Size(1002, 367);
            this.gridBatchImport.TabIndex = 0;
            this.gridBatchImport.TabStop = false;
            this.gridBatchImport.Sorted += new System.EventHandler(this.gridBatchImport_Sorted);
            // 
            // P05_Import
            // 
            this.ClientSize = new System.Drawing.Size(1002, 530);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.DefaultControl = "dateApproveDate";
            this.Name = "P05_Import";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Batch Import";
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnCancel;
        private Win.UI.Button btnImport;
        private Win.UI.Button btnFindNow;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.Label labelSPNo;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.Panel panel1;
        private Win.UI.Grid gridBatchImport;
        private Win.UI.DateRange dateInlineDate;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private System.Windows.Forms.Label label8;
        private Win.UI.Label labSewingInline;
        private Win.UI.Label labelNoQuoteHintColor;
        private Win.UI.Label labelNoSuppHint;
        private Win.UI.CheckBox checkBoxReqQtyHasValue;
        private Win.UI.CheckBox checkBoxAssignedSupp;
        private Win.UI.TextBox txtMasterSP;
        private Win.UI.Label label1;
        private Win.UI.CheckBox chkSize;
        private Win.UI.CheckBox chkArticle;
        private Win.UI.Label label2;
        private Win.UI.Label lblFilter;
    }
}
