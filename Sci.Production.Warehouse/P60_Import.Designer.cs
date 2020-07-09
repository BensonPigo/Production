namespace Sci.Production.Warehouse
{
    partial class P60_Import
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
            this.txtLocalPO = new Sci.Win.UI.TextBox();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.label9 = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.txtartworktype_ftyCategory = new Sci.Production.Class.Txtartworktype_fty();
            this.labelLocalPO = new Sci.Win.UI.Label();
            this.labelPOIssueDate = new Sci.Win.UI.Label();
            this.datePOIssueDate = new Sci.Win.UI.DateRange();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridImport = new Sci.Win.UI.Grid();
            this.detailBS = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.label1 = new Sci.Win.UI.Label();
            this.dateRangeBuyerDelivery = new Sci.Win.UI.DateRange();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailBS)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(864, 16);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(768, 16);
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
            this.btnFindNow.Location = new System.Drawing.Point(827, 50);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(101, 30);
            this.btnFindNow.TabIndex = 7;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.btnFindNow_Click);
            // 
            // txtLocalPO
            // 
            this.txtLocalPO.BackColor = System.Drawing.Color.White;
            this.txtLocalPO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocalPO.Location = new System.Drawing.Point(107, 19);
            this.txtLocalPO.MaxLength = 13;
            this.txtLocalPO.Name = "txtLocalPO";
            this.txtLocalPO.Size = new System.Drawing.Size(141, 23);
            this.txtLocalPO.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 617);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(966, 53);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dateRangeBuyerDelivery);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.labelCategory);
            this.groupBox1.Controls.Add(this.txtartworktype_ftyCategory);
            this.groupBox1.Controls.Add(this.labelLocalPO);
            this.groupBox1.Controls.Add(this.labelPOIssueDate);
            this.groupBox1.Controls.Add(this.datePOIssueDate);
            this.groupBox1.Controls.Add(this.labelSPNo);
            this.groupBox1.Controls.Add(this.txtSPNoEnd);
            this.groupBox1.Controls.Add(this.txtSPNoStart);
            this.groupBox1.Controls.Add(this.btnFindNow);
            this.groupBox1.Controls.Add(this.txtLocalPO);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(966, 93);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.Control;
            this.label9.Location = new System.Drawing.Point(232, 57);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(19, 23);
            this.label9.TabIndex = 106;
            this.label9.Text = "~";
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(674, 19);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(104, 23);
            this.labelCategory.TabIndex = 105;
            this.labelCategory.Text = "Category";
            // 
            // txtartworktype_ftyCategory
            // 
            this.txtartworktype_ftyCategory.BackColor = System.Drawing.Color.White;
            this.txtartworktype_ftyCategory.CClassify = "";
            this.txtartworktype_ftyCategory.CSubprocess = "";
            this.txtartworktype_ftyCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtartworktype_ftyCategory.Location = new System.Drawing.Point(781, 19);
            this.txtartworktype_ftyCategory.Name = "txtartworktype_ftyCategory";
            this.txtartworktype_ftyCategory.Size = new System.Drawing.Size(140, 23);
            this.txtartworktype_ftyCategory.TabIndex = 3;
            // 
            // labelLocalPO
            // 
            this.labelLocalPO.Location = new System.Drawing.Point(9, 19);
            this.labelLocalPO.Name = "labelLocalPO";
            this.labelLocalPO.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelLocalPO.Size = new System.Drawing.Size(95, 23);
            this.labelLocalPO.TabIndex = 103;
            this.labelLocalPO.Text = "Local PO#";
            this.labelLocalPO.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelPOIssueDate
            // 
            this.labelPOIssueDate.BackColor = System.Drawing.Color.PaleGreen;
            this.labelPOIssueDate.Location = new System.Drawing.Point(266, 19);
            this.labelPOIssueDate.Name = "labelPOIssueDate";
            this.labelPOIssueDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelPOIssueDate.Size = new System.Drawing.Size(104, 23);
            this.labelPOIssueDate.TabIndex = 102;
            this.labelPOIssueDate.Text = "PO Issue Date";
            this.labelPOIssueDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // datePOIssueDate
            // 
            // 
            // 
            // 
            this.datePOIssueDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.datePOIssueDate.DateBox1.Name = "";
            this.datePOIssueDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.datePOIssueDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.datePOIssueDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.datePOIssueDate.DateBox2.Name = "";
            this.datePOIssueDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.datePOIssueDate.DateBox2.TabIndex = 1;
            this.datePOIssueDate.Location = new System.Drawing.Point(373, 19);
            this.datePOIssueDate.Name = "datePOIssueDate";
            this.datePOIssueDate.Size = new System.Drawing.Size(280, 23);
            this.datePOIssueDate.TabIndex = 2;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(9, 54);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSPNo.Size = new System.Drawing.Size(95, 23);
            this.labelSPNo.TabIndex = 100;
            this.labelSPNo.Text = "SP#";
            this.labelSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(254, 54);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(116, 23);
            this.txtSPNoEnd.TabIndex = 5;
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(107, 54);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(122, 23);
            this.txtSPNoStart.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridImport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 93);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(966, 524);
            this.panel1.TabIndex = 20;
            // 
            // gridImport
            // 
            this.gridImport.AllowUserToAddRows = false;
            this.gridImport.AllowUserToDeleteRows = false;
            this.gridImport.AllowUserToResizeRows = false;
            this.gridImport.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridImport.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridImport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridImport.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridImport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridImport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridImport.Location = new System.Drawing.Point(0, 0);
            this.gridImport.Name = "gridImport";
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridImport.RowTemplate.Height = 24;
            this.gridImport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridImport.ShowCellToolTips = false;
            this.gridImport.Size = new System.Drawing.Size(966, 524);
            this.gridImport.TabIndex = 0;
            this.gridImport.TabStop = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.PaleGreen;
            this.label1.Location = new System.Drawing.Point(373, 54);
            this.label1.Name = "label1";
            this.label1.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label1.Size = new System.Drawing.Size(104, 23);
            this.label1.TabIndex = 108;
            this.label1.Text = "Buyer Delivery ";
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateRangeBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateRangeBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeBuyerDelivery.DateBox1.Name = "";
            this.dateRangeBuyerDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeBuyerDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeBuyerDelivery.DateBox2.Name = "";
            this.dateRangeBuyerDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateRangeBuyerDelivery.Location = new System.Drawing.Point(480, 54);
            this.dateRangeBuyerDelivery.Name = "dateRangeBuyerDelivery";
            this.dateRangeBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateRangeBuyerDelivery.TabIndex = 6;
            // 
            // P60_Import
            // 
            this.ClientSize = new System.Drawing.Size(966, 670);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "P60_Import";
            this.Text = "P60. Import Detail";
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailBS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnCancel;
        private Win.UI.Button btnImport;
        private Win.UI.Button btnFindNow;
        private Win.UI.TextBox txtLocalPO;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Panel panel1;
        private Win.UI.Grid gridImport;
        private Win.UI.ListControlBindingSource detailBS;
        private Win.UI.Label labelSPNo;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.Label labelLocalPO;
        private Win.UI.Label labelPOIssueDate;
        private Win.UI.DateRange datePOIssueDate;
        private Win.UI.Label labelCategory;
        private Class.Txtartworktype_fty txtartworktype_ftyCategory;
        private Win.UI.Label label9;
        private Win.UI.Label label1;
        private Win.UI.DateRange dateRangeBuyerDelivery;
    }
}
