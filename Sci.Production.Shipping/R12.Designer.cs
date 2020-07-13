namespace Sci.Production.Shipping
{
    partial class R12
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
            this.labelFCRDate = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.dateFCRDate = new Sci.Win.UI.DateRange();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txtcountryDestination = new Sci.Production.Class.Txtcountry();
            this.txtcustcd = new Sci.Production.Class.Txtcustcd();
            this.labelDestination = new Sci.Win.UI.Label();
            this.labelCustCD = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.txtGBNoEnd = new Sci.Win.UI.TextBox();
            this.txtGBNoStart = new Sci.Win.UI.TextBox();
            this.labelGBNo = new Sci.Win.UI.Label();
            this.dateInvoiceDate = new Sci.Win.UI.DateRange();
            this.labelInvoiceDate = new Sci.Win.UI.Label();
            this.datePulloutDate = new Sci.Win.UI.DateRange();
            this.labelPulloutDate = new Sci.Win.UI.Label();
            this.txtbuyer = new Sci.Production.Class.Txtbuyer();
            this.labelBuyer = new Sci.Win.UI.Label();
            this.comboCategory = new Sci.Production.Class.ComboDropDownList(this.components);
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(488, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(488, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(488, 84);
            // 
            // labelFCRDate
            // 
            this.labelFCRDate.Location = new System.Drawing.Point(13, 12);
            this.labelFCRDate.Name = "labelFCRDate";
            this.labelFCRDate.Size = new System.Drawing.Size(95, 23);
            this.labelFCRDate.TabIndex = 94;
            this.labelFCRDate.Text = "FCR Date";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(13, 227);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(95, 23);
            this.labelBrand.TabIndex = 97;
            this.labelBrand.Text = "Brand";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(13, 334);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(95, 23);
            this.labelCategory.TabIndex = 101;
            this.labelCategory.Text = "Category";
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
            this.dateFCRDate.Location = new System.Drawing.Point(112, 12);
            this.dateFCRDate.Name = "dateFCRDate";
            this.dateFCRDate.Size = new System.Drawing.Size(280, 23);
            this.dateFCRDate.TabIndex = 102;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(112, 227);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(84, 23);
            this.txtbrand.TabIndex = 105;
            // 
            // txtcountryDestination
            // 
            this.txtcountryDestination.DisplayBox1Binding = "";
            this.txtcountryDestination.Location = new System.Drawing.Point(112, 299);
            this.txtcountryDestination.Name = "txtcountryDestination";
            this.txtcountryDestination.Size = new System.Drawing.Size(232, 22);
            this.txtcountryDestination.TabIndex = 116;
            this.txtcountryDestination.TextBox1Binding = "";
            // 
            // txtcustcd
            // 
            this.txtcustcd.BackColor = System.Drawing.Color.White;
            this.txtcustcd.BrandObjectName = null;
            this.txtcustcd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcustcd.Location = new System.Drawing.Point(112, 262);
            this.txtcustcd.Name = "txtcustcd";
            this.txtcustcd.Size = new System.Drawing.Size(125, 23);
            this.txtcustcd.TabIndex = 115;
            // 
            // labelDestination
            // 
            this.labelDestination.Location = new System.Drawing.Point(13, 299);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(95, 23);
            this.labelDestination.TabIndex = 114;
            this.labelDestination.Text = "Destination";
            // 
            // labelCustCD
            // 
            this.labelCustCD.Location = new System.Drawing.Point(13, 262);
            this.labelCustCD.Name = "labelCustCD";
            this.labelCustCD.Size = new System.Drawing.Size(95, 23);
            this.labelCustCD.TabIndex = 113;
            this.labelCustCD.Text = "Cust CD";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(370, 122);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(22, 23);
            this.label7.TabIndex = 120;
            this.label7.Text = "～";
            this.label7.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label7.TextStyle.Color = System.Drawing.Color.Black;
            this.label7.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label7.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtGBNoEnd
            // 
            this.txtGBNoEnd.BackColor = System.Drawing.Color.White;
            this.txtGBNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtGBNoEnd.Location = new System.Drawing.Point(112, 152);
            this.txtGBNoEnd.Name = "txtGBNoEnd";
            this.txtGBNoEnd.Size = new System.Drawing.Size(255, 23);
            this.txtGBNoEnd.TabIndex = 119;
            // 
            // txtGBNoStart
            // 
            this.txtGBNoStart.BackColor = System.Drawing.Color.White;
            this.txtGBNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtGBNoStart.Location = new System.Drawing.Point(112, 122);
            this.txtGBNoStart.Name = "txtGBNoStart";
            this.txtGBNoStart.Size = new System.Drawing.Size(255, 23);
            this.txtGBNoStart.TabIndex = 118;
            // 
            // labelGBNo
            // 
            this.labelGBNo.Location = new System.Drawing.Point(13, 122);
            this.labelGBNo.Name = "labelGBNo";
            this.labelGBNo.Size = new System.Drawing.Size(95, 23);
            this.labelGBNo.TabIndex = 117;
            this.labelGBNo.Text = "GB#";
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
            this.dateInvoiceDate.Location = new System.Drawing.Point(112, 48);
            this.dateInvoiceDate.Name = "dateInvoiceDate";
            this.dateInvoiceDate.Size = new System.Drawing.Size(280, 23);
            this.dateInvoiceDate.TabIndex = 122;
            // 
            // labelInvoiceDate
            // 
            this.labelInvoiceDate.Location = new System.Drawing.Point(13, 48);
            this.labelInvoiceDate.Name = "labelInvoiceDate";
            this.labelInvoiceDate.Size = new System.Drawing.Size(95, 23);
            this.labelInvoiceDate.TabIndex = 121;
            this.labelInvoiceDate.Text = "Invoice Date";
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
            this.datePulloutDate.Location = new System.Drawing.Point(112, 84);
            this.datePulloutDate.Name = "datePulloutDate";
            this.datePulloutDate.Size = new System.Drawing.Size(280, 23);
            this.datePulloutDate.TabIndex = 124;
            // 
            // labelPulloutDate
            // 
            this.labelPulloutDate.Location = new System.Drawing.Point(13, 84);
            this.labelPulloutDate.Name = "labelPulloutDate";
            this.labelPulloutDate.Size = new System.Drawing.Size(95, 23);
            this.labelPulloutDate.TabIndex = 123;
            this.labelPulloutDate.Text = "Pull-out Date";
            // 
            // txtbuyer
            // 
            this.txtbuyer.BackColor = System.Drawing.Color.White;
            this.txtbuyer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbuyer.Location = new System.Drawing.Point(112, 191);
            this.txtbuyer.Name = "txtbuyer";
            this.txtbuyer.Size = new System.Drawing.Size(66, 23);
            this.txtbuyer.TabIndex = 125;
            // 
            // labelBuyer
            // 
            this.labelBuyer.Location = new System.Drawing.Point(13, 191);
            this.labelBuyer.Name = "labelBuyer";
            this.labelBuyer.Size = new System.Drawing.Size(95, 23);
            this.labelBuyer.TabIndex = 126;
            this.labelBuyer.Text = "Buyer";
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(111, 333);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.Size = new System.Drawing.Size(238, 24);
            this.comboCategory.TabIndex = 141;
            this.comboCategory.Type = "Pms_GMT_Simple";
            // 
            // R12
            // 
            this.ClientSize = new System.Drawing.Size(580, 409);
            this.Controls.Add(this.comboCategory);
            this.Controls.Add(this.labelBuyer);
            this.Controls.Add(this.txtbuyer);
            this.Controls.Add(this.datePulloutDate);
            this.Controls.Add(this.labelPulloutDate);
            this.Controls.Add(this.dateInvoiceDate);
            this.Controls.Add(this.labelInvoiceDate);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtGBNoEnd);
            this.Controls.Add(this.txtGBNoStart);
            this.Controls.Add(this.labelGBNo);
            this.Controls.Add(this.txtcountryDestination);
            this.Controls.Add(this.txtcustcd);
            this.Controls.Add(this.labelDestination);
            this.Controls.Add(this.labelCustCD);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.dateFCRDate);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelFCRDate);
            this.IsSupportToPrint = false;
            this.Name = "R12";
            this.Text = "R12. Factory CMT Report";
            this.Controls.SetChildIndex(this.labelFCRDate, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelCategory, 0);
            this.Controls.SetChildIndex(this.dateFCRDate, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelCustCD, 0);
            this.Controls.SetChildIndex(this.labelDestination, 0);
            this.Controls.SetChildIndex(this.txtcustcd, 0);
            this.Controls.SetChildIndex(this.txtcountryDestination, 0);
            this.Controls.SetChildIndex(this.labelGBNo, 0);
            this.Controls.SetChildIndex(this.txtGBNoStart, 0);
            this.Controls.SetChildIndex(this.txtGBNoEnd, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.labelInvoiceDate, 0);
            this.Controls.SetChildIndex(this.dateInvoiceDate, 0);
            this.Controls.SetChildIndex(this.labelPulloutDate, 0);
            this.Controls.SetChildIndex(this.datePulloutDate, 0);
            this.Controls.SetChildIndex(this.txtbuyer, 0);
            this.Controls.SetChildIndex(this.labelBuyer, 0);
            this.Controls.SetChildIndex(this.comboCategory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelFCRDate;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelCategory;
        private Win.UI.DateRange dateFCRDate;
        private Class.Txtbrand txtbrand;
        private Class.Txtcountry txtcountryDestination;
        private Class.Txtcustcd txtcustcd;
        private Win.UI.Label labelDestination;
        private Win.UI.Label labelCustCD;
        private Win.UI.Label label7;
        private Win.UI.TextBox txtGBNoEnd;
        private Win.UI.TextBox txtGBNoStart;
        private Win.UI.Label labelGBNo;
        private Win.UI.DateRange dateInvoiceDate;
        private Win.UI.Label labelInvoiceDate;
        private Win.UI.DateRange datePulloutDate;
        private Win.UI.Label labelPulloutDate;
        private Class.Txtbuyer txtbuyer;
        private Win.UI.Label labelBuyer;
        private Class.ComboDropDownList comboCategory;
    }
}
