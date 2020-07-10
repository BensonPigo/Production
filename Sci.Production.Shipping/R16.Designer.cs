namespace Sci.Production.Shipping
{
    partial class R16
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.LabShippingMode = new Sci.Win.UI.Label();
            this.labBrand = new Sci.Win.UI.Label();
            this.labShipper = new Sci.Win.UI.Label();
            this.labCategory = new Sci.Win.UI.Label();
            this.labBuyer = new Sci.Win.UI.Label();
            this.labFactory = new Sci.Win.UI.Label();
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.labCustCD = new Sci.Win.UI.Label();
            this.labDestination = new Sci.Win.UI.Label();
            this.comboshipmode = new Sci.Production.Class.Txtshipmode();
            this.comboCategory = new Sci.Production.Class.ComboDropDownList(this.components);
            this.txtcountry = new Sci.Production.Class.Txtcountry();
            this.txtcustcd = new Sci.Production.Class.Txtcustcd();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txtbuyer = new Sci.Production.Class.Txtbuyer();
            this.txtShipper = new Sci.Win.UI.TextBox();
            this.chkcancelOrder = new Sci.Win.UI.CheckBox();
            this.chkFOC = new Sci.Win.UI.CheckBox();
            this.chkGMTCompleteShortage = new Sci.Win.UI.CheckBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(535, 98);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(535, 26);
            this.toexcel.TabIndex = 10;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(535, 62);
            this.close.TabIndex = 11;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(489, 134);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(515, 170);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(515, 197);
            // 
            // LabShippingMode
            // 
            this.LabShippingMode.Location = new System.Drawing.Point(49, 296);
            this.LabShippingMode.Name = "LabShippingMode";
            this.LabShippingMode.Size = new System.Drawing.Size(101, 23);
            this.LabShippingMode.TabIndex = 134;
            this.LabShippingMode.Text = "Shipping Mode";
            // 
            // labBrand
            // 
            this.labBrand.Location = new System.Drawing.Point(49, 81);
            this.labBrand.Name = "labBrand";
            this.labBrand.Size = new System.Drawing.Size(101, 23);
            this.labBrand.TabIndex = 133;
            this.labBrand.Text = "Brand";
            // 
            // labShipper
            // 
            this.labShipper.Location = new System.Drawing.Point(49, 147);
            this.labShipper.Name = "labShipper";
            this.labShipper.Size = new System.Drawing.Size(101, 23);
            this.labShipper.TabIndex = 130;
            this.labShipper.Text = "Shipper";
            // 
            // labCategory
            // 
            this.labCategory.Location = new System.Drawing.Point(49, 257);
            this.labCategory.Name = "labCategory";
            this.labCategory.Size = new System.Drawing.Size(101, 23);
            this.labCategory.TabIndex = 126;
            this.labCategory.Text = "Category";
            // 
            // labBuyer
            // 
            this.labBuyer.Location = new System.Drawing.Point(49, 48);
            this.labBuyer.Name = "labBuyer";
            this.labBuyer.Size = new System.Drawing.Size(101, 23);
            this.labBuyer.TabIndex = 125;
            this.labBuyer.Text = "Buyer";
            // 
            // labFactory
            // 
            this.labFactory.Location = new System.Drawing.Point(49, 114);
            this.labFactory.Name = "labFactory";
            this.labFactory.Size = new System.Drawing.Size(101, 23);
            this.labFactory.TabIndex = 124;
            this.labFactory.Text = "Factory";
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Location = new System.Drawing.Point(49, 15);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(101, 23);
            this.labelBuyerDelivery.TabIndex = 123;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
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
            this.dateBuyerDelivery.Location = new System.Drawing.Point(158, 15);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 0;
            // 
            // labCustCD
            // 
            this.labCustCD.Location = new System.Drawing.Point(49, 182);
            this.labCustCD.Name = "labCustCD";
            this.labCustCD.Size = new System.Drawing.Size(101, 23);
            this.labCustCD.TabIndex = 141;
            this.labCustCD.Text = "Cust CD";
            // 
            // labDestination
            // 
            this.labDestination.Location = new System.Drawing.Point(49, 218);
            this.labDestination.Name = "labDestination";
            this.labDestination.Size = new System.Drawing.Size(101, 23);
            this.labDestination.TabIndex = 143;
            this.labDestination.Text = "Destination";
            // 
            // comboshipmode
            // 
            this.comboshipmode.BackColor = System.Drawing.Color.White;
            this.comboshipmode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboshipmode.FormattingEnabled = true;
            this.comboshipmode.IsSupportUnselect = true;
            this.comboshipmode.Location = new System.Drawing.Point(158, 296);
            this.comboshipmode.Name = "comboshipmode";
            this.comboshipmode.OldText = "";
            this.comboshipmode.Size = new System.Drawing.Size(170, 24);
            this.comboshipmode.TabIndex = 8;
            this.comboshipmode.UseFunction = null;
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(158, 257);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(170, 24);
            this.comboCategory.TabIndex = 7;
            this.comboCategory.Type = "Pms_GMT_Simple";
            // 
            // txtcountry
            // 
            this.txtcountry.DisplayBox1Binding = "";
            this.txtcountry.Location = new System.Drawing.Point(158, 218);
            this.txtcountry.Name = "txtcountry";
            this.txtcountry.Size = new System.Drawing.Size(232, 22);
            this.txtcountry.TabIndex = 6;
            this.txtcountry.TextBox1Binding = "";
            // 
            // txtcustcd
            // 
            this.txtcustcd.BackColor = System.Drawing.Color.White;
            this.txtcustcd.BrandObjectName = null;
            this.txtcustcd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcustcd.Location = new System.Drawing.Point(158, 182);
            this.txtcustcd.Name = "txtcustcd";
            this.txtcustcd.Size = new System.Drawing.Size(125, 23);
            this.txtcustcd.TabIndex = 5;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(158, 114);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(125, 23);
            this.txtfactory.TabIndex = 3;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(158, 81);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(125, 23);
            this.txtbrand.TabIndex = 2;
            // 
            // txtbuyer
            // 
            this.txtbuyer.BackColor = System.Drawing.Color.White;
            this.txtbuyer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbuyer.Location = new System.Drawing.Point(158, 48);
            this.txtbuyer.Name = "txtbuyer";
            this.txtbuyer.Size = new System.Drawing.Size(125, 23);
            this.txtbuyer.TabIndex = 1;
            // 
            // txtShipper
            // 
            this.txtShipper.BackColor = System.Drawing.Color.White;
            this.txtShipper.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtShipper.Location = new System.Drawing.Point(158, 147);
            this.txtShipper.Name = "txtShipper";
            this.txtShipper.Size = new System.Drawing.Size(125, 23);
            this.txtShipper.TabIndex = 144;
            this.txtShipper.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtShipper_PopUp);
            this.txtShipper.Validating += new System.ComponentModel.CancelEventHandler(this.TxtShipper_Validating);
            // 
            // chkcancelOrder
            // 
            this.chkcancelOrder.AutoSize = true;
            this.chkcancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkcancelOrder.Location = new System.Drawing.Point(49, 326);
            this.chkcancelOrder.Name = "chkcancelOrder";
            this.chkcancelOrder.Size = new System.Drawing.Size(160, 21);
            this.chkcancelOrder.TabIndex = 9;
            this.chkcancelOrder.Text = "Include Cancel Order";
            this.chkcancelOrder.UseVisualStyleBackColor = true;
            // 
            // chkFOC
            // 
            this.chkFOC.AutoSize = true;
            this.chkFOC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkFOC.Location = new System.Drawing.Point(49, 353);
            this.chkFOC.Name = "chkFOC";
            this.chkFOC.Size = new System.Drawing.Size(104, 21);
            this.chkFOC.TabIndex = 145;
            this.chkFOC.Text = "Include FOC";
            this.chkFOC.UseVisualStyleBackColor = true;
            // 
            // chkGMTCompleteShortage
            // 
            this.chkGMTCompleteShortage.AutoSize = true;
            this.chkGMTCompleteShortage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkGMTCompleteShortage.Location = new System.Drawing.Point(49, 380);
            this.chkGMTCompleteShortage.Name = "chkGMTCompleteShortage";
            this.chkGMTCompleteShortage.Size = new System.Drawing.Size(233, 21);
            this.chkGMTCompleteShortage.TabIndex = 146;
            this.chkGMTCompleteShortage.Text = "Include GMT Complete-Shortage";
            this.chkGMTCompleteShortage.UseVisualStyleBackColor = true;
            // 
            // R16
            // 
            this.ClientSize = new System.Drawing.Size(627, 446);
            this.Controls.Add(this.chkGMTCompleteShortage);
            this.Controls.Add(this.chkFOC);
            this.Controls.Add(this.chkcancelOrder);
            this.Controls.Add(this.txtShipper);
            this.Controls.Add(this.comboshipmode);
            this.Controls.Add(this.comboCategory);
            this.Controls.Add(this.txtcountry);
            this.Controls.Add(this.labDestination);
            this.Controls.Add(this.txtcustcd);
            this.Controls.Add(this.labCustCD);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.txtbuyer);
            this.Controls.Add(this.dateBuyerDelivery);
            this.Controls.Add(this.LabShippingMode);
            this.Controls.Add(this.labBrand);
            this.Controls.Add(this.labShipper);
            this.Controls.Add(this.labCategory);
            this.Controls.Add(this.labBuyer);
            this.Controls.Add(this.labFactory);
            this.Controls.Add(this.labelBuyerDelivery);
            this.Name = "R16";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R16. Outstanding Garment Booking List";
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.labFactory, 0);
            this.Controls.SetChildIndex(this.labBuyer, 0);
            this.Controls.SetChildIndex(this.labCategory, 0);
            this.Controls.SetChildIndex(this.labShipper, 0);
            this.Controls.SetChildIndex(this.labBrand, 0);
            this.Controls.SetChildIndex(this.LabShippingMode, 0);
            this.Controls.SetChildIndex(this.dateBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.txtbuyer, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.labCustCD, 0);
            this.Controls.SetChildIndex(this.txtcustcd, 0);
            this.Controls.SetChildIndex(this.labDestination, 0);
            this.Controls.SetChildIndex(this.txtcountry, 0);
            this.Controls.SetChildIndex(this.comboCategory, 0);
            this.Controls.SetChildIndex(this.comboshipmode, 0);
            this.Controls.SetChildIndex(this.txtShipper, 0);
            this.Controls.SetChildIndex(this.chkcancelOrder, 0);
            this.Controls.SetChildIndex(this.chkFOC, 0);
            this.Controls.SetChildIndex(this.chkGMTCompleteShortage, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label LabShippingMode;
        private Win.UI.Label labBrand;
        private Win.UI.Label labShipper;
        private Win.UI.Label labCategory;
        private Win.UI.Label labBuyer;
        private Win.UI.Label labFactory;
        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.DateRange dateBuyerDelivery;
        private Class.Txtbuyer txtbuyer;
        private Class.Txtbrand txtbrand;
        private Class.Txtfactory txtfactory;
        private Win.UI.Label labCustCD;
        private Class.Txtcustcd txtcustcd;
        private Win.UI.Label labDestination;
        private Class.Txtcountry txtcountry;
        private Class.ComboDropDownList comboCategory;
        private Class.Txtshipmode comboshipmode;
        private Win.UI.TextBox txtShipper;
        private Win.UI.CheckBox chkcancelOrder;
        private Win.UI.CheckBox chkFOC;
        private Win.UI.CheckBox chkGMTCompleteShortage;
    }
}
