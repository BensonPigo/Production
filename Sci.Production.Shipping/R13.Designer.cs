namespace Sci.Production.Shipping
{
    partial class R13
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
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.labelShipper = new Sci.Win.UI.Label();
            this.lbSCIDelivery = new Sci.Win.UI.Label();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.chkIncludeCancelOrder = new Sci.Win.UI.CheckBox();
            this.chkGMTComplete = new Sci.Win.UI.CheckBox();
            this.txtmultifactory = new Sci.Production.Class.Txtmultifactory();
            this.txtmultiShipper = new Sci.Production.Class.TxtmulitFSRCpuCost();
            this.comboCategory = new Sci.Production.Class.ComboDropDownList(this.components);
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.comboOrderCompany = new Sci.Production.Class.ComboCompany(this.components);
            this.label5 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(434, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(434, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(434, 84);
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Location = new System.Drawing.Point(13, 12);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(102, 23);
            this.labelBuyerDelivery.TabIndex = 94;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(13, 84);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(102, 23);
            this.labelBrand.TabIndex = 97;
            this.labelBrand.Text = "Brand";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 159);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(102, 23);
            this.labelFactory.TabIndex = 100;
            this.labelFactory.Text = "Factory";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(13, 195);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(102, 23);
            this.labelCategory.TabIndex = 101;
            this.labelCategory.Text = "Category";
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
            this.dateBuyerDelivery.Location = new System.Drawing.Point(118, 12);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 102;
            // 
            // labelShipper
            // 
            this.labelShipper.Location = new System.Drawing.Point(13, 120);
            this.labelShipper.Name = "labelShipper";
            this.labelShipper.Size = new System.Drawing.Size(102, 23);
            this.labelShipper.TabIndex = 98;
            this.labelShipper.Text = "Shipper";
            // 
            // lbSCIDelivery
            // 
            this.lbSCIDelivery.Location = new System.Drawing.Point(13, 48);
            this.lbSCIDelivery.Name = "lbSCIDelivery";
            this.lbSCIDelivery.Size = new System.Drawing.Size(102, 23);
            this.lbSCIDelivery.TabIndex = 143;
            this.lbSCIDelivery.Text = "SCI Delivery";
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
            this.dateSCIDelivery.Location = new System.Drawing.Point(118, 48);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 144;
            // 
            // chkIncludeCancelOrder
            // 
            this.chkIncludeCancelOrder.AutoSize = true;
            this.chkIncludeCancelOrder.Checked = true;
            this.chkIncludeCancelOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeCancelOrder.Location = new System.Drawing.Point(13, 256);
            this.chkIncludeCancelOrder.Name = "chkIncludeCancelOrder";
            this.chkIncludeCancelOrder.Size = new System.Drawing.Size(160, 21);
            this.chkIncludeCancelOrder.TabIndex = 145;
            this.chkIncludeCancelOrder.Text = "Include Cancel Order";
            this.chkIncludeCancelOrder.UseVisualStyleBackColor = true;
            // 
            // chkGMTComplete
            // 
            this.chkGMTComplete.AutoSize = true;
            this.chkGMTComplete.Checked = true;
            this.chkGMTComplete.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGMTComplete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkGMTComplete.Location = new System.Drawing.Point(13, 283);
            this.chkGMTComplete.Name = "chkGMTComplete";
            this.chkGMTComplete.Size = new System.Drawing.Size(174, 21);
            this.chkGMTComplete.TabIndex = 146;
            this.chkGMTComplete.Text = "Include GMT. Complete";
            this.chkGMTComplete.UseVisualStyleBackColor = true;
            // 
            // txtmultifactory
            // 
            this.txtmultifactory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtmultifactory.CheckFtyGroup = true;
            this.txtmultifactory.CheckProduceFty = false;
            this.txtmultifactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtmultifactory.IsDataFromA2B = false;
            this.txtmultifactory.IsForPackingA2B = false;
            this.txtmultifactory.IsMDivisionID = false;
            this.txtmultifactory.IsSupportEditMode = false;
            this.txtmultifactory.Location = new System.Drawing.Point(118, 159);
            this.txtmultifactory.MDivisionID = "";
            this.txtmultifactory.Name = "txtmultifactory";
            this.txtmultifactory.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditMode;
            this.txtmultifactory.ReadOnly = true;
            this.txtmultifactory.Size = new System.Drawing.Size(308, 23);
            this.txtmultifactory.SystemName = "";
            this.txtmultifactory.TabIndex = 148;
            // 
            // txtmultiShipper
            // 
            this.txtmultiShipper.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtmultiShipper.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtmultiShipper.IsSupportEditMode = false;
            this.txtmultiShipper.Location = new System.Drawing.Point(118, 120);
            this.txtmultiShipper.Name = "txtmultiShipper";
            this.txtmultiShipper.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditMode;
            this.txtmultiShipper.ReadOnly = true;
            this.txtmultiShipper.Size = new System.Drawing.Size(308, 23);
            this.txtmultiShipper.TabIndex = 147;
            // 
            // comboCategory
            // 
            this.comboCategory.AddAllItem = false;
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(118, 194);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(238, 24);
            this.comboCategory.TabIndex = 142;
            this.comboCategory.Type = "Pms_RepGMTForecast";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(118, 84);
            this.txtbrand.MyDocumentdName = null;
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(84, 23);
            this.txtbrand.TabIndex = 105;
            // 
            // comboOrderCompany
            // 
            this.comboOrderCompany.BackColor = System.Drawing.Color.White;
            this.comboOrderCompany.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboOrderCompany.FormattingEnabled = true;
            this.comboOrderCompany.IsAddEmpty = true;
            this.comboOrderCompany.IsOrderCompany = true;
            this.comboOrderCompany.IsSupportUnselect = true;
            this.comboOrderCompany.Junk = null;
            this.comboOrderCompany.Location = new System.Drawing.Point(118, 231);
            this.comboOrderCompany.Name = "comboOrderCompany";
            this.comboOrderCompany.OldText = "";
            this.comboOrderCompany.Size = new System.Drawing.Size(238, 24);
            this.comboOrderCompany.TabIndex = 150;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(13, 231);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 22);
            this.label5.TabIndex = 149;
            this.label5.Text = "Order Company";
            // 
            // R13
            // 
            this.ClientSize = new System.Drawing.Size(526, 337);
            this.Controls.Add(this.comboOrderCompany);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtmultifactory);
            this.Controls.Add(this.txtmultiShipper);
            this.Controls.Add(this.chkGMTComplete);
            this.Controls.Add(this.chkIncludeCancelOrder);
            this.Controls.Add(this.dateSCIDelivery);
            this.Controls.Add(this.lbSCIDelivery);
            this.Controls.Add(this.comboCategory);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.dateBuyerDelivery);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelShipper);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelBuyerDelivery);
            this.IsSupportToPrint = false;
            this.Name = "R13";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R13. Factory CMT Forecast";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.labelBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelShipper, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelCategory, 0);
            this.Controls.SetChildIndex(this.dateBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.comboCategory, 0);
            this.Controls.SetChildIndex(this.lbSCIDelivery, 0);
            this.Controls.SetChildIndex(this.dateSCIDelivery, 0);
            this.Controls.SetChildIndex(this.chkIncludeCancelOrder, 0);
            this.Controls.SetChildIndex(this.chkGMTComplete, 0);
            this.Controls.SetChildIndex(this.txtmultiShipper, 0);
            this.Controls.SetChildIndex(this.txtmultifactory, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.comboOrderCompany, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelCategory;
        private Win.UI.DateRange dateBuyerDelivery;
        private Class.Txtbrand txtbrand;
        private Win.UI.Label labelShipper;
        private Class.ComboDropDownList comboCategory;
        private Win.UI.Label lbSCIDelivery;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.CheckBox chkIncludeCancelOrder;
        private Win.UI.CheckBox chkGMTComplete;
        private Class.TxtmulitFSRCpuCost txtmultiShipper;
        private Class.Txtmultifactory txtmultifactory;
        private Class.ComboCompany comboOrderCompany;
        private Win.UI.Label label5;
    }
}
