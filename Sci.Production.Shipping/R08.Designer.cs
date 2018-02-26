namespace Sci.Production.Shipping
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
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.labelCutOffDate = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelCustCD = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.dateCutOffDate = new Sci.Win.UI.DateRange();
            this.txtbrand = new Sci.Production.Class.txtbrand();
            this.txtCustcd = new Sci.Production.Class.txtcustcd();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.checkOnlyPrintTheIrregularData = new Sci.Win.UI.CheckBox();
            this.comboCategory = new Sci.Production.Class.comboDropDownList(this.components);
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(434, 12);
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
            this.labelBuyerDelivery.Size = new System.Drawing.Size(95, 23);
            this.labelBuyerDelivery.TabIndex = 94;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(13, 48);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(95, 23);
            this.labelSCIDelivery.TabIndex = 95;
            this.labelSCIDelivery.Text = "SCI Delivery";
            // 
            // labelCutOffDate
            // 
            this.labelCutOffDate.Location = new System.Drawing.Point(13, 84);
            this.labelCutOffDate.Name = "labelCutOffDate";
            this.labelCutOffDate.Size = new System.Drawing.Size(95, 23);
            this.labelCutOffDate.TabIndex = 96;
            this.labelCutOffDate.Text = "Cut-Off Date";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(13, 120);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(95, 23);
            this.labelBrand.TabIndex = 97;
            this.labelBrand.Text = "Brand";
            // 
            // labelCustCD
            // 
            this.labelCustCD.Location = new System.Drawing.Point(13, 156);
            this.labelCustCD.Name = "labelCustCD";
            this.labelCustCD.Size = new System.Drawing.Size(95, 23);
            this.labelCustCD.TabIndex = 98;
            this.labelCustCD.Text = "Cust CD";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 192);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(95, 23);
            this.labelM.TabIndex = 99;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 228);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(95, 23);
            this.labelFactory.TabIndex = 100;
            this.labelFactory.Text = "Factory";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(13, 264);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(95, 23);
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
            this.dateBuyerDelivery.Location = new System.Drawing.Point(112, 12);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 102;
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
            this.dateSCIDelivery.Location = new System.Drawing.Point(112, 48);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 103;
            // 
            // dateCutOffDate
            // 
            // 
            // 
            // 
            this.dateCutOffDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCutOffDate.DateBox1.Name = "";
            this.dateCutOffDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateCutOffDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCutOffDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateCutOffDate.DateBox2.Name = "";
            this.dateCutOffDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateCutOffDate.DateBox2.TabIndex = 1;
            this.dateCutOffDate.IsRequired = false;
            this.dateCutOffDate.Location = new System.Drawing.Point(112, 84);
            this.dateCutOffDate.Name = "dateCutOffDate";
            this.dateCutOffDate.Size = new System.Drawing.Size(280, 23);
            this.dateCutOffDate.TabIndex = 104;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(112, 120);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(84, 23);
            this.txtbrand.TabIndex = 105;
            // 
            // txtCustcd
            // 
            this.txtCustcd.BackColor = System.Drawing.Color.White;
            this.txtCustcd.BrandObjectName = null;
            this.txtCustcd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCustcd.Location = new System.Drawing.Point(112, 156);
            this.txtCustcd.Name = "txtCustcd";
            this.txtCustcd.Size = new System.Drawing.Size(125, 23);
            this.txtCustcd.TabIndex = 106;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(112, 192);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(74, 24);
            this.comboM.TabIndex = 107;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(112, 228);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(74, 24);
            this.comboFactory.TabIndex = 108;
            // 
            // checkOnlyPrintTheIrregularData
            // 
            this.checkOnlyPrintTheIrregularData.AutoSize = true;
            this.checkOnlyPrintTheIrregularData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkOnlyPrintTheIrregularData.Location = new System.Drawing.Point(13, 300);
            this.checkOnlyPrintTheIrregularData.Name = "checkOnlyPrintTheIrregularData";
            this.checkOnlyPrintTheIrregularData.Size = new System.Drawing.Size(202, 21);
            this.checkOnlyPrintTheIrregularData.TabIndex = 110;
            this.checkOnlyPrintTheIrregularData.Text = "Only Print the irregular data";
            this.checkOnlyPrintTheIrregularData.UseVisualStyleBackColor = true;
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(112, 263);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.Size = new System.Drawing.Size(238, 24);
            this.comboCategory.TabIndex = 140;
            this.comboCategory.Type = "Pms_GMT_Simple";
            // 
            // R08
            // 
            this.ClientSize = new System.Drawing.Size(526, 357);
            this.Controls.Add(this.comboCategory);
            this.Controls.Add(this.checkOnlyPrintTheIrregularData);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.txtCustcd);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.dateCutOffDate);
            this.Controls.Add(this.dateSCIDelivery);
            this.Controls.Add(this.dateBuyerDelivery);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelCustCD);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelCutOffDate);
            this.Controls.Add(this.labelSCIDelivery);
            this.Controls.Add(this.labelBuyerDelivery);
            this.IsSupportToPrint = false;
            this.Name = "R08";
            this.Text = "R08. Packing Check List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.labelSCIDelivery, 0);
            this.Controls.SetChildIndex(this.labelCutOffDate, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelCustCD, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelCategory, 0);
            this.Controls.SetChildIndex(this.dateBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.dateSCIDelivery, 0);
            this.Controls.SetChildIndex(this.dateCutOffDate, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.txtCustcd, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.checkOnlyPrintTheIrregularData, 0);
            this.Controls.SetChildIndex(this.comboCategory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Label labelCutOffDate;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelCustCD;
        private Win.UI.Label labelM;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelCategory;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.DateRange dateCutOffDate;
        private Class.txtbrand txtbrand;
        private Class.txtcustcd txtCustcd;
        private Win.UI.ComboBox comboM;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.CheckBox checkOnlyPrintTheIrregularData;
        private Class.comboDropDownList comboCategory;
    }
}
