namespace Sci.Production.Sewing
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
            this.labelSewingOutputDate = new Sci.Win.UI.Label();
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelFtyZone = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.dateSewingOutputDate = new Sci.Win.UI.DateRange();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.labelStyle = new Sci.Win.UI.Label();
            this.chkType = new Sci.Win.UI.CheckBox();
            this.txtstyle = new Sci.Production.Class.Txtstyle();
            this.comboDropDownListCategory = new Sci.Production.Class.ComboDropDownList(this.components);
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txtseason = new Sci.Production.Class.Txtseason();
            this.comboFtyZone = new Sci.Production.Class.ComboFtyZone(this.components);
            this.numNewStyleBaseOn = new Sci.Win.UI.NumericUpDown();
            this.labelmonth = new Sci.Win.UI.Label();
            this.labelNewStyleBaseOn = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numNewStyleBaseOn)).BeginInit();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(391, 12);
            this.print.TabIndex = 9;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(391, 48);
            this.toexcel.TabIndex = 10;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(391, 84);
            this.close.TabIndex = 11;
            // 
            // labelSewingOutputDate
            // 
            this.labelSewingOutputDate.Location = new System.Drawing.Point(13, 12);
            this.labelSewingOutputDate.Name = "labelSewingOutputDate";
            this.labelSewingOutputDate.Size = new System.Drawing.Size(128, 23);
            this.labelSewingOutputDate.TabIndex = 94;
            this.labelSewingOutputDate.Text = "Sewing Output Date";
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Location = new System.Drawing.Point(13, 43);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(128, 23);
            this.labelBuyerDelivery.TabIndex = 95;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(13, 74);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(128, 23);
            this.labelSCIDelivery.TabIndex = 96;
            this.labelSCIDelivery.Text = "SCI Delivery";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(13, 105);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(128, 23);
            this.labelSeason.TabIndex = 97;
            this.labelSeason.Text = "Season";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(13, 136);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(128, 23);
            this.labelBrand.TabIndex = 98;
            this.labelBrand.Text = "Brand";
            // 
            // labelFtyZone
            // 
            this.labelFtyZone.Location = new System.Drawing.Point(13, 196);
            this.labelFtyZone.Name = "labelFtyZone";
            this.labelFtyZone.Size = new System.Drawing.Size(128, 23);
            this.labelFtyZone.TabIndex = 99;
            this.labelFtyZone.Text = "FtyZone";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 227);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(128, 23);
            this.labelFactory.TabIndex = 100;
            this.labelFactory.Text = "Factory";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(13, 258);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(128, 23);
            this.labelCategory.TabIndex = 101;
            this.labelCategory.Text = "Category";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(13, 347);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(416, 23);
            this.label9.TabIndex = 8;
            this.label9.Text = "Hint:  All subcon-out、non sister subcon-in excluded.";
            this.label9.TextStyle.BorderColor = System.Drawing.Color.Red;
            this.label9.TextStyle.Color = System.Drawing.Color.Red;
            this.label9.TextStyle.ExtBorderColor = System.Drawing.Color.Red;
            this.label9.TextStyle.GradientColor = System.Drawing.Color.Red;
            // 
            // dateSewingOutputDate
            // 
            // 
            // 
            // 
            this.dateSewingOutputDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSewingOutputDate.DateBox1.Name = "";
            this.dateSewingOutputDate.DateBox1.Size = new System.Drawing.Size(102, 23);
            this.dateSewingOutputDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSewingOutputDate.DateBox2.Location = new System.Drawing.Point(124, 0);
            this.dateSewingOutputDate.DateBox2.Name = "";
            this.dateSewingOutputDate.DateBox2.Size = new System.Drawing.Size(102, 23);
            this.dateSewingOutputDate.DateBox2.TabIndex = 1;
            this.dateSewingOutputDate.IsRequired = false;
            this.dateSewingOutputDate.Location = new System.Drawing.Point(145, 12);
            this.dateSewingOutputDate.Name = "dateSewingOutputDate";
            this.dateSewingOutputDate.Size = new System.Drawing.Size(226, 23);
            this.dateSewingOutputDate.TabIndex = 0;
            // 
            // dateBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerDelivery.DateBox1.Name = "";
            this.dateBuyerDelivery.DateBox1.Size = new System.Drawing.Size(102, 23);
            this.dateBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox2.Location = new System.Drawing.Point(124, 0);
            this.dateBuyerDelivery.DateBox2.Name = "";
            this.dateBuyerDelivery.DateBox2.Size = new System.Drawing.Size(102, 23);
            this.dateBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateBuyerDelivery.IsRequired = false;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(145, 43);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(226, 23);
            this.dateBuyerDelivery.TabIndex = 1;
            // 
            // dateSCIDelivery
            // 
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSCIDelivery.DateBox1.Name = "";
            this.dateSCIDelivery.DateBox1.Size = new System.Drawing.Size(102, 23);
            this.dateSCIDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox2.Location = new System.Drawing.Point(124, 0);
            this.dateSCIDelivery.DateBox2.Name = "";
            this.dateSCIDelivery.DateBox2.Size = new System.Drawing.Size(102, 23);
            this.dateSCIDelivery.DateBox2.TabIndex = 1;
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(145, 74);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(226, 23);
            this.dateSCIDelivery.TabIndex = 2;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(145, 227);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(80, 24);
            this.comboFactory.TabIndex = 6;
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(13, 166);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(128, 23);
            this.labelStyle.TabIndex = 98;
            this.labelStyle.Text = "Style";
            // 
            // chkType
            // 
            this.chkType.AutoSize = true;
            this.chkType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkType.Location = new System.Drawing.Point(13, 319);
            this.chkType.Name = "chkType";
            this.chkType.Size = new System.Drawing.Size(172, 21);
            this.chkType.TabIndex = 224;
            this.chkType.Text = "Exclude sample factory";
            this.chkType.UseVisualStyleBackColor = true;
            // 
            // txtstyle
            // 
            this.txtstyle.BackColor = System.Drawing.Color.White;
            this.txtstyle.BrandObjectName = null;
            this.txtstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle.Location = new System.Drawing.Point(145, 166);
            this.txtstyle.Name = "txtstyle";
            this.txtstyle.Size = new System.Drawing.Size(130, 23);
            this.txtstyle.TabIndex = 104;
            this.txtstyle.TarBrand = null;
            this.txtstyle.TarSeason = null;
            // 
            // comboDropDownListCategory
            // 
            this.comboDropDownListCategory.AddAllItem = false;
            this.comboDropDownListCategory.BackColor = System.Drawing.Color.White;
            this.comboDropDownListCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDropDownListCategory.FormattingEnabled = true;
            this.comboDropDownListCategory.IsSupportUnselect = true;
            this.comboDropDownListCategory.Location = new System.Drawing.Point(145, 258);
            this.comboDropDownListCategory.Name = "comboDropDownListCategory";
            this.comboDropDownListCategory.OldText = "";
            this.comboDropDownListCategory.Size = new System.Drawing.Size(160, 24);
            this.comboDropDownListCategory.TabIndex = 7;
            this.comboDropDownListCategory.Type = "Pms_GMT_Simple";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(145, 136);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(80, 23);
            this.txtbrand.TabIndex = 4;
            // 
            // txtseason
            // 
            this.txtseason.BackColor = System.Drawing.Color.White;
            this.txtseason.BrandObjectName = null;
            this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason.Location = new System.Drawing.Point(145, 105);
            this.txtseason.Name = "txtseason";
            this.txtseason.Size = new System.Drawing.Size(80, 23);
            this.txtseason.TabIndex = 3;
            // 
            // comboFtyZone
            // 
            this.comboFtyZone.BackColor = System.Drawing.Color.White;
            this.comboFtyZone.FilteMDivision = false;
            this.comboFtyZone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFtyZone.FormattingEnabled = true;
            this.comboFtyZone.IsIncludeSampleRoom = false;
            this.comboFtyZone.IsProduceFty = false;
            this.comboFtyZone.IssupportJunk = true;
            this.comboFtyZone.IsSupportUnselect = true;
            this.comboFtyZone.Location = new System.Drawing.Point(145, 196);
            this.comboFtyZone.Name = "comboFtyZone";
            this.comboFtyZone.OldText = "";
            this.comboFtyZone.SelectTable = "Factory";
            this.comboFtyZone.Size = new System.Drawing.Size(80, 24);
            this.comboFtyZone.TabIndex = 225;
            // 
            // numNewStyleBaseOn
            // 
            this.numNewStyleBaseOn.BackColor = System.Drawing.Color.White;
            this.numNewStyleBaseOn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numNewStyleBaseOn.Location = new System.Drawing.Point(145, 288);
            this.numNewStyleBaseOn.Name = "numNewStyleBaseOn";
            this.numNewStyleBaseOn.Size = new System.Drawing.Size(53, 23);
            this.numNewStyleBaseOn.TabIndex = 226;
            this.numNewStyleBaseOn.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelmonth
            // 
            this.labelmonth.Location = new System.Drawing.Point(201, 288);
            this.labelmonth.Name = "labelmonth";
            this.labelmonth.Size = new System.Drawing.Size(61, 23);
            this.labelmonth.TabIndex = 228;
            this.labelmonth.Text = "month(s)";
            // 
            // labelNewStyleBaseOn
            // 
            this.labelNewStyleBaseOn.Location = new System.Drawing.Point(13, 289);
            this.labelNewStyleBaseOn.Name = "labelNewStyleBaseOn";
            this.labelNewStyleBaseOn.Size = new System.Drawing.Size(128, 23);
            this.labelNewStyleBaseOn.TabIndex = 227;
            this.labelNewStyleBaseOn.Text = "New Style Base on";
            // 
            // R03
            // 
            this.ClientSize = new System.Drawing.Size(551, 399);
            this.Controls.Add(this.numNewStyleBaseOn);
            this.Controls.Add(this.labelmonth);
            this.Controls.Add(this.labelNewStyleBaseOn);
            this.Controls.Add(this.comboFtyZone);
            this.Controls.Add(this.chkType);
            this.Controls.Add(this.txtstyle);
            this.Controls.Add(this.comboDropDownListCategory);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.txtseason);
            this.Controls.Add(this.dateSCIDelivery);
            this.Controls.Add(this.dateBuyerDelivery);
            this.Controls.Add(this.dateSewingOutputDate);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelFtyZone);
            this.Controls.Add(this.labelStyle);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelSeason);
            this.Controls.Add(this.labelSCIDelivery);
            this.Controls.Add(this.labelBuyerDelivery);
            this.Controls.Add(this.labelSewingOutputDate);
            this.DefaultControl = "dateSewingOutputDate";
            this.DefaultControlForEdit = "dateSewingOutputDate";
            this.IsSupportToPrint = false;
            this.Name = "R03";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R03. Prod. Efficiency Analysis Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelSewingOutputDate, 0);
            this.Controls.SetChildIndex(this.labelBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.labelSCIDelivery, 0);
            this.Controls.SetChildIndex(this.labelSeason, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelStyle, 0);
            this.Controls.SetChildIndex(this.labelFtyZone, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelCategory, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.dateSewingOutputDate, 0);
            this.Controls.SetChildIndex(this.dateBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.dateSCIDelivery, 0);
            this.Controls.SetChildIndex(this.txtseason, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.comboDropDownListCategory, 0);
            this.Controls.SetChildIndex(this.txtstyle, 0);
            this.Controls.SetChildIndex(this.chkType, 0);
            this.Controls.SetChildIndex(this.comboFtyZone, 0);
            this.Controls.SetChildIndex(this.labelNewStyleBaseOn, 0);
            this.Controls.SetChildIndex(this.labelmonth, 0);
            this.Controls.SetChildIndex(this.numNewStyleBaseOn, 0);
            ((System.ComponentModel.ISupportInitialize)(this.numNewStyleBaseOn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelSewingOutputDate;
        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelFtyZone;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelCategory;
        private Win.UI.Label label9;
        private Win.UI.DateRange dateSewingOutputDate;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.DateRange dateSCIDelivery;
        private Class.Txtseason txtseason;
        private Class.Txtbrand txtbrand;
        private Win.UI.ComboBox comboFactory;
        private Class.ComboDropDownList comboDropDownListCategory;
        private Win.UI.Label labelStyle;
        private Class.Txtstyle txtstyle;
        private Win.UI.CheckBox chkType;
        private Class.ComboFtyZone comboFtyZone;
        private Win.UI.NumericUpDown numNewStyleBaseOn;
        private Win.UI.Label labelmonth;
        private Win.UI.Label labelNewStyleBaseOn;
    }
}
