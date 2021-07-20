namespace Sci.Production.Subcon
{
    partial class P30_ImportCartonReplacement
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
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.txtfactory1 = new Sci.Production.Class.Txtfactory();
            this.label1 = new Sci.Win.UI.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelApproveDate = new Sci.Win.UI.Label();
            this.dateApproveDate = new Sci.Win.UI.DateRange();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.labelSewingInlineDate = new Sci.Win.UI.Label();
            this.labelIssueDate = new Sci.Win.UI.Label();
            this.dateSewingInlineDate = new Sci.Win.UI.DateRange();
            this.dateIssueDate = new Sci.Win.UI.DateRange();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridImport = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(912, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(816, 16);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 30);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // btnFindNow
            // 
            this.btnFindNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFindNow.Location = new System.Drawing.Point(901, 15);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(101, 30);
            this.btnFindNow.TabIndex = 9;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.BtnFindNow_Click);
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(140, 44);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(122, 23);
            this.txtSPNoStart.TabIndex = 5;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(9, 44);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(128, 23);
            this.labelSPNo.TabIndex = 0;
            this.labelSPNo.Text = "SP#";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 500);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1008, 53);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtfactory1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtbrand);
            this.groupBox1.Controls.Add(this.labelBrand);
            this.groupBox1.Controls.Add(this.labelApproveDate);
            this.groupBox1.Controls.Add(this.dateApproveDate);
            this.groupBox1.Controls.Add(this.labelSCIDelivery);
            this.groupBox1.Controls.Add(this.dateSCIDelivery);
            this.groupBox1.Controls.Add(this.labelSewingInlineDate);
            this.groupBox1.Controls.Add(this.labelIssueDate);
            this.groupBox1.Controls.Add(this.dateSewingInlineDate);
            this.groupBox1.Controls.Add(this.dateIssueDate);
            this.groupBox1.Controls.Add(this.txtSPNoEnd);
            this.groupBox1.Controls.Add(this.btnFindNow);
            this.groupBox1.Controls.Add(this.txtSPNoStart);
            this.groupBox1.Controls.Add(this.labelSPNo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1008, 113);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.BoolFtyGroupList = true;
            this.txtfactory1.FilteMDivision = true;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.IsProduceFty = false;
            this.txtfactory1.IssupportJunk = false;
            this.txtfactory1.Location = new System.Drawing.Point(140, 73);
            this.txtfactory1.MDivision = null;
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.Size = new System.Drawing.Size(66, 23);
            this.txtfactory1.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 23);
            this.label1.TabIndex = 21;
            this.label1.Text = "Factory";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(268, 47);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(22, 17);
            this.label8.TabIndex = 20;
            this.label8.Text = "～";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(298, 73);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(122, 23);
            this.txtbrand.TabIndex = 8;
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(214, 73);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(81, 23);
            this.labelBrand.TabIndex = 19;
            this.labelBrand.Text = "Brand";
            // 
            // labelApproveDate
            // 
            this.labelApproveDate.Location = new System.Drawing.Point(450, 73);
            this.labelApproveDate.Name = "labelApproveDate";
            this.labelApproveDate.Size = new System.Drawing.Size(125, 23);
            this.labelApproveDate.TabIndex = 18;
            this.labelApproveDate.Text = "Approve Date";
            // 
            // dateApproveDate
            // 
            // 
            // 
            // 
            this.dateApproveDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateApproveDate.DateBox1.Name = "";
            this.dateApproveDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateApproveDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateApproveDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateApproveDate.DateBox2.Name = "";
            this.dateApproveDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateApproveDate.DateBox2.TabIndex = 1;
            this.dateApproveDate.Location = new System.Drawing.Point(578, 73);
            this.dateApproveDate.Name = "dateApproveDate";
            this.dateApproveDate.Size = new System.Drawing.Size(280, 23);
            this.dateApproveDate.TabIndex = 7;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(450, 15);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(125, 23);
            this.labelSCIDelivery.TabIndex = 16;
            this.labelSCIDelivery.Text = "SCI Delivery";
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
            this.dateSCIDelivery.Location = new System.Drawing.Point(578, 15);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 2;
            // 
            // labelSewingInlineDate
            // 
            this.labelSewingInlineDate.Location = new System.Drawing.Point(450, 44);
            this.labelSewingInlineDate.Name = "labelSewingInlineDate";
            this.labelSewingInlineDate.Size = new System.Drawing.Size(125, 23);
            this.labelSewingInlineDate.TabIndex = 12;
            this.labelSewingInlineDate.Text = "Sewing Inline Date";
            // 
            // labelIssueDate
            // 
            this.labelIssueDate.Location = new System.Drawing.Point(9, 15);
            this.labelIssueDate.Name = "labelIssueDate";
            this.labelIssueDate.Size = new System.Drawing.Size(128, 23);
            this.labelIssueDate.TabIndex = 11;
            this.labelIssueDate.Text = "Issue Date";
            // 
            // dateSewingInlineDate
            // 
            // 
            // 
            // 
            this.dateSewingInlineDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSewingInlineDate.DateBox1.Name = "";
            this.dateSewingInlineDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSewingInlineDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSewingInlineDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSewingInlineDate.DateBox2.Name = "";
            this.dateSewingInlineDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSewingInlineDate.DateBox2.TabIndex = 1;
            this.dateSewingInlineDate.Location = new System.Drawing.Point(578, 44);
            this.dateSewingInlineDate.Name = "dateSewingInlineDate";
            this.dateSewingInlineDate.Size = new System.Drawing.Size(280, 23);
            this.dateSewingInlineDate.TabIndex = 4;
            // 
            // dateIssueDate
            // 
            // 
            // 
            // 
            this.dateIssueDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateIssueDate.DateBox1.Name = "";
            this.dateIssueDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateIssueDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateIssueDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateIssueDate.DateBox2.Name = "";
            this.dateIssueDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateIssueDate.DateBox2.TabIndex = 1;
            this.dateIssueDate.Location = new System.Drawing.Point(140, 15);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(280, 23);
            this.dateIssueDate.TabIndex = 1;
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(298, 44);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(122, 23);
            this.txtSPNoEnd.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridImport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 113);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 387);
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
            this.gridImport.DataSource = this.listControlBindingSource1;
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
            this.gridImport.Size = new System.Drawing.Size(1008, 387);
            this.gridImport.TabIndex = 0;
            this.gridImport.TabStop = false;
            // 
            // P30_ImportCartonReplacement
            // 
            this.ClientSize = new System.Drawing.Size(1008, 553);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.DefaultControl = "dateEstBookingDate";
            this.Name = "P30_ImportCartonReplacement";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Import Carton Replacement";
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
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
        private Win.UI.Grid gridImport;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Label labelSewingInlineDate;
        private Win.UI.Label labelIssueDate;
        private Win.UI.DateRange dateSewingInlineDate;
        private Win.UI.DateRange dateIssueDate;
        private Class.Txtbrand txtbrand;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelApproveDate;
        private Win.UI.DateRange dateApproveDate;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.DateRange dateSCIDelivery;
        private System.Windows.Forms.Label label8;
        private Class.Txtfactory txtfactory1;
        private Win.UI.Label label1;
    }
}
