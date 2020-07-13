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
            this.dateInvoiceDate = new Sci.Win.UI.DateRange();
            this.dateETD = new Sci.Win.UI.DateRange();
            this.comboStatus = new Sci.Win.UI.ComboBox();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioDetailList = new Sci.Win.UI.RadioButton();
            this.radioMainList = new Sci.Win.UI.RadioButton();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.dateFCR = new Sci.Win.UI.DateRange();
            this.dateCutoff = new Sci.Win.UI.DateRange();
            this.dateConfirm = new Sci.Win.UI.DateRange();
            this.txtcountryDestination = new Sci.Production.Class.Txtcountry();
            this.txtshiptermShipmentTerm = new Sci.Production.Class.Txtshipterm();
            this.txtshipmodeShippingMode = new Sci.Production.Class.Txtshipmode();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.label4 = new Sci.Win.UI.Label();
            this.dateDelivery = new Sci.Win.UI.DateRange();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(492, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(492, 48);
            this.toexcel.TabIndex = 15;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(492, 84);
            this.close.TabIndex = 16;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(446, 120);
            this.buttonCustomized.Visible = true;
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(472, 156);
            this.checkUseCustomized.Visible = true;
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(472, 183);
            this.txtVersion.Visible = true;
            // 
            // labelShipper
            // 
            this.labelShipper.Location = new System.Drawing.Point(13, 12);
            this.labelShipper.Name = "labelShipper";
            this.labelShipper.Size = new System.Drawing.Size(115, 23);
            this.labelShipper.TabIndex = 94;
            this.labelShipper.Text = "Shipper";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(13, 46);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(115, 23);
            this.labelBrand.TabIndex = 95;
            this.labelBrand.Text = "Brand";
            // 
            // labelInvoiceDate
            // 
            this.labelInvoiceDate.Location = new System.Drawing.Point(13, 80);
            this.labelInvoiceDate.Name = "labelInvoiceDate";
            this.labelInvoiceDate.Size = new System.Drawing.Size(115, 23);
            this.labelInvoiceDate.TabIndex = 96;
            this.labelInvoiceDate.Text = "Invoice Date";
            // 
            // labelETD
            // 
            this.labelETD.Location = new System.Drawing.Point(13, 148);
            this.labelETD.Name = "labelETD";
            this.labelETD.Size = new System.Drawing.Size(115, 23);
            this.labelETD.TabIndex = 97;
            this.labelETD.Text = "ETD";
            // 
            // labelShippingMode
            // 
            this.labelShippingMode.Location = new System.Drawing.Point(13, 284);
            this.labelShippingMode.Name = "labelShippingMode";
            this.labelShippingMode.Size = new System.Drawing.Size(115, 23);
            this.labelShippingMode.TabIndex = 98;
            this.labelShippingMode.Text = "Shipping Mode";
            // 
            // labelShipmentTerm
            // 
            this.labelShipmentTerm.Location = new System.Drawing.Point(13, 318);
            this.labelShipmentTerm.Name = "labelShipmentTerm";
            this.labelShipmentTerm.Size = new System.Drawing.Size(115, 23);
            this.labelShipmentTerm.TabIndex = 99;
            this.labelShipmentTerm.Text = "Shipment Term";
            // 
            // labelDestination
            // 
            this.labelDestination.Location = new System.Drawing.Point(13, 352);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(115, 23);
            this.labelDestination.TabIndex = 100;
            this.labelDestination.Text = "Destination";
            // 
            // labelStatus
            // 
            this.labelStatus.Location = new System.Drawing.Point(13, 386);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(115, 23);
            this.labelStatus.TabIndex = 101;
            this.labelStatus.Text = "Status";
            // 
            // labelReportType
            // 
            this.labelReportType.Location = new System.Drawing.Point(13, 420);
            this.labelReportType.Name = "labelReportType";
            this.labelReportType.Size = new System.Drawing.Size(115, 23);
            this.labelReportType.TabIndex = 102;
            this.labelReportType.Text = "Report Type";
            // 
            // comboShipper
            // 
            this.comboShipper.BackColor = System.Drawing.Color.White;
            this.comboShipper.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboShipper.FormattingEnabled = true;
            this.comboShipper.IsSupportUnselect = true;
            this.comboShipper.Location = new System.Drawing.Point(135, 11);
            this.comboShipper.Name = "comboShipper";
            this.comboShipper.OldText = "";
            this.comboShipper.Size = new System.Drawing.Size(121, 24);
            this.comboShipper.TabIndex = 1;
            // 
            // dateInvoiceDate
            // 
            // 
            // 
            // 
            this.dateInvoiceDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateInvoiceDate.DateBox1.Name = "";
            this.dateInvoiceDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateInvoiceDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateInvoiceDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateInvoiceDate.DateBox2.Name = "";
            this.dateInvoiceDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateInvoiceDate.DateBox2.TabIndex = 1;
            this.dateInvoiceDate.IsRequired = false;
            this.dateInvoiceDate.Location = new System.Drawing.Point(135, 80);
            this.dateInvoiceDate.Name = "dateInvoiceDate";
            this.dateInvoiceDate.Size = new System.Drawing.Size(280, 23);
            this.dateInvoiceDate.TabIndex = 3;
            // 
            // dateETD
            // 
            // 
            // 
            // 
            this.dateETD.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateETD.DateBox1.Name = "";
            this.dateETD.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateETD.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateETD.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateETD.DateBox2.Name = "";
            this.dateETD.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateETD.DateBox2.TabIndex = 1;
            this.dateETD.IsRequired = false;
            this.dateETD.Location = new System.Drawing.Point(134, 148);
            this.dateETD.Name = "dateETD";
            this.dateETD.Size = new System.Drawing.Size(280, 23);
            this.dateETD.TabIndex = 5;
            // 
            // comboStatus
            // 
            this.comboStatus.BackColor = System.Drawing.Color.White;
            this.comboStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStatus.FormattingEnabled = true;
            this.comboStatus.IsSupportUnselect = true;
            this.comboStatus.Location = new System.Drawing.Point(135, 385);
            this.comboStatus.Name = "comboStatus";
            this.comboStatus.OldText = "";
            this.comboStatus.Size = new System.Drawing.Size(121, 24);
            this.comboStatus.TabIndex = 12;
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioDetailList);
            this.radioPanel1.Controls.Add(this.radioMainList);
            this.radioPanel1.Location = new System.Drawing.Point(134, 422);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(248, 31);
            this.radioPanel1.TabIndex = 13;
            // 
            // radioDetailList
            // 
            this.radioDetailList.AutoSize = true;
            this.radioDetailList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetailList.Location = new System.Drawing.Point(138, 3);
            this.radioDetailList.Name = "radioDetailList";
            this.radioDetailList.Size = new System.Drawing.Size(88, 21);
            this.radioDetailList.TabIndex = 15;
            this.radioDetailList.TabStop = true;
            this.radioDetailList.Text = "Detail List";
            this.radioDetailList.UseVisualStyleBackColor = true;
            this.radioDetailList.CheckedChanged += new System.EventHandler(this.Radio_CheckedChanged);
            // 
            // radioMainList
            // 
            this.radioMainList.AutoSize = true;
            this.radioMainList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioMainList.Location = new System.Drawing.Point(4, 3);
            this.radioMainList.Name = "radioMainList";
            this.radioMainList.Size = new System.Drawing.Size(82, 21);
            this.radioMainList.TabIndex = 14;
            this.radioMainList.TabStop = true;
            this.radioMainList.Text = "Main List";
            this.radioMainList.UseVisualStyleBackColor = true;
            this.radioMainList.CheckedChanged += new System.EventHandler(this.Radio_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 182);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 23);
            this.label1.TabIndex = 112;
            this.label1.Text = "FCR Date";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 216);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 23);
            this.label2.TabIndex = 113;
            this.label2.Text = "Cut-Off Date";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 250);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 23);
            this.label3.TabIndex = 114;
            this.label3.Text = "S/O Confirm Date";
            // 
            // dateFCR
            // 
            // 
            // 
            // 
            this.dateFCR.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateFCR.DateBox1.Name = "";
            this.dateFCR.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateFCR.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateFCR.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateFCR.DateBox2.Name = "";
            this.dateFCR.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateFCR.DateBox2.TabIndex = 1;
            this.dateFCR.IsRequired = false;
            this.dateFCR.Location = new System.Drawing.Point(134, 182);
            this.dateFCR.Name = "dateFCR";
            this.dateFCR.Size = new System.Drawing.Size(280, 23);
            this.dateFCR.TabIndex = 6;
            // 
            // dateCutoff
            // 
            // 
            // 
            // 
            this.dateCutoff.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCutoff.DateBox1.Name = "";
            this.dateCutoff.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateCutoff.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCutoff.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateCutoff.DateBox2.Name = "";
            this.dateCutoff.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateCutoff.DateBox2.TabIndex = 1;
            this.dateCutoff.IsRequired = false;
            this.dateCutoff.Location = new System.Drawing.Point(134, 216);
            this.dateCutoff.Name = "dateCutoff";
            this.dateCutoff.Size = new System.Drawing.Size(280, 23);
            this.dateCutoff.TabIndex = 7;
            // 
            // dateConfirm
            // 
            // 
            // 
            // 
            this.dateConfirm.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateConfirm.DateBox1.Name = "";
            this.dateConfirm.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateConfirm.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateConfirm.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateConfirm.DateBox2.Name = "";
            this.dateConfirm.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateConfirm.DateBox2.TabIndex = 1;
            this.dateConfirm.IsRequired = false;
            this.dateConfirm.Location = new System.Drawing.Point(134, 250);
            this.dateConfirm.Name = "dateConfirm";
            this.dateConfirm.Size = new System.Drawing.Size(280, 23);
            this.dateConfirm.TabIndex = 8;
            // 
            // txtcountryDestination
            // 
            this.txtcountryDestination.DisplayBox1Binding = "";
            this.txtcountryDestination.Location = new System.Drawing.Point(135, 352);
            this.txtcountryDestination.Name = "txtcountryDestination";
            this.txtcountryDestination.Size = new System.Drawing.Size(232, 22);
            this.txtcountryDestination.TabIndex = 11;
            this.txtcountryDestination.TextBox1Binding = "";
            // 
            // txtshiptermShipmentTerm
            // 
            this.txtshiptermShipmentTerm.BackColor = System.Drawing.Color.White;
            this.txtshiptermShipmentTerm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtshiptermShipmentTerm.Location = new System.Drawing.Point(134, 318);
            this.txtshiptermShipmentTerm.Name = "txtshiptermShipmentTerm";
            this.txtshiptermShipmentTerm.Size = new System.Drawing.Size(50, 23);
            this.txtshiptermShipmentTerm.TabIndex = 10;
            // 
            // txtshipmodeShippingMode
            // 
            this.txtshipmodeShippingMode.BackColor = System.Drawing.Color.White;
            this.txtshipmodeShippingMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtshipmodeShippingMode.FormattingEnabled = true;
            this.txtshipmodeShippingMode.IsSupportUnselect = true;
            this.txtshipmodeShippingMode.Location = new System.Drawing.Point(134, 283);
            this.txtshipmodeShippingMode.Name = "txtshipmodeShippingMode";
            this.txtshipmodeShippingMode.OldText = "";
            this.txtshipmodeShippingMode.Size = new System.Drawing.Size(80, 24);
            this.txtshipmodeShippingMode.TabIndex = 9;
            this.txtshipmodeShippingMode.UseFunction = "ORDER";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(135, 46);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(86, 23);
            this.txtbrand.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(13, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 23);
            this.label4.TabIndex = 118;
            this.label4.Text = "Delivery";
            // 
            // dateDelivery
            // 
            // 
            // 
            // 
            this.dateDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateDelivery.DateBox1.Name = "";
            this.dateDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateDelivery.DateBox2.Name = "";
            this.dateDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateDelivery.DateBox2.TabIndex = 1;
            this.dateDelivery.IsRequired = false;
            this.dateDelivery.IsSupportEditMode = false;
            this.dateDelivery.Location = new System.Drawing.Point(135, 114);
            this.dateDelivery.Name = "dateDelivery";
            this.dateDelivery.ReadOnly = true;
            this.dateDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateDelivery.TabIndex = 4;
            // 
            // R01
            // 
            this.ClientSize = new System.Drawing.Size(584, 506);
            this.Controls.Add(this.dateDelivery);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateConfirm);
            this.Controls.Add(this.dateCutoff);
            this.Controls.Add(this.dateFCR);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
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
            this.IsSupportCustomized = true;
            this.IsSupportToPrint = false;
            this.Name = "R01";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R01. Garment Booking Report";
            this.Controls.SetChildIndex(this.txtVersion, 0);
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
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.dateFCR, 0);
            this.Controls.SetChildIndex(this.dateCutoff, 0);
            this.Controls.SetChildIndex(this.dateConfirm, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.dateDelivery, 0);
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
        private Class.Txtbrand txtbrand;
        private Win.UI.DateRange dateInvoiceDate;
        private Win.UI.DateRange dateETD;
        private Class.Txtshipmode txtshipmodeShippingMode;
        private Class.Txtshipterm txtshiptermShipmentTerm;
        private Class.Txtcountry txtcountryDestination;
        private Win.UI.ComboBox comboStatus;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioDetailList;
        private Win.UI.RadioButton radioMainList;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.DateRange dateFCR;
        private Win.UI.DateRange dateCutoff;
        private Win.UI.DateRange dateConfirm;
        private Win.UI.Label label4;
        private Win.UI.DateRange dateDelivery;
    }
}
