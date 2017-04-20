namespace Sci.Production.Shipping
{
    partial class R01
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
            this.labelShipper = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelInvoiceDate = new Sci.Win.UI.Label();
            this.labelETD = new Sci.Win.UI.Label();
            this.labelShippingMode = new Sci.Win.UI.Label();
            this.labelShipmentTerm = new Sci.Win.UI.Label();
            this.labelDestination = new Sci.Win.UI.Label();
            this.labelStatus = new Sci.Win.UI.Label();
            this.labelReportType = new Sci.Win.UI.Label();
            this.comboShipper = new Sci.Win.UI.ComboBox();
            this.txtbrand = new Sci.Production.Class.txtbrand();
            this.dateInvoiceDate = new Sci.Win.UI.DateRange();
            this.dateETD = new Sci.Win.UI.DateRange();
            this.txtshipmodeShippingMode = new Sci.Production.Class.txtshipmode();
            this.txtshiptermShipmentTerm = new Sci.Production.Class.txtshipterm();
            this.txtcountryDestination = new Sci.Production.Class.txtcountry();
            this.comboStatus = new Sci.Win.UI.ComboBox();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioDetailList = new Sci.Win.UI.RadioButton();
            this.radioMainList = new Sci.Win.UI.RadioButton();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(430, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(430, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(430, 84);
            // 
            // labelShipper
            // 
            this.labelShipper.Lines = 0;
            this.labelShipper.Location = new System.Drawing.Point(13, 12);
            this.labelShipper.Name = "labelShipper";
            this.labelShipper.Size = new System.Drawing.Size(98, 23);
            this.labelShipper.TabIndex = 94;
            this.labelShipper.Text = "Shipper";
            // 
            // labelBrand
            // 
            this.labelBrand.Lines = 0;
            this.labelBrand.Location = new System.Drawing.Point(13, 48);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(98, 23);
            this.labelBrand.TabIndex = 95;
            this.labelBrand.Text = "Brand";
            // 
            // labelInvoiceDate
            // 
            this.labelInvoiceDate.Lines = 0;
            this.labelInvoiceDate.Location = new System.Drawing.Point(13, 84);
            this.labelInvoiceDate.Name = "labelInvoiceDate";
            this.labelInvoiceDate.Size = new System.Drawing.Size(98, 23);
            this.labelInvoiceDate.TabIndex = 96;
            this.labelInvoiceDate.Text = "Invoice Date";
            // 
            // labelETD
            // 
            this.labelETD.Lines = 0;
            this.labelETD.Location = new System.Drawing.Point(13, 120);
            this.labelETD.Name = "labelETD";
            this.labelETD.Size = new System.Drawing.Size(98, 23);
            this.labelETD.TabIndex = 97;
            this.labelETD.Text = "ETD";
            // 
            // labelShippingMode
            // 
            this.labelShippingMode.Lines = 0;
            this.labelShippingMode.Location = new System.Drawing.Point(13, 156);
            this.labelShippingMode.Name = "labelShippingMode";
            this.labelShippingMode.Size = new System.Drawing.Size(98, 23);
            this.labelShippingMode.TabIndex = 98;
            this.labelShippingMode.Text = "Shipping Mode";
            // 
            // labelShipmentTerm
            // 
            this.labelShipmentTerm.Lines = 0;
            this.labelShipmentTerm.Location = new System.Drawing.Point(13, 192);
            this.labelShipmentTerm.Name = "labelShipmentTerm";
            this.labelShipmentTerm.Size = new System.Drawing.Size(98, 23);
            this.labelShipmentTerm.TabIndex = 99;
            this.labelShipmentTerm.Text = "Shipment Term";
            // 
            // labelDestination
            // 
            this.labelDestination.Lines = 0;
            this.labelDestination.Location = new System.Drawing.Point(13, 228);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(98, 23);
            this.labelDestination.TabIndex = 100;
            this.labelDestination.Text = "Destination";
            // 
            // labelStatus
            // 
            this.labelStatus.Lines = 0;
            this.labelStatus.Location = new System.Drawing.Point(13, 264);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(98, 23);
            this.labelStatus.TabIndex = 101;
            this.labelStatus.Text = "Status";
            // 
            // labelReportType
            // 
            this.labelReportType.Lines = 0;
            this.labelReportType.Location = new System.Drawing.Point(13, 300);
            this.labelReportType.Name = "labelReportType";
            this.labelReportType.Size = new System.Drawing.Size(98, 23);
            this.labelReportType.TabIndex = 102;
            this.labelReportType.Text = "Report Type";
            // 
            // comboShipper
            // 
            this.comboShipper.BackColor = System.Drawing.Color.White;
            this.comboShipper.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboShipper.FormattingEnabled = true;
            this.comboShipper.IsSupportUnselect = true;
            this.comboShipper.Location = new System.Drawing.Point(114, 11);
            this.comboShipper.Name = "comboShipper";
            this.comboShipper.Size = new System.Drawing.Size(121, 24);
            this.comboShipper.TabIndex = 103;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(114, 48);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(86, 23);
            this.txtbrand.TabIndex = 104;
            // 
            // dateInvoiceDate
            // 
            this.dateInvoiceDate.IsRequired = false;
            this.dateInvoiceDate.Location = new System.Drawing.Point(115, 84);
            this.dateInvoiceDate.Name = "dateInvoiceDate";
            this.dateInvoiceDate.Size = new System.Drawing.Size(280, 23);
            this.dateInvoiceDate.TabIndex = 105;
            // 
            // dateETD
            // 
            this.dateETD.IsRequired = false;
            this.dateETD.Location = new System.Drawing.Point(114, 120);
            this.dateETD.Name = "dateETD";
            this.dateETD.Size = new System.Drawing.Size(280, 23);
            this.dateETD.TabIndex = 106;
            // 
            // txtshipmodeShippingMode
            // 
            this.txtshipmodeShippingMode.BackColor = System.Drawing.Color.White;
            this.txtshipmodeShippingMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtshipmodeShippingMode.FormattingEnabled = true;
            this.txtshipmodeShippingMode.IsSupportUnselect = true;
            this.txtshipmodeShippingMode.Location = new System.Drawing.Point(114, 156);
            this.txtshipmodeShippingMode.Name = "txtshipmodeShippingMode";
            this.txtshipmodeShippingMode.Size = new System.Drawing.Size(80, 24);
            this.txtshipmodeShippingMode.TabIndex = 107;
            this.txtshipmodeShippingMode.UseFunction = "ORDER";
            // 
            // txtshiptermShipmentTerm
            // 
            this.txtshiptermShipmentTerm.BackColor = System.Drawing.Color.White;
            this.txtshiptermShipmentTerm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtshiptermShipmentTerm.Location = new System.Drawing.Point(114, 192);
            this.txtshiptermShipmentTerm.Name = "txtshiptermShipmentTerm";
            this.txtshiptermShipmentTerm.Size = new System.Drawing.Size(50, 23);
            this.txtshiptermShipmentTerm.TabIndex = 108;
            // 
            // txtcountryDestination
            // 
            this.txtcountryDestination.DisplayBox1Binding = "";
            this.txtcountryDestination.Location = new System.Drawing.Point(115, 228);
            this.txtcountryDestination.Name = "txtcountryDestination";
            this.txtcountryDestination.Size = new System.Drawing.Size(232, 22);
            this.txtcountryDestination.TabIndex = 109;
            this.txtcountryDestination.TextBox1Binding = "";
            // 
            // comboStatus
            // 
            this.comboStatus.BackColor = System.Drawing.Color.White;
            this.comboStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStatus.FormattingEnabled = true;
            this.comboStatus.IsSupportUnselect = true;
            this.comboStatus.Location = new System.Drawing.Point(114, 263);
            this.comboStatus.Name = "comboStatus";
            this.comboStatus.Size = new System.Drawing.Size(121, 24);
            this.comboStatus.TabIndex = 110;
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioDetailList);
            this.radioPanel1.Controls.Add(this.radioMainList);
            this.radioPanel1.Location = new System.Drawing.Point(114, 299);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(248, 31);
            this.radioPanel1.TabIndex = 111;
            // 
            // radioDetailList
            // 
            this.radioDetailList.AutoSize = true;
            this.radioDetailList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetailList.Location = new System.Drawing.Point(138, 3);
            this.radioDetailList.Name = "radioDetailList";
            this.radioDetailList.Size = new System.Drawing.Size(88, 21);
            this.radioDetailList.TabIndex = 1;
            this.radioDetailList.TabStop = true;
            this.radioDetailList.Text = "Detail List";
            this.radioDetailList.UseVisualStyleBackColor = true;
            // 
            // radioMainList
            // 
            this.radioMainList.AutoSize = true;
            this.radioMainList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioMainList.Location = new System.Drawing.Point(4, 3);
            this.radioMainList.Name = "radioMainList";
            this.radioMainList.Size = new System.Drawing.Size(82, 21);
            this.radioMainList.TabIndex = 0;
            this.radioMainList.TabStop = true;
            this.radioMainList.Text = "Main List";
            this.radioMainList.UseVisualStyleBackColor = true;
            // 
            // R01
            // 
            this.ClientSize = new System.Drawing.Size(522, 360);
            this.Controls.Add(this.radioPanel1);
            this.Controls.Add(this.comboStatus);
            this.Controls.Add(this.txtcountryDestination);
            this.Controls.Add(this.txtshiptermShipmentTerm);
            this.Controls.Add(this.txtshipmodeShippingMode);
            this.Controls.Add(this.dateETD);
            this.Controls.Add(this.dateInvoiceDate);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.comboShipper);
            this.Controls.Add(this.labelReportType);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelDestination);
            this.Controls.Add(this.labelShipmentTerm);
            this.Controls.Add(this.labelShippingMode);
            this.Controls.Add(this.labelETD);
            this.Controls.Add(this.labelInvoiceDate);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelShipper);
            this.IsSupportToPrint = false;
            this.Name = "R01";
            this.Text = "R01. Garment Booking Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelShipper, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelInvoiceDate, 0);
            this.Controls.SetChildIndex(this.labelETD, 0);
            this.Controls.SetChildIndex(this.labelShippingMode, 0);
            this.Controls.SetChildIndex(this.labelShipmentTerm, 0);
            this.Controls.SetChildIndex(this.labelDestination, 0);
            this.Controls.SetChildIndex(this.labelStatus, 0);
            this.Controls.SetChildIndex(this.labelReportType, 0);
            this.Controls.SetChildIndex(this.comboShipper, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.dateInvoiceDate, 0);
            this.Controls.SetChildIndex(this.dateETD, 0);
            this.Controls.SetChildIndex(this.txtshipmodeShippingMode, 0);
            this.Controls.SetChildIndex(this.txtshiptermShipmentTerm, 0);
            this.Controls.SetChildIndex(this.txtcountryDestination, 0);
            this.Controls.SetChildIndex(this.comboStatus, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelShipper;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelInvoiceDate;
        private Win.UI.Label labelETD;
        private Win.UI.Label labelShippingMode;
        private Win.UI.Label labelShipmentTerm;
        private Win.UI.Label labelDestination;
        private Win.UI.Label labelStatus;
        private Win.UI.Label labelReportType;
        private Win.UI.ComboBox comboShipper;
        private Class.txtbrand txtbrand;
        private Win.UI.DateRange dateInvoiceDate;
        private Win.UI.DateRange dateETD;
        private Class.txtshipmode txtshipmodeShippingMode;
        private Class.txtshipterm txtshiptermShipmentTerm;
        private Class.txtcountry txtcountryDestination;
        private Win.UI.ComboBox comboStatus;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioDetailList;
        private Win.UI.RadioButton radioMainList;
    }
}
