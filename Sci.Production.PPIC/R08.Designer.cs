namespace Sci.Production.PPIC
{
    partial class R08
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
            this.labelCreateDate = new Sci.Win.UI.Label();
            this.labelApvDate = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelType = new Sci.Win.UI.Label();
            this.dateCreateDate = new Sci.Win.UI.DateRange();
            this.dateApvDate = new Sci.Win.UI.DateRange();
            this.comboType = new Sci.Win.UI.ComboBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.dateLock = new Sci.Win.UI.DateRange();
            this.dateCfm = new Sci.Win.UI.DateRange();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.cmbStatus = new Sci.Win.UI.ComboBox();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.txtSharedept = new Sci.Win.UI.TextBox();
            this.label5 = new Sci.Win.UI.Label();
            this.cmbReportType = new Sci.Win.UI.ComboBox();
            this.chkJunk = new Sci.Win.UI.CheckBox();
            this.chkReplacementReport = new Sci.Win.UI.CheckBox();
            this.labVoucherdate = new Sci.Win.UI.Label();
            this.dateVoucher = new Sci.Win.UI.DateRange();
            this.comboFactory = new Sci.Production.Class.ComboFactory(this.components);
            this.comboM = new Sci.Production.Class.ComboMDivision(this.components);
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(469, 12);
            this.print.TabIndex = 13;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(469, 48);
            this.toexcel.TabIndex = 14;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(469, 84);
            this.close.TabIndex = 15;
            // 
            // labelCreateDate
            // 
            this.labelCreateDate.Location = new System.Drawing.Point(14, 12);
            this.labelCreateDate.Name = "labelCreateDate";
            this.labelCreateDate.Size = new System.Drawing.Size(99, 23);
            this.labelCreateDate.TabIndex = 94;
            this.labelCreateDate.Text = "Create Date";
            // 
            // labelApvDate
            // 
            this.labelApvDate.Location = new System.Drawing.Point(14, 41);
            this.labelApvDate.Name = "labelApvDate";
            this.labelApvDate.Size = new System.Drawing.Size(99, 23);
            this.labelApvDate.TabIndex = 95;
            this.labelApvDate.Text = "Apv. Date";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(14, 160);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(99, 23);
            this.labelM.TabIndex = 96;
            this.labelM.Text = "M";
            // 
            // labelType
            // 
            this.labelType.Location = new System.Drawing.Point(14, 219);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(99, 23);
            this.labelType.TabIndex = 97;
            this.labelType.Text = "Type";
            // 
            // dateCreateDate
            // 
            // 
            // 
            // 
            this.dateCreateDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCreateDate.DateBox1.Name = "";
            this.dateCreateDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateCreateDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCreateDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateCreateDate.DateBox2.Name = "";
            this.dateCreateDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateCreateDate.DateBox2.TabIndex = 1;
            this.dateCreateDate.IsRequired = false;
            this.dateCreateDate.Location = new System.Drawing.Point(116, 12);
            this.dateCreateDate.Name = "dateCreateDate";
            this.dateCreateDate.Size = new System.Drawing.Size(280, 23);
            this.dateCreateDate.TabIndex = 0;
            // 
            // dateApvDate
            // 
            // 
            // 
            // 
            this.dateApvDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateApvDate.DateBox1.Name = "";
            this.dateApvDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateApvDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateApvDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateApvDate.DateBox2.Name = "";
            this.dateApvDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateApvDate.DateBox2.TabIndex = 1;
            this.dateApvDate.IsRequired = false;
            this.dateApvDate.Location = new System.Drawing.Point(116, 41);
            this.dateApvDate.Name = "dateApvDate";
            this.dateApvDate.Size = new System.Drawing.Size(280, 23);
            this.dateApvDate.TabIndex = 1;
            // 
            // comboType
            // 
            this.comboType.BackColor = System.Drawing.Color.White;
            this.comboType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboType.FormattingEnabled = true;
            this.comboType.IsSupportUnselect = true;
            this.comboType.Location = new System.Drawing.Point(116, 219);
            this.comboType.Name = "comboType";
            this.comboType.OldText = "";
            this.comboType.Size = new System.Drawing.Size(112, 24);
            this.comboType.TabIndex = 7;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(14, 189);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(99, 23);
            this.labelFactory.TabIndex = 102;
            this.labelFactory.Text = "Factory";
            // 
            // dateLock
            // 
            // 
            // 
            // 
            this.dateLock.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateLock.DateBox1.Name = "";
            this.dateLock.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateLock.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateLock.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateLock.DateBox2.Name = "";
            this.dateLock.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateLock.DateBox2.TabIndex = 1;
            this.dateLock.Location = new System.Drawing.Point(116, 70);
            this.dateLock.Name = "dateLock";
            this.dateLock.Size = new System.Drawing.Size(280, 23);
            this.dateLock.TabIndex = 2;
            // 
            // dateCfm
            // 
            // 
            // 
            // 
            this.dateCfm.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCfm.DateBox1.Name = "";
            this.dateCfm.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateCfm.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCfm.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateCfm.DateBox2.Name = "";
            this.dateCfm.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateCfm.DateBox2.TabIndex = 1;
            this.dateCfm.Location = new System.Drawing.Point(116, 99);
            this.dateCfm.Name = "dateCfm";
            this.dateCfm.Size = new System.Drawing.Size(280, 23);
            this.dateCfm.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 23);
            this.label1.TabIndex = 105;
            this.label1.Text = "Lock Date";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(14, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 23);
            this.label2.TabIndex = 106;
            this.label2.Text = "Cfm dept. date";
            // 
            // cmbStatus
            // 
            this.cmbStatus.BackColor = System.Drawing.Color.White;
            this.cmbStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.IsSupportUnselect = true;
            this.cmbStatus.Location = new System.Drawing.Point(116, 249);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.OldText = "";
            this.cmbStatus.Size = new System.Drawing.Size(112, 24);
            this.cmbStatus.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(14, 249);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 23);
            this.label3.TabIndex = 108;
            this.label3.Text = "Status";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(14, 279);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 23);
            this.label4.TabIndex = 109;
            this.label4.Text = "Share dept.";
            // 
            // txtSharedept
            // 
            this.txtSharedept.BackColor = System.Drawing.Color.White;
            this.txtSharedept.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSharedept.Location = new System.Drawing.Point(116, 279);
            this.txtSharedept.Name = "txtSharedept";
            this.txtSharedept.Size = new System.Drawing.Size(152, 23);
            this.txtSharedept.TabIndex = 9;
            this.txtSharedept.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSharedept_PopUp);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(14, 309);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 23);
            this.label5.TabIndex = 111;
            this.label5.Text = "Report Type";
            // 
            // cmbReportType
            // 
            this.cmbReportType.BackColor = System.Drawing.Color.White;
            this.cmbReportType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbReportType.FormattingEnabled = true;
            this.cmbReportType.IsSupportUnselect = true;
            this.cmbReportType.Location = new System.Drawing.Point(116, 308);
            this.cmbReportType.Name = "cmbReportType";
            this.cmbReportType.OldText = "";
            this.cmbReportType.Size = new System.Drawing.Size(136, 24);
            this.cmbReportType.TabIndex = 10;
            this.cmbReportType.SelectedIndexChanged += new System.EventHandler(this.CmbReportType_SelectedIndexChanged);
            // 
            // chkJunk
            // 
            this.chkJunk.AutoSize = true;
            this.chkJunk.Enabled = false;
            this.chkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkJunk.Location = new System.Drawing.Point(14, 344);
            this.chkJunk.Name = "chkJunk";
            this.chkJunk.Size = new System.Drawing.Size(106, 21);
            this.chkJunk.TabIndex = 11;
            this.chkJunk.Text = "Include Junk";
            this.chkJunk.UseVisualStyleBackColor = true;
            // 
            // chkReplacementReport
            // 
            this.chkReplacementReport.AutoSize = true;
            this.chkReplacementReport.Enabled = false;
            this.chkReplacementReport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkReplacementReport.Location = new System.Drawing.Point(14, 371);
            this.chkReplacementReport.Name = "chkReplacementReport";
            this.chkReplacementReport.Size = new System.Drawing.Size(525, 21);
            this.chkReplacementReport.TabIndex = 12;
            this.chkReplacementReport.Text = "Outstanding(Replacement report not yet locked and all material isn\'t mark junk)";
            this.chkReplacementReport.UseVisualStyleBackColor = true;
            // 
            // labVoucherdate
            // 
            this.labVoucherdate.Location = new System.Drawing.Point(14, 129);
            this.labVoucherdate.Name = "labVoucherdate";
            this.labVoucherdate.Size = new System.Drawing.Size(99, 23);
            this.labVoucherdate.TabIndex = 113;
            this.labVoucherdate.Text = "Voucher date";
            // 
            // dateVoucher
            // 
            // 
            // 
            // 
            this.dateVoucher.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateVoucher.DateBox1.Name = "";
            this.dateVoucher.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateVoucher.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateVoucher.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateVoucher.DateBox2.Name = "";
            this.dateVoucher.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateVoucher.DateBox2.TabIndex = 1;
            this.dateVoucher.Location = new System.Drawing.Point(116, 129);
            this.dateVoucher.Name = "dateVoucher";
            this.dateVoucher.Size = new System.Drawing.Size(280, 23);
            this.dateVoucher.TabIndex = 4;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.FilteMDivision = true;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IssupportJunk = false;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(115, 189);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(67, 24);
            this.comboFactory.TabIndex = 565;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(116, 160);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(66, 24);
            this.comboM.TabIndex = 564;
            // 
            // R08
            // 
            this.ClientSize = new System.Drawing.Size(561, 434);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.labVoucherdate);
            this.Controls.Add(this.dateVoucher);
            this.Controls.Add(this.chkReplacementReport);
            this.Controls.Add(this.chkJunk);
            this.Controls.Add(this.cmbReportType);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSharedept);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateCfm);
            this.Controls.Add(this.dateLock);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.comboType);
            this.Controls.Add(this.dateApvDate);
            this.Controls.Add(this.dateCreateDate);
            this.Controls.Add(this.labelType);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelApvDate);
            this.Controls.Add(this.labelCreateDate);
            this.DefaultControl = "dateCreateDate";
            this.DefaultControlForEdit = "dateCreateDate";
            this.IsSupportToPrint = false;
            this.Name = "R08";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R08. Replacement Report List";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelCreateDate, 0);
            this.Controls.SetChildIndex(this.labelApvDate, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelType, 0);
            this.Controls.SetChildIndex(this.dateCreateDate, 0);
            this.Controls.SetChildIndex(this.dateApvDate, 0);
            this.Controls.SetChildIndex(this.comboType, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.dateLock, 0);
            this.Controls.SetChildIndex(this.dateCfm, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cmbStatus, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.txtSharedept, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.cmbReportType, 0);
            this.Controls.SetChildIndex(this.chkJunk, 0);
            this.Controls.SetChildIndex(this.chkReplacementReport, 0);
            this.Controls.SetChildIndex(this.dateVoucher, 0);
            this.Controls.SetChildIndex(this.labVoucherdate, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelCreateDate;
        private Win.UI.Label labelApvDate;
        private Win.UI.Label labelM;
        private Win.UI.Label labelType;
        private Win.UI.DateRange dateCreateDate;
        private Win.UI.DateRange dateApvDate;
        private Win.UI.ComboBox comboType;
        private Win.UI.Label labelFactory;
        private Win.UI.DateRange dateLock;
        private Win.UI.DateRange dateCfm;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.ComboBox cmbStatus;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.TextBox txtSharedept;
        private Win.UI.Label label5;
        private Win.UI.ComboBox cmbReportType;
        private Win.UI.CheckBox chkJunk;
        private Win.UI.CheckBox chkReplacementReport;
        private Win.UI.Label labVoucherdate;
        private Win.UI.DateRange dateVoucher;
        private Class.ComboFactory comboFactory;
        private Class.ComboMDivision comboM;
    }
}
