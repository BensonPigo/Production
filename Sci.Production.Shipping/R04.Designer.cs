namespace Sci.Production.Shipping
{
    partial class R04
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
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.labelEstimatePullout = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelOrderNo = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.checkIncludeLocalOrder = new Sci.Win.UI.CheckBox();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.dateEstimatePullout = new Sci.Win.UI.DateRange();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.txtOrderNo = new Sci.Win.UI.TextBox();
            this.lbBuyer = new Sci.Win.UI.Label();
            this.lbFCRDate = new Sci.Win.UI.Label();
            this.lbCustCD = new Sci.Win.UI.Label();
            this.lbDestinction = new Sci.Win.UI.Label();
            this.dateFCRDate = new Sci.Win.UI.DateRange();
            this.chkIncludeCancelOrder = new Sci.Win.UI.CheckBox();
            this.lbSummaryBy = new Sci.Win.UI.Label();
            this.txtbuyer = new Sci.Production.Class.Txtbuyer();
            this.txtcountryDestination = new Sci.Production.Class.Txtcountry();
            this.txtcustcd = new Sci.Production.Class.Txtcustcd();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.comboCategory = new Sci.Production.Class.ComboDropDownList(this.components);
            this.comboSummaryBy = new Sci.Win.UI.ComboBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(448, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(448, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(448, 84);
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Location = new System.Drawing.Point(13, 12);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(111, 23);
            this.labelBuyerDelivery.TabIndex = 94;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            // 
            // labelEstimatePullout
            // 
            this.labelEstimatePullout.Location = new System.Drawing.Point(13, 48);
            this.labelEstimatePullout.Name = "labelEstimatePullout";
            this.labelEstimatePullout.Size = new System.Drawing.Size(111, 23);
            this.labelEstimatePullout.TabIndex = 95;
            this.labelEstimatePullout.Text = "Estimate Pull-out";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(13, 149);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(111, 23);
            this.labelBrand.TabIndex = 96;
            this.labelBrand.Text = "Brand";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 250);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(111, 23);
            this.labelM.TabIndex = 97;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 286);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(111, 23);
            this.labelFactory.TabIndex = 98;
            this.labelFactory.Text = "Factory";
            // 
            // labelOrderNo
            // 
            this.labelOrderNo.Location = new System.Drawing.Point(13, 322);
            this.labelOrderNo.Name = "labelOrderNo";
            this.labelOrderNo.Size = new System.Drawing.Size(111, 23);
            this.labelOrderNo.TabIndex = 99;
            this.labelOrderNo.Text = "Order# (NGC#)";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(13, 355);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(111, 23);
            this.labelCategory.TabIndex = 100;
            this.labelCategory.Text = "Category";
            // 
            // checkIncludeLocalOrder
            // 
            this.checkIncludeLocalOrder.AutoSize = true;
            this.checkIncludeLocalOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIncludeLocalOrder.Location = new System.Drawing.Point(13, 419);
            this.checkIncludeLocalOrder.Name = "checkIncludeLocalOrder";
            this.checkIncludeLocalOrder.Size = new System.Drawing.Size(151, 21);
            this.checkIncludeLocalOrder.TabIndex = 101;
            this.checkIncludeLocalOrder.Text = "Include Local Order";
            this.checkIncludeLocalOrder.UseVisualStyleBackColor = true;
            // 
            // dateBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerDelivery.DateBox1.Name = "";
            this.dateBuyerDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBuyerDelivery.DateBox2.Name = "";
            this.dateBuyerDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateBuyerDelivery.IsRequired = false;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(128, 12);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 102;
            // 
            // dateEstimatePullout
            // 
            // 
            // 
            // 
            this.dateEstimatePullout.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateEstimatePullout.DateBox1.Name = "";
            this.dateEstimatePullout.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateEstimatePullout.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateEstimatePullout.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateEstimatePullout.DateBox2.Name = "";
            this.dateEstimatePullout.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateEstimatePullout.DateBox2.TabIndex = 1;
            this.dateEstimatePullout.IsRequired = false;
            this.dateEstimatePullout.Location = new System.Drawing.Point(128, 48);
            this.dateEstimatePullout.Name = "dateEstimatePullout";
            this.dateEstimatePullout.Size = new System.Drawing.Size(280, 23);
            this.dateEstimatePullout.TabIndex = 103;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(128, 250);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(84, 24);
            this.comboM.TabIndex = 105;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(128, 286);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(84, 24);
            this.comboFactory.TabIndex = 106;
            // 
            // txtOrderNo
            // 
            this.txtOrderNo.BackColor = System.Drawing.Color.White;
            this.txtOrderNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtOrderNo.Location = new System.Drawing.Point(128, 322);
            this.txtOrderNo.Name = "txtOrderNo";
            this.txtOrderNo.Size = new System.Drawing.Size(238, 23);
            this.txtOrderNo.TabIndex = 107;
            // 
            // lbBuyer
            // 
            this.lbBuyer.Location = new System.Drawing.Point(13, 117);
            this.lbBuyer.Name = "lbBuyer";
            this.lbBuyer.Size = new System.Drawing.Size(111, 23);
            this.lbBuyer.TabIndex = 140;
            this.lbBuyer.Text = "Buyer";
            // 
            // lbFCRDate
            // 
            this.lbFCRDate.Location = new System.Drawing.Point(13, 84);
            this.lbFCRDate.Name = "lbFCRDate";
            this.lbFCRDate.Size = new System.Drawing.Size(111, 23);
            this.lbFCRDate.TabIndex = 141;
            this.lbFCRDate.Text = "FCR Date";
            // 
            // lbCustCD
            // 
            this.lbCustCD.Location = new System.Drawing.Point(13, 183);
            this.lbCustCD.Name = "lbCustCD";
            this.lbCustCD.Size = new System.Drawing.Size(111, 23);
            this.lbCustCD.TabIndex = 142;
            this.lbCustCD.Text = "Cust CD";
            // 
            // lbDestinction
            // 
            this.lbDestinction.Location = new System.Drawing.Point(13, 218);
            this.lbDestinction.Name = "lbDestinction";
            this.lbDestinction.Size = new System.Drawing.Size(111, 23);
            this.lbDestinction.TabIndex = 143;
            this.lbDestinction.Text = "Destination";
            // 
            // dateFCRDate
            // 
            // 
            // 
            // 
            this.dateFCRDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateFCRDate.DateBox1.Name = "";
            this.dateFCRDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateFCRDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateFCRDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateFCRDate.DateBox2.Name = "";
            this.dateFCRDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateFCRDate.DateBox2.TabIndex = 1;
            this.dateFCRDate.IsRequired = false;
            this.dateFCRDate.Location = new System.Drawing.Point(128, 84);
            this.dateFCRDate.Name = "dateFCRDate";
            this.dateFCRDate.Size = new System.Drawing.Size(280, 23);
            this.dateFCRDate.TabIndex = 144;
            // 
            // chkIncludeCancelOrder
            // 
            this.chkIncludeCancelOrder.AutoSize = true;
            this.chkIncludeCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeCancelOrder.Location = new System.Drawing.Point(13, 442);
            this.chkIncludeCancelOrder.Name = "chkIncludeCancelOrder";
            this.chkIncludeCancelOrder.Size = new System.Drawing.Size(157, 21);
            this.chkIncludeCancelOrder.TabIndex = 171;
            this.chkIncludeCancelOrder.Text = "Include Cancel order";
            this.chkIncludeCancelOrder.UseVisualStyleBackColor = true;
            // 
            // lbSummaryBy
            // 
            this.lbSummaryBy.Location = new System.Drawing.Point(14, 386);
            this.lbSummaryBy.Name = "lbSummaryBy";
            this.lbSummaryBy.Size = new System.Drawing.Size(111, 23);
            this.lbSummaryBy.TabIndex = 172;
            this.lbSummaryBy.Text = "Summary By";
            // 
            // txtbuyer
            // 
            this.txtbuyer.BackColor = System.Drawing.Color.White;
            this.txtbuyer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbuyer.Location = new System.Drawing.Point(128, 117);
            this.txtbuyer.Name = "txtbuyer";
            this.txtbuyer.Size = new System.Drawing.Size(84, 23);
            this.txtbuyer.TabIndex = 151;
            // 
            // txtcountryDestination
            // 
            this.txtcountryDestination.DisplayBox1Binding = "";
            this.txtcountryDestination.Location = new System.Drawing.Point(128, 218);
            this.txtcountryDestination.Name = "txtcountryDestination";
            this.txtcountryDestination.Size = new System.Drawing.Size(232, 22);
            this.txtcountryDestination.TabIndex = 150;
            this.txtcountryDestination.TextBox1Binding = "";
            // 
            // txtcustcd
            // 
            this.txtcustcd.BackColor = System.Drawing.Color.White;
            this.txtcustcd.BrandObjectName = this.txtbrand;
            this.txtcustcd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcustcd.Location = new System.Drawing.Point(128, 183);
            this.txtcustcd.Name = "txtcustcd";
            this.txtcustcd.Size = new System.Drawing.Size(125, 23);
            this.txtcustcd.TabIndex = 149;
            this.txtcustcd.Validating += new System.ComponentModel.CancelEventHandler(this.Txtcustcd_Validating);
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(128, 149);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(84, 23);
            this.txtbrand.TabIndex = 104;
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(128, 355);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(238, 24);
            this.comboCategory.TabIndex = 139;
            this.comboCategory.Type = "Pms_GMT_Simple";
            // 
            // comboSummaryBy
            // 
            this.comboSummaryBy.BackColor = System.Drawing.Color.White;
            this.comboSummaryBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSummaryBy.FormattingEnabled = true;
            this.comboSummaryBy.IsSupportUnselect = true;
            this.comboSummaryBy.Location = new System.Drawing.Point(128, 385);
            this.comboSummaryBy.Name = "comboSummaryBy";
            this.comboSummaryBy.OldText = "";
            this.comboSummaryBy.Size = new System.Drawing.Size(112, 24);
            this.comboSummaryBy.TabIndex = 173;
            // 
            // R04
            // 
            this.ClientSize = new System.Drawing.Size(540, 505);
            this.Controls.Add(this.comboSummaryBy);
            this.Controls.Add(this.lbSummaryBy);
            this.Controls.Add(this.chkIncludeCancelOrder);
            this.Controls.Add(this.txtbuyer);
            this.Controls.Add(this.txtcountryDestination);
            this.Controls.Add(this.txtcustcd);
            this.Controls.Add(this.dateFCRDate);
            this.Controls.Add(this.lbDestinction);
            this.Controls.Add(this.lbCustCD);
            this.Controls.Add(this.lbFCRDate);
            this.Controls.Add(this.lbBuyer);
            this.Controls.Add(this.comboCategory);
            this.Controls.Add(this.txtOrderNo);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.dateEstimatePullout);
            this.Controls.Add(this.dateBuyerDelivery);
            this.Controls.Add(this.checkIncludeLocalOrder);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.labelOrderNo);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelEstimatePullout);
            this.Controls.Add(this.labelBuyerDelivery);
            this.IsSupportToPrint = false;
            this.Name = "R04";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R04. Estimate/Outstanding Shipment Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.labelEstimatePullout, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelOrderNo, 0);
            this.Controls.SetChildIndex(this.labelCategory, 0);
            this.Controls.SetChildIndex(this.checkIncludeLocalOrder, 0);
            this.Controls.SetChildIndex(this.dateBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.dateEstimatePullout, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.txtOrderNo, 0);
            this.Controls.SetChildIndex(this.comboCategory, 0);
            this.Controls.SetChildIndex(this.lbBuyer, 0);
            this.Controls.SetChildIndex(this.lbFCRDate, 0);
            this.Controls.SetChildIndex(this.lbCustCD, 0);
            this.Controls.SetChildIndex(this.lbDestinction, 0);
            this.Controls.SetChildIndex(this.dateFCRDate, 0);
            this.Controls.SetChildIndex(this.txtcustcd, 0);
            this.Controls.SetChildIndex(this.txtcountryDestination, 0);
            this.Controls.SetChildIndex(this.txtbuyer, 0);
            this.Controls.SetChildIndex(this.chkIncludeCancelOrder, 0);
            this.Controls.SetChildIndex(this.lbSummaryBy, 0);
            this.Controls.SetChildIndex(this.comboSummaryBy, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.Label labelEstimatePullout;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelM;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelOrderNo;
        private Win.UI.Label labelCategory;
        private Win.UI.CheckBox checkIncludeLocalOrder;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.DateRange dateEstimatePullout;
        private Class.Txtbrand txtbrand;
        private Win.UI.ComboBox comboM;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.TextBox txtOrderNo;
        private Class.ComboDropDownList comboCategory;
        private Win.UI.Label lbBuyer;
        private Win.UI.Label lbFCRDate;
        private Win.UI.Label lbCustCD;
        private Win.UI.Label lbDestinction;
        private Win.UI.DateRange dateFCRDate;
        private Class.Txtcustcd txtcustcd;
        private Class.Txtcountry txtcountryDestination;
        private Class.Txtbuyer txtbuyer;
        private Win.UI.CheckBox chkIncludeCancelOrder;
        private Win.UI.Label lbSummaryBy;
        private Win.UI.ComboBox comboSummaryBy;
    }
}
