namespace Sci.Production.Quality
{
    partial class R02
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
            this.comboCategory = new Sci.Production.Class.ComboDropDownList(this.components);
            this.txtSeason = new Sci.Production.Class.Txtseason();
            this.txtBrand = new Sci.Production.Class.Txtbrand();
            this.txtsupplier = new Sci.Production.Class.Txtsupplier();
            this.comboOverallResultStatus = new Sci.Win.UI.ComboBox();
            this.txtRefno = new Sci.Win.UI.TextBox();
            this.label13 = new Sci.Win.UI.Label();
            this.txtSPEnd = new Sci.Win.UI.TextBox();
            this.txtSPStart = new Sci.Win.UI.TextBox();
            this.dateEstCuttingDate = new Sci.Win.UI.DateRange();
            this.dateSewingInLineDate = new Sci.Win.UI.DateRange();
            this.dateRange4 = new Sci.Win.UI.DateRange();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.dateArriveWHDate = new Sci.Win.UI.DateRange();
            this.labelOverallResultStatus = new Sci.Win.UI.Label();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.labelRefno = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelSP = new Sci.Win.UI.Label();
            this.labelEstCuttingDate = new Sci.Win.UI.Label();
            this.labelSewingInLineDate = new Sci.Win.UI.Label();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.labelArriveWHDate = new Sci.Win.UI.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboCategory);
            this.panel1.Controls.Add(this.txtSeason);
            this.panel1.Controls.Add(this.txtBrand);
            this.panel1.Controls.Add(this.txtsupplier);
            this.panel1.Controls.Add(this.comboOverallResultStatus);
            this.panel1.Controls.Add(this.txtRefno);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.txtSPEnd);
            this.panel1.Controls.Add(this.txtSPStart);
            this.panel1.Controls.Add(this.dateEstCuttingDate);
            this.panel1.Controls.Add(this.dateSewingInLineDate);
            this.panel1.Controls.Add(this.dateRange4);
            this.panel1.Controls.Add(this.dateSCIDelivery);
            this.panel1.Controls.Add(this.dateArriveWHDate);
            this.panel1.Controls.Add(this.labelOverallResultStatus);
            this.panel1.Controls.Add(this.labelSupplier);
            this.panel1.Controls.Add(this.labelCategory);
            this.panel1.Controls.Add(this.labelRefno);
            this.panel1.Controls.Add(this.labelBrand);
            this.panel1.Controls.Add(this.labelSeason);
            this.panel1.Controls.Add(this.labelSP);
            this.panel1.Controls.Add(this.labelEstCuttingDate);
            this.panel1.Controls.Add(this.labelSewingInLineDate);
            this.panel1.Controls.Add(this.labelSCIDelivery);
            this.panel1.Controls.Add(this.labelArriveWHDate);
            this.panel1.Location = new System.Drawing.Point(20, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(472, 466);
            this.panel1.TabIndex = 94;
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(174, 338);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.Size = new System.Drawing.Size(185, 24);
            this.comboCategory.TabIndex = 52;
            this.comboCategory.Type = "Pms_MtlCategory";
            // 
            // txtSeason
            // 
            this.txtSeason.BackColor = System.Drawing.Color.White;
            this.txtSeason.BrandObjectName = null;
            this.txtSeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeason.Location = new System.Drawing.Point(174, 215);
            this.txtSeason.Name = "txtSeason";
            this.txtSeason.Size = new System.Drawing.Size(135, 23);
            this.txtSeason.TabIndex = 50;
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(174, 257);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(93, 23);
            this.txtBrand.TabIndex = 42;
            // 
            // txtsupplier
            // 
            this.txtsupplier.DisplayBox1Binding = "";
            this.txtsupplier.Location = new System.Drawing.Point(174, 380);
            this.txtsupplier.Name = "txtsupplier";
            this.txtsupplier.Size = new System.Drawing.Size(147, 23);
            this.txtsupplier.TabIndex = 41;
            this.txtsupplier.TextBox1Binding = "";
            // 
            // comboOverallResultStatus
            // 
            this.comboOverallResultStatus.BackColor = System.Drawing.Color.White;
            this.comboOverallResultStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboOverallResultStatus.FormattingEnabled = true;
            this.comboOverallResultStatus.IsSupportUnselect = true;
            this.comboOverallResultStatus.Items.AddRange(new object[] {
            "All",
            "Pass",
            "Fail",
            "Empty Result",
            "N/A inspection & test"});
            this.comboOverallResultStatus.Location = new System.Drawing.Point(174, 424);
            this.comboOverallResultStatus.Name = "comboOverallResultStatus";
            this.comboOverallResultStatus.Size = new System.Drawing.Size(161, 24);
            this.comboOverallResultStatus.TabIndex = 40;
            // 
            // txtRefno
            // 
            this.txtRefno.BackColor = System.Drawing.Color.White;
            this.txtRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefno.Location = new System.Drawing.Point(174, 298);
            this.txtRefno.Name = "txtRefno";
            this.txtRefno.Size = new System.Drawing.Size(253, 23);
            this.txtRefno.TabIndex = 38;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label13.Location = new System.Drawing.Point(302, 170);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(22, 23);
            this.label13.TabIndex = 35;
            this.label13.Text = "～";
            this.label13.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSPEnd
            // 
            this.txtSPEnd.BackColor = System.Drawing.Color.White;
            this.txtSPEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPEnd.Location = new System.Drawing.Point(323, 170);
            this.txtSPEnd.Name = "txtSPEnd";
            this.txtSPEnd.Size = new System.Drawing.Size(131, 23);
            this.txtSPEnd.TabIndex = 34;
            // 
            // txtSPStart
            // 
            this.txtSPStart.BackColor = System.Drawing.Color.White;
            this.txtSPStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPStart.Location = new System.Drawing.Point(174, 170);
            this.txtSPStart.Name = "txtSPStart";
            this.txtSPStart.Size = new System.Drawing.Size(125, 23);
            this.txtSPStart.TabIndex = 33;
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
            this.dateEstCuttingDate.Location = new System.Drawing.Point(174, 131);
            this.dateEstCuttingDate.Name = "dateEstCuttingDate";
            this.dateEstCuttingDate.Size = new System.Drawing.Size(280, 23);
            this.dateEstCuttingDate.TabIndex = 32;
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
            this.dateSewingInLineDate.Location = new System.Drawing.Point(174, 93);
            this.dateSewingInLineDate.Name = "dateSewingInLineDate";
            this.dateSewingInLineDate.Size = new System.Drawing.Size(280, 23);
            this.dateSewingInLineDate.TabIndex = 31;
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
            this.dateRange4.Location = new System.Drawing.Point(247, 93);
            this.dateRange4.Name = "dateRange4";
            this.dateRange4.Size = new System.Drawing.Size(8, 23);
            this.dateRange4.TabIndex = 30;
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
            this.dateSCIDelivery.Location = new System.Drawing.Point(174, 56);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 29;
            // 
            // dateArriveWHDate
            // 
            // 
            // 
            // 
            this.dateArriveWHDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateArriveWHDate.DateBox1.Name = "";
            this.dateArriveWHDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateArriveWHDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateArriveWHDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateArriveWHDate.DateBox2.Name = "";
            this.dateArriveWHDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateArriveWHDate.DateBox2.TabIndex = 1;
            this.dateArriveWHDate.IsRequired = false;
            this.dateArriveWHDate.Location = new System.Drawing.Point(174, 19);
            this.dateArriveWHDate.Name = "dateArriveWHDate";
            this.dateArriveWHDate.Size = new System.Drawing.Size(280, 23);
            this.dateArriveWHDate.TabIndex = 28;
            // 
            // labelOverallResultStatus
            // 
            this.labelOverallResultStatus.Location = new System.Drawing.Point(9, 425);
            this.labelOverallResultStatus.Name = "labelOverallResultStatus";
            this.labelOverallResultStatus.Size = new System.Drawing.Size(154, 23);
            this.labelOverallResultStatus.TabIndex = 22;
            this.labelOverallResultStatus.Text = "Overall Result Status";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Location = new System.Drawing.Point(9, 381);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(154, 23);
            this.labelSupplier.TabIndex = 21;
            this.labelSupplier.Text = "Supplier";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(9, 339);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(154, 23);
            this.labelCategory.TabIndex = 20;
            this.labelCategory.Text = "Category";
            // 
            // labelRefno
            // 
            this.labelRefno.Location = new System.Drawing.Point(9, 298);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.Size = new System.Drawing.Size(154, 23);
            this.labelRefno.TabIndex = 19;
            this.labelRefno.Text = "Refno";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(9, 257);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(154, 23);
            this.labelBrand.TabIndex = 18;
            this.labelBrand.Text = "Brand";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(9, 215);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(154, 23);
            this.labelSeason.TabIndex = 17;
            this.labelSeason.Text = "Season";
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(9, 170);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(154, 23);
            this.labelSP.TabIndex = 16;
            this.labelSP.Text = "SP#";
            // 
            // labelEstCuttingDate
            // 
            this.labelEstCuttingDate.Location = new System.Drawing.Point(9, 131);
            this.labelEstCuttingDate.Name = "labelEstCuttingDate";
            this.labelEstCuttingDate.Size = new System.Drawing.Size(154, 23);
            this.labelEstCuttingDate.TabIndex = 15;
            this.labelEstCuttingDate.Text = "Est. Cutting Date";
            // 
            // labelSewingInLineDate
            // 
            this.labelSewingInLineDate.Location = new System.Drawing.Point(9, 93);
            this.labelSewingInLineDate.Name = "labelSewingInLineDate";
            this.labelSewingInLineDate.Size = new System.Drawing.Size(154, 23);
            this.labelSewingInLineDate.TabIndex = 14;
            this.labelSewingInLineDate.Text = "Sewing in-line Date";
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(9, 56);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(154, 23);
            this.labelSCIDelivery.TabIndex = 13;
            this.labelSCIDelivery.Text = "SCI Delivery";
            // 
            // labelArriveWHDate
            // 
            this.labelArriveWHDate.Location = new System.Drawing.Point(9, 19);
            this.labelArriveWHDate.Name = "labelArriveWHDate";
            this.labelArriveWHDate.Size = new System.Drawing.Size(154, 23);
            this.labelArriveWHDate.TabIndex = 12;
            this.labelArriveWHDate.Text = "Arrive W/H Date";
            // 
            // R02
            // 
            this.ClientSize = new System.Drawing.Size(627, 509);
            this.Controls.Add(this.panel1);
            this.Name = "R02";
            this.Text = "R02.Accessory Inspection & Laboratory Status";
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
        private Win.UI.Label labelOverallResultStatus;
        private Win.UI.Label labelSupplier;
        private Win.UI.Label labelCategory;
        private Win.UI.Label labelRefno;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelSP;
        private Win.UI.Label labelEstCuttingDate;
        private Win.UI.Label labelSewingInLineDate;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Label labelArriveWHDate;
        private Class.Txtsupplier txtsupplier;
        private Win.UI.ComboBox comboOverallResultStatus;
        private Win.UI.TextBox txtRefno;
        private Win.UI.Label label13;
        private Win.UI.TextBox txtSPEnd;
        private Win.UI.TextBox txtSPStart;
        private Win.UI.DateRange dateEstCuttingDate;
        private Win.UI.DateRange dateSewingInLineDate;
        private Win.UI.DateRange dateRange4;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.DateRange dateArriveWHDate;
        private Class.Txtbrand txtBrand;
        private Class.Txtseason txtSeason;
        private Class.ComboDropDownList comboCategory;
    }
}
