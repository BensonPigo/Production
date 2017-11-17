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
            this.txtbrand = new Sci.Production.Class.txtbrand();
            this.txtcustcd = new Sci.Production.Class.txtcustcd();
            this.txtcountryDestination = new Sci.Production.Class.txtcountry();
            this.txtshipmode = new Sci.Production.Class.txtshipmode();
            this.txtsubconForwarder = new Sci.Production.Class.txtsubcon();
            this.radioPanel2 = new Sci.Win.UI.RadioPanel();
            this.radioDetailListBySPNoByFeeType = new Sci.Win.UI.RadioButton();
            this.radioDetailListbySPNo = new Sci.Win.UI.RadioButton();
            this.radioExportFeeReport = new Sci.Win.UI.RadioButton();
            this.radioPanel1.SuspendLayout();
            this.radioPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(433, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(433, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(433, 84);
            // 
            // labelReportContent
            // 
            this.labelReportContent.Lines = 0;
            this.labelReportContent.Location = new System.Drawing.Point(13, 12);
            this.labelReportContent.Name = "labelReportContent";
            this.labelReportContent.Size = new System.Drawing.Size(101, 23);
            this.labelReportContent.TabIndex = 94;
            this.labelReportContent.Text = "Report Content";
            // 
            // labelPulloutDate
            // 
            this.labelPulloutDate.Lines = 0;
            this.labelPulloutDate.Location = new System.Drawing.Point(13, 80);
            this.labelPulloutDate.Name = "labelPulloutDate";
            this.labelPulloutDate.Size = new System.Drawing.Size(101, 23);
            this.labelPulloutDate.TabIndex = 95;
            this.labelPulloutDate.Text = "Pullout Date";
            // 
            // labelAPApvDate
            // 
            this.labelAPApvDate.Lines = 0;
            this.labelAPApvDate.Location = new System.Drawing.Point(13, 116);
            this.labelAPApvDate.Name = "labelAPApvDate";
            this.labelAPApvDate.Size = new System.Drawing.Size(101, 23);
            this.labelAPApvDate.TabIndex = 96;
            this.labelAPApvDate.Text = "AP Apv. Date";
            // 
            // labelBrand
            // 
            this.labelBrand.Lines = 0;
            this.labelBrand.Location = new System.Drawing.Point(13, 152);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(101, 23);
            this.labelBrand.TabIndex = 97;
            this.labelBrand.Text = "Brand";
            // 
            // labelCustCD
            // 
            this.labelCustCD.Lines = 0;
            this.labelCustCD.Location = new System.Drawing.Point(13, 189);
            this.labelCustCD.Name = "labelCustCD";
            this.labelCustCD.Size = new System.Drawing.Size(101, 23);
            this.labelCustCD.TabIndex = 98;
            this.labelCustCD.Text = "Cust CD";
            // 
            // labelDestination
            // 
            this.labelDestination.Lines = 0;
            this.labelDestination.Location = new System.Drawing.Point(13, 225);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(101, 23);
            this.labelDestination.TabIndex = 99;
            this.labelDestination.Text = "Destination";
            // 
            // labelShipMode
            // 
            this.labelShipMode.Lines = 0;
            this.labelShipMode.Location = new System.Drawing.Point(13, 261);
            this.labelShipMode.Name = "labelShipMode";
            this.labelShipMode.Size = new System.Drawing.Size(101, 23);
            this.labelShipMode.TabIndex = 100;
            this.labelShipMode.Text = "Ship Mode";
            // 
            // labelForwarder
            // 
            this.labelForwarder.Lines = 0;
            this.labelForwarder.Location = new System.Drawing.Point(13, 297);
            this.labelForwarder.Name = "labelForwarder";
            this.labelForwarder.Size = new System.Drawing.Size(101, 23);
            this.labelForwarder.TabIndex = 101;
            this.labelForwarder.Text = "Forwarder";
            // 
            // labelReportType
            // 
            this.labelReportType.Lines = 0;
            this.labelReportType.Location = new System.Drawing.Point(13, 334);
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
            this.radioPanel1.TabIndex = 103;
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
            this.radioRowMaterial.Text = "Row Material";
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
            this.datePulloutDate.IsRequired = false;
            this.datePulloutDate.Location = new System.Drawing.Point(118, 80);
            this.datePulloutDate.Name = "datePulloutDate";
            this.datePulloutDate.Size = new System.Drawing.Size(280, 23);
            this.datePulloutDate.TabIndex = 104;
            // 
            // dateAPApvDate
            // 
            this.dateAPApvDate.IsRequired = false;
            this.dateAPApvDate.Location = new System.Drawing.Point(118, 116);
            this.dateAPApvDate.Name = "dateAPApvDate";
            this.dateAPApvDate.Size = new System.Drawing.Size(280, 23);
            this.dateAPApvDate.TabIndex = 105;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(119, 152);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(81, 23);
            this.txtbrand.TabIndex = 106;
            // 
            // txtcustcd
            // 
            this.txtcustcd.BackColor = System.Drawing.Color.White;
            this.txtcustcd.BrandObjectName = null;
            this.txtcustcd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcustcd.Location = new System.Drawing.Point(119, 189);
            this.txtcustcd.Name = "txtcustcd";
            this.txtcustcd.Size = new System.Drawing.Size(125, 23);
            this.txtcustcd.TabIndex = 107;
            // 
            // txtcountryDestination
            // 
            this.txtcountryDestination.DisplayBox1Binding = "";
            this.txtcountryDestination.Location = new System.Drawing.Point(118, 225);
            this.txtcountryDestination.Name = "txtcountryDestination";
            this.txtcountryDestination.Size = new System.Drawing.Size(232, 22);
            this.txtcountryDestination.TabIndex = 108;
            this.txtcountryDestination.TextBox1Binding = "";
            // 
            // txtshipmode
            // 
            this.txtshipmode.BackColor = System.Drawing.Color.White;
            this.txtshipmode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtshipmode.FormattingEnabled = true;
            this.txtshipmode.IsSupportUnselect = true;
            this.txtshipmode.Location = new System.Drawing.Point(118, 261);
            this.txtshipmode.Name = "txtshipmode";
            this.txtshipmode.Size = new System.Drawing.Size(80, 24);
            this.txtshipmode.TabIndex = 109;
            this.txtshipmode.UseFunction = null;
            // 
            // txtsubconForwarder
            // 
            this.txtsubconForwarder.DisplayBox1Binding = "";
            this.txtsubconForwarder.IsIncludeJunk = true;
            this.txtsubconForwarder.Location = new System.Drawing.Point(119, 297);
            this.txtsubconForwarder.Name = "txtsubconForwarder";
            this.txtsubconForwarder.Size = new System.Drawing.Size(170, 23);
            this.txtsubconForwarder.TabIndex = 110;
            this.txtsubconForwarder.TextBox1Binding = "";
            // 
            // radioPanel2
            // 
            this.radioPanel2.Controls.Add(this.radioDetailListBySPNoByFeeType);
            this.radioPanel2.Controls.Add(this.radioDetailListbySPNo);
            this.radioPanel2.Controls.Add(this.radioExportFeeReport);
            this.radioPanel2.Location = new System.Drawing.Point(118, 332);
            this.radioPanel2.Name = "radioPanel2";
            this.radioPanel2.Size = new System.Drawing.Size(232, 84);
            this.radioPanel2.TabIndex = 111;
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
            // R10
            // 
            this.ClientSize = new System.Drawing.Size(525, 451);
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
            this.Text = "R10. Share Expense Report - Export";
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
        private Class.txtbrand txtbrand;
        private Class.txtcustcd txtcustcd;
        private Class.txtcountry txtcountryDestination;
        private Class.txtshipmode txtshipmode;
        private Class.txtsubcon txtsubconForwarder;
        private Win.UI.RadioPanel radioPanel2;
        private Win.UI.RadioButton radioDetailListBySPNoByFeeType;
        private Win.UI.RadioButton radioDetailListbySPNo;
        private Win.UI.RadioButton radioExportFeeReport;
    }
}
