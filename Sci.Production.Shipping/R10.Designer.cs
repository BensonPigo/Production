namespace Sci.Production.Shipping
{
    partial class R10
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
            this.labelReportContent = new Sci.Win.UI.Label();
            this.labelPulloutDate = new Sci.Win.UI.Label();
            this.labelAPApvDate = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelCustCD = new Sci.Win.UI.Label();
            this.labelDestination = new Sci.Win.UI.Label();
            this.labelShipMode = new Sci.Win.UI.Label();
            this.labelForwarder = new Sci.Win.UI.Label();
            this.labelReportType = new Sci.Win.UI.Label();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioRowMaterial = new Sci.Win.UI.RadioButton();
            this.radioGarment = new Sci.Win.UI.RadioButton();
            this.datePulloutDate = new Sci.Win.UI.DateRange();
            this.dateAPApvDate = new Sci.Win.UI.DateRange();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txtcustcd = new Sci.Production.Class.Txtcustcd();
            this.txtcountryDestination = new Sci.Production.Class.Txtcountry();
            this.txtshipmode = new Sci.Production.Class.Txtshipmode();
            this.txtsubconForwarder = new Sci.Production.Class.TxtsubconNoConfirm();
            this.radioPanel2 = new Sci.Win.UI.RadioPanel();
            this.radioDetailListBySPNoByFeeType = new Sci.Win.UI.RadioButton();
            this.radioDetailListbySPNo = new Sci.Win.UI.RadioButton();
            this.radioExportFeeReport = new Sci.Win.UI.RadioButton();
            this.dateOnBoardDate = new Sci.Win.UI.DateRange();
            this.labelOnBoardDate = new Sci.Win.UI.Label();
            this.lbVoucherDate = new Sci.Win.UI.Label();
            this.dateVoucherDate = new Sci.Win.UI.DateRange();
            this.lbRateType = new Sci.Win.UI.Label();
            this.comboRateType = new Sci.Win.UI.ComboBox();
            this.radioAirPrepaidExpenseReport = new Sci.Win.UI.RadioButton();
            this.radioPanel1.SuspendLayout();
            this.radioPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(436, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(436, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(436, 84);
            // 
            // labelReportContent
            // 
            this.labelReportContent.Location = new System.Drawing.Point(13, 12);
            this.labelReportContent.Name = "labelReportContent";
            this.labelReportContent.Size = new System.Drawing.Size(101, 23);
            this.labelReportContent.TabIndex = 94;
            this.labelReportContent.Text = "Report Content";
            // 
            // labelPulloutDate
            // 
            this.labelPulloutDate.Location = new System.Drawing.Point(13, 68);
            this.labelPulloutDate.Name = "labelPulloutDate";
            this.labelPulloutDate.Size = new System.Drawing.Size(101, 23);
            this.labelPulloutDate.TabIndex = 95;
            this.labelPulloutDate.Text = "Pullout Date";
            // 
            // labelAPApvDate
            // 
            this.labelAPApvDate.Location = new System.Drawing.Point(13, 96);
            this.labelAPApvDate.Name = "labelAPApvDate";
            this.labelAPApvDate.Size = new System.Drawing.Size(101, 23);
            this.labelAPApvDate.TabIndex = 96;
            this.labelAPApvDate.Text = "AP Apv. Date";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(13, 180);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(101, 23);
            this.labelBrand.TabIndex = 97;
            this.labelBrand.Text = "Brand";
            // 
            // labelCustCD
            // 
            this.labelCustCD.Location = new System.Drawing.Point(13, 208);
            this.labelCustCD.Name = "labelCustCD";
            this.labelCustCD.Size = new System.Drawing.Size(101, 23);
            this.labelCustCD.TabIndex = 98;
            this.labelCustCD.Text = "Cust CD";
            // 
            // labelDestination
            // 
            this.labelDestination.Location = new System.Drawing.Point(13, 236);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(101, 23);
            this.labelDestination.TabIndex = 99;
            this.labelDestination.Text = "Destination";
            // 
            // labelShipMode
            // 
            this.labelShipMode.Location = new System.Drawing.Point(13, 264);
            this.labelShipMode.Name = "labelShipMode";
            this.labelShipMode.Size = new System.Drawing.Size(101, 23);
            this.labelShipMode.TabIndex = 100;
            this.labelShipMode.Text = "Ship Mode";
            // 
            // labelForwarder
            // 
            this.labelForwarder.Location = new System.Drawing.Point(13, 292);
            this.labelForwarder.Name = "labelForwarder";
            this.labelForwarder.Size = new System.Drawing.Size(101, 23);
            this.labelForwarder.TabIndex = 101;
            this.labelForwarder.Text = "Forwarder";
            // 
            // labelReportType
            // 
            this.labelReportType.Location = new System.Drawing.Point(13, 348);
            this.labelReportType.Name = "labelReportType";
            this.labelReportType.Size = new System.Drawing.Size(101, 23);
            this.labelReportType.TabIndex = 102;
            this.labelReportType.Text = "Report Type";
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioRowMaterial);
            this.radioPanel1.Controls.Add(this.radioGarment);
            this.radioPanel1.Location = new System.Drawing.Point(118, 10);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(114, 55);
            this.radioPanel1.TabIndex = 0;
            // 
            // radioRowMaterial
            // 
            this.radioRowMaterial.AutoSize = true;
            this.radioRowMaterial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioRowMaterial.Location = new System.Drawing.Point(1, 29);
            this.radioRowMaterial.Name = "radioRowMaterial";
            this.radioRowMaterial.Size = new System.Drawing.Size(107, 21);
            this.radioRowMaterial.TabIndex = 1;
            this.radioRowMaterial.TabStop = true;
            this.radioRowMaterial.Text = "Raw Material";
            this.radioRowMaterial.UseVisualStyleBackColor = true;
            this.radioRowMaterial.CheckedChanged += new System.EventHandler(this.RadioRowMaterial_CheckedChanged);
            // 
            // radioGarment
            // 
            this.radioGarment.AutoSize = true;
            this.radioGarment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioGarment.Location = new System.Drawing.Point(1, 1);
            this.radioGarment.Name = "radioGarment";
            this.radioGarment.Size = new System.Drawing.Size(81, 21);
            this.radioGarment.TabIndex = 0;
            this.radioGarment.TabStop = true;
            this.radioGarment.Text = "Garment";
            this.radioGarment.UseVisualStyleBackColor = true;
            this.radioGarment.CheckedChanged += new System.EventHandler(this.RadioGarment_CheckedChanged);
            // 
            // datePulloutDate
            // 
            // 
            // 
            // 
            this.datePulloutDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.datePulloutDate.DateBox1.Name = "";
            this.datePulloutDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.datePulloutDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.datePulloutDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.datePulloutDate.DateBox2.Name = "";
            this.datePulloutDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.datePulloutDate.DateBox2.TabIndex = 1;
            this.datePulloutDate.IsRequired = false;
            this.datePulloutDate.Location = new System.Drawing.Point(119, 68);
            this.datePulloutDate.Name = "datePulloutDate";
            this.datePulloutDate.Size = new System.Drawing.Size(280, 23);
            this.datePulloutDate.TabIndex = 1;
            // 
            // dateAPApvDate
            // 
            // 
            // 
            // 
            this.dateAPApvDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateAPApvDate.DateBox1.Name = "";
            this.dateAPApvDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateAPApvDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateAPApvDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateAPApvDate.DateBox2.Name = "";
            this.dateAPApvDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateAPApvDate.DateBox2.TabIndex = 1;
            this.dateAPApvDate.IsRequired = false;
            this.dateAPApvDate.Location = new System.Drawing.Point(119, 96);
            this.dateAPApvDate.Name = "dateAPApvDate";
            this.dateAPApvDate.Size = new System.Drawing.Size(280, 23);
            this.dateAPApvDate.TabIndex = 2;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(119, 180);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(81, 23);
            this.txtbrand.TabIndex = 5;
            // 
            // txtcustcd
            // 
            this.txtcustcd.BackColor = System.Drawing.Color.White;
            this.txtcustcd.BrandObjectName = null;
            this.txtcustcd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcustcd.Location = new System.Drawing.Point(119, 208);
            this.txtcustcd.Name = "txtcustcd";
            this.txtcustcd.Size = new System.Drawing.Size(125, 23);
            this.txtcustcd.TabIndex = 6;
            // 
            // txtcountryDestination
            // 
            this.txtcountryDestination.DisplayBox1Binding = "";
            this.txtcountryDestination.Location = new System.Drawing.Point(119, 236);
            this.txtcountryDestination.Name = "txtcountryDestination";
            this.txtcountryDestination.Size = new System.Drawing.Size(232, 22);
            this.txtcountryDestination.TabIndex = 7;
            this.txtcountryDestination.TextBox1Binding = "";
            // 
            // txtshipmode
            // 
            this.txtshipmode.BackColor = System.Drawing.Color.White;
            this.txtshipmode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtshipmode.FormattingEnabled = true;
            this.txtshipmode.IsSupportUnselect = true;
            this.txtshipmode.Location = new System.Drawing.Point(119, 264);
            this.txtshipmode.Name = "txtshipmode";
            this.txtshipmode.OldText = "";
            this.txtshipmode.Size = new System.Drawing.Size(80, 24);
            this.txtshipmode.TabIndex = 8;
            this.txtshipmode.UseFunction = null;
            // 
            // txtsubconForwarder
            // 
            this.txtsubconForwarder.DisplayBox1Binding = "";
            this.txtsubconForwarder.IsIncludeJunk = true;
            this.txtsubconForwarder.IsMisc = false;
            this.txtsubconForwarder.IsShipping = false;
            this.txtsubconForwarder.IsSubcon = false;
            this.txtsubconForwarder.Location = new System.Drawing.Point(119, 292);
            this.txtsubconForwarder.Name = "txtsubconForwarder";
            this.txtsubconForwarder.Size = new System.Drawing.Size(170, 23);
            this.txtsubconForwarder.TabIndex = 9;
            this.txtsubconForwarder.TextBox1Binding = "";
            // 
            // radioPanel2
            // 
            this.radioPanel2.Controls.Add(this.radioAirPrepaidExpenseReport);
            this.radioPanel2.Controls.Add(this.radioDetailListBySPNoByFeeType);
            this.radioPanel2.Controls.Add(this.radioDetailListbySPNo);
            this.radioPanel2.Controls.Add(this.radioExportFeeReport);
            this.radioPanel2.Location = new System.Drawing.Point(118, 348);
            this.radioPanel2.Name = "radioPanel2";
            this.radioPanel2.Size = new System.Drawing.Size(232, 115);
            this.radioPanel2.TabIndex = 11;
            // 
            // radioDetailListBySPNoByFeeType
            // 
            this.radioDetailListBySPNoByFeeType.AutoSize = true;
            this.radioDetailListBySPNoByFeeType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetailListBySPNoByFeeType.Location = new System.Drawing.Point(1, 57);
            this.radioDetailListBySPNoByFeeType.Name = "radioDetailListBySPNoByFeeType";
            this.radioDetailListBySPNoByFeeType.Size = new System.Drawing.Size(220, 21);
            this.radioDetailListBySPNoByFeeType.TabIndex = 2;
            this.radioDetailListBySPNoByFeeType.TabStop = true;
            this.radioDetailListBySPNoByFeeType.Text = "Detail List by SP# by Fee Type";
            this.radioDetailListBySPNoByFeeType.UseVisualStyleBackColor = true;
            // 
            // radioDetailListbySPNo
            // 
            this.radioDetailListbySPNo.AutoSize = true;
            this.radioDetailListbySPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetailListbySPNo.Location = new System.Drawing.Point(1, 29);
            this.radioDetailListbySPNo.Name = "radioDetailListbySPNo";
            this.radioDetailListbySPNo.Size = new System.Drawing.Size(137, 21);
            this.radioDetailListbySPNo.TabIndex = 1;
            this.radioDetailListbySPNo.TabStop = true;
            this.radioDetailListbySPNo.Text = "Detail List by SP#";
            this.radioDetailListbySPNo.UseVisualStyleBackColor = true;
            // 
            // radioExportFeeReport
            // 
            this.radioExportFeeReport.AutoSize = true;
            this.radioExportFeeReport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioExportFeeReport.Location = new System.Drawing.Point(1, 1);
            this.radioExportFeeReport.Name = "radioExportFeeReport";
            this.radioExportFeeReport.Size = new System.Drawing.Size(141, 21);
            this.radioExportFeeReport.TabIndex = 0;
            this.radioExportFeeReport.TabStop = true;
            this.radioExportFeeReport.Text = "Export Fee Report";
            this.radioExportFeeReport.UseVisualStyleBackColor = true;
            // 
            // dateOnBoardDate
            // 
            // 
            // 
            // 
            this.dateOnBoardDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateOnBoardDate.DateBox1.Name = "";
            this.dateOnBoardDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateOnBoardDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateOnBoardDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateOnBoardDate.DateBox2.Name = "";
            this.dateOnBoardDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateOnBoardDate.DateBox2.TabIndex = 1;
            this.dateOnBoardDate.IsRequired = false;
            this.dateOnBoardDate.Location = new System.Drawing.Point(119, 124);
            this.dateOnBoardDate.Name = "dateOnBoardDate";
            this.dateOnBoardDate.Size = new System.Drawing.Size(280, 23);
            this.dateOnBoardDate.TabIndex = 3;
            // 
            // labelOnBoardDate
            // 
            this.labelOnBoardDate.Location = new System.Drawing.Point(13, 124);
            this.labelOnBoardDate.Name = "labelOnBoardDate";
            this.labelOnBoardDate.Size = new System.Drawing.Size(101, 23);
            this.labelOnBoardDate.TabIndex = 112;
            this.labelOnBoardDate.Text = "On Board Date";
            // 
            // lbVoucherDate
            // 
            this.lbVoucherDate.Location = new System.Drawing.Point(13, 152);
            this.lbVoucherDate.Name = "lbVoucherDate";
            this.lbVoucherDate.Size = new System.Drawing.Size(101, 23);
            this.lbVoucherDate.TabIndex = 114;
            this.lbVoucherDate.Text = "Voucher Date";
            // 
            // dateVoucherDate
            // 
            // 
            // 
            // 
            this.dateVoucherDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateVoucherDate.DateBox1.Name = "";
            this.dateVoucherDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateVoucherDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateVoucherDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateVoucherDate.DateBox2.Name = "";
            this.dateVoucherDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateVoucherDate.DateBox2.TabIndex = 1;
            this.dateVoucherDate.IsRequired = false;
            this.dateVoucherDate.Location = new System.Drawing.Point(119, 152);
            this.dateVoucherDate.Name = "dateVoucherDate";
            this.dateVoucherDate.Size = new System.Drawing.Size(280, 23);
            this.dateVoucherDate.TabIndex = 4;
            // 
            // lbRateType
            // 
            this.lbRateType.Location = new System.Drawing.Point(13, 320);
            this.lbRateType.Name = "lbRateType";
            this.lbRateType.Size = new System.Drawing.Size(101, 23);
            this.lbRateType.TabIndex = 116;
            this.lbRateType.Text = "Rate Type";
            // 
            // comboRateType
            // 
            this.comboRateType.BackColor = System.Drawing.Color.White;
            this.comboRateType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboRateType.FormattingEnabled = true;
            this.comboRateType.IsSupportUnselect = true;
            this.comboRateType.Location = new System.Drawing.Point(118, 320);
            this.comboRateType.Name = "comboRateType";
            this.comboRateType.OldText = "";
            this.comboRateType.Size = new System.Drawing.Size(171, 24);
            this.comboRateType.TabIndex = 10;
            // 
            // radioAirPrepaidExpenseReport
            // 
            this.radioAirPrepaidExpenseReport.AutoSize = true;
            this.radioAirPrepaidExpenseReport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioAirPrepaidExpenseReport.Location = new System.Drawing.Point(1, 84);
            this.radioAirPrepaidExpenseReport.Name = "radioAirPrepaidExpenseReport";
            this.radioAirPrepaidExpenseReport.Size = new System.Drawing.Size(201, 21);
            this.radioAirPrepaidExpenseReport.TabIndex = 3;
            this.radioAirPrepaidExpenseReport.TabStop = true;
            this.radioAirPrepaidExpenseReport.Text = "Air Prepaid Expense Report";
            this.radioAirPrepaidExpenseReport.UseVisualStyleBackColor = true;
            // 
            // R10
            // 
            this.ClientSize = new System.Drawing.Size(525, 517);
            this.Controls.Add(this.comboRateType);
            this.Controls.Add(this.lbRateType);
            this.Controls.Add(this.dateVoucherDate);
            this.Controls.Add(this.lbVoucherDate);
            this.Controls.Add(this.dateOnBoardDate);
            this.Controls.Add(this.labelOnBoardDate);
            this.Controls.Add(this.radioPanel2);
            this.Controls.Add(this.txtsubconForwarder);
            this.Controls.Add(this.txtshipmode);
            this.Controls.Add(this.txtcountryDestination);
            this.Controls.Add(this.txtcustcd);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.dateAPApvDate);
            this.Controls.Add(this.datePulloutDate);
            this.Controls.Add(this.radioPanel1);
            this.Controls.Add(this.labelReportType);
            this.Controls.Add(this.labelForwarder);
            this.Controls.Add(this.labelShipMode);
            this.Controls.Add(this.labelDestination);
            this.Controls.Add(this.labelCustCD);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelAPApvDate);
            this.Controls.Add(this.labelPulloutDate);
            this.Controls.Add(this.labelReportContent);
            this.IsSupportToPrint = false;
            this.Name = "R10";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R10. Share Expense Report - Export";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelReportContent, 0);
            this.Controls.SetChildIndex(this.labelPulloutDate, 0);
            this.Controls.SetChildIndex(this.labelAPApvDate, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelCustCD, 0);
            this.Controls.SetChildIndex(this.labelDestination, 0);
            this.Controls.SetChildIndex(this.labelShipMode, 0);
            this.Controls.SetChildIndex(this.labelForwarder, 0);
            this.Controls.SetChildIndex(this.labelReportType, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.Controls.SetChildIndex(this.datePulloutDate, 0);
            this.Controls.SetChildIndex(this.dateAPApvDate, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.txtcustcd, 0);
            this.Controls.SetChildIndex(this.txtcountryDestination, 0);
            this.Controls.SetChildIndex(this.txtshipmode, 0);
            this.Controls.SetChildIndex(this.txtsubconForwarder, 0);
            this.Controls.SetChildIndex(this.radioPanel2, 0);
            this.Controls.SetChildIndex(this.labelOnBoardDate, 0);
            this.Controls.SetChildIndex(this.dateOnBoardDate, 0);
            this.Controls.SetChildIndex(this.lbVoucherDate, 0);
            this.Controls.SetChildIndex(this.dateVoucherDate, 0);
            this.Controls.SetChildIndex(this.lbRateType, 0);
            this.Controls.SetChildIndex(this.comboRateType, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.radioPanel2.ResumeLayout(false);
            this.radioPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelReportContent;
        private Win.UI.Label labelPulloutDate;
        private Win.UI.Label labelAPApvDate;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelCustCD;
        private Win.UI.Label labelDestination;
        private Win.UI.Label labelShipMode;
        private Win.UI.Label labelForwarder;
        private Win.UI.Label labelReportType;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioRowMaterial;
        private Win.UI.RadioButton radioGarment;
        private Win.UI.DateRange datePulloutDate;
        private Win.UI.DateRange dateAPApvDate;
        private Class.Txtbrand txtbrand;
        private Class.Txtcustcd txtcustcd;
        private Class.Txtcountry txtcountryDestination;
        private Class.Txtshipmode txtshipmode;
        private Class.TxtsubconNoConfirm txtsubconForwarder;
        private Win.UI.RadioPanel radioPanel2;
        private Win.UI.RadioButton radioDetailListBySPNoByFeeType;
        private Win.UI.RadioButton radioDetailListbySPNo;
        private Win.UI.RadioButton radioExportFeeReport;
        private Win.UI.DateRange dateOnBoardDate;
        private Win.UI.Label labelOnBoardDate;
        private Win.UI.Label lbVoucherDate;
        private Win.UI.DateRange dateVoucherDate;
        private Win.UI.Label lbRateType;
        private Win.UI.ComboBox comboRateType;
        private Win.UI.RadioButton radioAirPrepaidExpenseReport;
    }
}
