namespace Sci.Production.Quality
{
    partial class R07
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
            this.labelArriveWHDate = new Sci.Win.UI.Label();
            this.panel1 = new Sci.Win.UI.Panel();
            this.comboCategory = new Sci.Production.Class.ComboDropDownList(this.components);
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.comboMaterialType = new Sci.Win.UI.ComboBox();
            this.txtsupplier = new Sci.Production.Class.Txtsupplier();
            this.txtRefno = new Sci.Win.UI.TextBox();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txtseason = new Sci.Production.Class.Txtseason();
            this.txtSPEnd = new Sci.Win.UI.TextBox();
            this.label13 = new Sci.Win.UI.Label();
            this.txtSPStart = new Sci.Win.UI.TextBox();
            this.dateEstCuttingDate = new Sci.Win.UI.DateRange();
            this.dateSewingInLineDate = new Sci.Win.UI.DateRange();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.dateArriveWHDate = new Sci.Win.UI.DateRange();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelMaterialType = new Sci.Win.UI.Label();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.labelRefno = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelSP = new Sci.Win.UI.Label();
            this.labelEstCuttingDate = new Sci.Win.UI.Label();
            this.labelSewingInLineDate = new Sci.Win.UI.Label();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.rdbtnbyWK = new Sci.Win.UI.RadioButton();
            this.rdbtnbyRoll = new Sci.Win.UI.RadioButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelArriveWHDate
            // 
            this.labelArriveWHDate.Location = new System.Drawing.Point(14, 18);
            this.labelArriveWHDate.Name = "labelArriveWHDate";
            this.labelArriveWHDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelArriveWHDate.Size = new System.Drawing.Size(135, 23);
            this.labelArriveWHDate.TabIndex = 95;
            this.labelArriveWHDate.Text = "Arrive W/H Date";
            this.labelArriveWHDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rdbtnbyRoll);
            this.panel1.Controls.Add(this.rdbtnbyWK);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.comboCategory);
            this.panel1.Controls.Add(this.comboFactory);
            this.panel1.Controls.Add(this.comboMaterialType);
            this.panel1.Controls.Add(this.txtsupplier);
            this.panel1.Controls.Add(this.txtRefno);
            this.panel1.Controls.Add(this.txtbrand);
            this.panel1.Controls.Add(this.txtseason);
            this.panel1.Controls.Add(this.txtSPEnd);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.txtSPStart);
            this.panel1.Controls.Add(this.dateEstCuttingDate);
            this.panel1.Controls.Add(this.dateSewingInLineDate);
            this.panel1.Controls.Add(this.dateSCIDelivery);
            this.panel1.Controls.Add(this.dateArriveWHDate);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.labelMaterialType);
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
            this.panel1.Location = new System.Drawing.Point(14, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(499, 516);
            this.panel1.TabIndex = 96;
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(161, 320);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(185, 24);
            this.comboCategory.TabIndex = 117;
            this.comboCategory.Type = "Pms_MtlCategory";
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(161, 440);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(142, 24);
            this.comboFactory.TabIndex = 120;
            // 
            // comboMaterialType
            // 
            this.comboMaterialType.BackColor = System.Drawing.Color.White;
            this.comboMaterialType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMaterialType.FormattingEnabled = true;
            this.comboMaterialType.IsSupportUnselect = true;
            this.comboMaterialType.Location = new System.Drawing.Point(161, 401);
            this.comboMaterialType.Name = "comboMaterialType";
            this.comboMaterialType.OldText = "";
            this.comboMaterialType.Size = new System.Drawing.Size(142, 24);
            this.comboMaterialType.TabIndex = 119;
            // 
            // txtsupplier
            // 
            this.txtsupplier.DisplayBox1Binding = "";
            this.txtsupplier.Location = new System.Drawing.Point(161, 361);
            this.txtsupplier.Name = "txtsupplier";
            this.txtsupplier.Size = new System.Drawing.Size(155, 23);
            this.txtsupplier.TabIndex = 118;
            this.txtsupplier.TextBox1Binding = "";
            // 
            // txtRefno
            // 
            this.txtRefno.BackColor = System.Drawing.Color.White;
            this.txtRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefno.Location = new System.Drawing.Point(161, 281);
            this.txtRefno.Name = "txtRefno";
            this.txtRefno.Size = new System.Drawing.Size(260, 23);
            this.txtRefno.TabIndex = 116;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(161, 242);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(112, 23);
            this.txtbrand.TabIndex = 115;
            // 
            // txtseason
            // 
            this.txtseason.BackColor = System.Drawing.Color.White;
            this.txtseason.BrandObjectName = null;
            this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason.Location = new System.Drawing.Point(161, 202);
            this.txtseason.Name = "txtseason";
            this.txtseason.Size = new System.Drawing.Size(155, 23);
            this.txtseason.TabIndex = 114;
            // 
            // txtSPEnd
            // 
            this.txtSPEnd.BackColor = System.Drawing.Color.White;
            this.txtSPEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPEnd.Location = new System.Drawing.Point(313, 166);
            this.txtSPEnd.Name = "txtSPEnd";
            this.txtSPEnd.Size = new System.Drawing.Size(128, 23);
            this.txtSPEnd.TabIndex = 113;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.SystemColors.Control;
            this.label13.Location = new System.Drawing.Point(287, 166);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 23);
            this.label13.TabIndex = 112;
            this.label13.Text = " ～";
            this.label13.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSPStart
            // 
            this.txtSPStart.BackColor = System.Drawing.Color.White;
            this.txtSPStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPStart.Location = new System.Drawing.Point(161, 166);
            this.txtSPStart.Name = "txtSPStart";
            this.txtSPStart.Size = new System.Drawing.Size(128, 23);
            this.txtSPStart.TabIndex = 111;
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
            this.dateEstCuttingDate.Location = new System.Drawing.Point(161, 127);
            this.dateEstCuttingDate.Name = "dateEstCuttingDate";
            this.dateEstCuttingDate.Size = new System.Drawing.Size(280, 23);
            this.dateEstCuttingDate.TabIndex = 110;
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
            this.dateSewingInLineDate.Location = new System.Drawing.Point(161, 89);
            this.dateSewingInLineDate.Name = "dateSewingInLineDate";
            this.dateSewingInLineDate.Size = new System.Drawing.Size(280, 23);
            this.dateSewingInLineDate.TabIndex = 109;
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
            this.dateSCIDelivery.Location = new System.Drawing.Point(161, 52);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 108;
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
            this.dateArriveWHDate.Location = new System.Drawing.Point(161, 18);
            this.dateArriveWHDate.Name = "dateArriveWHDate";
            this.dateArriveWHDate.Size = new System.Drawing.Size(280, 23);
            this.dateArriveWHDate.TabIndex = 107;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(14, 440);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(135, 23);
            this.labelFactory.TabIndex = 106;
            this.labelFactory.Text = "Factory";
            // 
            // labelMaterialType
            // 
            this.labelMaterialType.Location = new System.Drawing.Point(14, 401);
            this.labelMaterialType.Name = "labelMaterialType";
            this.labelMaterialType.Size = new System.Drawing.Size(135, 23);
            this.labelMaterialType.TabIndex = 105;
            this.labelMaterialType.Text = "Material Type";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Location = new System.Drawing.Point(14, 361);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(135, 23);
            this.labelSupplier.TabIndex = 104;
            this.labelSupplier.Text = "Supplier";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(14, 320);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(135, 23);
            this.labelCategory.TabIndex = 103;
            this.labelCategory.Text = "Category";
            // 
            // labelRefno
            // 
            this.labelRefno.Location = new System.Drawing.Point(14, 281);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.Size = new System.Drawing.Size(135, 23);
            this.labelRefno.TabIndex = 102;
            this.labelRefno.Text = "Refno";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(14, 242);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(135, 23);
            this.labelBrand.TabIndex = 101;
            this.labelBrand.Text = "Brand";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(14, 202);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(135, 23);
            this.labelSeason.TabIndex = 100;
            this.labelSeason.Text = "Season";
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(14, 166);
            this.labelSP.Name = "labelSP";
            this.labelSP.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSP.Size = new System.Drawing.Size(135, 23);
            this.labelSP.TabIndex = 99;
            this.labelSP.Text = "SP#";
            this.labelSP.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelEstCuttingDate
            // 
            this.labelEstCuttingDate.Location = new System.Drawing.Point(14, 127);
            this.labelEstCuttingDate.Name = "labelEstCuttingDate";
            this.labelEstCuttingDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelEstCuttingDate.Size = new System.Drawing.Size(135, 23);
            this.labelEstCuttingDate.TabIndex = 98;
            this.labelEstCuttingDate.Text = "Est. Cutting Date";
            this.labelEstCuttingDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelSewingInLineDate
            // 
            this.labelSewingInLineDate.Location = new System.Drawing.Point(14, 89);
            this.labelSewingInLineDate.Name = "labelSewingInLineDate";
            this.labelSewingInLineDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSewingInLineDate.Size = new System.Drawing.Size(135, 23);
            this.labelSewingInLineDate.TabIndex = 97;
            this.labelSewingInLineDate.Text = "Sewing in-line Date";
            this.labelSewingInLineDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(14, 52);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSCIDelivery.Size = new System.Drawing.Size(135, 23);
            this.labelSCIDelivery.TabIndex = 96;
            this.labelSCIDelivery.Text = "SCI Delivery";
            this.labelSCIDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 479);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 23);
            this.label1.TabIndex = 122;
            this.label1.Text = "Report Type";
            // 
            // rdbtnbyWK
            // 
            this.rdbtnbyWK.AutoSize = true;
            this.rdbtnbyWK.Checked = true;
            this.rdbtnbyWK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdbtnbyWK.Location = new System.Drawing.Point(161, 481);
            this.rdbtnbyWK.Name = "rdbtnbyWK";
            this.rdbtnbyWK.Size = new System.Drawing.Size(75, 21);
            this.rdbtnbyWK.TabIndex = 121;
            this.rdbtnbyWK.TabStop = true;
            this.rdbtnbyWK.Text = "by WK#";
            this.rdbtnbyWK.UseVisualStyleBackColor = true;
            // 
            // rdbtnbyRoll
            // 
            this.rdbtnbyRoll.AutoSize = true;
            this.rdbtnbyRoll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdbtnbyRoll.Location = new System.Drawing.Point(242, 481);
            this.rdbtnbyRoll.Name = "rdbtnbyRoll";
            this.rdbtnbyRoll.Size = new System.Drawing.Size(69, 21);
            this.rdbtnbyRoll.TabIndex = 122;
            this.rdbtnbyRoll.Text = "by Roll";
            this.rdbtnbyRoll.UseVisualStyleBackColor = true;
            // 
            // R07
            // 
            this.ClientSize = new System.Drawing.Size(627, 562);
            this.Controls.Add(this.panel1);
            this.Name = "R07";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R07.Urgent Inspection Report";
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

        private Win.UI.Label labelArriveWHDate;
        private Win.UI.Panel panel1;
        private Win.UI.TextBox txtSPEnd;
        private Win.UI.Label label13;
        private Win.UI.TextBox txtSPStart;
        private Win.UI.DateRange dateEstCuttingDate;
        private Win.UI.DateRange dateSewingInLineDate;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.DateRange dateArriveWHDate;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelMaterialType;
        private Win.UI.Label labelSupplier;
        private Win.UI.Label labelCategory;
        private Win.UI.Label labelRefno;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelSP;
        private Win.UI.Label labelEstCuttingDate;
        private Win.UI.Label labelSewingInLineDate;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.TextBox txtRefno;
        private Class.Txtbrand txtbrand;
        private Class.Txtseason txtseason;
        private Win.UI.ComboBox comboMaterialType;
        private Class.Txtsupplier txtsupplier;
        private Win.UI.ComboBox comboFactory;
        private Class.ComboDropDownList comboCategory;
        private Win.UI.RadioButton rdbtnbyRoll;
        private Win.UI.RadioButton rdbtnbyWK;
        private Win.UI.Label label1;
    }
}
