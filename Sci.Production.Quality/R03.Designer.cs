namespace Sci.Production.Quality
{
    partial class R03
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.comboCategory = new Sci.Production.Class.comboDropDownList(this.components);
            this.txtSeason = new Sci.Production.Class.txtseason();
            this.txtBrand = new Sci.Production.Class.txtbrand();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.dateEstCuttingDate = new Sci.Win.UI.DateRange();
            this.dateSewingInLineDate = new Sci.Win.UI.DateRange();
            this.dateRange4 = new Sci.Win.UI.DateRange();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.labelEstCuttingDate = new Sci.Win.UI.Label();
            this.labelSewingInLineDate = new Sci.Win.UI.Label();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.chkIncludeCancelOrder = new Sci.Win.UI.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(499, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(499, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(499, 84);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkIncludeCancelOrder);
            this.panel1.Controls.Add(this.comboCategory);
            this.panel1.Controls.Add(this.txtSeason);
            this.panel1.Controls.Add(this.txtBrand);
            this.panel1.Controls.Add(this.comboFactory);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.labelCategory);
            this.panel1.Controls.Add(this.labelBrand);
            this.panel1.Controls.Add(this.labelSeason);
            this.panel1.Controls.Add(this.dateEstCuttingDate);
            this.panel1.Controls.Add(this.dateSewingInLineDate);
            this.panel1.Controls.Add(this.dateRange4);
            this.panel1.Controls.Add(this.dateSCIDelivery);
            this.panel1.Controls.Add(this.labelEstCuttingDate);
            this.panel1.Controls.Add(this.labelSewingInLineDate);
            this.panel1.Controls.Add(this.labelSCIDelivery);
            this.panel1.Location = new System.Drawing.Point(33, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(447, 308);
            this.panel1.TabIndex = 94;
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(142, 202);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(185, 24);
            this.comboCategory.TabIndex = 53;
            this.comboCategory.Type = "Pms_MtlCategory";
            // 
            // txtSeason
            // 
            this.txtSeason.BackColor = System.Drawing.Color.White;
            this.txtSeason.BrandObjectName = null;
            this.txtSeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeason.Location = new System.Drawing.Point(142, 121);
            this.txtSeason.Name = "txtSeason";
            this.txtSeason.Size = new System.Drawing.Size(135, 23);
            this.txtSeason.TabIndex = 49;
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(142, 163);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(107, 23);
            this.txtBrand.TabIndex = 48;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(142, 243);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(125, 24);
            this.comboFactory.TabIndex = 47;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(12, 243);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(127, 23);
            this.labelFactory.TabIndex = 46;
            this.labelFactory.Text = "Factory";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(12, 202);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(127, 23);
            this.labelCategory.TabIndex = 44;
            this.labelCategory.Text = "Category";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(12, 163);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(127, 23);
            this.labelBrand.TabIndex = 41;
            this.labelBrand.Text = "Brand";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(12, 121);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(127, 23);
            this.labelSeason.TabIndex = 40;
            this.labelSeason.Text = "Season";
            // 
            // dateEstCuttingDate
            // 
            // 
            // 
            // 
            this.dateEstCuttingDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateEstCuttingDate.DateBox1.Name = "";
            this.dateEstCuttingDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateEstCuttingDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateEstCuttingDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateEstCuttingDate.DateBox2.Name = "";
            this.dateEstCuttingDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateEstCuttingDate.DateBox2.TabIndex = 1;
            this.dateEstCuttingDate.IsRequired = false;
            this.dateEstCuttingDate.Location = new System.Drawing.Point(142, 85);
            this.dateEstCuttingDate.Name = "dateEstCuttingDate";
            this.dateEstCuttingDate.Size = new System.Drawing.Size(280, 23);
            this.dateEstCuttingDate.TabIndex = 39;
            // 
            // dateSewingInLineDate
            // 
            // 
            // 
            // 
            this.dateSewingInLineDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSewingInLineDate.DateBox1.Name = "";
            this.dateSewingInLineDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSewingInLineDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSewingInLineDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSewingInLineDate.DateBox2.Name = "";
            this.dateSewingInLineDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSewingInLineDate.DateBox2.TabIndex = 1;
            this.dateSewingInLineDate.IsRequired = false;
            this.dateSewingInLineDate.Location = new System.Drawing.Point(142, 47);
            this.dateSewingInLineDate.Name = "dateSewingInLineDate";
            this.dateSewingInLineDate.Size = new System.Drawing.Size(280, 23);
            this.dateSewingInLineDate.TabIndex = 38;
            // 
            // dateRange4
            // 
            // 
            // 
            // 
            this.dateRange4.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRange4.DateBox1.Name = "";
            this.dateRange4.DateBox1.Size = new System.Drawing.Size(1, 23);
            this.dateRange4.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRange4.DateBox2.Location = new System.Drawing.Point(1, 0);
            this.dateRange4.DateBox2.Name = "";
            this.dateRange4.DateBox2.Size = new System.Drawing.Size(1, 23);
            this.dateRange4.DateBox2.TabIndex = 1;
            this.dateRange4.Location = new System.Drawing.Point(215, 47);
            this.dateRange4.Name = "dateRange4";
            this.dateRange4.Size = new System.Drawing.Size(8, 23);
            this.dateRange4.TabIndex = 37;
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
            this.dateSCIDelivery.Location = new System.Drawing.Point(142, 10);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 36;
            // 
            // labelEstCuttingDate
            // 
            this.labelEstCuttingDate.Location = new System.Drawing.Point(12, 85);
            this.labelEstCuttingDate.Name = "labelEstCuttingDate";
            this.labelEstCuttingDate.Size = new System.Drawing.Size(127, 23);
            this.labelEstCuttingDate.TabIndex = 35;
            this.labelEstCuttingDate.Text = "Est. Cutting Date";
            // 
            // labelSewingInLineDate
            // 
            this.labelSewingInLineDate.Location = new System.Drawing.Point(12, 47);
            this.labelSewingInLineDate.Name = "labelSewingInLineDate";
            this.labelSewingInLineDate.Size = new System.Drawing.Size(127, 23);
            this.labelSewingInLineDate.TabIndex = 34;
            this.labelSewingInLineDate.Text = "Sewing in-line Date";
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(12, 10);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(127, 23);
            this.labelSCIDelivery.TabIndex = 33;
            this.labelSCIDelivery.Text = "SCI Delivery";
            // 
            // chkIncludeCancelOrder
            // 
            this.chkIncludeCancelOrder.AutoSize = true;
            this.chkIncludeCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeCancelOrder.Location = new System.Drawing.Point(12, 273);
            this.chkIncludeCancelOrder.Name = "chkIncludeCancelOrder";
            this.chkIncludeCancelOrder.Size = new System.Drawing.Size(157, 21);
            this.chkIncludeCancelOrder.TabIndex = 166;
            this.chkIncludeCancelOrder.Text = "Include Cancel order";
            this.chkIncludeCancelOrder.UseVisualStyleBackColor = true;
            // 
            // R03
            // 
            this.ClientSize = new System.Drawing.Size(591, 349);
            this.Controls.Add(this.panel1);
            this.Name = "R03";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R03.Material Inspection & Laboratory Status";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.DateRange dateEstCuttingDate;
        private Win.UI.DateRange dateSewingInLineDate;
        private Win.UI.DateRange dateRange4;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.Label labelEstCuttingDate;
        private Win.UI.Label labelSewingInLineDate;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelCategory;
        private Win.UI.ComboBox comboFactory;
        private Class.txtbrand txtBrand;
        private Class.txtseason txtSeason;
        private Class.comboDropDownList comboCategory;
        private Win.UI.CheckBox chkIncludeCancelOrder;
    }
}
